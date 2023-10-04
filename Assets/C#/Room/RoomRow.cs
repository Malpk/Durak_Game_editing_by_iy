using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Непосредственно связан с Room. Отвечает за обработку общих данных о комнате (игроки, ставки и т.п.)
public class RoomRow : BaseScreen
{
    public bool isAlone = false; //Только 1 живой игрок?

    private Room _room;

    public GameUIs GameUI;

    [Header("RoomDATA")]

    public uint _roomOwnerID;
    private uint _roomID;
    public ESuit Trump;
    public ETypeGame GameType;
    public int maxCards_number;
    public int maxPlayers_number;
    public int Bet;

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
        set { _roomID = value; Init(value); }
    }

    public bool isGameStarted;

    //Отоюражение аватарок, список игроков
    [Header("Player Image")]
    public AvatarScr PlayerAvatar;
    [Header("Players")]
    public List<User> roomPlayers;

    public EStatus status;

    //Отображение ставок
    [Header("bet's")]
    public TMP_Text Users_Bet;
    public TMP_Text Rooms_Bet;

    public void Start()
    {
        GameUI = GetComponent<GameUIs>();
        SocketNetwork.changePlayers += UpdateRoomPlayers;

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
        Rooms_Bet.text = Bet.ToString();

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

    //Нужно не просто закрыть комнату, но и указать это в соответствующих полях и удалить комнату, созданную ранее из prefab
    public void ExitClickHandler()
    {
        Debug.Log("RoomRow: ExitClick Handler {");

        m_socketNetwork.EmitExitRoom(_roomID);
        Destroy(gameObject); //Я не знаю, почему нельзя сделать переход между сценами, тут уже поздно это исправлять
        m_screenDirector.ActiveScreen();

        Debug.Log("}");
    }
}