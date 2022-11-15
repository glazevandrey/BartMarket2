using System;

namespace BartMarket
{
    public class StatModel
    {
        public DateTime Date { get; set; }
        public string ElapsedLite { get; set; }
        public string ElapsedFull { get; set; }

        public bool Success { get; set; } = true;
        public string Error { get; set; }
        public int Count { get; set; }



    }
}
