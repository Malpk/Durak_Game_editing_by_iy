using JSON_card;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Управляет картами. В основном, спрайтами и их внешним видом. Если игра вылетает при задании одного из стилей,
//проблема либо здесь, либо стили не загружены/не указаны в ассетах
public class CardController: MonoBehaviour
{
    public int RotationMultiplyer;
    public int PlaceMultiplyer;

    public Room m_room; //Указатель на текущую игровую комнату

    //Префабы для создания карт на основе указанных стилей
    public static GameObject m_prefabEmpty;

    //Стили и спрайты карт, т.е. их внешний вид
    #region styles
    [Space, Space, Header("Styleshes"), Space]

    [Header("base")]
    public List<Sprite> BaseCardsHeartsTexturies;
    public List<Sprite> BaseCardsDiamondsTexturies;
    public List<Sprite> BaseCardsClubsTexturies;
    public List<Sprite> BaseCardsSpadesTexturies;
    
    [Header("russisn")]
    public List<Sprite> RussisnCardsHeartsTexturies;
    public List<Sprite> RussisnCardsDiamondsTexturies;
    public List<Sprite> RussisnCardsClubsTexturies;
    public List<Sprite> RussisnCardsSpadesTexturies;

    [Header("natureMiddleLine")]
    public List<Sprite> natureMiddleLineCardsHeartsTexturies;
    public List<Sprite> natureMiddleLineCardsDiamondsTexturies;
    public List<Sprite> natureMiddleLineCardsClubsTexturies;
    public List<Sprite> natureMiddleLineCardsSpadesTexturies;

    [Header("fallout")]
    public List<Sprite> falloutCardsHeartsTexturies;
    public List<Sprite> falloutCardsDiamondsTexturies;
    public List<Sprite> falloutCardsClubsTexturies;
    public List<Sprite> falloutCardsSpadesTexturies;

    [Header("natureTropicks")]
    public List<Sprite> natureTropicksCardsHeartsTexturies;
    public List<Sprite> natureTropicksCardsDiamondsTexturies;
    public List<Sprite> natureTropicksCardsClubsTexturies;
    public List<Sprite> natureTropicksCardsSpadesTexturies;

    [Header("herouse")]
    public List<Sprite> herouseCardsHeartsTexturies;
    public List<Sprite> herouseCardsDiamondsTexturies;
    public List<Sprite> herouseCardsClubsTexturies;
    public List<Sprite> herouseCardsSpadesTexturies;

    [Header("cars")]
    public List<Sprite> carsCardsHeartsTexturies;
    public List<Sprite> carsCardsDiamondsTexturies;
    public List<Sprite> carsCardsClubsTexturies;
    public List<Sprite> carsCardsSpadesTexturies;
    
    [Header("horror")]
    public List<Sprite> horrorCardsHeartsTexturies;
    public List<Sprite> horrorCardsDiamondsTexturies;
    public List<Sprite> horrorCardsClubsTexturies;
    public List<Sprite> horrorCardsSpadesTexturies;

    [Header("erotick")]
    public List<Sprite> erotickCardsHeartsTexturies;
    public List<Sprite> erotickCardsDiamondsTexturies;
    public List<Sprite> erotickCardsClubsTexturies;
    public List<Sprite> erotickCardsSpadesTexturies;

    [Space, Space]
    // storage card-gameobjects
    public Transform StartOfCards;

    public List<GameCard> PlayerCards = new List<GameCard>();

    public List<Sprite> cards_texturies_Hearts = new List<Sprite>();
    public List<Sprite> cards_texturies_Diamonds = new List<Sprite>();
    public List<Sprite> cards_texturies_Clubs = new List<Sprite>();
    public List<Sprite> cards_texturies_Spades = new List<Sprite>();
    #endregion

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

