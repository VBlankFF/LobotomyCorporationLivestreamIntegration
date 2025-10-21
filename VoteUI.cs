using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace LiveStreamIntegration
{
    public class VoteUI : MonoBehaviour
    {
        public Canvas voteCanvas;
        public GameObject voteCanvasObject;
        public Text topText;
        public Text timerText;
        public GameObject topTextObject;
        public VoteOptionRow[] voteOptions;
        public void Init()
        {
            try
            {
                voteCanvasObject = new GameObject();
                voteCanvasObject.name = "VoteCanvas";
                voteCanvasObject.AddComponent<Canvas>();
                voteCanvas = voteCanvasObject.GetComponent<Canvas>();
                topTextObject = new GameObject();
                topTextObject.name = "VoteTopText";
                topText = topTextObject.AddComponent<Text>();
                topText.text = "Vote for an effect!";
                topText.alignment = TextAnchor.LowerRight;
                GameObject timerTextObject = new GameObject();
                timerText = timerTextObject.AddComponent<Text>();
                timerText.text = "(0s)";
                RectTransform timerTextTrans = timerTextObject.GetComponent<RectTransform>();
                timerTextObject.transform.SetParent(voteCanvasObject.transform);
                RectTransform topTextTransform = topTextObject.GetComponent<RectTransform>();
                topTextTransform.SetParent(voteCanvasObject.transform);
                voteCanvasObject.transform.position = new Vector3(0, 0, 0);
                timerTextTrans.anchoredPosition = new Vector2(Screen.width / 2 - 30, Screen.height / -2 + 150);
                topTextTransform.anchoredPosition = new Vector2(Screen.width / 2 - 170, Screen.height / -2 + 150);
                RectTransform canvasTransform = voteCanvasObject.GetComponent<RectTransform>();
                topTextTransform.sizeDelta = new Vector2(200, 100);
                voteCanvas.gameObject.SetActive(true);
                topText.gameObject.SetActive(true);
                Font arial;
                arial = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
                topText.fontSize = 20;
                topText.font = arial;
                timerText.fontSize = 20;
                timerText.font = arial;
                timerText.alignment = TextAnchor.LowerCenter;
                voteCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                voteOptions = new VoteOptionRow[Constants.NUM_VOTING_OPTIONS];
                voteOptions[0] = new VoteOptionRow("1", "Option1", new Vector2(Screen.width - 270, 85));
                voteOptions[0].voteOptionRowObj.transform.SetParent(voteCanvasObject.transform);
                voteOptions[1] = new VoteOptionRow("2", "Option2", new Vector2(Screen.width - 270, 60));
                voteOptions[1].voteOptionRowObj.transform.SetParent(voteCanvasObject.transform);
                voteOptions[2] = new VoteOptionRow("3", "Option3", new Vector2(Screen.width - 270, 35));
                voteOptions[2].voteOptionRowObj.transform.SetParent(voteCanvasObject.transform);
                Harmony_Patch.UpdateVoteUINames();
            }
            catch (Exception ex)
            {
                LobotomyBaseMod.ModDebug.Log(ex.ToString());
            }
        }
        public class VoteOptionRow
        {
            public GameObject voteOptionRowObj;
            public Text optionId;
            public Text numVotes;
            public Text optionName;
            public VoteOptionRow(string optionId, string optionName, Vector2 position)
            {
                this.voteOptionRowObj = new GameObject("voteOptionRowObj");
                this.voteOptionRowObj.transform.position = position;
                this.Init(optionId);
                this.SetName(optionName);

            }
            public void Init(string strOptionId)
            {
                // is there a way to make this better?
                GameObject optionIdObj = new GameObject("VoteOptionId");
                GameObject numVotesObj = new GameObject("NumVotes");
                GameObject optionNameObj = new GameObject("VoteOptionName");
                optionId = optionIdObj.AddComponent<Text>();
                numVotes = numVotesObj.AddComponent<Text>();
                optionName = optionNameObj.AddComponent<Text>();
                optionId.alignment = TextAnchor.MiddleLeft;
                numVotes.alignment = TextAnchor.MiddleCenter;
                optionName.alignment = TextAnchor.MiddleLeft;
                RectTransform optionIdTrans = optionIdObj.GetComponent<RectTransform>();
                RectTransform numVotesTrans = numVotesObj.GetComponent<RectTransform>();
                RectTransform optionNameTrans = optionNameObj.GetComponent<RectTransform>();
                optionIdTrans.SetParent(voteOptionRowObj.transform);
                numVotesTrans.SetParent(voteOptionRowObj.transform);
                optionNameTrans.SetParent(voteOptionRowObj.transform);
                optionIdTrans.offsetMax = new Vector2(30, 30);
                numVotesTrans.offsetMax = new Vector2(50, 30);
                optionNameTrans.offsetMax = new Vector2(200, 30);
                optionIdTrans.offsetMin = new Vector2(0, -30);
                numVotesTrans.offsetMin = new Vector2(0, -30);
                optionNameTrans.offsetMin = new Vector2(0, -30);
                optionIdTrans.localPosition = new Vector3();
                numVotesTrans.localPosition = new Vector3(240, 0);
                optionNameTrans.localPosition = new Vector3(110, 0);
                optionId.text = strOptionId + ":";
                numVotes.text = "(0)";
                optionName.text = "null";
                Font arial;
                arial = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
                optionId.font = arial;
                numVotes.font = arial;
                optionName.font = arial;
                optionId.fontSize = 20;
                numVotes.fontSize = 20;
                optionName.fontSize = 20;
                optionIdObj.SetActive(true);
                numVotesObj.SetActive(true);
                optionNameObj.SetActive(true);
            }
            public void SetName(string name)
            {
                optionName.text = name;
            }
            public void SetNumVotes(int numVotes) 
            {
                this.numVotes.text = "(" + numVotes + ")";
            }
        }
        public void SetVoteTime(int voteTime)
        {
            timerText.text = "(" + voteTime.ToString() + ")";
        }
    }
}
