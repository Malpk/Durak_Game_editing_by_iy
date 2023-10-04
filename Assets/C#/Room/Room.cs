using JSON_card;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Table;

//Текущая игровая комната, создаётся из prefab'а и на основе данных из сокета
public class Room : MonoBehaviour
{
    //Отображение текущих правил игры, количества игроков и т.п.
    public static string CurrentRules = "Error: no rules defined";
    public static string CurrentPlayers = "Error: no players defined";
    public static string CurrentTableID = "Table: local (with bot)";

    public Sprite cardBack;

    public float Cooficent;
    public float ScreenWith = 1980;

    public float PlaceMultiplyer;
    public float RotationMultiplyer;

    //Создание новых карт (из колоды) на основе префабов
    public GameObject PREFAB_BACK;
    public GameObject PREFAB_CARD;

    //Работа с интерфейсом и окнами
    public GameObject StartScreen;
    public GameObject OwnerStartGameButton;
    public GameObject PlayerCard;
    public GameObject WinScreen;

    //Управление картами (частично вынесено в отдельный класс)
    public CardController _cardController;

    //Текущий стол
    public Table _table;
    public RoomRow _roomRow;

    //Текущий сокет для работы с сервером
    private SocketNetwork m_socketNetwork;

    [Header("coloda")] //Колода как отображаемый спрайт
    public Transform Coloda;
    public Transform TrumpCardPos;

    public GameObject card;
    public GameObject ColodaObject;

    public GameObject _coloda_Obj;
    public GameObject _trump_Obj;

    [Header("alone game bots")]
    public GameObject alone_Game_BOT; //Скрипт бота

    [Space, Header("win panel")]
    public GameObject win_panel; //Окно, открывающееся после окончания игры

    #region events
    public delegate void Events();
    public static event Events foldEvent;
    public static event Events grabEvent;
    public static event Events passEvent;
    public static event Events grabbing;
    #endregion

    private void Start()
    {
        Debug.Log("Room: Start {");

        ScreenWith = Screen.width;

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

        rulesUI = GameObject.FindWithTag("RulesScreen");
        rulesUI.SetActive(false);

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
        TextMesh text = g.GetComponent<TextMesh>();

        text.text = "No game";
        g.SetActive(true);
    }


    //UI Rules update
    private GameObject rulesUI;
    public void upd_rules()
    {
        Debug.Log("Room: upd_rules {");

        bool screen_active = rulesUI.activeInHierarchy;
        rulesUI.SetActive(true);

        try
        {
            CurrentRules = $"Rules: {_roomRow.GameType}";
            CurrentPlayers = $"Players: {_roomRow.maxPlayers_number} / {_roomRow.roomPlayers.Count}";
            CurrentTableID = $"ID Room: {_roomRow.RoomID}";

            GameObject.FindWithTag("PlayersTXT").GetComponent<TextMeshProUGUI>().text =
            CurrentPlayers;

            GameObject.FindWithTag("RulesTXT").GetComponent<TextMeshProUGUI>().text =
            CurrentRules;

            GameObject.FindWithTag("TableNumberTXT").GetComponent<TextMeshProUGUI>().text =
            CurrentTableID;
        }
        catch (Exception ex)
        {
            Debug.LogError("Can't update rules' UI");
            Debug.Log(ex.Message);
        }

        rulesUI.SetActive(screen_active);

        Debug.Log("}");
    }

    //Обращение к сокету текущей комнаты и подготовка к началу игры
    public void StartGame()
    {
        Debug.Log("Room: StartGame {");

        m_socketNetwork.EmitReady(_roomRow.RoomID);

        CardController.m_prefabCard = PREFAB_CARD;
        CardController.m_prefabBackCard = PREFAB_BACK;

        Debug.Log("}");
    }
    public void startGameAlone()
    {
        Debug.Log("Room: startGameAlone {");
        _roomRow.isAlone = true;

        Debug.Log("Room: bot init");
        alone_Game_BOT game_BOT = Instantiate(alone_Game_BOT, gameObject.transform).GetComponent<alone_Game_BOT>();

        game_BOT.Init(this, _roomRow, _table);

        OnReady(game_BOT._trump);
        Debug.Log("Room: bot omready finished");

        Debug.Log("}");
    }

