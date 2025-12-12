using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LiveStreamIntegration.SettingsUI
{
    public class BasePanel : MonoBehaviour
    {
        public GameObject upperPanel;
        public GameObject midPanel;
        public CanvasRenderer renderer;
        // Use this for initialization
        void Start()
        {
            renderer = gameObject.AddComponent<CanvasRenderer>();
            RectTransform rect = gameObject.AddComponent<RectTransform>();
            rect.localPosition = new Vector3(0, 0);
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = new Vector2(400, 570);
            Image thisImage = gameObject.AddComponent<Image>();
            thisImage.sprite = (Sprite)Resources.Load("UI", typeof(Sprite));
            thisImage.material = MakeUI.Norwester.material;
            thisImage.color = new Color(0f, 0f, 0f);
            upperPanel = new GameObject("SUpperPanel");
            upperPanel.transform.SetParent(this.transform);
            upperPanel.AddComponent<UpperPanel>();
            midPanel = new GameObject("SMidPanel");
            midPanel.transform.SetParent(this.transform);
            midPanel.AddComponent<MiddlePanel>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}