using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickJoinBtn : MonoBehaviour
{
    [SerializeField] private InputField inputField_userName;

    public void SetUserNameAndLoadNextScene()
    {
        if (inputField_userName.text != "")
        {
            GameManager.instance.UserName = inputField_userName.text;
            GameManager.instance.LoadNextScene();
        }
        else
        {
            print("닉네임 입력이 안됨.");
        }
    }
}
