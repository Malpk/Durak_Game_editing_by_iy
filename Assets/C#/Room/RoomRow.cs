using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//��������������� ������ � Room. �������� �� ��������� ����� ������ � ������� (������, ������ � �.�.)
public class RoomRow : BaseScreen
{
    public bool isAlone = false; //������ 1 ����� �����?

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

    //���������� ��������� ������� � ��������� ��� ����
    public uint RoomOwner
    {
        get { return _roomOwnerID; }
        set { _roomOwnerID = value; roomPlayers.Add(new User { UserID = value }); }
    }

    //���������� ������� + ������������� �������
    public uint RoomID
    {
        get { return _roomID; }
        set { _roomID = value; Init(value); }
    }

    public bool isGameStarted;

    //����������� ��������, ������ �������
    [Header("Player Image")]
    public AvatarScr PlayerAvatar;
    [Header("Players")]
    public List<User> roomPlayers;

    public EStatus status;

    //����������� ������
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

    //������������� �������������� ����� ���������� Room
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

    //��������� ������ �������, �������� ������ �������
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

    //����� �� ������ ������� �������, �� � ������� ��� � ��������������� ����� � ������� �������, ��������� ����� �� prefab
    public void ExitClickHandler()
    {
        Debug.Log("RoomRow: ExitClick Handler {");

        m_socketNetwork.EmitExitRoom(_roomID);
        Destroy(gameObject); //� �� ����, ������ ������ ������� ������� ����� �������, ��� ��� ������ ��� ����������
        m_screenDirector.ActiveScreen();

        Debug.Log("}");
    }
}