using System;
using System.IO;
using OtusCSharpBasicFinalProject.Telegram;

namespace OtusCSharpBasicFinalProject
{
    class Program
    {
        private const int Port = 20000;
        private const string Server = "192.168.88.14";
        private const string ApiKeyPath = @"D:\OTUS\BotApiKey\BotApiKey.txt";
        private const string ApprovedUsersPath = @"D:\OTUS\BotApiKey\approvedUsersFile.txt";

        static void Main(string[] args)
        {
            OpenWebNet.OpenWebNet.StartWorking(Server,Port);
            var telegramBot = new TelegramBotApi();
            telegramBot.ConnectBot(ApiKeyPath,ApprovedUsersPath);
            Console.ReadKey();
        }
    }
}