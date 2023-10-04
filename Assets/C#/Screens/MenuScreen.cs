using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using UnityEngine.EventSystems;
using System.IO;
using TMPro;
using SimpleFileBrowser;
using UnityEngine.Events;

public class MenuScreen : BaseScreen
{
    [Header("User data")] //Интерфейс с данными о пользователе
    public Text m_name;
    public TMP_Text m_chips;
    public Transform m_content;

    [Header("Create new room UI")] //Комбо боксы параметров для создания новой комнаты
    public Slider m_betSlider;
    public Image sliderImage;
    public Dropdown m_cardsDropdown;
    public Dropdown m_typeGameDropdown;
    public Dropdown m_maxPlayersDropdown;
    public Dropdown m_isPrivateDropdown;

    [Header("free rooms")] //список свободных комнат
    public VerticalLayoutGroup _listOfFreeRooms;
    public GameObject FreeRoomPanel;

    [Header("Message")] //для вывода  сообщений (самописный аналог Toast в Android)
    public GameObject MessageScreen;
    public TMP_Text MessageText;

    [Header("exit button")] //кнопка "назад"
    public Button exitButton;

    //Данные с интерфейса для создания комнаты
    private uint m_bet;
    private uint m_numberOfCards;
    private ETypeGame m_typeOfGame;
    private int m_maxPlayers;
    private int m_isPrivate;

    //Данные о ставках (смешано)
    public Text m_betText; 
    public uint[] m_betValues;

    public void Start()
    {
        //Обновляем сразу всё, что можно
        BetValueChangedHandler();
        CardsValueChangedHandler();
        TypeGameValueChangedHandler();
        MaxPlayersValueChangedHandler();
        IsPrivateValueChangedHandler();

        //Говорим сокету, как нужно дополнительно обработать событие, используя методы из этого класса
        SocketNetwork.roomChange += reloadRooms;
        SocketNetwork.gotChips += GetChipsSuccessed;
        SocketNetwork.gotGames += GetGamesStatsSuccessed;
        SocketNetwork.error += PrintMaessage;

        //Кнопка назад, задаём слушатель
        exitButton.onClick.AddListener(() => m_screenDirector.ActiveScreen(EScreens.LoginScreen));
    }

    //Когда показываем экран, обновляем часть интерфейса
    public void OnShow()
    {
        m_socketNetwork.GetFreeRooms();
        m_socketNetwork.GetChips(Session.Token);
        m_socketNetwork.get_gameStat();

        m_name.text = Session.Name;
    }

    //Обновляем данные о комнате(ах)
    public void reloadRooms(uint[] FreeRoomsID)
    {
        MainThreadDispatcher.RunOnMainThread(() =>
        {
            for (int i = _listOfFreeRooms.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(_listOfFreeRooms.transform.GetChild(i).gameObject);
            }

            if (FreeRoomsID.Length <= 0) return;

            foreach (uint RoomID in FreeRoomsID)
            {
                GameObject _freeRoomPanel = Instantiate(FreeRoomPanel);
                _freeRoomPanel.transform.SetParent(_listOfFreeRooms.transform);

                _freeRoomPanel.transform.localScale = new Vector3(1, 1, 1);

                _freeRoomPanel.transform.Find("RoomID").GetComponent<TMP_Text>().text = "ID: " + RoomID.ToString();
                _freeRoomPanel.transform.Find("Button").GetComponent<Button>().onClick.AddListener(() => { m_socketNetwork.EmitJoinRoom(RoomID); });
            }
        });
    }

    //Измение значения ставки (ползунок)
    public void BetValueChangedHandler()
    {
        try
        {
            uint value = (uint)Mathf.RoundToInt(m_betSlider.value);

            switch (m_betValues[value])
            {
                case 100:
                    m_betText.text = "100";
                    break;
                case 500:
                    m_betText.text = "500";
                    break;
                case 2000:
                    m_betText.text = "2 K";
                    break;
                case 5000:
                    m_betText.text = "5 K";
                    break;
                case 10000:
                    m_betText.text = "10 K";
                    break;
                case 50000:
                    m_betText.text = "50 K";
                    break;
                case 100000:
                    m_betText.text = "100 K";
                    break;
                case 500000:
                    m_betText.text = "500 K";
                    break;
                case 1000000:
                    m_betText.text = "1 M";
                    break;
                case 10000000:
                    m_betText.text = "10 M";
                    break;
            }
            m_bet = m_betValues[value];
        }
        catch (Exception)
        {
            m_bet = 0;
        }
    }

