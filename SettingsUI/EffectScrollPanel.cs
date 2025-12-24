using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LiveStreamIntegration.SettingsUI
{
    public class EffectScrollPanel : MonoBehaviour
    {
        public CanvasRenderer cRenderer;
        public GameObject content;
        public RectTransform contentrect;
        public GameObject viewPort;
        public GameObject scrollBar;
        public int numRows;
        // Use this for initialization
        void Start()
        {
            numRows = 0;
            cRenderer = gameObject.AddComponent<CanvasRenderer>();
            RectTransform rect = gameObject.AddComponent<RectTransform>();
            ScrollRect screct = gameObject.AddComponent<ScrollRect>();
            rect.anchorMin = new Vector2(0.5f, 1f);
            rect.anchorMax = new Vector2(0.5f, 1f);
            rect.pivot = new Vector2(0.5f, 1f);
            rect.sizeDelta = new Vector2(408, 360);
            transform.localPosition = new Vector3(0, -35);
            Image thisImage = gameObject.AddComponent<Image>();
            thisImage.sprite = MakeUI.Background;
            thisImage.type = Image.Type.Sliced;
            thisImage.fillCenter = false;
            thisImage.material = MakeUI.Norwester.material;
            Color noColor = new Color();
            noColor.a = 0;
            thisImage.color = noColor;
            transform.localScale = new Vector3(1, 1, 1);
            viewPort = new GameObject("EffectViewPort");
            viewPort.AddComponent(typeof(Mask));
            screct.viewport = viewPort.GetComponent<RectTransform>();
            screct.viewport.transform.SetParent(rect);
            viewPort.transform.localPosition = new Vector3(0, 0);
            screct.viewport.anchorMin = new Vector2(0.5f, 1f);
            screct.viewport.anchorMax = new Vector2(0.5f, 1f);
            screct.viewport.pivot = new Vector2(0.5f, 1f);
            screct.viewport.sizeDelta = new Vector2(400, 360);
            screct.viewport.localScale = new Vector3(1, 1, 1);
            viewPort.transform.localPosition = new Vector3(0, 0);
            Image viewPortMask = viewPort.AddComponent<Image>();
            viewPortMask.type = Image.Type.Tiled;
            viewPortMask.color = new Color(0, 0, 0, 1);
            screct.scrollSensitivity = 20;
            screct.inertia = false;

            content = new GameObject("Content");
            content.transform.SetParent(viewPort.transform);
            contentrect = content.AddComponent<RectTransform>();
            content.transform.localPosition = new Vector3(0, 0);
            contentrect.anchorMin = new Vector2(0.5f, 1f);
            contentrect.anchorMax = new Vector2(0.5f, 1f);
            contentrect.pivot = new Vector2(0.5f, 1f);
            contentrect.sizeDelta = new Vector2(390, 0);
            contentrect.localScale = new Vector3(1, 1, 1);
            content.transform.localPosition = new Vector3(0, 0);
            screct.content = contentrect;

            scrollBar = new GameObject("Scrollbar");
            RectTransform scrollBarRect = scrollBar.AddComponent<RectTransform>();
            Scrollbar scrollBarBar = scrollBar.AddComponent<Scrollbar>();
            scrollBarBar.direction = Scrollbar.Direction.BottomToTop;
            screct.verticalScrollbar = scrollBarBar;
            scrollBarRect.SetParent(transform);
            scrollBarRect.anchoredPosition = new Vector2(-5, -6);
            scrollBarRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 20);
            scrollBarRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, -12);
            scrollBarRect.anchorMin = new Vector2(1f, 0f);
            scrollBarRect.anchorMax = new Vector2(1f, 1f);
            scrollBarRect.pivot = new Vector2(1f, 1f);
            scrollBarRect.localScale = new Vector3(1, 1, 1);
            scrollBar.AddComponent<Image>().sprite = MakeUI.Background;
            scrollBar.GetComponent<Image>().type = Image.Type.Sliced;


            GameObject slidingArea = new GameObject("Sliding Area");
            RectTransform slidingAreaRect = slidingArea.AddComponent<RectTransform>();
            slidingAreaRect.SetParent(scrollBarRect);
            slidingAreaRect.anchoredPosition = new Vector2(0, 0);
            slidingAreaRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, -20);
            slidingAreaRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, -20);
            slidingAreaRect.anchorMin = new Vector2(0f, 0f);
            slidingAreaRect.anchorMax = new Vector2(1f, 1f);
            slidingAreaRect.pivot = new Vector2(0.5f, 0.5f);
            slidingAreaRect.localScale = new Vector3(1, 1, 1);

            GameObject handle = new GameObject("Handle");
            RectTransform handleRect = handle.AddComponent<RectTransform>();
            handleRect.SetParent(slidingAreaRect);
            handleRect.anchoredPosition = new Vector2(0, 0);
            handleRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 20);
            handleRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 20);
            handleRect.anchorMin = new Vector2(0f, 0.61327f);
            handleRect.anchorMax = new Vector2(1f, 1f);
            handleRect.pivot = new Vector2(0.5f, 0.5f);
            handleRect.localScale = new Vector3(1, 1, 1);
            Image handleImage = handle.AddComponent<Image>();
            handleImage.sprite = MakeUI.Button;
            handleImage.type = Image.Type.Sliced;
            scrollBarBar.targetGraphic = handleImage;
            scrollBarBar.handleRect = handleRect;
            foreach (Effect eff in LiveStreamIntegration.Harmony_Patch.effects)
            {
                AddEffect(eff);
            }

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void AddEffect(Effect eff)
        {
            GameObject effect = new GameObject("EffectRowUI");
            RectTransform effRect = effect.AddComponent<RectTransform>();
            effRect.SetParent(content.transform);
            effRect.anchorMin = new Vector2(0.5f, 1f);
            effRect.anchorMax = new Vector2(0.5f, 1f);
            effRect.pivot = new Vector2(0.5f, 1f);
            effRect.sizeDelta = new Vector2(390, 40);
            effRect.localScale = new Vector3(1, 1, 1);
            effRect.anchoredPosition = new Vector3(-1f, numRows * -40);
            EffectRowUI row = effect.AddComponent<EffectRowUI>();
            row.associatedEffect = eff;
            row.effectNameText.text = eff.name;
            if (!eff.isEnabled)
            {
                row.toggleEnabledButton.ForceOff();
            }
            numRows++;
            contentrect.sizeDelta = new Vector2(390, numRows * 40);
        }
    }
}