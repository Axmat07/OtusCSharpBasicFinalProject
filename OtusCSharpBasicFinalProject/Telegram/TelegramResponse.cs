using Telegram.Bot.Types.ReplyMarkups;

namespace OtusCSharpBasicFinalProject.Telegram
{
    public class TelegramResponse
    {
        public string ResponseMessage { get; set; }
        public ReplyKeyboardMarkup Rkp { get; set; }
        public TelegramState TelegramState { get; set; }

        public TelegramResponse(TelegramState telegramState, string responseMessage, ReplyKeyboardMarkup rkp)
        {
            TelegramState = telegramState;
            ResponseMessage = responseMessage;
            Rkp = rkp;
        }
    }
}