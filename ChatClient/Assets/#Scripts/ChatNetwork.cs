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
        socket = go.GetComponent<SocketIOComponent>(); // socket.io 가져오기
        chatManager = GetComponent<ChatManager>();  // ChatManager.cs 가져오기          

        socket.On("open", TestOpen);
        socket.On("error", TestError);
        socket.On("close", TestClose);

        socket.On("broadcastMsg", OnBroadcastMsg);

    }

    #region 송신 메시지 처리
    public void SendMsgToServer(string msg)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("msg", msg);
        JSONObject jObject = new JSONObject(data);
        socket.Emit("newMsg", jObject);
    }
    #endregion

    #region 수신 이벤트 처리
    private void OnBroadcastMsg(SocketIOEvent obj)
    {
        chatManager.SendMsgToChat(obj.data.GetField("msg").str);
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