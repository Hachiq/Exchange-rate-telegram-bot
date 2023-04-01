using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyBot
{
    internal class ExchangeRateData
    {
        [JsonProperty("rates")]
        public Dictionary<string, decimal> Rates { get; set; }
    }
}
