using System.Collections.Generic;
using TMPro;
using UnityEngine;

//Управляет экранами в лобби
public class ScreenDirector : BaseScreen
{
    //О пользователе
    public TMP_Text ID_text;
    public AvatarScr avatar;

    //Экраны/окна
    public GameObject StartScreen;
    public GameObject SignInScreen;
    public GameObject LoginScreenl;
    public GameObject MenuScreen;
    public GameObject SettingsScreen;
    public GameObject PolicyScreen;
    public GameObject ShopScreen;
    public GameObject RatingScreen;
    public GameObject CollectionsScreen;
    public GameObject RewardsScreen;
    public GameObject SkinsScreen;
    public GameObject AdminPanel;

    //Список всех экранов. Заполняется при инициализации
    List<GameObject> screens = new List<GameObject>();
    void Start()
    {
        screens.Add(StartScreen);
        screens.Add(SignInScreen);
        screens.Add(LoginScreenl);
        screens.Add(MenuScreen);
        screens.Add(SettingsScreen);
        screens.Add(PolicyScreen);
        screens.Add(ShopScreen);
        screens.Add(RatingScreen);
        screens.Add(CollectionsScreen);
        screens.Add(RewardsScreen);
        screens.Add(SkinsScreen);

        ActiveScreen(EScreens.StartScreen);
        //Устанавливаем новй активный экран

        Session.changeUId += UpdateID;
        //Обновленный айди сессии - надо учесть и тут!
    }

    //Установка нового активного экрана (админ. панель не в счёт)
    public void ActiveScreen(EScreens _screenToActive = EScreens.MenuScreen)
    {
        foreach (GameObject _screen in screens)
        {
            _screen.SetActive(false);
        }

        switch (_screenToActive)
        {
            case EScreens.StartScreen:
                StartScreen.SetActive(true);
                break;

            case EScreens.MenuScreen:
                MenuScreen.SetActive(true);
                MenuScreen.GetComponent<MenuScreen>().OnShow();
                break;

            case EScreens.LoginScreen:
                LoginScreenl.SetActive(true);
                break;

            case EScreens.SignInScreen_NameAvatar:
                SignInScreen.SetActive(true);
                break;

            case EScreens.PolicyScreen:
                PolicyScreen.SetActive(true);
                break;

            case EScreens.CollectionsScreen:
                CollectionsScreen.SetActive(true);
                CollectionsScreen.GetComponent<CollectionsScreen>().OnShow();
                break;

            case EScreens.RatingScreen:
                RatingScreen.SetActive(true);
                RatingScreen.GetComponent<RatingScreen>().OnShow();
                break;

            case EScreens.ShopScreen:
                ShopScreen.SetActive(true);
                break;

            case EScreens.SkinsScreen:
                SkinsScreen.SetActive(true);
                break;

            case EScreens.SettingsScreen:
                SettingsScreen.SetActive(true);
                break;

            case EScreens.RewardsScreen:
                RewardsScreen.SetActive(true);
                break;

            default:
                break;
        }
    }

    //Обновлён айди сессии? Тут тоже обновим!
    public void UpdateID(uint ID)
    {
        if(ID != 0) ID_text.text = "ID: " + ID.ToString();
        avatar.UserID = ID;

        m_socketNetwork.getAvatar(ID);
    }

    //Активация админской панели со статистикой
    public void activeAdminPanel()
    {
        AdminPanel.SetActive(true);
    }
}
