using JSON_card;
using System.Linq;
using System.Net.Mail;

//Тут заспамлено: "Толко для работы с JSON файлами!"
//Это для того, чтобы сама IDE предостерегала вас от того, чтобы наломать здесь дров

//JSON-ы для общения с сервером. См. класс сокета
namespace JSON_server
{

    //Запросы для регистрации
    #region registration requests

    //Только для работы с json
    public class ClientLogin
    {
        public string name;
        public string password;
    }

    //Только для работы с json
    public class ClientSignIN
    {
        public string name;

        public string password;
        public string email;
    }

    //Только для работы с json
    public class Client_changeEmail
    {
        public string token;

        public string old_email;
        public string new_email;
    }

    //Только для работы с json
    public class get_chips
    {
        public string token;

        public int chips;
    }

    //Только для работы с json
    public class UserData
    {
        public string token;
        public uint UserID;

        public uint RoomID;
    }
    #endregion

    //Запросы на присоединение к игре (войти в комнату)
    #region room enter requests

    //Создание комнаты
    //Только для работы с json
    public class ServerCreateRoom
    {
        public uint RoomID;

        public string token;

        public int isPrivate;

        public string key;

        public uint bet;

        public uint cards;

        public int type;

        public int maxPlayers;

        public uint roomOwner;
    }

    //Присоединение к комнате
    //Только для работы с json
    public class ServerJoinRoom
    {
        public uint uid;

        public string Token;

        public uint RoomID;

        public string key;

        public uint roomOwner;

        public int type;
    }

    //Выход из комнаты
    //Только для работы с json
    public class ServerExitRoom
    {
        public uint rid;
        public string token;
    }
    #endregion

    //Запросы во время игры (кто как походил)
    #region playing requests

    //Только для работы с json
    public class Throw
    {
        public uint UserID;
        public uint RoomID;
        public Card card;
    }

    //Только для работы с json
    public class Battle
    {
        public uint UserID;
        public uint RoomID;

        public Card attackingCard;
        public Card attackedCard;
    }

    #endregion

    //Запросы, связанные с внутриигровым чатом
    #region chat server requests

    //Только для работы с json
    public class SendMessage
    {
        public uint RoomID;
        public string token;
        public string message;
    }
    #endregion

    //Получение аватара
    //Только для работы с json
    public class AvatarData
    {
        public uint UserID;
        public string avatarImage;
    }

    //Проверка пароля, достаточно ли он сложный, а также адреса почты (реально ли ввели что-то типа tankoman228@gmail.com или ffsdfsd34fds@211234d23)
    public static class data_validator
    {
        public static bool CheckPassword(string pass)
        {
            //min 6 chars, max 12 chars
            if (pass.Length < 6 || pass.Length > 12)
                return false;

            //No white space
            if (pass.Contains(" "))
                return false;

            //At least 1 upper case letter
            if (!pass.Any(char.IsUpper))
                return false;

            //At least 1 lower case letter
            if (!pass.Any(char.IsLower))
                return false;

            //No two similar chars consecutively
            for (int i = 0; i < pass.Length - 1; i++)
            {
                if (pass[i] == pass[i + 1])
                    return false;
            }

            //At least 1 special char
            string specialCharacters = @"%!@#$%^&*()?/>.<,:;'\|}]{[_~`+=-" + "\"";
            char[] specialCharactersArray = specialCharacters.ToCharArray();
            foreach (char c in specialCharactersArray)
            {
                if (pass.Contains(c))
                    return true;
            }
            return false;
        }

        //Проверка, правда ли это почта или набор символов
        public static bool CheckEmail(string email)
        {
            var valid = true;

            try
            {
                var emailAddress = new MailAddress(email);
            }
            catch
            {
                valid = false;
            }

            return valid;
        }
    }
}
