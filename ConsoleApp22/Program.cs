using System;
using System.Linq;

namespace ConsoleApp22
{
    class Program
    {
        static void Main(string[] args)
        {
            NBGRates nBGRates = new NBGRates();
           var rates = nBGRates.GetRates(DateTime.Now);
            
            foreach (var item in rates)
            {
                Console.WriteLine(item.Code + "/" + item.Name + "/" + item.Rate.ToString());
            }
            Console.WriteLine("***");
            Console.WriteLine(ExchangeRate("USD", "GEL"));
            Console.ReadKey();
        }


        public static decimal ExchangeRate(String from, String to)
        {
            NBGRates nBGRates = new NBGRates();
            var rates = nBGRates.GetRates(DateTime.Now);
            CurrencyRate fromRate = new CurrencyRate();
            CurrencyRate toRate =new CurrencyRate();

            if (from== "GEL")
            {
                fromRate.Rate = 1;
                toRate = rates.Where(x => x.Code.Contains(to)).FirstOrDefault();
            }
            if (to == "GEL")
            {
                toRate.Rate = 1;
                fromRate = rates.Where(x => x.Code.Contains(from)).FirstOrDefault();
            }
            return fromRate.Rate * toRate.Rate;
                
        }
    }
}
