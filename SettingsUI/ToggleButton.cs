using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class ToggleButton : MonoBehaviour {
    public GameObject buttonObj;
    public Button button;
    public bool on;
    public Image backLightImage;
    // Use this for initialization
    void Awake () {
        on = true;
        RectTransform backLightRect = gameObject.AddComponent<RectTransform>();
        backLightRect.anchoredPosition = new Vector2(0, 0);
        backLightRect.pivot = new Vector2(0f, 0.5f);
        backLightRect.anchorMin = new Vector2(0f, 0.5f);
        backLightRect.anchorMax = new Vector2(0f, 0.5f);
        backLightRect.sizeDelta = new Vector2(40, 40);
        backLightRect.localScale = new Vector3(1, 1, 1);
        backLightImage = gameObject.AddComponent<Image>();
        backLightImage.sprite = (Sprite)Resources.Load("Background", typeof(Sprite));
        backLightImage.type = Image.Type.Sliced;
        backLightImage.color = new Color(255 / 255f, 255 / 255f, 161 / 255f, 255f / 255f);
        buttonObj = new GameObject("ToggleButton");
        button = buttonObj.AddComponent<Button>();
        RectTransform buttonRect = buttonObj.AddComponent<RectTransform>();
        buttonRect.SetParent(backLightRect);
        buttonRect.pivot = new Vector2(0.5f, 0.5f);
        buttonRect.anchorMin = new Vector2(0.5f, 0.5f);
        buttonRect.anchorMax = new Vector2(0.5f, 0.5f);
        buttonRect.sizeDelta = new Vector2(30, 30);
        buttonRect.localScale = new Vector3(1, 1, 1);
        buttonRect.anchoredPosition = new Vector2(0, 0);
        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.sprite = (Sprite)Resources.Load("Button", typeof(Sprite));
        buttonImage.type = Image.Type.Sliced;
        button.targetGraphic = buttonImage;
        button.onClick.AddListener(OnClick);
    }
    void OnClick()
    {
        if (on)
        {
            backLightImage.color = new Color(25f / 255f, 255f / 255f, 255f / 255f, 87f / 255f);
            on = false;
        }
        else
        {
            backLightImage.color = new Color(255 / 255f, 255 / 255f, 161 / 255f, 255f / 255f);
            on = true;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
