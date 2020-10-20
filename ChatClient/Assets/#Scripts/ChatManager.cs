using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    public GameObject chatPanel, textPrefab;
    public InputField chatBox;

    private void Update()
    {
        // 키보드 입력 감지 -> 입력한 내용을 SendMagToServer() 메서드로 전달
        if(Input.GetKeyDown(KeyCode.Return)) // Enter 키
        {
            // 입력한 내용을 SendMagToServer() 메서드로 전달
        }
    }
}
