using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using Newtonsoft.Json;

namespace CurrencyBot
{
    public class ExchangeBot
    {
        private readonly TelegramBotClient _bot;

        public ExchangeBot(string botToken)
        {
            _bot = new TelegramBotClient(botToken);
        }

        public async Task StartAsync()
        {
            _bot.OnMessage += BotOnMessageReceived;
            _bot.StartReceiving();
            await Task.Delay(-1);
        }

        private async void BotOnMessageReceived(object sender, MessageEventArgs e)
        {
            if (e.Message.Type != MessageType.Text)
                return;

            var input = e.Message.Text.Split(' ');

            if (input.Length != 4 || !decimal.TryParse(input[0], out decimal amount))
            {
                await _bot.SendTextMessageAsync(e.Message.Chat.Id, "Invalid input. Please enter a valid amount and currency codes (e.g. '10 USD to EUR').");
                return;
            }

            var baseCurrency = input[1].ToUpper();
            var targetCurrency = input[3].ToUpper();

            var rate = await GetExchangeRateAsync(baseCurrency, targetCurrency);
            var convertedAmount = amount * rate;

            var message = $"{amount} {baseCurrency} is {convertedAmount} {targetCurrency}.";
            await _bot.SendTextMessageAsync(e.Message.Chat.Id, message);
        }

        public async Task<decimal> GetExchangeRateAsync(string baseCurrency, string targetCurrency)
        {
            using (var httpClient = new HttpClient())
            {
                string apiKey = "rO3mwqaZXPlz05oSDHE2Iv4Bz2n7SypS";
                var response = await httpClient.GetAsync($"https://api.apilayer.com/exchangerates_data/latest?base={baseCurrency}&symbols={targetCurrency}&apikey={apiKey}");
                
                var content = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ExchangeRateData>(content);

                if (data != null && data.Rates.TryGetValue(targetCurrency, out decimal rate))
                {
                    return rate;
                }

                throw new Exception($"Failed to get exchange rate for {baseCurrency}/{targetCurrency}.");
            }
        }
    }
}
