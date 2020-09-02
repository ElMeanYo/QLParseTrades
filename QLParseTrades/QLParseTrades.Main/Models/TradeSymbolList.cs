using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace QLParseTrades.Main.Models
{
    /// <summary>
    /// Build, process and store a list of symbols, thier trades and summary values.
    /// </summary>
    public class TradeSymbolList
    {
        public SortedDictionary<string, TradeSymbol> Symbols;

        #region Public Methods

        /// <summary>
        /// Load trades from a comma-delimited file
        /// </summary>
        public void Load(string fileName)
        {
            Symbols = new SortedDictionary<string, TradeSymbol>();

            using (var reader = new StreamReader(fileName))
            {
                // Iterate through rows until the end of the file
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');   // Split by commas

                    if (string.IsNullOrEmpty(values[1])) continue;  // Ensure there is a symbol

                    var tradeSymbol = GetSymbol(values[1]);         // Get/Create the trade symbol

                    if (tradeSymbol != null) 
                        tradeSymbol.AddTrade(values);  // Add the trade
                }
            }
        }

        /// <summary>
        /// Calculate symbol totals from the trades
        /// </summary>
        public void Calculate()
        {
            foreach (var symbol in Symbols)
                symbol.Value.Calculate();
        }

        /// <summary>
        /// Save the symbols to a comma-delimited file
        /// </summary>
        public void Save(string fileName)
        {
            if (!Symbols.Any()) return;

            var sb = new StringBuilder();

            foreach (var s in Symbols.Values)
                sb.AppendLine($@"{s.Symbol},{s.MaxTimeGap},{s.TotalVolume},{s.WeightedAveragePrice},{s.MaxPrice}");

            // This overwrites.  You could provide a parameter to append instead of overwrite (AppendAllLines).
            File.WriteAllText(fileName, sb.ToString());
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Get/Add a symbol by its code
        /// </summary>
        private TradeSymbol GetSymbol(string symbol)
        {
            if (!Symbols.TryGetValue(symbol, out var tradeSymbol))
            {
                tradeSymbol = new TradeSymbol(symbol);
                Symbols.Add(symbol, tradeSymbol);
            }
            return tradeSymbol;
        }

        #endregion

    }
}