        switch (style)
        {
            case "Russian":
                cards_texturies_Hearts = RussisnCardsHeartsTexturies;
                cards_texturies_Diamonds = RussisnCardsDiamondsTexturies;
                cards_texturies_Clubs = RussisnCardsClubsTexturies;
                cards_texturies_Spades = RussisnCardsSpadesTexturies;
                break;

            case "nature_middleLine":
                cards_texturies_Hearts = natureMiddleLineCardsHeartsTexturies;
                cards_texturies_Diamonds = natureMiddleLineCardsDiamondsTexturies;
                cards_texturies_Clubs = natureMiddleLineCardsClubsTexturies;
                cards_texturies_Spades = natureMiddleLineCardsSpadesTexturies;
                break;

            case "Fallout":
                cards_texturies_Hearts = falloutCardsHeartsTexturies;
                cards_texturies_Diamonds = falloutCardsDiamondsTexturies;
                cards_texturies_Clubs = falloutCardsClubsTexturies;
                cards_texturies_Spades = falloutCardsSpadesTexturies;
                break;

            case "nature_tropicks":
                cards_texturies_Hearts = natureTropicksCardsHeartsTexturies;
                cards_texturies_Diamonds = natureTropicksCardsDiamondsTexturies;
                cards_texturies_Clubs = natureTropicksCardsClubsTexturies;
                cards_texturies_Spades = natureTropicksCardsSpadesTexturies;
                break;

            case "herouse":
                cards_texturies_Hearts = herouseCardsHeartsTexturies;
                cards_texturies_Diamonds = herouseCardsDiamondsTexturies;
                cards_texturies_Clubs = herouseCardsClubsTexturies;
                cards_texturies_Spades = herouseCardsSpadesTexturies;
                break;

            case "cars":
                cards_texturies_Hearts = carsCardsHeartsTexturies;
                cards_texturies_Diamonds = carsCardsDiamondsTexturies;
                cards_texturies_Clubs = carsCardsClubsTexturies;
                cards_texturies_Spades = carsCardsSpadesTexturies;
                break;

            case "horror":
                cards_texturies_Hearts = horrorCardsHeartsTexturies;
                cards_texturies_Diamonds = horrorCardsDiamondsTexturies;
                cards_texturies_Clubs = horrorCardsClubsTexturies;
                cards_texturies_Spades = horrorCardsSpadesTexturies;
                break;

            case "erotick":
                cards_texturies_Hearts = erotickCardsHeartsTexturies;
                cards_texturies_Diamonds = erotickCardsDiamondsTexturies;
                cards_texturies_Clubs = erotickCardsClubsTexturies;
                cards_texturies_Spades = erotickCardsSpadesTexturies;
                break;

            default:
                cards_texturies_Hearts = BaseCardsHeartsTexturies;
                cards_texturies_Diamonds = BaseCardsDiamondsTexturies;
                cards_texturies_Clubs = BaseCardsClubsTexturies;
                cards_texturies_Spades = BaseCardsSpadesTexturies;
                break;
        }
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

        Debug.Log("CardController: Instantinate pref card");
        GameObject pref_card = Instantiate(m_room.PREFAB_CARD, StartOfCards.position, StartOfCards.rotation);
        pref_card.transform.localScale = StartOfCards.localScale;
        pref_card.transform.SetParent(gameObject.transform);
        pref_card.tag = "tableNotBeatingCard";

        GameCard cardData = pref_card.GetComponent<GameCard>();

        Debug.Log("CardController: cardData init");
        cardData.Init(cardbytes);

        Debug.Log("CardController: card_data_math_suit");
        switch (cardData.math.Suit)
        {
            case ESuit.CLOVERS:
                pref_card.GetComponent<SpriteRenderer>().sprite = chooseCardNumber(cards_texturies_Clubs, cardData.math.Nominal);
                break;
            case ESuit.TILE:
                pref_card.GetComponent<SpriteRenderer>().sprite = chooseCardNumber(cards_texturies_Diamonds, cardData.math.Nominal);
                break;
            case ESuit.PIKES:
                pref_card.GetComponent<SpriteRenderer>().sprite = chooseCardNumber(cards_texturies_Spades, cardData.math.Nominal);
                break;
            default:
                pref_card.GetComponent<SpriteRenderer>().sprite = chooseCardNumber(cards_texturies_Hearts, cardData.math.Nominal);
                break;
        }

