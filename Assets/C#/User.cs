using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Класс пользователя/игрока 
public class User : BaseScreen
{
    public uint UserID;
    public float PlaceMultiplyer = 20;
    [Header("Reference")]
    public Transform CardHolder; //Точка спавна карт
    public AvatarScr Avatar; //Для отображения аватарки
    public TMP_Text UId; //Для отображения его номера
    public TMP_Text MassegeText; //Для вывода сообщения
    public List<GameObject> UserCards = new List<GameObject>(); //Карты, которые в руке у игрока

    private ERole _role = ERole.thrower; //Установка роли игрока (что сейчас должен делать во время хода)
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

    //Изначально игрок ничего не делает = статуса никакого нет
    public EStatus status = EStatus.Null;

    //Получаем ID для игры. Если пользователь = бот, установит соответствующие значения
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

    //Выводит сообщение от(к) пользователя(ю)
    public void PrintMessage(string massege)
    {
        if(MassegeText != null) MassegeText.text = massege;
    }

    //Организация физического передвижения спрайтов
    public IEnumerator MoveTo(Vector2 MoveToPoint)
    {
        yield return moveTo(MoveToPoint);
    }

    public void UpdateCardPosition()
    {
        var i = 0;
        foreach (var card in UserCards)
        {
            card.transform.SetParent(CardHolder);
            card.GetComponent<SpriteRenderer>().sortingOrder = i;
            Vector3 pos = new Vector3((Screen.height / PlaceMultiplyer) *
                (i - ((UserCards.Count) / 2)), transform.position.y - 1.2f, 0);
            card.transform.localScale = Vector3.one * 50;
            StartCoroutine(MoveCard(card, pos, Vector3.zero));
            i++;
        }
    }

    private bool moveTo(Vector2 MoveToPoint)
    {
        LeanTween.moveLocal(gameObject, MoveToPoint, 2);
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), 2);

        return true;
    }

    private IEnumerator MoveCard(GameObject card, Vector3 newCardPos, Vector3 rotate)
    {
        Debug.Log("Room: MoveCard (LeanTween)");

        LeanTween.moveLocal(card, newCardPos, 2);
        LeanTween.rotate(card, rotate, 2);
        yield return null;
    }
}
