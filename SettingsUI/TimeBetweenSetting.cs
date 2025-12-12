using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeBetweenSetting : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        RectTransform rect = gameObject.AddComponent<RectTransform>();
        rect.pivot = new Vector2(0f, 1f);
        rect.anchorMin = new Vector2(0f, 1f);
        rect.anchorMax = new Vector2(0f, 1f);
        rect.sizeDelta = new Vector2(400, 50);
        rect.localScale = new Vector3(1, 1, 1);

        GameObject settingsText = new GameObject("TimeBetween Text");
        var settingsTextRect = settingsText.AddComponent<RectTransform>();
        settingsTextRect.SetParent(rect);
        settingsTextRect.SetParent(this.transform);
        settingsTextRect.anchorMin = new Vector2(0f, 0.5f);
        settingsTextRect.anchorMax = new Vector2(0f, 0.5f);
        settingsTextRect.pivot = new Vector2(0f, 0.5f);
        settingsTextRect.sizeDelta = new Vector2(300, 50);
        settingsTextRect.localScale = new Vector3(1, 1, 1);
        var settingsTextText = settingsText.AddComponent<Text>();
        settingsTextText.alignment = TextAnchor.MiddleLeft;
        settingsTextText.fontSize = 26;
        settingsTextText.color = new Color(62f / 255f, 252f / 255f, 164f / 255f);
        settingsTextText.font = MakeUI.Norwester;
        settingsTextText.text = "Seconds between effects";
        settingsTextRect.anchoredPosition = new Vector2(6, 0);

        GameObject timeInput = new GameObject("Time Input Box");
        var timeInputRect = timeInput.AddComponent<RectTransform>();
        timeInputRect.SetParent(rect);
        timeInputRect.SetParent(this.transform);
        timeInputRect.anchorMin = new Vector2(0f, 0.5f);
        timeInputRect.anchorMax = new Vector2(0f, 0.5f);
        timeInputRect.pivot = new Vector2(1f, 0.5f);
        timeInputRect.sizeDelta = new Vector2(80, 40);
        timeInputRect.anchoredPosition = new Vector2(385, 0);
        timeInputRect.localScale = new Vector3(1, 1, 1);
        InputField input = timeInput.AddComponent<InputField>();
        Image timeInputImage = timeInput.AddComponent<Image>();
        timeInputImage.sprite = (Sprite)Resources.Load("Background", typeof(Sprite));
        timeInputImage.color = MakeUI.borderColor;
        timeInputImage.type = Image.Type.Sliced;
        timeInputImage.fillCenter = false;
        timeInputImage.material = MakeUI.Norwester.material;

        GameObject timeTextObj = new GameObject("Time Text");
        Text timeText = timeTextObj.AddComponent<Text>();
        RectTransform timeTextRect = timeTextObj.GetComponent<RectTransform>();
        input.textComponent = timeText;
        timeTextRect.SetParent(timeInputRect);
        timeText.alignment = TextAnchor.MiddleCenter;
        timeText.fontSize = 26;
        timeText.color = new Color(62f / 255f, 252f / 255f, 164f / 255f);
        timeText.font = MakeUI.Norwester;
        timeText.text = "120";
        timeText.horizontalOverflow = HorizontalWrapMode.Overflow;
        timeText.verticalOverflow = VerticalWrapMode.Overflow;
        timeTextRect.anchorMin = new Vector2(0f, 0f);
        timeTextRect.anchorMax = new Vector2(1f, 1f);
        timeTextRect.pivot = new Vector2(0.5f, 0.5f);
        timeTextRect.anchoredPosition = new Vector2(0, 0);
        timeTextRect.localScale = new Vector3(1, 1, 1);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
