namespace OtusCSharpBasicFinalProject.Telegram
{
    public class TelegramUser
    {
        private string Name { get; }
        public long TelegramId { get; }
        public TelegramState CurrentState { get; set; }

        public TelegramUser(string name, long telegramId)
        {
            Name = name;
            TelegramId = telegramId;
            CurrentState = 0;
        }
    }
}