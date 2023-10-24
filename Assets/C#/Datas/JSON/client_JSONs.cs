using JSON_card;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

//Тут заспамлено: "Толко для работы с JSON файлами!"
//Это для того, чтобы сама IDE предостерегала вас от того, чтобы наломать здесь дров

//JSON-ы для общения с сервером. См. класс сокета
namespace JSON_client
{
    //Для регистрации
    #region registration requests

    //Толко для работы с JSON файлами!
    public class ClientLogin
    {
        public string token;
        public string name;
        public uint UserID;
    }
    //Толко для работы с JSON файлами!
    public class Sucsessed_emailChange
    {
        public string newEmail;
    }
    #endregion

    #region basic requests

    //Так передаётся запрос к серверу
    //Толко для работы с JSON файлами!
    public class MessageData
    {
        public string eventType;
        public string data;
    }

    //ОЧЕНЬ ВАЖНЫЙ ЭЛЕМЕНТ, вокруг него половина запросов крутится. Без него сервер не даёт ответов
    //Толко для работы с JSON файлами!
    public class Token
    {
        public string token;
        public uint RoomID;
    }

    //Роль пользователя
    //Толко для работы с JSON файлами!
    public class Role
    {
        public uint UserID;
        public int role;
    }

    //Получение аватарки
    //Толко для работы с JSON файлами!
    public class AvatarData
    {
        public uint UserID;
        public string avatarImage;
    }

    //Сколько игр игрок сыграл? Нужно ля определения, какие обложки ему доступны
    //Толко для работы с JSON файлами!
    public class PlayedUserGames
    {
        public int games;
    }

    //Список айдишников доступных комнат
    //Толко для работы с JSON файлами!
    public class FreeRooms
    {
        public uint[] FreeRoomsID;
    }
    //Соот. кол-во игроков
    //Толко для работы с JSON файлами!
    public class PlayersInRoom
    {
        public uint[] PlayersID;
    }

    //Данные о клиенте. Не забываем про ТОКЕН
    //Толко для работы с JSON файлами!
    public class ClientData
    {
        public string token;
        public int chips;
    }

    //Игрок готов к игре? Толко для работы с JSON файлами!
    public class ClientReady
    {
        public uint first;
        public Card trump;
    }
    #endregion

    //Запросы на то, чтобы войти в комнату
    #region room enter requests 
    //Толко для работы с JSON файлами!
    public class JoinRoom
    {
        public uint roomOwnerID;
        public uint RoomID;

        public int bet;
        public ETypeGame type;
        public int cards;
        public int maxPlayers;
    }
    //Толко для работы с JSON файлами!
    public class PlayerJoin
    {
        public uint playerID;
    }
    //Толко для работы с JSON файлами!
    public class PlayerExit
    {
        public uint playerID;
    }
    #endregion

    //То, как игрок решил походить, а также раздача карт игрокам
    #region playing requests
    //Толко для работы с JSON файлами!
    public class Battle
    {
        public uint UserID;
        public uint RoomID;
        
        [JsonProperty("attackingCard")]
        public Card attacingCard;
        [JsonProperty("attackedCard")]
        public Card attacedCard;
    }
    //Толко для работы с JSON файлами!
    public class ClientThrow
    {
        public uint RoomID;
        public uint UserID;

        public Card card;
    }
    //Толко для работы с JSON файлами!
    public class ServerBattle
    {
        public string token;

        public byte[] attacked;
        public byte[] attacking;
    }
    //Толко для работы с JSON файлами!
    public class ClientDistribution
    {
        public List<byte> cards;
    }
    //Толко для работы с JSON файлами!
    public class ClientGrab
    {
        public uint uid;
    }
    //Толко для работы с JSON файлами!
    public class ClientFold
    {
        public uint uid;
    }
    //Толко для работы с JSON файлами!
    public class playerWon
    {
        public uint UserID;
    }
    //Толко для работы с JSON файлами!
    public class won
    {
        public int chips;
    }
    #endregion

    //Запросы, связанные с сообщениями в игровом чате
    #region chat client requests
    //Толко для работы с JSON файлами!
    public class GotMessage
    {
        public uint UserID;
        public string message;
    }
    #endregion

    //Толко для работы с JSON файлами!
    public class Client
    {
        public uint UserID;
    }
}
