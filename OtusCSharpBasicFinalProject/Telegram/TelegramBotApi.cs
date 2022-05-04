using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using File = System.IO.File;

namespace OtusCSharpBasicFinalProject.Telegram
{
    public class TelegramBotApi
    {
        private const int LivingRoomTemperature = 23;

        private const int LivingRoomSetTemperature = 23;
        //private List<(int, string)> LightGroupList { get; set; } = new();

        private List<(string, int)> _approvedUsers = new();

        #region RkmRegion
        
        private readonly ReplyKeyboardMarkup _rkmTemp = new(new[]
        {
            new KeyboardButton[] {"Детская/Гостиная", "Кухня",}, new KeyboardButton[] {"Спальная", "Назад",}
        }) {ResizeKeyboard = true};

        private readonly ReplyKeyboardMarkup _rkmTempLivingroom = new(new[]
        {
            new KeyboardButton[] {$"Гостиная {LivingRoomTemperature}°C", $"Целевая {LivingRoomSetTemperature}°C"},
            new KeyboardButton[] {"21🌡", "22🌡", "23🌡"}, new KeyboardButton[] {"24🌡", "25🌡", "Назад"}
        }) {ResizeKeyboard = true};

        private readonly ReplyKeyboardMarkup _rkmTempKitchen = new(new[]
        {
            new KeyboardButton[] {$"Кухня {LivingRoomTemperature}°C", $"Целевая {LivingRoomSetTemperature}°C"},
            new KeyboardButton[] {"21🌡", "22🌡", "23🌡"}, new KeyboardButton[] {"24🌡", "25🌡", "Назад"}
        }) {ResizeKeyboard = true};

        private readonly ReplyKeyboardMarkup _rkmTempBedroom = new(new[]
        {
            new KeyboardButton[] {$"Гостиная {LivingRoomTemperature}°C", $"Целевая {LivingRoomSetTemperature}°C"},
            new KeyboardButton[] {"21🌡", "22🌡", "23🌡"}, new KeyboardButton[] {"24🌡", "25🌡", "Назад"}
        }) {ResizeKeyboard = true};

        #endregion

        public void ConnectBot(string apiKeyPath, string approvedListPath)
        {
            _approvedUsers = GetUsers(approvedListPath);
            var apiKey = File.ReadAllText(apiKeyPath);
            var bot = new TelegramBotClient(apiKey);
            var allowedUpdates = new[] {UpdateType.Message};
            var receiverOptions = new ReceiverOptions {AllowedUpdates = allowedUpdates};
            bot.StartReceiving(UpdateHandler, ErrorHandler, receiverOptions, CancellationToken.None);
        }

        private Task ErrorHandler(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            Console.WriteLine(arg2.Message);
            return Task.CompletedTask;
        }

        private async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken arg3)
        {
            if (update.Message != null && CheckUser(_approvedUsers, update.Message.Chat.Id))
                await WorkWithWorkClientAsync(botClient, update, arg3);
        }

