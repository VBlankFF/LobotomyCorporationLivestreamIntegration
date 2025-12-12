using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LiveStreamIntegration.SettingsUI
{
    public class EffectRowUI : MonoBehaviour
    {
        public GameObject toggleEnabled;
        public ToggleButton toggleEnabledButton;
        public GameObject effectName;
        public Text effectNameText;
        RectTransform effectNameRect;
        // Use this for initialization
        void Awake()
        {
            toggleEnabled = new GameObject("EffectEnabledButton");
            toggleEnabled.transform.SetParent(this.transform);
            toggleEnabledButton = toggleEnabled.AddComponent<ToggleButton>();
            toggleEnabled.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            effectName = new GameObject("EffectName");
            effectName.transform.SetParent(this.transform);
            effectNameText = effectName.AddComponent<Text>();
            Font effFont = (Font)Resources.Load(@"NORWESTER", typeof(Font));
            effectNameText.font = effFont;
            effectNameRect = effectName.GetComponent<RectTransform>();
            effectName.transform.localPosition = new Vector2(40, 0);
            effectNameRect.SetParent(this.transform);
            effectNameRect.anchorMin = new Vector2(0f, 0.5f);
            effectNameRect.anchorMax = new Vector2(0f, 0.5f);
            effectNameRect.pivot = new Vector2(0f, 0.5f);
            effectNameRect.sizeDelta = new Vector2(390, 40);
            effectNameRect.localScale = new Vector3(1, 1, 1);
            effectNameText.alignment = TextAnchor.MiddleLeft;
            effectNameText.fontSize = 26;
            effectNameText.color = new Color(62f / 255f, 252f / 255f, 164f / 255f);
            effectNameRect.anchoredPosition = new Vector2(42, 0);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}