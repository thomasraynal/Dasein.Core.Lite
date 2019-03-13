﻿using Dasein.Core.Lite.Demo.Server;
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
using System.Net.Http.Headers;

namespace Dasein.Core.Lite.Tests
{

    [TestFixture]
    public class TestE2E
    {
        private Host<TradeServiceStartup> _host;
        private IWebHost _app;
        private IServiceConfiguration _configuration;
        private static TradeServiceToken _traderToken;
        private ITradeService _tradeClient;
        private IPriceService _priceClient;

        //Use test server

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

            _traderToken = ApiServiceBuilder<IUserService>.Build("http://localhost:8080")
                                                            .Create()
                                                            .Login(new Credentials()
                                                            {
                                                                Username = "EQ-Trader",
                                                                Password = "password"
                                                            }).Result;

            _tradeClient = ApiServiceBuilder<ITradeService>.Build("http://localhost:8080")
                                             .AddAuthorizationHeader(() => _traderToken.Digest)
                                             .Create();

            _priceClient = AppCore.Instance.GetService<IPriceService>();

            Task.Delay(1000);

            var publisher = AppCore.Instance.Get<IPublisher>();
             publisher.Stop().Wait();

        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            await Task.Delay(3000);

            await _app.StopAsync();
        }

        [Test]
        public async Task ShouldTestService()
        {
            var noAuthClient = ApiServiceBuilder<ITradeService>.Build("http://localhost:8080").Create();

            var eqTraderClient = _tradeClient;

            var traderToken = ApiServiceBuilder<IUserService>.Build("http://localhost:8080")
                                                .Create()
                                                .Login(new Credentials()
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

            Assert.IsNotNull(error, "erroe should be send to the client in json format");
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

        class TestHttpHandler : DelegatingHandler
        {
            protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _traderToken.Digest);

                var response = await base.SendAsync(request, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    throw new GraphQlServiceException(await response.Content.ReadAsStringAsync());
                }

                return response;
            }
        }

     //   [Test]
     //   public async Task ShouldTestGraphQL()
     //   {
     //       var options = new GraphQLClientOptions()
     //       {
     //           JsonSerializerSettings = AppCore.Instance.Get<JsonSerializerSettings>(),
     //           HttpMessageHandler = new TestHttpHandler()
     //           {
     //               InnerHandler = new HttpClientHandler()
     //   }

     //       };

     //       var graphQLClient = new GraphQLClient("http://localhost:8080/api/v1/graphql/trades", options);

     //       var query = new GraphQLRequest()
     //       {
     //           Query = "{trades {id,date,counterparty}}",
     //           OperationName = "trades"
     //       };

     //       var faultyQuery = new GraphQLRequest()
     //       {
     //           Query = "{trades {nothing}}",
     //           OperationName = "trades"
     //       };

     //       var queryResult = await graphQLClient.PostAsync(query);
     //       var results = queryResult.GetDataFieldAs<List<TradeDto>>("trades");

     //       Assert.IsTrue(results.Count > 0);

     //       var trades = await _tradeClient.GetAllTrades();

     //       var tradeAndPricesQuery = new GraphQLRequest()
     //       {
     //           Query = @"query FetchTradeAndPrices($tradeId: String) {
     //            	prices(tradeId:$tradeId) 
     //               {
					//	value,
					//	date
					//}
     //           }",
     //           OperationName = "FetchTradeAndPrices",
     //           Variables = new
     //           {
     //               tradeId = trades.First().Id
     //           }
     //       };


     //       queryResult = await graphQLClient.PostAsync(tradeAndPricesQuery);

     //       Assert.IsNotNull(queryResult.Data.prices);

     //       Assert.CatchAsync(async () =>
     //       {
     //           var resultss = await graphQLClient.PostAsync(faultyQuery);
     //       });

     //   }

