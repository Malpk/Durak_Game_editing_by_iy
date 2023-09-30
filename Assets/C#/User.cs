using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

// ласс пользовател€/игрока 
public class User : BaseScreen
{
    public uint UserID;

    public AvatarScr Avatar; //ƒл€ отображени€ аватарки
    public TMP_Text UId; //ƒл€ отображени€ его номера
    public List<GameObject> UserCards; // арты, которые в руке у игрока
    public TMP_Text MassegeText; //ƒл€ вывода сообщени€

    private ERole _role = ERole.thrower; //”становка роли игрока (что сейчас должен делать во врем€ хода)
    public ERole role
    {
        set
        {
            Debug.Log("set role: " + value.ToString());
            _role = value;
            PrintMessage(value.ToString());
        }
        get
        {
            return _role;
        }
    }

    //»значально игрок ничего не делает = статуса никакого нет
    public EStatus status = EStatus.Null;

    //ѕолучаем ID дл€ игры. ≈сли пользователь = бот, установит соответствующие значени€
    public void Init(uint ID)
    {
        UserID = ID;
        UId.text = "BOT";

        if(ID != 0)
        {
            UId.text = "ID: " + ID.ToString();
            Avatar.UserID = ID;
            m_socketNetwork.getAvatar(ID);
        }
    }

    //¬ыводит сообщение от(к) пользовател€(ю)
    public void PrintMessage(string massege)
    {
        if(MassegeText != null) MassegeText.text = massege;
    }

    //ќрганизаци€ физического передвижени€ спрайтов
    public IEnumerator MoveTo(Vector2 MoveToPoint)
    {
        yield return moveTo(MoveToPoint);
    }
    private bool moveTo(Vector2 MoveToPoint)
    {
        LeanTween.moveLocal(gameObject, MoveToPoint, 2);
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), 2);

        return true;
    }
}
