using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Xml;

namespace ConsoleApp22
{
    public class NBGRates
    {
        private readonly string _serviceUrl = "http://www.nbg.ge/rss.php";

        public NBGRates()
        {

        }
        public NBGRates(string serviceUrl)
        {
            if (string.IsNullOrEmpty(serviceUrl))
            {
                throw new Exception("NBG service URL is not provided!");
            }

            _serviceUrl = serviceUrl;
        }

        public List<CurrencyRate> GetRates(DateTime date)
        {
            return ReadRss(date);
        }

        private List<CurrencyRate> ReadRss(DateTime date)
        {
            using (var http = new HttpClient() { BaseAddress = new Uri(_serviceUrl) })
            {
                var xml = http.GetStringAsync($"?date={date.ToString("yyyy-MM-dd")}").Result;

                xml = Regex.Match(xml, "<!\\[CDATA\\[(.*)\\]\\]", RegexOptions.Singleline).Groups[1].Value;
                xml = Regex.Replace(xml, "<td><img.*</td>", string.Empty);

                var xmlDoc = new XmlDocument();

                xmlDoc.LoadXml(xml);

                var currencyRecords = xmlDoc.GetElementsByTagName("tr");

                var currencyList = new List<CurrencyRate>();

                foreach (XmlNode item in currencyRecords)
                {
                    currencyList.Add(new CurrencyRate
                    {
                        Code = item.ChildNodes[0].InnerText.Trim(),
                        Name = item.ChildNodes[1].InnerText.Trim(),
                        Rate = decimal.Parse(item.ChildNodes[2].InnerText.Trim())
                    });
                }
                return currencyList;
            }
        }
    }
}
