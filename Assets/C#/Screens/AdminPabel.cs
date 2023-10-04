using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Админская панель. Данные о статистике и соответствующий UI управляется из сокета
public class AdminPabel : BaseScreen
{
    public TMP_InputField ChipsInput; //Устарел, но лучше не трогать

    //Output message about stats for Admin
    public static string stats = "No server responce";

    public void GetChipsAdminClickHandler()
    {
        //m_socketNetwork.admin_getChips(int.Parse(ChipsInput.text));
        m_socketNetwork.admin_getChips(Session.Token, int.Parse(ChipsInput.text)); //Устарел, но лучше не трогать
    }

    public void clearPlayerDATA()
    {
        PlayerPrefs.DeleteAll();
    }
}
