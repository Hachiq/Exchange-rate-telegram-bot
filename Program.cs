namespace CurrencyBot
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var bot = new ExchangeBot("5890810975:AAGpAc2fib0bNRPcgTbcVZCLfMtS7_PnSOk");
            await bot.StartAsync();
            Console.ReadLine();
        }
    }

}