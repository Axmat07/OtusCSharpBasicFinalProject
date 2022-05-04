using System.Collections.Generic;
using System.Linq;
using OtusCSharpBasicFinalProject.Telegram;

namespace OtusCSharpBasicFinalProject.Data
{
    public static class LightGroupList
    {
        static List<LightGroup> LightGroupListAll { get; } = new List<LightGroup>()
        {
             new LightGroup("гостиная", TelegramState.LightLivingRoom, "*1*1*28##","*1*0*28##","*#1*28##"),
             new LightGroup("детская", TelegramState.LightLivingRoom, "*1*1*26##","*1*0*26##","*#1*26##"),
             new LightGroup("вход", TelegramState.LightLivingRoom, "*1*1*29##","*1*0*29##","*#1*29##"),
             new LightGroup("основной", TelegramState.LightKitchen, "*1*1*16##","*1*0*16##","*#1*16##"),
             new LightGroup("точечный", TelegramState.LightKitchen,"*1*1*17##", "*1*0*17##", "*#1*17##"),
             new LightGroup("основной", TelegramState.LightCorridor, "*1*1*36##","*1*0*36##","*#1*36##"),
             new LightGroup("основной", TelegramState.LightBedroom, "*1*1*34##","*1*0*34##","*#1*34##"),
             new LightGroup("гардероб", TelegramState.LightBedroom, "*1*1*35##","*1*0*35##","*#1*35##"),
             new LightGroup("основной", TelegramState.LightWc, "*1*1*14##","*1*0*14##","*#1*14##"),
             new LightGroup("вентиляция", TelegramState.LightWc, "*1*1*15##","*1*0*15##", "*#1*15##")
        };
        
        public static List<LightGroup> GetLightGroupStringList(TelegramState state)
        {
            return LightGroupListAll.Where(u => u.Room == state).ToList();
        }

        public static LightGroup GetLightGroupByName(string name, TelegramState state)
        {
            return LightGroupListAll.Where(x=>x.Room ==state).FirstOrDefault(u => u.Name == name);
        }
        
    }
}