        [Test]
        public async Task ShouldValidateServiceRequest()
        {

            var wrongRequest = new TradeCreationRequest()
            {
                Counterparty = null,
                Asset = "XXXXXX",
                Price = 100.0,
                Volume = 100,
                Way = TradeWay.Buy
            };

            var correctRequest = new TradeCreationRequest()
            {
                Counterparty = "XXXXXX",
                Asset = "XXXXXX",
                Price = 100.0,
                Volume = 100,
                Way = TradeWay.Buy
            };

            Assert.CatchAsync(async () =>
            {
                var error = await _tradeClient.CreateTrade(wrongRequest);
            }, "'Counterparty should be set'");

            var result = await _tradeClient.CreateTrade(correctRequest);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.TradeId);

            await Task.Delay(1000);
        }

        [Test]
        public async Task ShouldTestResponseCache()
        {
            //response cache do not work with an auth header
            var tradeClient= ApiServiceBuilder<ITradeService>.Build("http://localhost:8080").Create();

            var initialTrades = await tradeClient.GetAllTradesViaCache();

            var request = new TradeCreationRequest()
            {
                Counterparty = "XXXXX",
                Asset = "XXXXX",
                Price = 100.0,
                Volume = 100,
                Way = TradeWay.Buy
            };

            var createdTradeResult = await _tradeClient.CreateTrade(request);

            await Task.Delay(1000);

            var cachedTrades = await tradeClient.GetAllTradesViaCache();
            var notCachedTrades = await _tradeClient.GetAllTrades();

            Assert.AreEqual(initialTrades.Count(), cachedTrades.Count());
            Assert.AreEqual(notCachedTrades.Count(), cachedTrades.Count() + 1);
            
       
            var tradeClientNoCache = ApiServiceBuilder<ITradeService>.Build("http://localhost:8080")
                          .AddHeader("Cache-Control", () => "no-cache ")
                          .Create();

            //wait for the cache to invalidate
            await Task.Delay(3000);

            cachedTrades = await tradeClientNoCache.GetAllTradesViaCache();

            Assert.AreEqual(notCachedTrades.Count(), cachedTrades.Count());

        }

        //refacto - failed on CI... to check...
        //[Test]
        //public async Task ShouldTestMethodCache()
        //{

        //    var prices = (await _priceClient.GetAllPrices()).ToList();

        //    await _priceClient.CreatePrice(new Price(Guid.NewGuid(), "XXX", 100.0, DateTime.Now));

        //    var actualPrices = (await _priceClient.GetAllPrices()).ToList();

        //    Assert.AreEqual(prices.Count(), actualPrices.Count());

        //    await Task.Delay(1000);

        //    prices = (await _priceClient.GetAllPrices()).ToList();

        //    Assert.Greater(prices.Count(), actualPrices.Count());
            
        //}

        [Test]
        public async Task ShouldTestSignalRClientResilientConnection()
        {
            var query = new PriceRequest((p) => p.Value > 50000);

            var service = SignalRServiceBuilder<Price, PriceRequest>
                            .Create()
                            .Build(query, (opts) =>
                            {
                                opts.AccessTokenProvider = () => Task.FromResult(_traderToken.Digest);
                            });

            var disposable = service.Connect(Scheduler.Default, 0)
                    .Subscribe(p =>
                    {
                        Assert.AreEqual("stock3", p.Asset);
                        Assert.AreEqual(60000, p.Value);
                    });

            await Task.Delay(100);

            while (service.Current.CurrentState != ConnectionStatus.Connected)
            {
                await Task.Delay(500);
            }

            await service.Current.Proxy.RaiseChange(new Price(Guid.NewGuid(), "stock1", 20000, DateTime.Now));
            await service.Current.Proxy.RaiseChange(new Price(Guid.NewGuid(), "stock2", 30000, DateTime.Now));
            await service.Current.Proxy.RaiseChange(new Price(Guid.NewGuid(), "stock3", 60000, DateTime.Now));


            await Task.Delay(500);

            disposable.Dispose();
            service.Disconnect();
        }

        [Test]
        public async Task ShouldTestMiddleware()
        {
            var trades = await _tradeClient.GetAllTradesViaMiddleware();
            Assert.IsTrue(trades.All(t => t.Asset.Contains(TradeServiceProxy.middlewareKey)));
        }

    }
}
