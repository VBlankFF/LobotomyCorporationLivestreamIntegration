using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LiveStreamIntegration.SettingsUI
{
    public class UpperPanel : MonoBehaviour
    {
        public GameObject basePanel;
        public CanvasRenderer renderer;
        // Use this for initialization
        void Start()
        {
            renderer = gameObject.AddComponent<CanvasRenderer>();
            RectTransform rect = gameObject.AddComponent<RectTransform>();
            transform.localPosition = new Vector3(0, 0);
            rect.anchorMin = new Vector2(0.5f, 1f);
            rect.anchorMax = new Vector2(0.5f, 1f);
            rect.pivot = new Vector2(0.5f, 1f);
            rect.sizeDelta = new Vector2(400, 170);
            Image thisImage = gameObject.AddComponent<Image>();
            thisImage.sprite = MakeUI.UI;
            thisImage.material = MakeUI.Norwester.material;
            thisImage.color = new Color(0, 0, 0, 0);
            transform.localScale = new Vector3(1, 1, 1);

            GameObject topText = new GameObject("Top Text");
            var topTextRect = topText.AddComponent<RectTransform>();
            topTextRect.SetParent(rect);
            topTextRect.SetParent(this.transform);
            topTextRect.anchorMin = new Vector2(0.5f, 1f);
            topTextRect.anchorMax = new Vector2(0.5f, 1f);
            topTextRect.pivot = new Vector2(0.5f, 0.5f);
            topTextRect.sizeDelta = new Vector2(375, 30);
            topTextRect.localScale = new Vector3(1, 1, 1);
            var topTextText = topText.AddComponent<Text>();
            topTextText.alignment = TextAnchor.UpperLeft;
            topTextText.verticalOverflow = VerticalWrapMode.Overflow;
            topTextText.fontSize = 26;
            topTextText.color = new Color(62f / 255f, 252f / 255f, 164f / 255f);
            topTextText.font = MakeUI.Norwester;
            topTextText.text = "Livestream Integration Settings";
            topTextRect.anchoredPosition = new Vector2(0, -20);

            GameObject enabledText = new GameObject("Enabled Text");
            var enabledTextRect = enabledText.AddComponent<RectTransform>();
            enabledTextRect.SetParent(rect);
            enabledTextRect.SetParent(this.transform);
            enabledTextRect.anchorMin = new Vector2(0f, 1f);
            enabledTextRect.anchorMax = new Vector2(0f, 1f);
            enabledTextRect.pivot = new Vector2(0f, 1f);
            enabledTextRect.sizeDelta = new Vector2(160, 30);
            enabledTextRect.localScale = new Vector3(1, 1, 1);
            var enabledTextText = enabledText.AddComponent<Text>();
            enabledTextText.alignment = TextAnchor.LowerLeft;
            enabledTextText.verticalOverflow = VerticalWrapMode.Overflow;
            enabledTextText.fontSize = 16;
            enabledTextText.color = new Color(62f / 255f, 252f / 255f, 164f / 255f);
            enabledTextText.font = MakeUI.Norwester;
            enabledTextText.text = "Enabled?";
            enabledTextRect.anchoredPosition = new Vector2(4, -25);

            GameObject timeOption = new GameObject("Ingame Time Option");
            timeOption.AddComponent<InGameTimeSetting>();
            timeOption.transform.SetParent(rect);
            timeOption.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -55);
            timeOption.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

            GameObject timeBetweenOption = new GameObject("Time Beween Effects Option");
            timeBetweenOption.AddComponent<TimeBetweenSetting>();
            timeBetweenOption.transform.SetParent(rect);
            timeBetweenOption.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -117.5f);
            timeBetweenOption.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);


        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}