using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace QLParseTrades.Main.Models
{
    /// <summary>
    /// Trade symbol with trades and summary values.
    /// </summary>
    public class TradeSymbol
    {
        private List<Trade> _trades;

        #region Properties

        public string Symbol { get; set; }

        public double MaxTimeGap { get; set; }

        public int TotalVolume { get; set; }

        public int MaxPrice { get; set; }

        public int WeightedAveragePrice { get; set; }

        #endregion

        #region Constructor

        public TradeSymbol(string symbol)
        {
            Symbol = symbol;
            _trades = new List<Trade>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add a trade from an array of string values
        /// </summary>
        public void AddTrade(string[] values)
        {
            if (values.Length < 4) return;

            // Skip this trade if any columns are not valid
            if (!double.TryParse(values[0], out double timeStamp)) return;  // Time Stamp
            if (!int.TryParse(values[2], out int quantity)) return;         // Quantity
            if (!int.TryParse(values[3], out int price)) return;            // Price

            _trades.Add(new Trade(timeStamp, quantity, price));
        }

        /// <summary>
        /// Calculate summary values for this symbol
        /// </summary>
        public void Calculate()
        {
            MaxTimeGap = CalcMaxTimeGap();
            TotalVolume = CalcTotalVolume();
            MaxPrice = CalcMaxPrice();
            WeightedAveragePrice = CalcWeightedAveragePrice();
        }

        #endregion

        #region Private Methods

        private double CalcMaxTimeGap()
        {
            var timeStamps = _trades.Select(t => t.TimeStamp).Distinct().OrderBy(t => t);
            double maxTimeGap = 0;
            double prevStamp = timeStamps.First();

            foreach (var stamp in timeStamps)
            {
                var gap = stamp - prevStamp;
                if (gap > maxTimeGap) maxTimeGap = gap;
                prevStamp = stamp;
            }

            return maxTimeGap;
        }

        private int CalcTotalVolume()
        {
            return _trades.Sum(t => t.Price);
        }

        private int CalcMaxPrice()
        {
            return _trades.Max(t => t.Price);
        }

        private int CalcWeightedAveragePrice()
        {
            if (!_trades.Any()) return 0;

            if (_trades.Sum(t => t.Quantity) != 0) // Div0 check
                return _trades.Sum(t => (t.Price * t.Quantity)) / _trades.Sum(t => t.Quantity);
            else
                return 0;
        }

        #endregion
    }
}
