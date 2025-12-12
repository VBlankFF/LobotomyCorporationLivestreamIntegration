using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LiveStreamIntegration.SettingsUI
{
    public class MakeUI : MonoBehaviour
    {
        public static Color borderColor = new Color(62f / 255f, 252f / 255f, 164f / 255f, 1f);
        public static Font Norwester;
        // Use this for initialization
        void Start()
        {
            Norwester = (Font)Resources.Load(@"NORWESTER");
            GameObject canvObj = new GameObject("SCanvas");
            Canvas canv = canvObj.AddComponent<Canvas>();
            canv.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler canvScaler = canvObj.AddComponent<CanvasScaler>();
            canvScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvScaler.referenceResolution = new Vector2(1920, 1080);
            GameObject borderPanelObj = new GameObject("SBorderPanel");
            borderPanelObj.transform.SetParent(canv.transform);
            borderPanelObj.AddComponent<BorderPanel>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
