using SocketIO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatNetwork : MonoBehaviour
{
    private SocketIOComponent socket;
    private ChatManager chatManager;

    void Start()
    {
        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();
        chatManager = GetComponent<ChatManager>();

        socket.On("open", TestOpen);
        socket.On("error", TestError);
        socket.On("close", TestClose);

        socket.On("broadcastMsg", OnBroadcastMsg);
        socket.On("joinNewUser", OnJoinNewUser);
        socket.On("disconnetUser", OnDissconnectUser);

        socket.On("playGame", OnPlayGame);
        socket.On("responseQuiz", OnResponseQuiz);
    }

    #region 송신 이벤트 처리
    // 새로운 메시지 송신 이벤트
    public void SendMsgToServer(string msg)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("msg", msg);
        JSONObject jObject = new JSONObject(data);
        socket.Emit("newMsg", jObject);
    }

    // 새로운 사용자 접속 송신 이벤트
    public void JoinNewUser()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("userName", GameManager.instance.UserName);
        JSONObject jObject = new JSONObject(data);
        socket.Emit("joinNewUser", jObject);
    }

    // 퀴즈 요청 메소드
    internal void RequestQuiz()
    {
        socket.Emit("requestQuiz");
    }
    #endregion

    #region 수신 이벤트 처리
    // 메시지 수신 이벤트
    private void OnBroadcastMsg(SocketIOEvent obj)
    {
        chatManager.SendMsgToChat(obj.data.GetField("msg").str, obj.data.GetField("userName").str);
    }

    // 새로운 사용자 접속 알림 이벤트
    private void OnJoinNewUser(SocketIOEvent obj)
    {
        chatManager.JoinNewUserToChat(obj.data.GetField("userName").str);
    }

    // 게임 시작 알림 이벤트
    private void OnPlayGame(SocketIOEvent obj)
    {
        chatManager.PlayGame();
    }

    // 서버가 새로운 퀴즈를 내려줌.
    private void OnResponseQuiz(SocketIOEvent obj)
    {
        //GetComponent<soundManager>().AudioPlayNewQuestion();
        print("OnResponseQuiz() 받음");
        chatManager.ShowQuiz(obj.data.GetField("quiz").str);
    }

    // 사용자 접속 끊김 알림 이벤트
    private void OnDissconnectUser(SocketIOEvent obj)
    {
        chatManager.DisconnectUserToChat(obj.data.GetField("userName").str);
    }
















    public void TestOpen(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
    }

    public void TestError(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Error received: " + e.name + " " + e.data);
    }

    public void TestClose(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Close received: " + e.name + " " + e.data);
    }


    #endregion
}