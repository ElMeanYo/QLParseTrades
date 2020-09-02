using System;
using System.IO;
using QLParseTrades.Main.Logging;
using QLParseTrades.Main.Models;

namespace QLParseTrades.Main
{
    /// <summary>
    /// Parse and calculate a list of trades.
    /// </summary>
    public class Engine
    {
        /// <summary>
        /// Event to subscribe to for basic logging.
        /// </summary>
        public event EventHandler<LogEventArgs> LogEvent;  

        /// <summary>
        /// Process a list of trades and save them group by symbol, with calculated summary values.
        /// </summary>
        /// <param name="inputFilename">Input comma-delimited file (csv).</param>
        /// <param name="outputFilename">Output comma-delimited file (csv).</param>
        public void Run(string inputFilename, string outputFilename)
        {
            try
            {
                
                if (!File.Exists(inputFilename))        // Throw error if input file not found
                    throw new FileNotFoundException($@"File not found: {inputFilename}");

                var tradeSymbols = new TradeSymbolList();

                tradeSymbols.Load(inputFilename);       // Load from file
                tradeSymbols.Calculate();               // Calculate
                tradeSymbols.Save(outputFilename);      // Save to file

            }
            catch (Exception ex)
            {
                RaiseMessage("A Fatal Error occurred.", LogEventLevel.Fatal, ex);
                throw;
            }
        }

        // Raise a message event which will be handled by the consumer
        // TODO: This event could be raised to log errors or data problems rather than just for the top-level exception handling.
        internal void RaiseMessage(string msg, LogEventLevel level, Exception ex = null)
        {
            LogEvent?.Invoke(null, new LogEventArgs(msg, level, ex));
        }
    }
}
