using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AdminPabel : BaseScreen
{
    public TMP_InputField ChipsInput;

    //Output message about stats for Admin
    public static string stats = "No server responce";

    public void GetChipsAdminClickHandler()
    {
        //m_socketNetwork.admin_getChips(int.Parse(ChipsInput.text));
        m_socketNetwork.admin_getChips(Session.Token, int.Parse(ChipsInput.text));
    }

    public void clearPlayerDATA()
    {
        PlayerPrefs.DeleteAll();
    }
}
