using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace LiveStreamIntegration
{
    /* An Effect that viewers can vote on. Effects have a name that is displayed on the voting UI, and a method that is invoked at the end
     * of the voting timer if the Effect has the most votes.
     */
    public class Effect
    {
        // The method to be run when this Effect activates
        public MethodInfo effectMethod;
        // The name to be displayed when this effect is a votable option
        public string name;
        // If this effect can currently show up as an option for voting
        public bool isEnabled;
        // Description of the Effect (currently unused)
        public string description;
        public Effect(MethodInfo effectMethod, string name)
        {
            this.effectMethod = effectMethod;
            this.name = name;
            this.isEnabled = true;
            this.description = "No description";
        }
        public Effect(MethodInfo effectMethod, string name, bool isEnabled)
        {
            this.effectMethod = effectMethod;
            this.name = name;
            this.isEnabled = isEnabled;
            this.description = "No description";
        }
        public Effect(MethodInfo effectMethod, string name, bool isEnabled, string description)
        {
            this.effectMethod = effectMethod;
            this.name = name;
            this.isEnabled = isEnabled;
            this.description = description;
        }
        /* Loads every dll in the Effects folder and adds their Effects to the list. If you are making a dll to add to the folder, ensure you have an
         EffectDefinitions class with a GetEffects method that returns an IEnumerable of Effects.*/
        public static List<Effect> LoadEffects()
        {
            var effects = new List<Effect>();
            DirectoryInfo effectDirectory = new DirectoryInfo(typeof(Effect).Assembly.Location);
            foreach (FileInfo file in new DirectoryInfo(effectDirectory.FullName.Replace("LivestreamIntegration.dll", @"Effects\")).GetFiles())
            {
                if (!file.FullName.EndsWith(".dll"))
                {
                    continue;
                }
                try
                {
                    Assembly effectAssem = Assembly.LoadFrom(file.FullName);
                    IEnumerable<Effect> thisAssemEffects = new List<Effect>();
                    foreach (Type t in effectAssem.GetTypes())
                    {
                        if (t.Name.EndsWith("EffectDefinitions"))
                        {
                            thisAssemEffects = (IEnumerable<Effect>)t.GetMethod("GetEffects", BindingFlags.Static | BindingFlags.Public).Invoke(null, null);
                        }
                    }
                    effects.AddRange(thisAssemEffects);
                }
                catch (Exception ex)
                {
                    LobotomyBaseMod.ModDebug.Log("LobotomyCorporationlivestreamIntegration failed to load Effect file " + file.Name + "\n" + "Error thrown: " + ex.Message);
                }
            } 
            return effects;
        }
        // Adds an Effect to the Effect list. Use only for mods with Effects that aren't added to the Effects folder.
        public static void ExternalLoadEffect(Effect externalEffect)
        {
            if (externalEffect is null)
            {
                LobotomyBaseMod.ModDebug.Log("LobotomyCorporationlivestreamIntegration: ExternalLoadEffect received null Effect");
                return;
            }
            Harmony_Patch.effects.Add(externalEffect);
        }
        // Adds Effects to the Effect list. Use only for mods with Effects that aren't added to the Effects folder.
        public static void ExternalLoadEffects(IEnumerable<Effect> externalEffects)
        {
            if (externalEffects is null)
            {
                LobotomyBaseMod.ModDebug.Log("LobotomyCorporationlivestreamIntegration: ExternalLoadEffects received null Effects");
                return;
            }
            foreach (Effect eff in externalEffects)
            {
                if (eff is null)
                {
                    LobotomyBaseMod.ModDebug.Log("LobotomyCorporationlivestreamIntegration: ExternalLoadEffects received a null Effect");
                    continue;
                }
                Harmony_Patch.effects.Add(eff);
            }
        }
    }
    // This will run a method after a given amount of ingame time passes. Useful if you need to undo an Effect after a certain amount of time.
    public class EffectAfterTime : MonoBehaviour 
    {
        public static List<KeyValuePair<MethodInfo, Timer>> effectList;
        private static EffectAfterTime _instance;
        public static EffectAfterTime instance {  
            get 
            { 
                if (_instance is null)
                {
                    _instance = new EffectAfterTime();
                }
                return _instance; 
            } 
        }
        public EffectAfterTime()
        {
            effectList = new List<KeyValuePair<MethodInfo, Timer>>();
        }
        public void AddEffect(MethodInfo effect, float time)
        {
            var effectToAdd = new KeyValuePair<MethodInfo, Timer>(effect, new Timer());
            effectToAdd.Value.StartTimer(time);
        }
        public void FixedUpdate()
        {
            foreach (var effect in effectList)
            {
                if (effect.Value.RunTimer())
                {
                    effect.Key.Invoke(null, null);
                    effectList.Remove(effect);
                }
            }
        }
    }
}
