using System;
using System.Collections.Generic;
using System.Text;

namespace QLParseTrades.Main.Models
{
    public class Trade
    {
        public Trade(double timeStamp, int quantity, int price)
        {
            TimeStamp = timeStamp;
            Quantity = quantity;
            Price = price;
        }

        public double TimeStamp { get; set; }

        public int Quantity { get; set; }

        public int Price { get; set; }
    }
}
