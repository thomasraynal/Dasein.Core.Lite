using Dasein.Core.Lite.Demo.Desktop.Infrastructure;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;
using Dasein.Core.Lite.Shared;
using Dasein.Core.Lite.Demo.Shared;

namespace Dasein.Core.Lite.Demo.Desktop
{
    public class MainViewModel : ViewModelBase, ICanLog
    {
        private ITradeService _tradeService;
        private IUserService _authService;
        private TradeServiceToken _userToken;
        private ObservableCollection<TradeViewModel> _trades;
        private ObservableCollection<String> _events;
        private ISignalRService<Price, PriceRequest> _priceEventService;
        private ISignalRService<TradeEvent, TradeEventRequest> _tradeEventService;


        public ObservableCollection<TradeViewModel> Trades
        {
            get
            {
                return _trades;
            }
            set
            {
                _trades = value;
                OnPropertyChanged(nameof(Trades));
            }
        }

        public ObservableCollection<String> Events
        {
            get
            {
                return _events;
            }
            set
            {
                _events = value;
                OnPropertyChanged(nameof(Events));
            }
        }

        public ICommand MakeTrade { get; private set; }

        public ICommand MakeTradeEvent { get; private set; }

        private String MakeEvent(String name, String sender, object subject, object arg)
        {
            return $"[{name}] - [{sender}] - [{subject} - {arg}]";
        }


        public void Notify(string @event)
        {
            Application.Current.Dispatcher.Invoke(() => Events.Add(@event));
        }

        public MainViewModel()
        {
  
            Events = new ObservableCollection<string>();

            _authService = ApiServiceBuilder<IUserService>
                                                  .Build(TradeServiceReferential.ServiceHost)
                                                  .Create();

            _userToken = _authService.Login(new Credentials()
            {
                Username = "EQ-Trader",
                Password = "password"
            }).Result;

            AppCore.Instance.ObjectProvider.Configure(config => config.For<TradeServiceToken>().Use(_userToken));

            _tradeService = ApiServiceBuilder<ITradeService>.Build(TradeServiceReferential.ServiceHost)
                                                            .AddAuthorizationHeader(() => _userToken.Digest)
                                                            .Create();

            _priceEventService = SignalRServiceBuilder<Price, PriceRequest>
                                                        .Create()
                                                        .Build(new PriceRequest((p) => true));                                    

            _priceEventService
                .Connect(Scheduler.Default, 1000)
                .Subscribe((priceEvent) =>
                {
                    foreach (var trade in Trades.Where(trade => trade.Asset == priceEvent.Asset).ToList())
                    {
                        Application.Current.Dispatcher.Invoke(() => trade.CurrentPrice = priceEvent.Value);
                    }

                    var ev = MakeEvent("PRICE", "PRICE", priceEvent.Asset, priceEvent.Value);
                    this.Notify(ev);
                });


            _tradeEventService = SignalRServiceBuilder<TradeEvent, TradeEventRequest>
                                                        .Create()
                                                        .Build(new TradeEventRequest((p) => true));
                                                        //.AddAuthorizationHeader(() => _userToken.Digest)

            _tradeEventService
                .Connect(Scheduler.Default, 500)
                .Subscribe((tradeEvent) =>
                {

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        var trade = Trades.FirstOrDefault(t => t.Id == tradeEvent.TradeId);
                        if (null == trade) return;
                        trade.Status = tradeEvent.Status;
                    });

                    this.Notify(MakeEvent("TRADE", _tradeEventService.Current.Endpoint, tradeEvent.TradeId, tradeEvent.Status));

                });


            Trades = new ObservableCollection<TradeViewModel>();

            MakeTrade = new Command(async () =>
            {
                var asset = TradeServiceReferential.Assets.Random();
                var counterparty = TradeServiceReferential.Counterparties.Random();

                var rand = new Random();

                var request = new TradeCreationRequest()
                {
                    Counterparty = counterparty,
                    Asset = asset.Name,
                    Price = asset.Price,
                    Volume = rand.Next(1, 50),
                    Way = TradeWay.Buy
                };

                var tradeEvent = await _tradeService.CreateTrade(request);

                var trade = await _tradeService.GetTradeById(tradeEvent.TradeId);

                Application.Current.Dispatcher.Invoke(() => Trades.Add(new TradeViewModel(trade)));
            });

            Application.Current.Dispatcher.Invoke(async () =>
            {
                var trades = await _tradeService.GetAllTrades();

                foreach (var trade in trades)
                {
                    var vm = new TradeViewModel(trade);
                    Trades.Add(vm);
                }

            });

        }


    }
}