using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Непосредственно связан с Room. Отвечает за обработку общих данных о комнате (игроки, ставки и т.п.)
public class RoomRow : BaseScreen
{
    public bool isAlone = false; //Только 1 живой игрок?
    public EStatus status;
    public float Cooficent;
    public float PlaceMultiplyer;
    [Header("Reference")]
    public GameUIs GameUI;
    public Transform NewPlayerSpawnPoint; //Место появления нового игрока

    [Header("RoomDATA")]

    public uint _roomOwnerID;
    public int Bet;
    public ESuit Trump;
    public ETypeGame GameType;
    public int maxCards_number;
    public int maxPlayers_number;

    public bool isGameStarted;

    //Отоюражение аватарок, список игроков
    [Header("Player Image")]
    public AvatarScr PlayerAvatar;
    [Header("Players")]
    public List<User> roomPlayers;


    //Отображение ставок
    [Header("bet's")]
    [SerializeField] private GameObject _startButton;
    [SerializeField] private TMP_Text Users_Bet;
    [SerializeField] private TMP_Text Rooms_Bet;
    [SerializeField] private TMP_Text Room_ID;

    private Room _room;
    private uint _roomID;
    private float ScreenWith = 1980;

    //Добавление владельца комнаты и установка его айди
    public uint RoomOwner
    {
        get { return _roomOwnerID; }
        set { _roomOwnerID = value; roomPlayers.Add(new User { UserID = value }); }
    }

    //Добавление комнаты + ИНИЦИАЛИЗАЦИЯ КОМНАТЫ
    public uint RoomID
    {
        get { return _roomID; }
        set { 
            _roomID = value;
            Init(value);
            Room_ID.SetText("ID: " + _roomID.ToString());
        }
    }

    public void Start()
    {
        ScreenWith = Screen.width;
        SocketNetwork.changePlayers += UpdateRoomPlayers;
        _startButton.SetActive(false);
        Debug.Log("RoomRow: Start");
    }

    private void OnDestroy()
    {
        SocketNetwork.changePlayers -= UpdateRoomPlayers;

        Debug.Log("RoomRow: Destroy");
    }

    //Инициализация соответственно ранее указанному Room
    public void Init(uint roomID) 
    {
        Debug.Log("RoomRow: Init {");

        Debug.Log("RoomRow: Screens");
        _room = GetComponent<Room>();
        _room.StartScreen.SetActive(true);

        if (_roomOwnerID == Session.UId)
        {
            Debug.Log("RoomRow: Room owner ID = UID");
            _room.OwnerStartGameButton.SetActive(true);
        }

        Debug.Log("RoomRow: player avatar");
        PlayerAvatar.UserID = Session.UId;
        m_socketNetwork.getAvatar(Session.UId);

        Debug.Log("RoomRow: setting text (cheaps)");
        Users_Bet.text = Session.Chips.ToString();
        Debug.LogWarning(Session.Chips);
        Debug.Log("}");
    }

    //Нужно не просто закрыть комнату, но и указать это в соответствующих полях и удалить комнату, созданную ранее из prefab
    public void ExitClickHandler()
    {
        Debug.Log("RoomRow: ExitClick Handler {");
        m_socketNetwork.EmitExitRoom(_roomID);
        Destroy(gameObject); //Я не знаю, почему нельзя сделать переход между сценами, тут уже поздно это исправлять
        m_screenDirector.ActiveScreen();
        Debug.Log("}");
    }

    public void DeleteCards(int userId, CardHolder card)
    {
        Debug.Log("CardController: room and row not null... foreach:");
        for (int i = 1; i < roomPlayers.Count; i++)
        {
            Debug.Log(">-----<");
            if (roomPlayers[i].UserID == userId)
            {
                Debug.Log("CardController: destroy card");
                card.CardDelete(roomPlayers[i].UserCards[0]);
                roomPlayers[i].UserCards.RemoveAt(0);
            }
        }
        Debug.Log("CardController: settall user cards");
    }

    public void AddPlayer(User user)
    {
        if (!roomPlayers.Contains(user))
        {
            roomPlayers.Add(user);
            Rooms_Bet.text = (Bet * roomPlayers.Count).ToString();
            _startButton.SetActive(roomPlayers.Count == maxPlayers_number);
        }
    }

    public User RemovePlayer(int id)
    {
        for (int i = 1; i < roomPlayers.Count; i++)
        {
            Debug.Log(">--<");
            if ((int)roomPlayers[i].UserID == id)
            {
                Debug.Log("Room: destroy player");
                var user = roomPlayers[i];
                roomPlayers.RemoveAt(i);
                Rooms_Bet.text = (Bet * roomPlayers.Count).ToString();
                _startButton.SetActive(roomPlayers.Count == maxPlayers_number);
                return user;
            }
        }
        return null;
    }

    //Устанавливаем позиции для спрайтов (~аватарок) игроков
    public void SetPositionsForAllUsers()
    {
        Debug.Log("Room: SetPositionsForAllUsers {");
        for (int i = 1; i < roomPlayers.Count; i++)
        {
            Vector3 position = Vector3.zero;
            position.x = (float)((ScreenWith * i / roomPlayers.Count) - ScreenWith * 0.5);
            position.y = NewPlayerSpawnPoint.localPosition.y + (float)(Mathf.Abs(position.x) / Cooficent) * -1;
            StartCoroutine(roomPlayers[i].MoveTo(position));
        }
        Debug.Log("}");
    }


    //Устанавливаем позиции для спрайтов карт
    public void SetPositionsForAllUserCards()
    {
        Debug.Log("Room: set positio nfor all user cards {");
        foreach (var player in roomPlayers)
        {
            player?.UpdateCardPosition();
            Debug.Log("<player pos />");
        }
        Debug.Log("}");
    }


    //Обновляем список игроков, согласно новому массиву
    private void UpdateRoomPlayers(uint[] PlayersID)
    {
        Debug.Log("RoomRow: Update RoomPlayers {");
        Debug.Log("RoomRow: users foreach");
        List<uint> usersID = new List<uint>();
        foreach (User _user in roomPlayers)
        {
            Debug.Log(">---<");
            usersID.Add(_user.UserID);
        }
        Debug.Log("RoomRow: for id's in PlayersIDS");
        foreach (uint ID in PlayersID)
        {
            Debug.Log(">---<");
            if (ID == Session.UId)
            {
                Debug.Log("RoomRow: ID == Session UID");
                break;
            }
            else
            {
                Debug.Log("RoomRow: player join");
                _room.NewPlayerJoin(ID);
            }
        }

        Debug.Log("}");
    }

}