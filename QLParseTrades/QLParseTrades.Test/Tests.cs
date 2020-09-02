using Microsoft.VisualStudio.TestTools.UnitTesting;
using QLParseTrades.Main;
using QLParseTrades.Main.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace QLParseTrades.Test
{
    [TestClass]
    public class Tests
    {
        /// <summary>
        /// Run a basic integration test with hard-coded input and output files.
        /// </summary>
        /// <remarks>
        /// No indication was made in the instructions as to whether a UI should be provided. Instead, this test can be used to run the excercise.
        /// NOTE: This test will fail if you have the input or output file open.
        /// </remarks>
        [TestMethod]
        public void TestTrades1()
        {
            var workingDir = Environment.CurrentDirectory;
            var projectDir = Directory.GetParent(workingDir).Parent.Parent.FullName;
            var inputFile = $@"{projectDir}\Data\input.csv";
            var outputFile = $@"{projectDir}\Data\output.csv";

            var tradesEngine = new Engine();

            tradesEngine.Run(inputFile, outputFile);

            Assert.IsTrue(File.Exists(outputFile), "Output File did not save.");
        }

        /// <summary>
        /// A unit test for calculations on a single symbol.
        /// </summary>
        /// <remarks>
        /// This could be broken out into tests for each calculation.
        /// </remarks>
        [TestMethod]
        public void TestSymbolCalc1()
        {
            var symbol = new TradeSymbol("aaa");
            Assert.IsTrue(symbol.Symbol == "aaa");

            symbol.AddTrade(new string[] { "10000000", "aaa", "20", "18" });
            symbol.AddTrade(new string[] { "10010000", "aaa", "5", "7" });

            Assert.IsTrue(symbol.Trades.Count == 2);

            symbol.Calculate();

            // As per the example provided in instructions.txt
            Assert.IsTrue(symbol.MaxPrice == 18);
            Assert.IsTrue(symbol.MaxTimeGap == 10000);
            Assert.IsTrue(symbol.Volume == 25);
            Assert.IsTrue(symbol.WeightedAveragePrice == 15);

        }

        /// <summary>
        /// Test multiple trades with bad data.
        /// </summary>
        [TestMethod]
        public void TestSymbolCalc2()
        {
            var symbol = new TradeSymbol("aaa");
            Assert.IsTrue(symbol.Symbol == "aaa");

            symbol.AddTrade(new string[] { "10000000", "aaa", "x", "y" });      // Test bad data
            symbol.AddTrade(new string[] { "10010000", "aaa", "12", "45" });
            symbol.AddTrade(new string[] { "10210000", "aaa", "3", "9" });
            symbol.AddTrade(new string[] { "10210000", "aaa", "01", "2.2" });   // Test bad data

            Assert.IsTrue(symbol.Trades.Count == 2); // Two of the trades are good

            symbol.Calculate();

            Assert.IsTrue(symbol.MaxPrice == 45);
            Assert.IsTrue(symbol.MaxTimeGap == 200000);
            Assert.IsTrue(symbol.Volume == 15);
            Assert.IsTrue(symbol.WeightedAveragePrice == 37);

        }

        /// <summary>
        /// Test a single trade
        /// </summary>
        [TestMethod]
        public void TestSymbolCalc3()
        {
            var symbol = new TradeSymbol("aaa");
            Assert.IsTrue(symbol.Symbol == "aaa");

            symbol.AddTrade(new string[] { "10010000", "aaa", "4", "51" });

            Assert.IsTrue(symbol.Trades.Count == 1); 

            symbol.Calculate();

            Assert.IsTrue(symbol.MaxPrice == 51);
            Assert.IsTrue(symbol.MaxTimeGap == 0);
            Assert.IsTrue(symbol.Volume == 4);
            Assert.IsTrue(symbol.WeightedAveragePrice == 51);

        }

        // TODO: Add more tests for different scenarios as needed.

    }
}
