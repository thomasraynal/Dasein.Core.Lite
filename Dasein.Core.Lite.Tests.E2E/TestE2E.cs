using Dasein.Core.Lite.Demo.Server;
using Dasein.Core.Lite.Demo.Shared;
using Dasein.Core.Lite;
using Dasein.Core.Lite.Shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using NUnit.Framework;
using System;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using StructureMap;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;
using System.Text;
using GraphQL.Common.Request;
using GraphQL.Client;
using System.Linq;
using System.Threading;
using System.Runtime.Serialization;

namespace Test
{

    [TestFixture]
    public class TestE2E
    {
        private Host<TradeServiceStartup> _host;
        private IWebHost _app;
        private IServiceConfiguration _configuration;
        private ITradeService _tradeClient;
        private IPriceService _priceClient;

        [OneTimeSetUp]
        public void SetUp()
        {
            _host = new Host<TradeServiceStartup>();
            _app = _host.Build();
            _app.Start();

            _configuration = AppCore.Instance.Get<IServiceConfiguration>();

            var jsonSettings = new AppJsonSerializer();
            var jsonSerializer = JsonSerializer.Create(jsonSettings);

            AppCore.Instance.ObjectProvider.Configure(conf => conf.For<JsonSerializerSettings>().Use(jsonSettings));
            AppCore.Instance.ObjectProvider.Configure(conf => conf.For<JsonSerializer>().Use(jsonSerializer));

            var traderToken = ApiServiceBuilder<IUserService>.Build("http://localhost:8080")
                                                            .Create()
                                                            .Login(new UserCredentials()
                                                            {
                                                                Username = "EQ-Trader",
                                                                Password = "password"
                                                            }).Result;

            _tradeClient = ApiServiceBuilder<ITradeService>.Build("http://localhost:8080")
                                             .AddAuthorizationHeader(() => traderToken.Digest)
                                             .Create();

            _priceClient = AppCore.Instance.GetService<IPriceService>();

            Task.Delay(1000);
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            await _app.StopAsync();
        }

        [Test]
        public async Task TestService()
        {
            var noAuthClient = ApiServiceBuilder<ITradeService>.Build("http://localhost:8080").Create();

            var eqTraderClient = _tradeClient;

            var traderToken = ApiServiceBuilder<IUserService>.Build("http://localhost:8080")
                                                .Create()
                                                .Login(new UserCredentials()
                                                {
                                                    Username = "Trader",
                                                    Password = "password"
                                                }).Result;

            var tradeClient = ApiServiceBuilder<ITradeService>.Build("http://localhost:8080")
                                             .AddAuthorizationHeader(() => traderToken.Digest)
                                             .Create();

            Assert.CatchAsync(async () =>
            {
                await noAuthClient.GetAllTrades();
            }, "Response status code does not indicate success: 403 (Forbidden).");

            var trades = await tradeClient.GetAllTrades();

            var request = new TradeCreationRequest()
            {
                Counterparty = "XXX",
                Asset = "XXX",
                Price = 100.0,
                Volume = 100,
                Way = TradeWay.Buy
            };

            Assert.CatchAsync(async () =>
            {
                await tradeClient.CreateTrade(request);
            }, "Response status code does not indicate success: 403 (Forbidden).");

            var createdTradeResult = await eqTraderClient.CreateTrade(request);

            Assert.IsNotNull(createdTradeResult);

            var createdTrade = await eqTraderClient.GetTradeById(createdTradeResult.TradeId);

            Assert.IsNotNull(createdTrade);
            Assert.AreEqual(request.Counterparty, createdTrade.Counterparty);
            Assert.AreEqual(request.Asset, createdTrade.Asset);


            var client = new HttpClient();
            var jsonError = await client.GetAsync("http://localhost:8080/api/v1/trade");
            var error = JsonConvert.DeserializeObject<ServiceErrorModel>(await jsonError.Content.ReadAsStringAsync());

            Assert.IsNotNull(error);
            Assert.AreEqual("User must have role [Trader]", error.Details);
            Assert.AreEqual("Unauthorized", error.Reason);

            await Task.Delay(500);

        }

        class GraphQlServiceException : Exception
        {
            public GraphQlServiceException()
            {
            }

            public GraphQlServiceException(string message) : base(message)
            {
            }

            public GraphQlServiceException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected GraphQlServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }

        class TestHttpHandler : HttpMessageHandler
        {
            protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var client = new HttpClient();
                var response =  client.SendAsync(request).Result;

                if (!response.IsSuccessStatusCode)
                {
                    throw new GraphQlServiceException(await response.Content.ReadAsStringAsync());
                }

