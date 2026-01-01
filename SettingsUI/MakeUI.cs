using LobotomyBaseMod;
using LobotomyBaseModLib;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;

namespace LiveStreamIntegration.SettingsUI
{
    public class MakeUI : MonoBehaviour
    {
        public static Color borderColor = new Color(62f / 255f, 252f / 255f, 164f / 255f, 1f);
        public static GameObject borderPanel;
        public static Font Norwester;
        public static Sprite UI;
        public static Sprite UIMask;
        public static Sprite Button;
        public static Sprite Background;
        // Use this for initialization
        public void Init()
        {
            if (GameObject.Find("SCanvas") != null) return;
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
            borderPanel = new GameObject("SBorderPanel");
            borderPanel.transform.SetParent(canv.transform);
            borderPanel.AddComponent<BorderPanel>();
        }

        // Update is called once per frame
        void Update()
        {
            if (NewTitleScript.instance == null && AlterTitleController.Controller == null)
            {
                borderPanel.SetActive(false);
                return;
            }
            if (CreatureInfoWindow.CurrentWindow != null && CreatureInfoWindow.CurrentWindow.IsEnabled)
            {
                borderPanel.SetActive(false);
                return;
            }
            if (OptionUI.Instance != null && OptionUI.Instance.IsEnabled)
            {
                borderPanel.SetActive(false);
                return;
            }
            if (GlobalGameManager.instance.loadingScreen.isLoading)
            {
                borderPanel.SetActive(false);
                return;
            }
            if (NewTitleScript.instance != null && (bool)typeof(NewTitleScript).GetField("isNewGame", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(NewTitleScript.instance))
            {
                borderPanel.SetActive(false);
                return;
            }
            borderPanel.SetActive(true);
        }
    }
}
