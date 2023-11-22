using JSON_card;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Управляет картами. В основном, спрайтами и их внешним видом. 
public class CardController: MonoBehaviour
{
    public int RotationMultiplyer;
    public int PlaceMultiplyer;

    public Room m_room; //Указатель на текущую игровую комнату

    // storage card-gameobjects
    public Transform StartOfCards;

    public List<GameCard> PlayerCards = new List<GameCard>();

    //Находим недавно созданную комнату (после её создания мы вскоре окажемся здесь) и устанавливаем внешний вид + фон
    private void Start()
    {
        m_room = GameObject.Find("Room(Clone)").GetComponent<Room>();

        //Говорим сокету, как нужно дополнительно обработать событие, используя методы из этого класса
        SocketNetwork.GetCard += GetCard;
        SocketNetwork.DestroyCard += DestroyCard;
        SocketNetwork.userGotCard += AtherUserGotCard;
        SocketNetwork.userDestroyCard += AtherUserDestroyCard;

        //Получаем выбранный ранее стиль и задаём его картам
        string style = PlayerPrefs.GetString("Style");
        CardStyleHub.Instance.Load(style);
    }
    private void OnDestroy()
    {
        //Говорим сокету, что эти события нашими функциями обрабатывать больше не надо, так как нас больше нет
        SocketNetwork.GetCard -= GetCard;
        SocketNetwork.DestroyCard -= DestroyCard;
        SocketNetwork.userGotCard -= AtherUserGotCard;
        SocketNetwork.userDestroyCard -= AtherUserDestroyCard;
    }

    //Карты в руках и их отображение (спрайты)
    #region hands cards
    public void GetCard(Card cardbytes)
    {
        Debug.Log("CardController: GetCards {");
        var pref_card = m_room.Card.CreateCard(cardbytes);
        pref_card.transform.position = StartOfCards.position;
        pref_card.transform.rotation = StartOfCards.rotation;
        pref_card.transform.localScale = StartOfCards.localScale;
        pref_card.transform.SetParent(gameObject.transform);
        pref_card.tag = "tableNotBeatingCard";

        PlayerCards.Add(pref_card);
        Debug.Log("CardController: finishing");
        SetAllCardsPos();

        Debug.Log("}");
    }



    public void DestroyCard(Card cardbytes)
    {
        Debug.Log("CardController: destroy card (foreach)");
        foreach (GameCard card in PlayerCards)
        {
            Debug.Log(">---<");
            if (card.math.strimg_Suit == cardbytes.suit)
            {
                if(card.math.str_Nnominal == cardbytes.nominal)
                {
                    Debug.Log("CardController: Destroying found card");
                    m_room.Card.CardDelete(card);
                    PlayerCards.Remove(card);

                    Debug.Log("CardController: Set all card pos");
                    SetAllCardsPos();

                    Debug.Log("}");
                    return;
                }
            }
        }

        Debug.Log("CardController: maybe no card destroyed");
        Debug.Log("}");
    }
    #endregion

    //Отрисовка карт других игроков. Именно эту шляпу надо переделать, она перегружает игру вплоть до вылета. Но, увы, меня не для анимаций наняли
    //Если вы аниматор - вам сюда. Вообще анимации в юнити не так делаются. С точки зрения кода - всё верно и багов нет. Но мой процессор так не думает
    #region other users cards
    public void AtherUserGotCard(uint UserID)
    {
        Debug.Log("CardController: Ather user got card"); //Кто писал эти запросы плохо знает английский, тут всё верно с точки зрения кода. НЕ ПРАВИТЬ!

        foreach (var player in m_room._roomRow.roomPlayers)
        {
            if (player.UserID == UserID)
            {
                Debug.Log("Try instinate");
                var card = m_room.Card.CreateBack().GetComponent<SpriteRenderer>();
                Debug.Log("After instinate");
                card.sortingLayerName = "Cards";
                player.UserCards.Add(card.gameObject);
                Debug.Log("After sprite");
            }
        }
        m_room._roomRow.SetPositionsForAllUserCards();
    }
    

    public void AtherUserDestroyCard(uint userID)
    {
        Debug.Log("CardController: Ather user destroy card {");
        if (m_room != null && m_room._roomRow != null)
        {
            m_room._roomRow.DeleteCards((int)userID, m_room.Card);
            m_room._roomRow.SetPositionsForAllUserCards();
        }
        Debug.Log("}");
    }
    #endregion

    //Вспомогательные функции для получения спрайтов, объектов или обработки. Вынесенные за скобки, условно говоря
    #region help functions
    public void SetAllCardsPos()
    {
        Debug.Log("CardController: Set all card pos {");

        Sort(new CardSorter());
        Debug.Log("CardController: sorted");

        for (int i = 0; i < PlayerCards.Count; i++)
        {
            Debug.Log(">---<");
            Debug.Log("CardController: new game object");
            PlayerCards[i].SetLayer(10 + i);

            Debug.Log("CardController: new vectors");
            Vector3 pos = new Vector3((Screen.height/ PlaceMultiplyer) *(i-((PlayerCards.Count)/2)), gameObject.transform.position.y, 0);
            Vector3 rotate = new Vector3(0, 0, (RotationMultiplyer * (i - ((PlayerCards.Count) / 2))) * -1);

            Debug.Log("CardController: set courutine");
            PlayerCards[i].GetComponent<GameCard>().StartMoveTo(pos, rotate, new Vector3(1.5f, 1.5f, 1.5f));
        }

        Debug.Log("}");
    }

    public void Sort(IComparer<GameCard> comparer) => PlayerCards.Sort(comparer);
    #endregion
}

//Сортировщик карт (в руке)
class CardSorter : IComparer<GameCard>
{
    public CardOrderMethod SortBy = PlayerPrefs.GetString("SortType") == "Suit" ? CardOrderMethod.SuitThenKind : CardOrderMethod.KindThenSuit;

    public int Compare(GameCard x, GameCard y)
    {
        if (SortBy == CardOrderMethod.SuitThenKind)
        {
            if (x.math.Suit > y.math.Suit)
            {
                return 1;
            }
            if (x.math.Suit < y.math.Suit)
            {
                return -1;
            }
            return x.math.Nominal > y.math.Nominal ? 1 : -1;
        }
        if (SortBy == CardOrderMethod.KindThenSuit)
        {
            if (x.math.Nominal > y.math.Nominal)
            {
                return 1;
            }
            if (x.math.Nominal < y.math.Nominal)
            {
                return -1;
            }
            return x.math.Suit > y.math.Suit ? 1 : -1;
        }
        throw new NotImplementedException($"CardOrderMethod {SortBy} is not implemented.");
    }
}