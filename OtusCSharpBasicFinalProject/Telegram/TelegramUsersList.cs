using System.Collections.Generic;
using System.Linq;

namespace OtusCSharpBasicFinalProject.Telegram
{
    public static class TelegramUsersList
    {
        private static readonly List<TelegramUser> ListOfUsers = new();

        public static TelegramUser GetOrAddIfNotExist(string name, long telegramId)
        {
            var telegramUser = ListOfUsers.FirstOrDefault(x => x.TelegramId == telegramId);
            if (telegramUser != null)
                return telegramUser;
            var newTelegramUser = new TelegramUser(name, telegramId);
            ListOfUsers.Add(newTelegramUser);
            return newTelegramUser;
        }
    }
}