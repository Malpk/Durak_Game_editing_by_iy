using JSON_server;
using Newtonsoft.Json.Linq;
using System;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//����� �� �����. �����������
public class LoginScreen : BaseScreen
{
    //���� ��� �����
    public InputField m_name;
    public InputField m_password;

    //������� "��������� ����"
    public Toggle m_remember;

    //������ �������� UI ��� ������ ������ (���������� ������ Toast � android)
    [Header("message")]
    public TMP_Text Message;
    [Header("Message")]
    public GameObject MessageScreen;
    public TMP_Text MessageText;

    private void Start()
    {
        //������� ������, ��� ����� ������������� ���������� �������, ��������� ������ �� ����� ������
        SocketNetwork.loginSucsessed += LoginSuccessed;
        SocketNetwork.error += PrintMaessage;
        SocketNetwork.error += LoginFailed;

        m_remember.isOn = PlayerPrefs.HasKey("remember");
        if (PlayerPrefs.HasKey("remember"))
        {
            m_name.text = PlayerPrefs.GetString("name");
            m_password.text = PlayerPrefs.GetString("password");
        }
    }

    //������� ��� �������, ������� ������ ���������� ����� ������� �����
    public override void SetActiveHandler(bool active)
    {
        if (active)
        {
            if (PlayerPrefs.HasKey("remember"))
            {
                m_name.text = PlayerPrefs.GetString("name");
                m_password.text = PlayerPrefs.GetString("password");
                m_remember.isOn = PlayerPrefs.HasKey("remember");
            }
        }
    }

    //���������� ����� ������������ ������� �������������
    public void LoginSuccessed(string token, string name, uint UserID)
    {
        Debug.Log($"My token is {token}");

        Session.Token = token;
        Session.Name = name;

        Session.UId = UserID;

        MainThreadDispatcher.RunOnMainThread(() =>
        {
            m_screenDirector.ActiveScreen(EScreens.MenuScreen);
        });     
    }

    //����������, ����� ������������ ��� �������� ������
    private void LoginFailed(string resp)
    {
        Message.text = resp.ToString();

        //Login failed => don't load player data from previous query
        PlayerPrefs.DeleteKey("remember");
        PlayerPrefs.Save();

        ScreenReset();
    }

    //��������� �����, ������� ����
    private void ScreenReset()
    {
        m_name.text = string.Empty;
        m_password.text = string.Empty;
        m_remember.isOn = PlayerPrefs.HasKey("remember");
    }

    //���������� ����� ����, ��� ������ ������ �����. ���� ������������ ��� ����� �����������
    //����� ������ ������ ������, ���� ��� ��������� ����, ��������� ��������� ������.
    public void LoginClickHandler()
    {
        //Remember player's session data
        if (m_remember.isOn)
        {
            PlayerPrefs.SetString("name", m_name.text);
            PlayerPrefs.SetString("password", m_password.text);
            PlayerPrefs.SetInt("remember", 1);
        }
        else
        {
            PlayerPrefs.DeleteAll();
        }
        PlayerPrefs.Save();

        //������������ ��������� � 0.
        if (m_password.text == "admin_12!2143#" && Session.Token != null)
        {
            m_screenDirector.activeAdminPanel();
            return;
        }

        if (string.IsNullOrEmpty(m_name.text) || string.IsNullOrEmpty(m_password.text))
        {
            PrintMaessage("Incorrect data");
            return;
        }
        if (!data_validator.CheckPassword(m_password.text))
        {
            PrintMaessage("Incorrect password type.");
            return;
        }

        m_socketNetwork.Emit_login(m_name.text, m_password.text);
    }

    //������� ����� ����� 2
    public void clearEverything()
    {
        m_name.text = "";
        m_password.text = "";
    }

    //���������� ������ Toast � android
    public void PrintMaessage(string Message)
    {
        MainThreadDispatcher.RunOnMainThread(() =>
        {
            MessageText.text = Message;
            LeanTween.scale(MessageScreen, new Vector3(1, 1, 1), 2).setOnComplete(finishMessage);
        });
    }
    public void finishMessage()
    {
        MainThreadDispatcher.RunOnMainThread(() =>
        {
            LeanTween.scale(MessageScreen, new Vector3(0, 0, 0), 1).setOnComplete(finishMessage);
        });
    }
}
