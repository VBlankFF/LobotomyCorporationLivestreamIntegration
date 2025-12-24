using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LiveStreamIntegration;

namespace LiveStreamIntegration.SettingsUI
{
    public class InGameTimeSetting : MonoBehaviour
    {

        // Use this for initialization
        void Awake()
        {
            RectTransform rect = gameObject.AddComponent<RectTransform>();
            rect.pivot = new Vector2(0f, 1f);
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(0f, 1f);
            rect.sizeDelta = new Vector2(400, 40);
            rect.localScale = new Vector3(1, 1, 1);

            GameObject button = new GameObject("Toggle Button");
            ToggleButton tButton = button.AddComponent<ToggleButton>();
            tButton.transform.SetParent(rect);
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(2.5f, 0);
            tButton.parent = this;
            tButton.OnPress = typeof(InGameTimeSetting).GetMethod("ChangeSetting");
            

            GameObject settingsText = new GameObject("InGameUISetting Text");
            var settingsTextRect = settingsText.AddComponent<RectTransform>();
            settingsTextRect.SetParent(rect);
            settingsTextRect.SetParent(this.transform);
            settingsTextRect.anchorMin = new Vector2(0f, 0.5f);
            settingsTextRect.anchorMax = new Vector2(0f, 0.5f);
            settingsTextRect.pivot = new Vector2(0f, 0.5f);
            settingsTextRect.sizeDelta = new Vector2(335, 80);
            settingsTextRect.localScale = new Vector3(1, 1, 1);
            var settingsTextText = settingsText.AddComponent<Text>();
            settingsTextText.alignment = TextAnchor.MiddleLeft;
            settingsTextText.fontSize = 26;
            settingsTextText.color = new Color(62f / 255f, 252f / 255f, 164f / 255f);
            settingsTextText.font = MakeUI.Norwester;
            settingsTextText.text = "Use ingame time\n(Uses real time if disabled) ";
            settingsTextRect.anchoredPosition = new Vector2(45, -15);
        }

        public static void ChangeSetting(bool newState)
        {
            Settings.IsIngameTime = newState;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}