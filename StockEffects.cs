using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace LiveStreamIntegration
{
    public static class EffectDefinitions
    {
        public static List<Effect> effects;
        public static List<Effect> GetEffects()
        {
            if (effects is null)
            {
                InitEffects();
            }
            return effects;
        }
        public static void InitEffects()
        {
            effects = new List<Effect>();
            Effect nobullets = new Effect(typeof(EffectDefinitions).GetMethod("NoBullets", BindingFlags.Static | BindingFlags.Public), "Set bullets to 0");
            effects.Add(nobullets);
            Effect pauseGame = new Effect(typeof(EffectDefinitions).GetMethod("Pause", BindingFlags.Static | BindingFlags.Public), "Pause game");
            effects.Add(pauseGame);
            Effect trainSound = new Effect(typeof(EffectDefinitions).GetMethod("PlayTrainSound", BindingFlags.Static | BindingFlags.Public), "Train horn");
            effects.Add(trainSound);
            Effect sendTrain = new Effect(typeof(EffectDefinitions).GetMethod("SendTrain", BindingFlags.Static | BindingFlags.Public), "Send train");
            effects.Add(sendTrain);
            Effect wideAgents = new Effect(typeof(EffectDefinitions).GetMethod("WideAgents", BindingFlags.Static | BindingFlags.Public), "Wide agents for 90s");
            effects.Add(wideAgents);
            return;
        }
        public static void NoBullets()
        {
            GlobalBullet.GlobalBulletManager.instance.currentBullet = 0;
        }
        public static void Pause()
        {
            
        }
        public static void PlayTrainSound()
        {

        }
        public static void SendTrain()
        {

        }
        public static void WideAgents()
        {

        }
    }
}
