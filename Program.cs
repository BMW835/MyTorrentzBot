using System;
using System.Net;
using Telegram.Bot;//Библиотека Telegram.Bot

namespace MyTorrentzBot
{
    class Program
    {
        static void Main(string[] args)
        {
            WebProxy proxy = new WebProxy("192.169.217.106:19420",true);
            Console.WriteLine("Hello World!");
            TelegramBotClient botClient = new TelegramBotClient("1043135404:AAHDCt7_CZqZHOvHwZf7O4jUKB1cNdHSvaE", proxy);
            var me = botClient.GetMeAsync().Result;
            Console.WriteLine(me.Username);

        }

    }
}
