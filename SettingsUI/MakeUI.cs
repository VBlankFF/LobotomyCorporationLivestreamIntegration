using LobotomyBaseMod;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LobotomyBaseModLib;

namespace LiveStreamIntegration.SettingsUI
{
    public class MakeUI : MonoBehaviour
    {
        public static Color borderColor = new Color(62f / 255f, 252f / 255f, 164f / 255f, 1f);
        public static Font Norwester;
        public static Sprite UI;
        public static Sprite UIMask;
        public static Sprite Button;
        public static Sprite Background;
        // Use this for initialization
        void Start()
        {
            AssetBundle uiBundle = Singleton<ModAssetBundleManager>.Instance.bundles["VBlankFF_LiveStreamIntegration"][0];
            UI = (Sprite)uiBundle.LoadAsset("UI", typeof(Sprite));
            UIMask = (Sprite)uiBundle.LoadAsset("UIMask", typeof(Sprite));
            Button = (Sprite)uiBundle.LoadAsset("Button", typeof(Sprite));
            Background = (Sprite)uiBundle.LoadAsset("Background", typeof(Sprite));
            Norwester = GlobalGameManager.currentLanguageFont.GetFont(FontType.TITLE).font;
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
