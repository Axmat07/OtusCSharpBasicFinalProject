using OtusCSharpBasicFinalProject.Telegram;

namespace OtusCSharpBasicFinalProject.Data
{
    public class LightGroup
    {
        public string Name { get; }
        public TelegramState Room { get; }
        
        public string SendCommandOn { get; }
        
        public string SendCommandOff { get; }
        
        public string GetCommand { get; }
        public LightGroup(string name, TelegramState state, string sendCommandOn, string sendCommandOff, string getCommand)
        {
            Name = name;
            Room = state;
            SendCommandOn = sendCommandOn;
            SendCommandOff = sendCommandOff;
            GetCommand = getCommand;
        }

    }
}