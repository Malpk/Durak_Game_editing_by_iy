using JSON_card;
using System.Collections.Generic;
using UnityEngine;
using static Table;

//Текущая игровая комната, создаётся из prefab'а и на основе данных из сокета
public class Room : MonoBehaviour
{
    public GameObject PlayerPrefab; //Игрок
    [Header("Reference")]
    [SerializeField] private RulePanel _rule;
    [SerializeField] private Transform _ui;
    [SerializeField] private TrumpHolder _trump;
    [SerializeField] private CardHolder _cardHolder;

    public CardController _cardController;

    public Table _table;
    public RoomRow _roomRow;


    //Работа с интерфейсом и окнами
    public GameObject StartScreen;
    public GameObject OwnerStartGameButton;
    public GameObject PlayerCard;

    //Управление картами (частично вынесено в отдельный класс)

    //Текущий сокет для работы с сервером
    private SocketNetwork m_socketNetwork;

    [Header("alone game bots")]
    public GameObject alone_Game_BOT; //Скрипт бота

    [Space, Header("win panel")]
    public GameObject win_panel; //Окно, открывающееся после окончания игры

    private List<User> _userPool = new List<User>();

    #region events
    public delegate void Events();
    public static event Events foldEvent;
    public static event Events grabEvent;
    public static event Events passEvent;
    public static event Events grabbing;
    #endregion

    public CardHolder Card => _cardHolder;

    private void Start()
    {

        Debug.Log("Room: Start {");

        m_socketNetwork = GameObject.FindGameObjectWithTag("SocketNetwork").GetComponent<SocketNetwork>();
        m_socketNetwork.GetAllRoomPlayersID();

        //Говорим сокету, как нужно дополнительно обработать событие, используя методы из этого класса

        Debug.Log("Room: socket init");
        SocketNetwork.ready += OnReady;
        SocketNetwork.player_Win += OnWinning;
        SocketNetwork.colodaIsEmpty += OnColodaEmpty;
        SocketNetwork.trumpIsDone += OnTrumpIsDone;
        SocketNetwork.cl_grab += cl_Grab;
        SocketNetwork.playerGrab += GrabCards;

        Session.roleChanged += ((ERole role) => { _roomRow.status = EStatus.Null; });

        Debug.Log("Room: UI init");

        GameObject.Find("UI").GetComponent<Canvas>().worldCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        //После этого UI (фон) находится на фоне, а не перед всем остальным. В редакторе просто отключайте на время, но не забывайте включить

        Debug.Log("}");
    }

    private void OnDestroy()
    {
        Debug.Log("Room: Destroy");

        //Говорим сокету, что эти события нашими функциями обрабатывать больше не надо, так как нас больше нет
        SocketNetwork.ready -= OnReady;
        SocketNetwork.player_Win -= OnWinning;

        SocketNetwork.colodaIsEmpty -= OnColodaEmpty;
        SocketNetwork.trumpIsDone -= OnTrumpIsDone;

        SocketNetwork.cl_grab -= cl_Grab;
        SocketNetwork.playerGrab -= GrabCards;
        Session.roleChanged -= ((ERole role) => { _roomRow.status = EStatus.Null; });

        //Отображение текущего козыря
        GameObject g = GameObject.FindWithTag("TrumpTXT");
        if (g)
        {
            TextMesh text = g.GetComponent<TextMesh>();

            text.text = "No game";
            g.SetActive(true);
        }
    }

    //Обращение к сокету текущей комнаты и подготовка к началу игры
    public void StartGame()
    {
        Debug.Log("Room: StartGame {");
        m_socketNetwork.EmitReady(_roomRow.RoomID);
       
        Debug.Log("}");
    }

    public void OnReady(Card trump) //Берём карту, обозначающую козыря и ставим куда надо
    {
        Debug.Log("     Room: OnReady {");
        _rule.UpdateData();
        Debug.Log("Room: intantinating objects");
        StartScreen.SetActive(false);
        _roomRow.isGameStarted = true;
        SetRoom(trump);
        Debug.Log("}");
    }

    private void SetRoom(Card trump)
    {
        var card = _cardHolder.CreateCard(trump);
        _trump.SetTrump(card);
        Debug.Log("Room: card data init (trump)");
        _roomRow.Trump = card.math.Suit;
    }


    //Вызывается по окночанию игры. Выводит, кто победил, кто проиграл и сколько фишек.
    public void OnWinning(uint UserID)
    {
        Debug.Log("Room: onWining {");
        //string game_result_for_current_player = "You WIN! You've earned 0 points"; //Change in future

        //Получаем экран победы и запускаем его
        win_panel.SetActive(true);

        //Здесь должна быть прописана обработка из json'a, который приходит с сервера

        /*
        GameObject g = GameObject.FindWithTag("WinTXT");
        TextMeshProUGUI text = g.GetComponent<TextMeshProUGUI>();

        text.text = game_result_for_current_player;

        Debug.Log("OnWinning(uint UserID) finished");

        
        if (UserID == Session.UId)
        {
            win_panel.SetActive(true);
        }
        else
        {
            for (int i = 1; i < _roomRow.roomPlayers.Count; i++)
            {
                if (_roomRow.roomPlayers[i].UserID == UserID)
                {
                    _roomRow.roomPlayers[i].PrintMessage("i won!!!");
                }
            }

        }
        */

        Debug.Log("}");
    }

