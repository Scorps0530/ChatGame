using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    private ChatNetwork chatNetwork;

    /* Chat UI */
    [SerializeField] private GameObject contentPanel, textPrefab;
    [SerializeField] private InputField chatInputField;

    /* Quiz UI */
    public Text QuizText;
    public Text AlertText;

    /* 파티클 효과 */
    public GameObject rightAnswerEffect, wrongAnswerEffect;
    public Transform answerEffectPosition;

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

    // 현재 사용자 접속 알림을 서버로 전송
    private void JoinNewUser()
    {
        chatNetwork.JoinNewUser();
    }

    // 새로운 사용자 접속 알림 텍스트 출력 메서드 
    public void JoinNewUserToChat(string userName)
    {
        GameObject newText = Instantiate(textPrefab, contentPanel.transform);
        newText.GetComponent<Text>().text = $"{userName}님이 입장하셨습니다.";
    }

    // 채팅창에 새로운 텍스트를 출력 메서드
    public void SendMsgToChat(string msg, string userName)
    {
        GameObject newText = Instantiate(textPrefab, contentPanel.transform);
        Text chatText = newText.GetComponent<Text>();

        if (userName != null)
        {
            if (GameManager.instance.UserName == userName)
                chatText.color = new Color32(255, 255, 255, 255);
            else
                chatText.color = new Color32(255, 150, 150, 255);

            chatText.text = $"{userName} : {msg}";
        }
        else
        {
            print("userName이 null값 입니다.");
        }
    }

    // 화면에 퀴즈 출력
    internal void ShowQuiz(string quiz)
    {
        Debug.Log("Show Quiz");
        QuizText.text = quiz;
    }

    // 게임 시작 메서드
    public void PlayGame()
    {
        Debug.Log("Play Game");
        StartCoroutine(Countdown(5));
    }

    private IEnumerator Countdown(int count)
    {
        for (int i = count; i >= 0; i--)
        {
            AlertText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        AlertText.text = "";
        chatNetwork.RequestQuiz();     // 서버에 퀴즈 요청
    }

    public void RightAnswer(string userName)
    {
        AlertText.text = $"{userName} 정답!";
        //GameObject effect = Instantiate(rightAnswerEffect, answerEffectPosition.position, Quaternion.identity);
        //Destroy(effect, 2f);
        Invoke("RequestQuiz", 3f);
    }

    public void WrongAnswer()
    {
        AlertText.text = "오답!!!";
        //GameObject effect = Instantiate(wrongAnswerEffect, answerEffectPosition.position, Quaternion.identity);
        //Destroy(effect, 2f);
    }

    // 사용자 퇴장 알림 출력 메서드
    internal void DisconnectUserToChat(string userName)
    {
        GameObject newText = Instantiate(textPrefab, contentPanel.transform);
        newText.GetComponent<Text>().text = $"{userName}님이 퇴장하셨습니다.";
    }
}