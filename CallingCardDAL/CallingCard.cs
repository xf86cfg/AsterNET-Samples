using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallingCardDAL
{
    public class CallingCard
    {
        public string CardNumber { get; set; }
        public double CardBalance { get; set; }

        public static CallingCard GetCardByNum(string num)
        {
            return new CallingCard()
            {
                CardNumber = num,
                CardBalance = 10.00
            };
        }
    }
}