    //Прячем справйт колоды и карты козыря, когда они заканчиваются
    public void OnColodaEmpty()
    {
        _trump.HideColode();
    }

    public void OnTrumpIsDone()
    {
        _trump.HideTrump();
    }

    public User NewPlayerJoin(uint UId = 0)
    {
        Debug.Log("Room: new player join {");
        User user = GetUser();
        user.transform.SetParent(_ui.transform);
        Debug.Log("Room: user sprite loading");
        user.Init(UId);
        _roomRow.AddPlayer(user);
        _roomRow.SetPositionsForAllUsers();
        _rule.UpdateData(); //обновление окошка с количеством игроков и правилами
        Debug.Log("}");
        return user;
    }

    //Игрок вышел / его выкинуло
    public void DeletePlayer(uint UId)
    {
        Debug.Log("Room: Delete player {");
        var player = _roomRow.RemovePlayer((int)UId);
        if (player)
        {
            DeleteUser(player);
            _roomRow.SetPositionsForAllUsers();
            _rule.UpdateData(); //обновление окошка с количеством игроков и правилами

        }
        Debug.Log("}");
    }

    private User GetUser()
    {
        if (_userPool.Count > 0)
        {
            var user = _userPool[0];
            user.gameObject.SetActive(true);
            _userPool.Remove(user);
            return user;
        }
        return Instantiate(PlayerPrefab).GetComponent<User>();
    }

    private void DeleteUser(User user)
    {
        user.gameObject.SetActive(false);
        if (!_userPool.Contains(user))
        {
            _userPool.Add(user);
        }
    }

    //Игровые действия (ходы)
    #region game_steps
    public void cl_Grab()
    {
        Debug.Log("Room: cl grab");

        if (Session.role != ERole.main)
        {
            _roomRow.GameUI.ShowPassButton();
        }

        for(int i = 1; i < _roomRow.roomPlayers.Count; i++)
        {
            if(_roomRow.roomPlayers[i].role == ERole.main)
            {
                _roomRow.roomPlayers[i].PrintMessage("i am taking!");
            }
        }
    }
    public void GrabCards()
    {
        Debug.Log("Room: Grab cards");

        if (Session.role == ERole.main)
        {
            foreach(CardPair _card in GetComponent<Table>().TableCardPairs)
            {
                Card first_card = new Card { nominal = _card.FirstCard.GetComponent<GameCard>().math.str_Nnominal, suit = _card.FirstCard.GetComponent<GameCard>().math.strimg_Suit };
                _cardController.GetCard(first_card);

                if (_card.isFull)
                {
                    Card second_card = new Card { nominal = _card.SecondCard.GetComponent<GameCard>().math.str_Nnominal, suit = _card.SecondCard.GetComponent<GameCard>().math.strimg_Suit };
                    _cardController.GetCard(second_card);
                }
                
                Destroy(_card.FirstCard);
                if (_card.isFull) Destroy(_card.SecondCard);
            }

            GetComponent<Table>().TableCardPairs = new List<CardPair>();

            _cardController.SetAllCardsPos();
        }

        else
        {
            for (int i = 1; i < _roomRow.roomPlayers.Count; i++)
            {
                if(_roomRow.roomPlayers[i].role == ERole.main)
                {
                    foreach(CardPair _card in GetComponent<Table>().TableCardPairs)
                    {
                        _cardController.AtherUserGotCard(_roomRow.roomPlayers[i].UserID);
                        if(_card.isFull) _cardController.AtherUserGotCard(_roomRow.roomPlayers[i].UserID);

                        Destroy(_card.FirstCard);
                        if (_card.isFull) Destroy(_card.SecondCard);
                    }

                    GetComponent<Table>().TableCardPairs = new List<CardPair>();
                }
            }

            GetComponent<Table>().SetAllTableCardsPos();
        }

        if (_roomRow.isAlone)
        {
            grabbing?.Invoke();
        }
    }
    public void Fold()
    {
        Debug.Log("Room: Fold");

        GetComponent<GameUIs>().HideActiveButton();

        _roomRow.status = EStatus.Fold;

        if (!_roomRow.isAlone) m_socketNetwork.EmitFold();
        else
        {
            foldEvent?.Invoke();
        }
    }
    public void Pass()
    {
        Debug.Log("Room: Pass");
        GetComponent<GameUIs>().HideActiveButton();

        _roomRow.status = EStatus.Pass;

        if (!_roomRow.isAlone)  m_socketNetwork.EmitPass();
        else
        {
            Debug.Log("start pass event");
            passEvent?.Invoke();
        }
    }
    public void Grab()
    {
        Debug.Log("Room: Grab");
        GetComponent<GameUIs>().HideActiveButton();

        _roomRow.status = EStatus.Grab;

        if (!_roomRow.isAlone)  m_socketNetwork.EmitGrab();
        else
        {
            grabEvent?.Invoke();
        }
    }
    #endregion
}