    //Обновляем данные с интерфейса после того, как с ним провзаимодействовали
    public void CardsValueChangedHandler()
    {
        try
        {
            string selected = m_cardsDropdown.options[m_cardsDropdown.value].text;
            m_numberOfCards = UInt32.Parse(selected.Split(" ")[0]); 
        }
        catch (Exception)
        {
            m_numberOfCards = 0;
        }

    }
    public void TypeGameValueChangedHandler()
    {
        m_typeOfGame = (ETypeGame)m_typeGameDropdown.value;
    }
    public void MaxPlayersValueChangedHandler()
    {
        try
        {
            m_maxPlayers = int.Parse(m_maxPlayersDropdown.options[m_maxPlayersDropdown.value].text);
            //this.Filter();
        }
        catch (Exception)
        {
            m_maxPlayers = 0;
        }
    }
    public void IsPrivateValueChangedHandler()
    {
        m_isPrivate = m_isPrivateDropdown.value != 0 ? 0:1;
    }


    //Получение некоторых значений (Get)
    public void GetChipsSuccessed(int chips)
    {
        MainThreadDispatcher.RunOnMainThread(() =>
        {
            Session.Chips = chips;
            if (chips != 0) m_chips.text = chips.ToString();
            else m_chips.text = "You dont have any chips";
        });
    }
    public void GetGamesStatsSuccessed(int games)
    {
        Session.PlayedGames = games;
        Debug.Log("Played games: " + games.ToString());
    }

    //Обновление аватара пользователя
    #region  set avatar
    public void OpenFileExplorer()
    {
        // Set the file filter to image files
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".png", ".jpg", ".jpeg", ".gif"));

        // Show the file dialog
        FileBrowser.ShowLoadDialog(HandleSelectedFilePath, null, FileBrowser.PickMode.Files, false, null, null, "Select Image", "Select");
    }

    //Получение пути к файлу, загрузка его в сокет
    private void HandleSelectedFilePath(string[] path)
    {
        if (path.Length > 0)
        {
            string imagePath = path[0];
            m_socketNetwork.setAvatar(imagePath);
        }

        m_socketNetwork.getAvatar(Session.UId);
    }
    #endregion

    //Пустые или почти пустые хендлеры для кликов (экраны)
    #region Screens
    public void AddChipsClickHandler(){ }
    public void ExchangeChipsClickHandler() { }
    public void RatingClickHandler()
    {
        m_screenDirector.ActiveScreen(EScreens.RatingScreen);
    }
    public void StoreClickHandler() { }
    public void CollectionsClickHandler()
    {
        m_screenDirector.ActiveScreen(EScreens.CollectionsScreen);
    }
    public void AwardsClickHandler() { }
    public void SettingsClickHandler()
    {
        m_screenDirector.ActiveScreen(EScreens.SettingsScreen);
    }
    #endregion

    //Добавление новой комнаты (действие со стороны пользователя)
    public void CreateRoomClickHandler()
    {
        string token = Session.Token;

        if (m_bet == 0 || m_maxPlayers == 0)
        {
            return;
        }

        switch (m_maxPlayers)
        {
            case 2:
                m_numberOfCards = 24;
                break;
            case 3:
                m_numberOfCards = 36;
                break;
            case 4:
                m_numberOfCards = 52;
                break;
            case 5:
                m_numberOfCards = 52;
                break;

            default:
                break;
        }

        m_socketNetwork.EmitCreateRoom(token, m_isPrivate, "", m_bet, m_numberOfCards, m_maxPlayers, m_typeOfGame);
    }

    //Самописный аналог Toast в Android
    #region Message
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
    #endregion


    private void OnApplicationQuit()
    {
        m_screenDirector.ActiveScreen(EScreens.StartScreen);
    }
}
