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
        #region Properties

        public List<Trade> Trades;  // List of trades for this symbol

        public string Symbol { get; set; }

        public double MaxTimeGap { get; set; }

        public int Volume { get; set; }

        public int MaxPrice { get; set; }

        public int WeightedAveragePrice { get; set; }

        #endregion

        #region Constructor

        public TradeSymbol(string symbol)
        {
            Symbol = symbol;
            Trades = new List<Trade>();
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

            Trades.Add(new Trade(timeStamp, quantity, price));
        }

        /// <summary>
        /// Calculate summary values for this symbol
        /// </summary>
        public void Calculate()
        {
            if (!Trades.Any()) return;
            MaxTimeGap = CalcMaxTimeGap();
            Volume = CalcTotalVolume();
            MaxPrice = CalcMaxPrice();
            WeightedAveragePrice = CalcWeightedAveragePrice();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Maximum time gap between all trades on this symbol.
        /// </summary>
        private double CalcMaxTimeGap()
        {
            // Sort unique timestamps
            var timeStamps = Trades.Select(t => t.TimeStamp).Distinct().OrderBy(t => t);
            double maxTimeGap = 0;
            double prevStamp = timeStamps.First();

            foreach (var stamp in timeStamps)
            {
                var gap = stamp - prevStamp;
                if (gap > maxTimeGap) 
                    maxTimeGap = gap;
                prevStamp = stamp;
            }

            return maxTimeGap;
        }

        /// <summary>
        /// Total volume (sum of quantities) on this symbol.
        /// </summary>
        private int CalcTotalVolume()
        {
            return Trades.Sum(t => t.Quantity);
        }

        /// <summary>
        /// Maximum price of all trades on this symbol.
        /// </summary>
        /// <remarks>
        /// Is this supposed to be Max(Price * Quantity)? Instructions unclear. Assuming Max(Price).
        /// </remarks>
        private int CalcMaxPrice()
        {
            return Trades.Max(t => t.Price);
        }

        /// <summary>
        /// Weighted Average for all trades on this symbol.
        /// </summary>
        private int CalcWeightedAveragePrice()
        {
            var totalQuantities = Trades.Sum(t => t.Quantity); // Do the quantity sum only once
            if (totalQuantities != 0) // Divide by zero check
                return Trades.Sum(t => (t.Price * t.Quantity)) / totalQuantities;
            else
                return 0;
        }

        #endregion
    }
}
