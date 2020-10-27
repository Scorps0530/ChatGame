using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    // UI 요소 연결
    public GameObject contentPanel, textPrefab;
    public InputField chatInputField;
    private ChatNetwork chatNetwork;

    private void Start()
    {
        chatNetwork = GetComponent<ChatNetwork>();
        Invoke("JoinNewUser", 1f);
    }

    void Update()
    {
        if (chatInputField.text != "")
        {
            // [Enter]키가 눌리면 화면에 InputField에 입력된 텍스트 출력
            if (Input.GetKeyDown(KeyCode.Return))
            {
                //SendMsgToChat(chatInputField.text);
                chatNetwork.SendMsgToServer(chatInputField.text);
                chatInputField.text = "";
                chatInputField.ActivateInputField();    // InputField에 포커스두기
            }
        }
        else
        {
            if (!chatInputField.isFocused && Input.GetKeyDown(KeyCode.Return))
            {
                chatInputField.ActivateInputField();
            }
        }
    }

    // 새로운 사용자 접속 알림
    private void JoinNewUser()
    {
        chatNetwork.JoinNewUser();
    }

    // 채팅창에 새로운 텍스트를 출력하는 메서드
    public void SendMsgToChat(string text)
    {
        GameObject newText = Instantiate(textPrefab, contentPanel.transform);
        newText.GetComponent<Text>().text = text;
    }

    public void JoinNewUserToChat(string userName)
    {
        GameObject newText = Instantiate(textPrefab, contentPanel.transform);
        newText.GetComponent<Text>().text = $"{userName}님이 입장하셨습니다.";
    }

    internal void DisconnectUserToChat(string userName)
    {
        GameObject newText = Instantiate(textPrefab, contentPanel.transform);
        newText.GetComponent<Text>().text = $"{userName}님이 퇴장하셨습니다.";
    }
}
