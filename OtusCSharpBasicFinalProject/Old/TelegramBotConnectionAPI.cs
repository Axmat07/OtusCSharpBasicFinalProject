// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading;
// using System.Threading.Tasks;
// using Telegram.Bot;
// using Telegram.Bot.Extensions.Polling;
// using Telegram.Bot.Types;
// using Telegram.Bot.Types.Enums;
// using Telegram.Bot.Types.ReplyMarkups;
// using File = System.IO.File;
//
// namespace OtusCSharpBasicFinalProject
// {
//     public static class TelegramBotConnectionApi
//     {
//         //public static string Apikey { get; set; }
//         private static List<(string, int)> ApprovedUsers { get; set; } = new();
//         private static ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
//         {
//             new KeyboardButton[] {"Help me", "Call me ☎️"},
//             new KeyboardButton[] {"Hallo", "My Friend"}
//         });
//         
//         static ReplyKeyboardMarkup rkmStart = new(new []
//         {
//             new KeyboardButton[] { "Свет 💡", "Температура 🌡", "Мониторинг 🏠"},
//         })
//         {
//             ResizeKeyboard = true
//         };
//         
//         private static List<(string, int)> GetUsers(string approvedUsersFile)
//         {
//             // var list = approvedUsersFile.Split('\n');
//             // var list2 = new List<(string, string)>();
//             // foreach (var i in list)
//             // {
//             //     list2.Add((i.Split(','));
//             // }
//             // var approvedList = new List<(string,int)>();
//             // foreach (var i in list)
//             // {
//             //     approvedList.Add();
//             // }
//             return new List<(string, int)>(){("Akhmat", 125340637)};
//         }
//
//
//         public static void ConnectBot(string apiKeyPath, string approvedListPath )
//         {
//             ApprovedUsers = GetUsers(approvedListPath);
//             var apiKey = File.ReadAllText(apiKeyPath);
//             TelegramBotClient bot = new (apiKey);
//             var allowedUpdates = new[] {UpdateType.Message};
//             var receiverOptions = new ReceiverOptions {AllowedUpdates = allowedUpdates};
//             bot.StartReceiving(UpdateHandler, ErrorHandler, receiverOptions, CancellationToken.None);
//         }
//
//         private static async Task ErrorHandler(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
//         {
//             Console.WriteLine(arg2.Message);
//         }
//
//         private static async Task UpdateHandler (ITelegramBotClient botClient, Update update, CancellationToken arg3)
//         {
//             if (update.Message != null && CheckUser(ApprovedUsers,update.Message.Chat.Id))
//             {
//                 await WorkWithWorkClient(botClient, update, arg3);
//             }
//         }
//
//         private static async Task WorkWithWorkClient(ITelegramBotClient botClient, Update update, CancellationToken arg3)
//         {
//             switch (update)
//             {
//                 case
//                 {
//                     Type: UpdateType.Message,
//                     Message: {Text: { } text, Chat: { } chat},
//                 } when text.Equals("/start", StringComparison.OrdinalIgnoreCase):
//                 {
//                     await botClient.SendTextMessageAsync(update.Message.Chat, "Добро пожаловать в бот управления умным домом!", cancellationToken: arg3);
//                     await botClient.SendTextMessageAsync(update.Message.Chat, "Выбери что хочешь сделать",replyMarkup:rkmStart, cancellationToken: arg3);
//                     Console.WriteLine(update.Message?.Text);
//                     break;
//                 }
//                 case
//                 {
//                     Type: UpdateType.Message,
//                     Message: {Text: { } text, Chat: { } chat},
//                 } :
//                 {
//                     await botClient.SendTextMessageAsync(update.Message.Chat, "Выбери что хочешь сделать",replyMarkup:rkmStart, cancellationToken: arg3);
//                     Console.WriteLine(update.Message?.Text);
//                     break;
//                 }
//             } 
//         }
//
//         private static bool CheckUser(List<(string, int)> listApprovedUsers, long chatId)
//         {
//             return listApprovedUsers.Any(i => i.Item2 == chatId);
//         }
//         
//     }
// }