    //Мы готовы и задаём КОЗЫРЬ
    public void OnReady(Card trump)
    {
        Debug.Log("Room: OnReady {");

        upd_rules();

        //Trump display

        Debug.Log("Trump init start");

        GameObject g = GameObject.FindWithTag("TrumpTXT");
        TextMeshProUGUI text = g.GetComponent<TextMeshProUGUI>();

        Debug.Log("Trump init get");
        Debug.Log("Trump is null? " + trump == null);

        text.text = trump.suit;
        //Задаём символ козыря и цвет масти
        if (trump.suit.Contains('♥') || trump.suit.Contains('♦'))
        {
            text.color = Color.red;
        }
        else
        {
            text.color = Color.black;
        }

        Debug.Log(text.text);

        Debug.Log("Trump init end");

        //Из данных о комнате составляем саму комнату, определяем и инициализируем спрайты
        Debug.Log("Room: intantinating objects");
        _roomRow = GetComponent<RoomRow>();

        StartScreen.SetActive(false);

        _roomRow.isGameStarted = true;

        _coloda_Obj = Instantiate(ColodaObject, Coloda.transform);

        _trump_Obj = Instantiate(card, TrumpCardPos.transform);
        _trump_Obj.transform.localScale = TrumpCardPos.localScale;
        _trump_Obj.transform.SetParent(gameObject.transform);

        //Берём карту, обозначающую козыря и ставим куда надо

        Debug.Log("Room: card data init (trump)");
        GameCard cardData = _trump_Obj.GetComponent<GameCard>();
        cardData.Init(trump);

        switch (cardData.math.Suit)
        {
            case ESuit.CLOVERS:
                _trump_Obj.GetComponent<SpriteRenderer>().sprite = _cardController.chooseCardNumber(_cardController.cards_texturies_Clubs, cardData.math.Nominal);
                break;
            case ESuit.TILE:
                _trump_Obj.GetComponent<SpriteRenderer>().sprite = _cardController.chooseCardNumber(_cardController.cards_texturies_Diamonds, cardData.math.Nominal);
                break;
            case ESuit.PIKES:
                _trump_Obj.GetComponent<SpriteRenderer>().sprite = _cardController.chooseCardNumber(_cardController.cards_texturies_Spades, cardData.math.Nominal);
                break;
            default:
                _trump_Obj.GetComponent<SpriteRenderer>().sprite = _cardController.chooseCardNumber(_cardController.cards_texturies_Hearts, cardData.math.Nominal);
                break;
        }

        _roomRow.Trump = cardData.math.Suit; //Передаём символ козырей

        Debug.Log("}");
    }

    //Вызывается по окночанию игры. Выводит, кто победил, кто проиграл и сколько фишек.
    public void OnWinning(uint UserID)
    {
        Debug.Log("Room: onWining {");
        //string game_result_for_current_player = "You WIN! You've earned 0 points"; //Change in future

        //Получаем экран победы и запускаем его
        WinScreen = GameObject.FindWithTag("WinScreen");
        WinScreen.SetActive(true);

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
        Debug.Log("Room: OnColodaEmpty");
        _coloda_Obj.SetActive(false);
    }
    public void OnTrumpIsDone()
    {
        Debug.Log("Room: OnTrumpIsDone");
        _trump_Obj.SetActive(false);
    }

    ///////\\\\\\\
    //NewPlayers\\
    ///////\\\\\\\
    public GameObject NewPlayer; //Игрок, вошедший в комнату
    public Transform NewPlayerSpawnPoint; //Место появления нового игрока

    /////////////\\\\\\\\\\\\
    /// players functions \\\
    /////////////\\\\\\\\\\\\
    public User NewPlayerJoin(uint UId = 0)
    {
        Debug.Log("Room: new player join {");

        Debug.Log("Room: user instantinate");
        User _user = Instantiate(NewPlayer, NewPlayerSpawnPoint.position, NewPlayerSpawnPoint.rotation).GetComponent<User>();

        //Установка спрайта пользователя и его инициализация
        Debug.Log("Room: user sprite loading");
        _user.gameObject.transform.localScale = NewPlayerSpawnPoint.localScale;
        _user.transform.SetParent(GameObject.Find("UI").transform);
        _roomRow.roomPlayers.Add(_user);
        _user.Init(UId);

        Debug.Log("Room: set pos for all users");
        SetPositionsForAllUsers(_roomRow.roomPlayers);

        upd_rules(); //обновление окошка с количеством игроков и правилами

        Debug.Log("}");

        return _user;
    }

