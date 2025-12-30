using LobotomyBaseMod;
using LobotomyBaseModLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml;
using System.Xml.XPath;

namespace LiveStreamIntegration
{
    public static class Settings
    {
        public static string CONFIG_NAME = "config.txt";
        public static string EFFECT_CONFIG_NAME = "effectconfig.txt";
        static Settings()
        {
            // Get the path of the config file
            string path = Assembly.GetExecutingAssembly().CodeBase;
            // Get rid of a certain troublesome string at the start
            path = path.Remove(0, 8);
            configPath = path.Remove(path.LastIndexOf('/') + 1) + @"Config/";
            if (!Directory.Exists(configPath))
            {
                Directory.CreateDirectory(configPath);
            }  
        }
        public static string configPath;
        public static Dictionary<EffectIdentity, bool> effectSettings = new Dictionary<EffectIdentity, bool>();
        private static int _timeBetweenEffects;
        private static bool _isIngameTime;
        public static int TimeBetweenEffects { get { return _timeBetweenEffects; } set { _timeBetweenEffects = value; } }
        public static bool IsIngameTime { get { return _isIngameTime; } set { _isIngameTime = value; } }
        public static void LoadSettings()
        {
            if (!File.Exists(configPath + CONFIG_NAME))
            {
                TimeBetweenEffects = 120;
                IsIngameTime = true;
                SaveSettings();
                return;
            }
            try
            {
                XmlDocument config = new XmlDocument();
                string fileContent = File.ReadAllText(configPath + CONFIG_NAME);
                if (fileContent.Length == 0)
                {
                    TimeBetweenEffects = 120;
                    IsIngameTime = true;
                    SaveSettings();
                    return;
                }
                config.LoadXml(fileContent);
                IsIngameTime = Boolean.Parse(config.SelectSingleNode("config/UseIngameTime").InnerText);
                TimeBetweenEffects = Int32.Parse(config.SelectSingleNode("config/SecondsBetweenEffects").InnerText);
            }
            catch (Exception ex)
            {
                TimeBetweenEffects = 120;
                IsIngameTime = true;
                UnityEngine.Debug.Log("LiveStreamIntegration: The config file failed to load. The default values have been loaded instead. Exception is " + ex.Message);
            }
        }
        public static void LoadEffectSettings()
        {
            if (!File.Exists(configPath + EFFECT_CONFIG_NAME))
            {
                File.Create(configPath + EFFECT_CONFIG_NAME).Dispose();
                SaveEffectSettings();
                return;
            }
            XmlDocument config = new XmlDocument();
            try
            {
                string fileContent = File.ReadAllText(configPath + EFFECT_CONFIG_NAME);
                if (fileContent.Length == 0)
                {
                    return;
                }
                config.LoadXml(fileContent);
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log("LiveStreamIntegration: The effect config file failed to load. The default values have been loaded instead. Exception is " + ex.Message);
            }
            foreach (XmlNode i in config.FirstChild.ChildNodes)
            {
                EffectIdentity eff = new EffectIdentity(i.Attributes.GetNamedItem("name").Value, i.InnerText);
                effectSettings[eff] = Boolean.Parse(i.Attributes.GetNamedItem("enabled").Value);
            }    
        }
        public static void SaveSettings()
        {
            try 
            {
                if (!File.Exists(configPath + CONFIG_NAME))
                {
                    File.Create(configPath + CONFIG_NAME).Dispose();
                }
                XmlDocument config = new XmlDocument();
                XmlElement root = config.CreateElement("config");
                XmlNode IngameTimeSetting = config.CreateElement("UseIngameTime");
                XmlNode SecondsBetweenEffectsSetting = config.CreateElement("SecondsBetweenEffects");
                IngameTimeSetting.InnerText = IsIngameTime.ToString();
                SecondsBetweenEffectsSetting.InnerText = TimeBetweenEffects.ToString();
                config.AppendChild(root);
                root.AppendChild(IngameTimeSetting);
                root.AppendChild(SecondsBetweenEffectsSetting);
                config.Save(configPath + CONFIG_NAME);
                SaveEffectSettings();
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log("LiveStreamIntegration: The config file failed to save. Exception is " + ex.Message);
            }
        }
        public static void SaveEffectSettings()
        {
            try
            {
                if (!File.Exists(configPath + EFFECT_CONFIG_NAME))
                {
                    File.Create(configPath + EFFECT_CONFIG_NAME).Dispose();
                }
                XmlDocument config = new XmlDocument();
                XmlElement root = config.CreateElement("config");
                foreach (KeyValuePair<EffectIdentity, bool> eff in effectSettings)
                {
                    EffectIdentity ident = eff.Key;
                    XmlElement effElem = config.CreateElement("Effect");
                    effElem.SetAttribute("name", ident.name);
                    effElem.SetAttribute("enabled", eff.Value.ToString());
                    effElem.InnerText = ident.source;
                    root.AppendChild(effElem);
                }
                config.AppendChild(root);
                config.Save(configPath + EFFECT_CONFIG_NAME);
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log("LiveStreamIntegration: The effects config file failed to save. Exception is " + ex.Message);
            }
        }
    }
    public enum EffectTimeSetting
    {
        IngameTime,
        FixedTime
    }
    public class EffectIdentity
    {
        public string name;
        public string source;
        public EffectIdentity(string name, string source)
        {
            this.name = name;
            this.source = source;
        }
        public bool Equals(EffectIdentity other)
        {
            if (other is null) return false;
            return this.name.Equals(other.name) && this.source.Equals(other.source);
        }
    }
}
