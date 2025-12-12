using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LiveStreamIntegration.SettingsUI
{
    public class MiddlePanel : MonoBehaviour
    {
        public CanvasRenderer renderer;
        public GameObject effectScrollPanel;
        // Use this for initialization
        void Start()
        {
            renderer = gameObject.AddComponent<CanvasRenderer>();
            RectTransform rect = gameObject.AddComponent<RectTransform>();
            transform.localPosition = new Vector3(0, -170);
            transform.localScale = new Vector3(1, 1, 1);
            rect.anchorMin = new Vector2(0.5f, 1f);
            rect.anchorMax = new Vector2(0.5f, 1f);
            rect.pivot = new Vector2(0.5f, 1f);
            rect.sizeDelta = new Vector2(408, 404);
            Image thisImage = gameObject.AddComponent<Image>();
            thisImage.sprite = (Sprite)Resources.Load("Background", typeof(Sprite));
            thisImage.material = MakeUI.Norwester.material;
            thisImage.color = new Color(62f / 255f, 252f / 255f, 164f / 255f);
            effectScrollPanel = new GameObject("SEffectScrollPanel");
            effectScrollPanel.transform.SetParent(this.transform);
            effectScrollPanel.AddComponent<EffectScrollPanel>();
            thisImage.type = Image.Type.Sliced;
            thisImage.fillCenter = false;

            GameObject enabledTextObj = new GameObject("EnabledText");
            RectTransform enabledTextRect = enabledTextObj.AddComponent<RectTransform>();
            enabledTextRect.SetParent(transform);
            enabledTextRect.anchoredPosition = new Vector2(8, 0);
            enabledTextRect.anchorMin = new Vector2(0f, 1f);
            enabledTextRect.anchorMax = new Vector2(0f, 1f);
            enabledTextRect.pivot = new Vector2(0f, 1f);
            enabledTextRect.sizeDelta = new Vector2(160, 30);
            enabledTextRect.localScale = new Vector3(1, 1, 1);
            Text enabledTextText = enabledTextObj.AddComponent<Text>();
            enabledTextText.alignment = TextAnchor.LowerLeft;
            enabledTextText.font = MakeUI.Norwester;
            enabledTextText.fontSize = 16;
            enabledTextText.color = MakeUI.borderColor;
            enabledTextText.text = "Enabled?";

            GameObject effectsTextObj = new GameObject("EffectsText");
            RectTransform effectsTextRect = effectsTextObj.AddComponent<RectTransform>();
            effectsTextRect.SetParent(transform);
            effectsTextRect.anchoredPosition = new Vector2(0, -5);
            effectsTextRect.anchorMin = new Vector2(0.5f, 1f);
            effectsTextRect.anchorMax = new Vector2(0.5f, 1f);
            effectsTextRect.pivot = new Vector2(0.5f, 1f);
            effectsTextRect.sizeDelta = new Vector2(160, 30);
            effectsTextRect.localScale = new Vector3(1, 1, 1);
            Text effectsTextText = effectsTextObj.AddComponent<Text>();
            effectsTextText.alignment = TextAnchor.MiddleCenter;
            effectsTextText.font = MakeUI.Norwester;
            effectsTextText.fontSize = 25;
            effectsTextText.color = MakeUI.borderColor;
            effectsTextText.text = "Effects";
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}