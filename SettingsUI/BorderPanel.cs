using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace LiveStreamIntegration.SettingsUI
{
    public class BorderPanel : MonoBehaviour
    {
        public GameObject basePanel;
        public CanvasRenderer renderer;
        // Use this for initialization
        void Start()
        {
            renderer = gameObject.AddComponent<CanvasRenderer>();
            RectTransform rect = gameObject.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.localPosition = new Vector3(0, 0);
            rect.sizeDelta = new Vector2(408, 578);
            Image thisImage = gameObject.AddComponent<Image>();
            thisImage.sprite = MakeUI.UI;
            thisImage.material = MakeUI.Norwester.material;
            thisImage.color = new Color(62f / 255f, 252f / 255f, 164f / 255f);
            basePanel = new GameObject("SBasePanel");
            basePanel.transform.SetParent(this.transform);
            basePanel.AddComponent<BasePanel>();
            gameObject.AddComponent<GraphicRaycaster>();
        }

        // Update is called once per frame
        void Update()
        {

        }

    }
}