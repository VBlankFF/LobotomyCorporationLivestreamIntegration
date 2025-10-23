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
        // The method to be run when this Effect activates. The method should be static.
        public MethodInfo effectMethod;
        // The name to be displayed when this effect is a votable option
        public string name;
        // If this effect can currently show up as an option for voting. This is intented to be used for a config, so you should avoid setting it yourself
        // outside of the constructor.
        public bool isEnabled;
        // Description of the Effect (currently unused)
        public string description;
        /* The condition that must be true for an Effect to be selected for voting. if null, is always votable (if enabled). The method should (probably) be static.
        This is reevaluated every time the selection changes, so the conditions can be things that change mid-day.*/
        public MethodInfo votableCondition;
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
        public Effect(MethodInfo effectMethod, string name, bool isEnabled = true, string description = "No description", MethodInfo votableCondition = null)
        {
            this.effectMethod = effectMethod;
            this.name = name;
            this.isEnabled = isEnabled;
            this.description = description;
            this.votableCondition = votableCondition;
        }
        public string GetName()
        {
            return name;
        }
        public MethodInfo GetEffectMethod()
        {
            return effectMethod;
        }
        public bool IsEnabled()
        {
            return isEnabled;
        }
        public string GetDescription()
        {
            return description;
        }
        public MethodInfo GetVotableCondition()
        {
            return votableCondition;
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
        /* Checks if votableCondition is true. Returns true if it is or if there is no votableCondition.
         This is reevaluated every time the selection changes, so the conditions can be things that change mid-day.*/
        public bool IsVotable()
        {
            if (GetVotableCondition() is null)
            {
                return true;
            }
            return isEnabled && (bool)GetVotableCondition().Invoke(this, null);
        }
    }
    /* This will run a method after a given amount of ingame time passes. Useful if you need to undo an Effect after a certain amount of time.
     * Note: I'm pretty sure this is destroyed on scene change (usually returning to the DeployUI or main menu). If this is an issue for your Effect,
     * you should probably make your own GameObject.
     */
    public class EffectAfterTime : MonoBehaviour 
    {
        public static List<KeyValuePair<MethodInfo, Timer>> effectList;
        private static EffectAfterTime _instance;
        public static EffectAfterTime instance {  
            get 
            { 
                if (_instance is null)
                {
                    GameObject thisThing = new GameObject("EffectAfterTime");
                    _instance = thisThing.AddComponent<EffectAfterTime>();
                }
                return _instance; 
            } 
        }
        // You don't need to make your own EffectAfterTime, use AddEffect instead.
        public EffectAfterTime()
        {
            if (_instance is null)
            {
                effectList = new List<KeyValuePair<MethodInfo, Timer>>();
            }
        }
        // Call from your Effect to add a method to run after the given amount of time.
        public void AddEffect(MethodInfo effect, float time)
        {
            var effectToAdd = new KeyValuePair<MethodInfo, Timer>(effect, new Timer());
            effectToAdd.Value.StartTimer(time);
            effectList.Add(effectToAdd);
        }
        // This runs the timers on the Effects and invokes their method when the time expires.
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
