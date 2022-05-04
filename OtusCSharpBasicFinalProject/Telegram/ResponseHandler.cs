using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OtusCSharpBasicFinalProject.Data;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace OtusCSharpBasicFinalProject.Telegram
{
    public static class ResponseHandler
    {
        public static readonly Dictionary<string, TelegramResponse> ResponsesDictionaryMainMenu = new();
        public static readonly Dictionary<string, TelegramResponse> ResponsesDictionaryLight = new();
        public static readonly Dictionary<string, TelegramResponse> ResponsesDictionaryLightLivingRoom = new();
        public static readonly Dictionary<string, TelegramResponse> ResponsesDictionaryLightKitchen = new();
        public static readonly Dictionary<string, TelegramResponse> ResponsesDictionaryLightBedroom = new();
        public static readonly Dictionary<string, TelegramResponse> ResponsesDictionaryLightCorridor = new();
        public static readonly Dictionary<string, TelegramResponse> ResponsesDictionaryLightWc = new();
        public static readonly Dictionary<string, TelegramResponse> ResponsesDictionaryTemp = new();
        public static readonly Dictionary<string, TelegramResponse> ResponsesDictionaryTempLivingRoom = new();
        public static readonly Dictionary<string, TelegramResponse> ResponsesDictionaryMon = new();
        private const int LivingRoomTemperature = 23;
        private const int LivingRoomSetTemperature = 23;

        #region RmmRegion
        
        private static readonly ReplyKeyboardMarkup RkmMainMenu = new(new[]
        {
            new KeyboardButton[] {"Свет 💡", "Температура 🌡", "Мониторинг 🏠"}
        })
        {
            ResizeKeyboard = true
        };

        private static readonly ReplyKeyboardMarkup RkmLight = new(new[]
        {
            new KeyboardButton[]
            {
                "Детская/Гостиная", "Кухня", "Коридор",
            },
            new KeyboardButton[]
            {
                "Спальная", "Санузел", "Назад",
            }
        })
        {
            ResizeKeyboard = true
        };

        private static readonly ReplyKeyboardMarkup RkmTemp = new(new[]
        {
            new KeyboardButton[]
            {
                "Детская/Гостиная", "Кухня",
            },
            new KeyboardButton[]
            {
                "Спальная", "Назад",
            }
        })
        {
            ResizeKeyboard = true
        };

        private static readonly ReplyKeyboardMarkup RkmTempLivingroom = new(new[]
        {
            new KeyboardButton[]
            {
                $"Гостиная {LivingRoomTemperature}°C", $"Целевая {LivingRoomSetTemperature}°C"
            },

            new KeyboardButton[]
            {
                "21🌡", "22🌡", "23🌡"
            },
            new KeyboardButton[]
            {
                "24🌡", "25🌡", "Назад"
            }
        })
        {
            ResizeKeyboard = true
        };

        private static readonly ReplyKeyboardMarkup RkmTempKitchen = new(new[]
        {
            new KeyboardButton[]
            {
                $"Кухня {LivingRoomTemperature}°C", $"Целевая {LivingRoomSetTemperature}°C"
            },

            new KeyboardButton[]
            {
                "21🌡", "22🌡", "23🌡"
            },
            new KeyboardButton[]
            {
                "24🌡", "25🌡", "Назад"
            }
        })
        {
            ResizeKeyboard = true
        };

        private static readonly ReplyKeyboardMarkup RkmTempBedroom = new(new[]
        {
            new KeyboardButton[]
            {
                $"Гостиная {LivingRoomTemperature}°C", $"Целевая {LivingRoomSetTemperature}°C"
            },

            new KeyboardButton[]
            {
                "21🌡", "22🌡", "23🌡"
            },
            new KeyboardButton[]
            {
                "24🌡", "25🌡", "Назад"
            }
        })
        {
            ResizeKeyboard = true
        };

        private static readonly ReplyKeyboardMarkup RkmMon = new(new[]
        {
            new KeyboardButton[]
            {
                "Свет💡", "Температура🌡",
            }
        })
        {
            ResizeKeyboard = true
        };

        private static readonly ReplyKeyboardMarkup RkmMonLight = new(new[]
        {
            new KeyboardButton[]
            {
                "Мониторинг света"
            },
            new KeyboardButton[]
            {
                "Час", "6 часов", "12 часов"
            },
            new KeyboardButton[]
            {
                "Сутки", "Неделя", "Назад"
            }
        })
        {
            ResizeKeyboard = true
        };

        private static readonly ReplyKeyboardMarkup RkmMonTemp = new(new[]
        {
            new KeyboardButton[]
            {
                "Мониторинг температуры"
            },
            new KeyboardButton[]
            {
                "Час", "6 часов", "12 часов"
            },
            new KeyboardButton[]
            {
                "Сутки", "Неделя", "Назад"
            }
        })
        {
            ResizeKeyboard = true
        };
        
        #endregion

        #region Responses

        private static readonly TelegramResponse DefaultResponse = new(TelegramState.MainMenu,
            "Что-то я тебя не понял, что сделать-то хочешь?", RkmMainMenu);

        static ResponseHandler()
        {
            ResponsesDictionaryMainMenu["/start"] = new TelegramResponse(TelegramState.MainMenu,
                "Добро пожаловать в бот управления умным домом!\nВыбери что хочешь сделать:",
                RkmMainMenu);

            ResponsesDictionaryMainMenu["свет 💡"] = new TelegramResponse(TelegramState.Light,
                "Где хочешь управлять светом?", RkmLight);

            ResponsesDictionaryMainMenu["температура 🌡"] = new TelegramResponse(TelegramState.Temperature,
                "Где хочешь управлять температурой?", RkmTemp);

            ResponsesDictionaryMainMenu["мониторинг 🏠"] = new TelegramResponse(TelegramState.Monitoring,
                "Что хочешь мониторить?", RkmMon);

            ResponsesDictionaryLight["детская/гостиная"] = new TelegramResponse(TelegramState.LightLivingRoom,
                "Какой группой?", RkmLight);

            ResponsesDictionaryLight["кухня"] = new TelegramResponse(TelegramState.LightKitchen,
                "Какой группой?", RkmLight);

            ResponsesDictionaryLight["спальная"] = new TelegramResponse(TelegramState.LightBedroom,
                "Какой группой?", RkmLight);

            ResponsesDictionaryLight["коридор"] = new TelegramResponse(TelegramState.LightCorridor,
                "Какой группой?", RkmLight);

            ResponsesDictionaryLight["санузел"] = new TelegramResponse(TelegramState.LightWc,
                "Какой группой?", RkmLight);

            ResponsesDictionaryLight["назад"] = new TelegramResponse(TelegramState.MainMenu,
                "понял назад", RkmMainMenu);

            ResponsesDictionaryTemp["детская/гостиная"] = new TelegramResponse(TelegramState.TempLivingRoom,
                "Какую температуру хочешь выставить?", RkmTempLivingroom);

            ResponsesDictionaryTemp["кухня"] = new TelegramResponse(TelegramState.TempKitchen,
                "Какую температуру хочешь выставить?", RkmTempKitchen);

            ResponsesDictionaryTemp["спальная"] = new TelegramResponse(TelegramState.TempBedroom,
                "Какую температуру хочешь выставить?", RkmTempBedroom);

            ResponsesDictionaryTemp["назад"] = new TelegramResponse(TelegramState.MainMenu,
                "ну ок назад", RkmMainMenu);

            ResponsesDictionaryMon["свет💡"] = new TelegramResponse(TelegramState.MonLight,
                "Какой период?", RkmMonLight);

            ResponsesDictionaryMon["температура🌡"] = new TelegramResponse(TelegramState.MonTemp,
                "Какой период?", RkmMon);

            ResponsesDictionaryMon["назад"] = new TelegramResponse(TelegramState.MainMenu,
                "эх", RkmMainMenu);

            ResponsesDictionaryLightLivingRoom["гостиная💡"] = new TelegramResponse(TelegramState.LightLivingRoom,
                "Выключил", RkmLight);

            ResponsesDictionaryLightLivingRoom["гостиная🕯"] = new TelegramResponse(TelegramState.LightLivingRoom,
                "Включил", RkmLight);

            ResponsesDictionaryLightLivingRoom["детская💡"] = new TelegramResponse(TelegramState.LightLivingRoom,
                "Выключил", RkmLight);

            ResponsesDictionaryLightLivingRoom["детская🕯"] = new TelegramResponse(TelegramState.LightLivingRoom,
                "Включил", RkmLight);

            ResponsesDictionaryLightLivingRoom["вход💡"] = new TelegramResponse(TelegramState.LightLivingRoom,
                "Выключил", RkmLight);

            ResponsesDictionaryLightLivingRoom["вход🕯"] = new TelegramResponse(TelegramState.LightLivingRoom,
                "Включил", RkmLight);

            ResponsesDictionaryLightLivingRoom["назад"] = new TelegramResponse(TelegramState.Light,
                "Опять назад", RkmLight);

            ResponsesDictionaryLightKitchen["основной💡"] = new TelegramResponse(TelegramState.LightKitchen,
                "Выключил", RkmLight);

            ResponsesDictionaryLightKitchen["основной🕯"] = new TelegramResponse(TelegramState.LightKitchen,
                "Включил", RkmLight);

            ResponsesDictionaryLightKitchen["точечный💡"] = new TelegramResponse(TelegramState.LightKitchen,
                "Выключил", RkmLight);

            ResponsesDictionaryLightKitchen["точечный🕯"] = new TelegramResponse(TelegramState.LightKitchen,
                "Включил", RkmLight);

            ResponsesDictionaryLightKitchen["назад"] = new TelegramResponse(TelegramState.Light,
                "Всё назад и назад", RkmLight);

            ResponsesDictionaryLightBedroom["основной💡"] = new TelegramResponse(TelegramState.LightBedroom,
                "Выключил", RkmLight);

            ResponsesDictionaryLightBedroom["основной🕯"] = new TelegramResponse(TelegramState.LightBedroom,
                "Включил", RkmLight);

            ResponsesDictionaryLightBedroom["гардероб💡"] = new TelegramResponse(TelegramState.LightBedroom,
                "Выключил", RkmLight);

            ResponsesDictionaryLightBedroom["гардероб🕯"] = new TelegramResponse(TelegramState.LightBedroom,
                "Включил", RkmLight);

            ResponsesDictionaryLightBedroom["назад"] = new TelegramResponse(TelegramState.Light,
                "нОзАд", RkmLight);

            ResponsesDictionaryLightCorridor["основной💡"] = new TelegramResponse(TelegramState.LightCorridor,
                "Выключил", RkmLight);

            ResponsesDictionaryLightCorridor["основной🕯"] = new TelegramResponse(TelegramState.LightCorridor,
                "Включил", RkmLight);

            ResponsesDictionaryLightCorridor["назад"] = new TelegramResponse(TelegramState.Light,
                "полный назад!", RkmLight);

            ResponsesDictionaryLightWc["основной💡"] = new TelegramResponse(TelegramState.LightWc,
                "Выключил", RkmLight);

            ResponsesDictionaryLightWc["основной🕯"] = new TelegramResponse(TelegramState.LightWc,
                "Включил", RkmLight);

            ResponsesDictionaryLightWc["вентиляция💡"] = new TelegramResponse(TelegramState.LightWc,
                "Выключил", RkmLight);

            ResponsesDictionaryLightWc["вентиляция🕯"] = new TelegramResponse(TelegramState.LightWc,
                "Включил", RkmLight);

            ResponsesDictionaryLightWc["назад"] = new TelegramResponse(TelegramState.Light,
                "валим отсюда", RkmLight);
            
            ResponsesDictionaryTempLivingRoom["21🌡"] = new TelegramResponse(TelegramState.TempBedroom,
                "выставил", RkmTempLivingroom);
            
            ResponsesDictionaryTempLivingRoom["22🌡"] = new TelegramResponse(TelegramState.TempBedroom,
                "выставил", RkmTempLivingroom);
            
            ResponsesDictionaryTempLivingRoom["23🌡"] = new TelegramResponse(TelegramState.TempBedroom,
                "выставил", RkmTempLivingroom);
            
            ResponsesDictionaryTempLivingRoom["24🌡"] = new TelegramResponse(TelegramState.TempBedroom,
                "выставил", RkmTempLivingroom);
            
            ResponsesDictionaryTempLivingRoom["25🌡"] = new TelegramResponse(TelegramState.TempBedroom,
                "выставил", RkmTempLivingroom);

        }

        #endregion

        public static async Task<TelegramState> Handle(ITelegramBotClient botClient, Update update,
            TelegramResponse telegramResponse, CancellationToken arg3)
        {
            if (update.Message == null) return telegramResponse.TelegramState;
            await botClient.SendTextMessageAsync(update.Message.Chat, telegramResponse.ResponseMessage,
                replyMarkup: telegramResponse.Rkp, cancellationToken: arg3);
            Console.WriteLine(update.Message.Text);

            return telegramResponse.TelegramState;
        }
        
        public static async Task<TelegramState> HandleAndBuildFirstRkm(ITelegramBotClient botClient, Update update,
            TelegramResponse telegramResponse, CancellationToken arg3)
        {
            if (update.Message?.Text == null) return telegramResponse.TelegramState;
            var message = update.Message.Text;
            await botClient.SendTextMessageAsync(update.Message.Chat, telegramResponse.ResponseMessage,
                replyMarkup: message.ToLower() == "назад"? RkmMainMenu: await FormRkm(telegramResponse.TelegramState), cancellationToken: arg3);
            
            Console.WriteLine(update.Message.Text);

            return telegramResponse.TelegramState;
        }
        public static async Task<TelegramState> HandleAndBuildRkm(ITelegramBotClient botClient, Update update,
            TelegramResponse telegramResponse, CancellationToken arg3)
        {
            if (update.Message?.Text == null) return telegramResponse.TelegramState;
            var message = update.Message.Text;
            if (message.Contains("💡") && !message.Contains("свет"))
                await OpenWebNet.OpenWebNet.SetGroupLightStateOff(LightGroupList.GetLightGroupByName(message.Replace("💡",""),telegramResponse.TelegramState));
            if (message.Contains("🕯") && !message.Contains("свет"))
                await OpenWebNet.OpenWebNet.SetGroupLightStateOn(LightGroupList.GetLightGroupByName(message.Replace("🕯",""),telegramResponse.TelegramState));
            await botClient.SendTextMessageAsync(update.Message.Chat, telegramResponse.ResponseMessage,
                replyMarkup: message.ToLower() == "назад"? RkmLight: await FormRkm(telegramResponse.TelegramState), cancellationToken: arg3);
            
            Console.WriteLine(update.Message.Text);

            return telegramResponse.TelegramState;
        }

        public static async Task<TelegramState> HandleDefault(ITelegramBotClient botClient, Update update,
            CancellationToken arg3)
        {
            if (update.Message == null) return DefaultResponse.TelegramState;
            await botClient.SendTextMessageAsync(update.Message.Chat, DefaultResponse.ResponseMessage,
                replyMarkup: DefaultResponse.Rkp, cancellationToken: arg3);
            Console.WriteLine(update.Message.Text);

            return DefaultResponse.TelegramState;
        }

        private static async Task<ReplyKeyboardMarkup> FormRkm(TelegramState state)
        {
            var lightningGroups = LightGroupList.GetLightGroupStringList(state);
            switch (lightningGroups.Count)
            {
                case 3:
                {
                    ReplyKeyboardMarkup rkm = new(new[]
                    {
                        new KeyboardButton[]
                        {
                            $"{lightningGroups[0].Name}{await OpenWebNet.OpenWebNet.GetGroupLightState(lightningGroups[0])}",
                            $"{lightningGroups[1].Name}{await OpenWebNet.OpenWebNet.GetGroupLightState(lightningGroups[1])}",
                        },
                        new KeyboardButton[]
                        {
                            $"{lightningGroups[2].Name}{await OpenWebNet.OpenWebNet.GetGroupLightState(lightningGroups[2])}",
                            "Назад"
                        }
                    })
                    {
                        ResizeKeyboard = true
                    };
                    return rkm;
                }
                case 2:
                {
                    ReplyKeyboardMarkup rkm = new(new[]
                    {
                        new KeyboardButton[]
                        {
                            $"{lightningGroups[0].Name}{await OpenWebNet.OpenWebNet.GetGroupLightState(lightningGroups[0])}",
                            $"{lightningGroups[1].Name}{await OpenWebNet.OpenWebNet.GetGroupLightState(lightningGroups[1])}",
                        },
                        new KeyboardButton[]
                        {
                            "Назад"
                        }
                    })
                    {
                        ResizeKeyboard = true
                    };
                    return rkm;
                }
                case 1:
                {
                    ReplyKeyboardMarkup rkm = new(new[]
                    {
                        new KeyboardButton[]
                        {
                            $"{lightningGroups[0].Name}{await OpenWebNet.OpenWebNet.GetGroupLightState(lightningGroups[0])}",
                            "Назад"
                        }
                    })
                    {
                        ResizeKeyboard = true
                    };
                    return rkm;
                }
            }

            return new ReplyKeyboardMarkup(new KeyboardButton("Что-то пошло не так"));
        }
    }
}