        PlayerCards.Add(pref_card.GetComponent<GameCard>());
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

                    Destroy(card.gameObject);
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

        for(int i = 1; i< m_room._roomRow.roomPlayers.Count; i++)
        {
            if (m_room._roomRow.roomPlayers[i].UserID == UserID)
            {
                Debug.Log("Try instinate");

                GameObject card = Instantiate(m_room.PREFAB_BACK);

                Debug.Log("After instinate");

                Sprite s = card.GetComponent<Sprite>();
                s = Sprite.Create(m_room._roomRow.GameUI.back_card_image, new Rect(0, 0, m_room._roomRow.GameUI.back_card_image.width, m_room._roomRow.GameUI.back_card_image.height), Vector2.zero);           
                //card.AddComponent<SpriteRenderer>().sprite = s; //? 
                
                card.GetComponent<SpriteRenderer>().sortingLayerName = "Cards";

                m_room._roomRow.roomPlayers[i].UserCards.Add(card);

                Debug.Log("After sprite");
            }
        }

        m_room.SetPositionsForAllUserCards();
    }
    public void AtherUserDestroyCard(uint UserID)
    {
        Debug.Log("CardController: Ather user destroy card {");

        if (m_room != null && m_room._roomRow != null)
        {
            Debug.Log("CardController: room and row not null... foreach:");

            for (int i = 1; i < m_room._roomRow.roomPlayers.Count; i++)
            {
                Debug.Log(">-----<");
                if (m_room._roomRow.roomPlayers[i].UserID == UserID)
                {
                    Debug.Log("CardController: destroy card");
                    Destroy(m_room._roomRow.roomPlayers[i].UserCards[0]);
                    m_room._roomRow.roomPlayers[i].UserCards.RemoveAt(0);
                }
            }

            Debug.Log("CardController: settall user cards");
            m_room.SetPositionsForAllUserCards();
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
            PlayerCards[i].gameObject.GetComponent<SpriteRenderer>().sortingOrder = i;

            Debug.Log("CardController: new vectors");
            Vector3 pos = new Vector3((Screen.height/ PlaceMultiplyer) *(i-((PlayerCards.Count)/2)), gameObject.transform.position.y, 0);
            Vector3 rotate = new Vector3(0, 0, (RotationMultiplyer * (i - ((PlayerCards.Count) / 2))) * -1);

            Debug.Log("CardController: set courutine");
            StartCoroutine(PlayerCards[i].GetComponent<GameCard>().MoveTo(pos, rotate, new Vector3(1.5f, 1.5f, 1.5f)));
        }

        Debug.Log("}");
    }
    public Sprite chooseCardNumber(List<Sprite> Cards, ENominal nominal)
    {
        Debug.Log("CardController: choose card number;");
        switch (nominal)
        {
            case ENominal.TWO:
                return Cards[1];
            case ENominal.THREE:
                return Cards[2];
            case ENominal.FOUR:
                return Cards[3];
            case ENominal.FIVE:
                return Cards[4];
            case ENominal.SIX:
                return Cards[5];
            case ENominal.SEVEN:
                return Cards[6];
            case ENominal.EIGHT:
                return Cards[7];
            case ENominal.NINE:
                return Cards[8];
            case ENominal.TEN:
                return Cards[9];
            case ENominal.JACK:
                return Cards[10];
            case ENominal.QUEEN:
                return Cards[11];
            case ENominal.KING:
                return Cards[12];
            default:
                return Cards[0];
        }
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