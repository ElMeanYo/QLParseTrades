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

            symbol.Calculate();

            // As per the example provided in instructions.txt
            Assert.IsTrue(symbol.MaxPrice == 18);
            Assert.IsTrue(symbol.MaxTimeGap == 10000);
            Assert.IsTrue(symbol.TotalVolume == 25);
            Assert.IsTrue(symbol.WeightedAveragePrice == 15);

        }

        // TODO: Add more tests for different scenarios as needed.

    }
}
