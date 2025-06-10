using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Collections;
using SimpleJSON;
using UnityEngine.UI;

public class ChatUIManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject chatPanel;
    public TMP_InputField inputField;
    public Transform contentArea;
    public GameObject userBubblePrefab;
    public GameObject botBubblePrefab;
    public GameObject Point;

    [Header("GPT API")]
    [TextArea(6, 10)]
    public string systemPrompt = @"넌 공포 게임 속 NPC야. 아래 질문이 들어오면 반드시 정해진 대답만 해.

[질문] 힌트 좀 알려줘
[대답] 원장실에 단서가 있어

[질문] 어디로 가야 해?
[대답] 지하 벙커로 내려가.

[질문] 이 집 누구 거야?
[대답] 예전엔 수녀원이었어.

그 외의 질문엔 항상 '…….'라고만 대답해.";

    [HideInInspector]
    public string apiKey =  // 네 API 키 넣기

    [Header("Control")]
    public MonoBehaviour playerController;

    public static bool isChatOpen = false;
    private bool isSending = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            
            isChatOpen = !isChatOpen;
            Point.SetActive(!isChatOpen);
            
           // 기존 말풍선 메시지 전부 삭제
            foreach (Transform child in contentArea)
            {
                Destroy(child.gameObject);
            }
            chatPanel.SetActive(isChatOpen);
           

            if (playerController != null)
                playerController.enabled = !isChatOpen;

            if (isChatOpen)
            {
                inputField.text = "";
                inputField.ActivateInputField();
                inputField.Select();
                    
            }
            
        }

        if (isChatOpen && Input.GetKeyDown(KeyCode.Return) && !string.IsNullOrWhiteSpace(inputField.text))
        {
            string userText = inputField.text.Trim();
            AddMessage(userText, true);
            StartCoroutine(SendToGPT(userText));
            inputField.text = "";
            inputField.ActivateInputField();
            inputField.Select();

            
        }
    }

    void AddMessage(string message, bool isUser)
    {
        GameObject prefab = isUser ? userBubblePrefab : botBubblePrefab;
        GameObject bubble = Instantiate(prefab, contentArea);
        TMP_Text text = bubble.GetComponentInChildren<TMP_Text>();
        if (text != null)
            text.text = message;

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)contentArea);
    }

    IEnumerator SendToGPT(string userInput)
    {
        if (isSending) yield break;
        isSending = true;

        string url = "https://api.openai.com/v1/chat/completions";
        string escapedPrompt = EscapeJson(systemPrompt);
        string escapedInput = EscapeJson(userInput);

        string json = "{\"model\":\"gpt-3.5-turbo\"," +
                      "\"messages\":[" +
                          "{\"role\":\"system\",\"content\":\"" + escapedPrompt + "\"}," +
                          "{\"role\":\"user\",\"content\":\"" + escapedInput + "\"}" +
                      "]}";

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", $"Bearer {apiKey}");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string result = request.downloadHandler.text;
            var root = JSON.Parse(result);
            string response = root["choices"][0]["message"]["content"];
            AddMessage(response.Trim(), false);
        }
        else
        {
            Debug.LogError("GPT 연결 실패: " + request.responseCode);
            AddMessage("GPT 응답 실패: 다시 시도해주세요.", false);
        }

        isSending = false;
    }

    // 🔧 JSON 문자열 이스케이프 함수
    string EscapeJson(string s)
    {
        return s.Replace("\\", "\\\\")
                .Replace("\"", "\\\"")
                .Replace("\n", "\\n")
                .Replace("\r", "");
    }
}
