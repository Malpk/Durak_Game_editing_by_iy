using UnityEngine;

//Сессия соединения с сервером. ОЧЕНЬ ВАЖНЫЙ КЛАСС
public static class Session
{

    //Обработчики событий. Задаются извне соответствующими классами
    public delegate void chipsChangeEvent(uint chips);
    public static event chipsChangeEvent changeChips;  
    public delegate void UIdChangeEvent(uint UId);
    public static event UIdChangeEvent changeUId;
    public delegate void roleChange(ERole role);
    public static event roleChange roleChanged;

    public static string Token; //Токен сессии. БЕЗ НЕГО СЕРВЕР НЕ ПРИНИМАЕТ ЗАПРОСЫ

    private static uint m_id; //ID сессии
    public static uint UId
    {
        get { return m_id; }
        set
        {
            m_id = value;
            changeUId?.Invoke((uint)value);
        }
    }

    public static string Name; //Имя

    private static int m_chips; //Деньги (фишки) игрока
    public static int Chips
    {
        get { return m_chips; }
        set { 
            m_chips = value;

            changeChips?.Invoke((uint)value);
        }
    }

    public static int PlayedGames; //Для выдачи обложки


    //Данные комнаты
    public static uint RoomID;

    public static uint Bet; //ставка

    private static ERole _role; //роль игрока
    public static ERole role
    {
        set
        {
            Debug.Log("set role of main: " + value.ToString());
            _role = value;
            roleChanged?.Invoke(value);
        }
        get { return _role; }
    }
}
