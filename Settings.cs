using LobotomyBaseMod;
using LobotomyBaseModLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiveStreamIntegration
{
    public static class Settings
    {
        public static int TimeBetweenEffects { get; set; }
        public static bool IsIngameTime { get; set; }
        public static void LoadSettings()
        {
            try
            {
                IsIngameTime = Singleton<ModOptionManager>.Instance.GetToggleValue("VBlankFF_LiveStreamIntegration", "Use Ingame Time");
                TimeBetweenEffects = Convert.ToInt32(Singleton<ModOptionManager>.Instance.GetSliderValue("VBlankFF_LiveStreamIntegration", "TSeconds Between Effects"));
            }
            catch
            {
                TimeBetweenEffects = 120;
                IsIngameTime = true;
            }
        }
        public static void SaveSettings()
        {
            Singleton<ModOptionManager>.Instance.SetToggleValue("VBlankFF_LiveStreamIntegration", "Use Ingame Time", IsIngameTime);
            Singleton<ModOptionManager>.Instance.SetSliderValue("VBlankFF_LiveStreamIntegration", "Seconds Between Effects", TimeBetweenEffects);
        }
    }
    public enum EffectTimeSetting
    {
        IngameTime,
        FixedTime
    }
}
