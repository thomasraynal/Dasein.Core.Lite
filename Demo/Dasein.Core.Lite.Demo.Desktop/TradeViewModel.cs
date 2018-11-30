using Dasein.Core.Lite.Demo.Desktop.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dasein.Core.Service.Demo.Shared;
using Dasein.Core.Lite.Demo.Shared;

namespace Dasein.Core.Lite.Demo.Desktop
{
    public class TradeViewModel : ViewModelBase, ITrade
    {
        private ITrade _trade;

        public TradeViewModel(ITrade trade)
        {
            _trade = trade;
            _currentPrice = -1;
        }

        private double _currentPrice;
        public double CurrentPrice
        {
            get
            {
                return _currentPrice;
            }
            set
            {
                _currentPrice = value;
                this.OnPropertyChanged("CurrentPrice");
            }
        }

        public Guid Id => _trade.Id;

        public DateTime Date => _trade.Date;

        public string Counterparty => _trade.Counterparty;

        public string Asset => _trade.Asset;

        public TradeStatus Status
        {
            get
            {
                return _trade.Status;
            }

            set
            {
                _trade.Status = value;
                this.OnPropertyChanged("Status");
            }
        }

        public TradeWay Way => _trade.Way;

        public double PriceOnTransaction => _trade.PriceOnTransaction;

        public double Volume => _trade.Volume;

    }
}