                return response;
            }
        }

        //refacto - working override of HttpMessageHandler
        [Test]
        public async Task TestGraphQL()
        {
            var options = new GraphQLClientOptions()
            {
                JsonSerializerSettings = AppCore.Instance.Get<JsonSerializerSettings>()
                //HttpMessageHandler = new TestHttpHandler()
            };

            var graphQLClient = new GraphQLClient("http://localhost:8080/api/v1/graphql/trades", options);

            var query = new GraphQLRequest()
            {
                Query = "{trades {id,date,counterparty}}",
                OperationName= "trades"
            };

            var faultyQuery = new GraphQLRequest()
            {
                Query = "{trades {nothing}}",
                OperationName = "trades"
            };

            var queryResult = await graphQLClient.PostAsync(query);
            var results = queryResult.GetDataFieldAs<List<TradeDto>>("trades");

            Assert.IsTrue(results.Count > 0);

            var trades = await _tradeClient.GetAllTrades();

            var tradeAndPricesQuery = new GraphQLRequest()
            {
                Query = @"query FetchTradeAndPrices($tradeId: String) {
                 	prices(tradeId:$tradeId) 
                    {
						value,
						date
					}
                }",
                OperationName = "FetchTradeAndPrices",
                Variables = new
                {
                    tradeId = trades.First().Id
                }
            };

      
            queryResult = await graphQLClient.PostAsync(tradeAndPricesQuery);

            Assert.IsNotNull(queryResult.Data.prices);

            //Assert.CatchAsync(async() =>
            //{
            //   var resultss = await graphQLClient.PostAsync(faultyQuery);
            //});

        }

        [Test]
        public async Task TestMethodCache()
        {
            var prices = (await _priceClient.GetAllPrices()).ToList();

            await _priceClient.CreatePrice(new Price(Guid.NewGuid(), "XXX", 100.0, DateTime.Now));

            var actualPrices = (await _priceClient.GetAllPrices()).ToList();

            Assert.AreEqual(prices.Count(), actualPrices.Count());

            await Task.Delay(1000);

            prices = (await _priceClient.GetAllPrices()).ToList();

            Assert.Greater(prices.Count(), actualPrices.Count());

        }

        [Test]
        public async Task TestSignalRClient()
        {
            var query = new PriceRequest((p) => p.Value > 50);

            var connection = new HubConnectionBuilder()
                 .WithQuery(_configuration.Root["urls"], query)
                 .Build();

            await connection.StartAsync();

            connection.On<Price>(TradeServiceReferential.OnPriceChanged, (p) =>
             {
                 Assert.AreEqual("stock3", p.Asset);
                 Assert.AreEqual(60, p.Value);
             });

            await connection.InvokeAsync(TradeServiceReferential.RaisePriceChanged, new Price(Guid.NewGuid(), "stock1", 20, DateTime.Now));
            await connection.InvokeAsync(TradeServiceReferential.RaisePriceChanged, new Price(Guid.NewGuid(), "stock2", 30, DateTime.Now));
            await connection.InvokeAsync(TradeServiceReferential.RaisePriceChanged, new Price(Guid.NewGuid(), "stock3", 60, DateTime.Now));

            await Task.Delay(500);

            await connection.DisposeAsync();
        }

        [Test]
        public async Task TestSignalRClientResilientConnection()
        {
            var query = new PriceRequest((p) => p.Value > 50);

            var service = SignalRServiceBuilder<Price, PriceRequest>
                            .Create()
                            .Build(query);

            var disposable = service.Connect(Scheduler.Default, 0)
                    .Subscribe(p =>
                    {
                        Assert.AreEqual("stock3", p.Asset);
                        Assert.AreEqual(60, p.Value);
                    });

            await Task.Delay(100);

            while (service.Current.CurrentState != ConnectionStatus.Connected)
            {
                await Task.Delay(500);
            }

            await service.Current.Proxy.InvokeAsync(TradeServiceReferential.RaisePriceChanged, new Price(Guid.NewGuid(), "stock1", 20, DateTime.Now));
            await service.Current.Proxy.InvokeAsync(TradeServiceReferential.RaisePriceChanged, new Price(Guid.NewGuid(), "stock2", 30, DateTime.Now));
            await service.Current.Proxy.InvokeAsync(TradeServiceReferential.RaisePriceChanged, new Price(Guid.NewGuid(), "stock3", 60, DateTime.Now));


            await Task.Delay(500);

            disposable.Dispose();
            service.Disconnect();
        }
    }
}