        private async Task WorkWithWorkClientAsync(ITelegramBotClient botClient, Update update, CancellationToken arg3)
        {
            if (update.Message?.Text == null) return;
            var currentUser =
                TelegramUsersList.GetOrAddIfNotExist(update.Message.Chat.Username, update.Message.Chat.Id);
            if (update.Message.Text.Equals("/start")) currentUser.CurrentState = TelegramState.None;
            var updateMessage = update.Message.Text.ToLower();
            switch (currentUser.CurrentState)
            {
                case TelegramState.None:
                    var dictStart = ResponseHandler.ResponsesDictionaryMainMenu;
                    if (dictStart.ContainsKey(updateMessage))
                    {
                        currentUser.CurrentState = await ResponseHandler.Handle(botClient, update,
                            dictStart[updateMessage], arg3);
                        break;
                    }

                    currentUser.CurrentState =
                        await ResponseHandler.HandleDefault(botClient, update, arg3);
                    break;

                case TelegramState.MainMenu:
                    var dictMainMenu = ResponseHandler.ResponsesDictionaryMainMenu;
                    if (dictMainMenu.ContainsKey(updateMessage))
                    {
                        currentUser.CurrentState = await ResponseHandler.Handle(botClient, update,
                            dictMainMenu[updateMessage], arg3);
                        break;
                    }

                    currentUser.CurrentState =
                        await ResponseHandler.HandleDefault(botClient, update, arg3);
                    break;

                case TelegramState.Light:

                    var dictLight = ResponseHandler.ResponsesDictionaryLight;
                    if (dictLight.ContainsKey(updateMessage))
                    {
                        currentUser.CurrentState = await ResponseHandler.HandleAndBuildFirstRkm(botClient, update,
                            dictLight[updateMessage], arg3);
                        break;
                    }

                    currentUser.CurrentState =
                        await ResponseHandler.HandleDefault(botClient, update, arg3);
                    break;

                case TelegramState.Temperature:
                    var dictTemp = ResponseHandler.ResponsesDictionaryTemp;
                    if (dictTemp.ContainsKey(updateMessage))
                    {
                        currentUser.CurrentState = await ResponseHandler.Handle(botClient, update,
                            dictTemp[updateMessage], arg3);
                        break;
                    }

                    currentUser.CurrentState =
                        await ResponseHandler.HandleDefault(botClient, update, arg3);
                    break;

                case TelegramState.Monitoring:
                    var dictMon = ResponseHandler.ResponsesDictionaryMon;
                    if (dictMon.ContainsKey(updateMessage))
                    {
                        currentUser.CurrentState = await ResponseHandler.Handle(botClient, update,
                            dictMon[updateMessage], arg3);
                        break;
                    }

                    currentUser.CurrentState =
                        await ResponseHandler.HandleDefault(botClient, update, arg3);
                    break;

                case TelegramState.LightLivingRoom:
                    var dictLightLivingroom = ResponseHandler.ResponsesDictionaryLightLivingRoom;
                    if (dictLightLivingroom.ContainsKey(updateMessage))
                    {
                        currentUser.CurrentState = await ResponseHandler.HandleAndBuildRkm(botClient, update,
                            dictLightLivingroom[updateMessage], arg3);
                        break;
                    }

                    currentUser.CurrentState =
                        await ResponseHandler.HandleDefault(botClient, update, arg3);
                    break;

                case TelegramState.LightKitchen:
                    var dictLightKitchen = ResponseHandler.ResponsesDictionaryLightKitchen;
                    if (dictLightKitchen.ContainsKey(updateMessage))
                    {
                        currentUser.CurrentState = await ResponseHandler.HandleAndBuildRkm(botClient, update,
                            dictLightKitchen[updateMessage], arg3);
                        break;
                    }

                    currentUser.CurrentState =
                        await ResponseHandler.HandleDefault(botClient, update, arg3);
                    break;
                
                case TelegramState.LightBedroom:
                    var dictLightBedroom = ResponseHandler.ResponsesDictionaryLightBedroom;
                    if (dictLightBedroom.ContainsKey(updateMessage))
                    {
                        currentUser.CurrentState = await ResponseHandler.HandleAndBuildRkm(botClient, update,
                            dictLightBedroom[updateMessage], arg3);
                        break;
                    }
                    
                    currentUser.CurrentState =
                        await ResponseHandler.HandleDefault(botClient, update, arg3);
                    break;
                
                case TelegramState.LightCorridor:
                    var dictLightCorridor = ResponseHandler.ResponsesDictionaryLightCorridor;
                    if (dictLightCorridor.ContainsKey(updateMessage))
                    {
                        currentUser.CurrentState = await ResponseHandler.HandleAndBuildRkm(botClient, update,
                            dictLightCorridor[updateMessage], arg3);
                        break;
                    }

                    currentUser.CurrentState =
                        await ResponseHandler.HandleDefault(botClient, update, arg3);
                    break;
                    
                case TelegramState.LightWc:
                    
                    var dictLightWc = ResponseHandler.ResponsesDictionaryLightWc;
                    if (dictLightWc.ContainsKey(updateMessage))
                    {
                        currentUser.CurrentState = await ResponseHandler.HandleAndBuildRkm(botClient, update,
                            dictLightWc[updateMessage], arg3);
                        break;
                    }

                    currentUser.CurrentState =
                        await ResponseHandler.HandleDefault(botClient, update, arg3);
                    break;
                
                case TelegramState.TempLivingRoom:

                    var dictTempRoom = ResponseHandler.ResponsesDictionaryTempLivingRoom;
                    if (dictTempRoom.ContainsKey(updateMessage))
                    {
                        currentUser.CurrentState = await ResponseHandler.HandleAndBuildRkm(botClient, update,
                            dictTempRoom[updateMessage], arg3);
                        break;
                    }
                    currentUser.CurrentState =
                        await ResponseHandler.HandleDefault(botClient, update, arg3);
                    break;
                    
                    // switch (update)
                    // {
                    //     case {Type: UpdateType.Message, Message: {Text: { } text, Chat: { } chat},}
                    //         when text.Equals("21🌡", StringComparison.OrdinalIgnoreCase) ||
                    //              text.Equals("22🌡", StringComparison.OrdinalIgnoreCase) ||
                    //              text.Equals("23🌡", StringComparison.OrdinalIgnoreCase) ||
                    //              text.Equals("24🌡", StringComparison.OrdinalIgnoreCase) ||
                    //              text.Equals("25🌡", StringComparison.OrdinalIgnoreCase):
                    //     {
                    //         await botClient.SendTextMessageAsync(update.Message.Chat, "Выставил",
                    //             replyMarkup: _rkmTempLivingroom, cancellationToken: arg3);
                    //         currentUser.CurrentState = TelegramState.TempLivingRoom;
                    //         break;
                    //     }
                    //
                    //     case {Type: UpdateType.Message, Message: {Text: { } text, Chat: { } chat},}
                    //         when text.Equals("Назад", StringComparison.OrdinalIgnoreCase):
                    //     {
                    //         await botClient.SendTextMessageAsync(update.Message.Chat, "назад так назад",
                    //             replyMarkup: _rkmTemp, cancellationToken: arg3);
                    //         currentUser.CurrentState = TelegramState.Temperature;
                    //         break;
                    //     }
                    //     default:
                    //     {
                    //         currentUser.CurrentState = TelegramState.MainMenu;
                    //         break;
                    //     }
                    // }
                    //
                    // break;

                case TelegramState.TempKitchen:
                    switch (update)
                    {
                        case {Type: UpdateType.Message, Message: {Text: { } text, Chat: { } chat},}
                            when text.Equals("21🌡", StringComparison.OrdinalIgnoreCase) ||
                                 text.Equals("22🌡", StringComparison.OrdinalIgnoreCase) ||
                                 text.Equals("23🌡", StringComparison.OrdinalIgnoreCase) ||
                                 text.Equals("24🌡", StringComparison.OrdinalIgnoreCase) ||
                                 text.Equals("25🌡", StringComparison.OrdinalIgnoreCase):
                        {
                            await botClient.SendTextMessageAsync(update.Message.Chat, "Выставил",
                                replyMarkup: _rkmTempKitchen, cancellationToken: arg3);
                            currentUser.CurrentState = TelegramState.TempKitchen;
                            break;
                        }

                        case {Type: UpdateType.Message, Message: {Text: { } text, Chat: { } chat},}
                            when text.Equals("Назад", StringComparison.OrdinalIgnoreCase):
                        {
                            await botClient.SendTextMessageAsync(update.Message.Chat, "назад так назад",
                                replyMarkup: _rkmTemp, cancellationToken: arg3);
                            currentUser.CurrentState = TelegramState.Temperature;
                            break;
                        }
                        default:
                        {
                            currentUser.CurrentState = TelegramState.MainMenu;
                            break;
                        }
                    }

                    break;
                case TelegramState.TempBedroom:
                    switch (update)
                    {
                        case {Type: UpdateType.Message, Message: {Text: { } text, Chat: { } chat},}
                            when text.Equals("21🌡", StringComparison.OrdinalIgnoreCase) ||
                                 text.Equals("22🌡", StringComparison.OrdinalIgnoreCase) ||
                                 text.Equals("23🌡", StringComparison.OrdinalIgnoreCase) ||
                                 text.Equals("24🌡", StringComparison.OrdinalIgnoreCase) ||
                                 text.Equals("25🌡", StringComparison.OrdinalIgnoreCase):
                        {
                            await botClient.SendTextMessageAsync(update.Message.Chat, "Выставил",
                                replyMarkup: _rkmTempBedroom, cancellationToken: arg3);
                            currentUser.CurrentState = TelegramState.TempBedroom;
                            break;
                        }

                        case {Type: UpdateType.Message, Message: {Text: { } text, Chat: { } chat},}
                            when text.Equals("Назад", StringComparison.OrdinalIgnoreCase):
                        {
                            await botClient.SendTextMessageAsync(update.Message.Chat, "назад так назад",
                                replyMarkup: _rkmTemp, cancellationToken: arg3);
                            currentUser.CurrentState = TelegramState.Temperature;
                            break;
                        }
                        default:
                        {
                            currentUser.CurrentState = TelegramState.MainMenu;
                            break;
                        }
                    }

                    break;
                case TelegramState.MonLight:
                {
                    break;
                }
                case TelegramState.MonTemp:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private List<(string, int)> GetUsers(string approvedUsersFile)
        {
            // var list = approvedUsersFile.Split('\n');
            // var list2 = new List<(string, string)>();
            // foreach (var i in list)
            // {
            //     list2.Add((i.Split(','));
            // }
            // var approvedList = new List<(string,int)>();
            // foreach (var i in list)
            // {
            //     approvedList.Add();
            // }
            return new List<(string, int)>() {("Akhmat", 125340637)};
        }

        private bool CheckUser(IEnumerable<(string, int)> listApprovedUsers, long chatId)
        {
            return listApprovedUsers.Any(i => i.Item2 == chatId);
        }
    }
}