    //Игрок вышел / его выкинуло
    public void DeletePlayer(uint UId)
    {
        Debug.Log("Room: Delete player {");

        for (int i = 1; i < _roomRow.roomPlayers.Count; i++)
        {
            Debug.Log(">--<");
            if ((int)_roomRow.roomPlayers[i].UserID == (int)UId)
            {
                Debug.Log("Room: destroy player");
                Destroy(_roomRow.roomPlayers[i].gameObject);
                _roomRow.roomPlayers.RemoveAt(i);
            }      
        } 
        SetPositionsForAllUsers(_roomRow.roomPlayers);

        upd_rules(); //обновление окошка с количеством игроков и правилами

        Debug.Log("}");
    }

    //Устанавливаем позиции для спрайтов карт
    public void SetPositionsForAllUserCards()
    {
        Debug.Log("Room: set positio nfor all user cards {");

        for (int j = 1; j < _roomRow.roomPlayers.Count; j++)
        {
            Debug.Log("<player pos>");
            Vector3 playerPos = _roomRow.roomPlayers[j].gameObject.transform.position;
            playerPos.y -= 1;


            for (int i = 0; i < _roomRow.roomPlayers[j].UserCards.Count; i++)
            {
                Debug.Log(">---<");

                _roomRow.roomPlayers[j].UserCards[i].transform.SetParent(_roomRow.roomPlayers[j].gameObject.transform);
                _roomRow.roomPlayers[j].UserCards[i].gameObject.GetComponent<SpriteRenderer>().sortingOrder = i;

                Debug.Log("Room: vectors move");
                Vector3 pos = new Vector3((Screen.height / PlaceMultiplyer) * (i - ((_roomRow.roomPlayers[j].UserCards.Count) / 2)), gameObject.transform.position.y - 1.2f, 0);
                Vector3 rotate = new Vector3(0, 0, 0);

                _roomRow.roomPlayers[j].UserCards[i].transform.localScale = new Vector3(20,20,20);

                Debug.Log("Room: start courutine");
                StartCoroutine(MoveCard(_roomRow.roomPlayers[j].UserCards[i], pos, rotate));
            }
            Debug.Log("<player pos />");
        }

        Debug.Log("}");
    }
   
    //Устанавливаем позиции для спрайтов (~аватарок) игроков
    public void SetPositionsForAllUsers(List<User> users)
    {
        Debug.Log("Room: SetPositionsForAllUsers {");

        int i = 1;

        while (i < users.Count)
        {
            Debug.Log(">---<");

            float x = (float)((ScreenWith * i / users.Count) - ScreenWith * 0.5);
            float y = (float)(Math.Abs(x) / Cooficent) * -1;

            Debug.Log(y);

            Vector3 coords = new Vector3(x, y, 0);

            StartCoroutine(users[i].MoveTo(coords));

            i++;
        }

        Debug.Log("}");
    }

    //Игровые действия (ходы)
    #region game_steps
    public void cl_Grab()
    {
        Debug.Log("Room: cl grab");

        if (Session.role != ERole.main)
        {
            _roomRow.GameUI.showPassButton();
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

        GetComponent<GameUIs>().hideFoldButton();

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
        GetComponent<GameUIs>().hidePassButton();

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
        GetComponent<GameUIs>().hideGrabButton();

        _roomRow.status = EStatus.Grab;

        if (!_roomRow.isAlone)  m_socketNetwork.EmitGrab();
        else
        {
            grabEvent?.Invoke();
        }
    }
    #endregion

    //////////////\\\\\\\\\\\\\
    /// lean twin functions \\\
    //////////////\\\\\\\\\\\\\
    public IEnumerator MoveCard(GameObject card, Vector3 newCardPos, Vector3 rotate)
    {
        Debug.Log("Room: MoveCard (LeanTween)");

        LeanTween.moveLocal(card, newCardPos, 2);
        LeanTween.rotate(card, rotate, 2);
        yield return null;
    }
}

