using JSON_card;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using static Table;

//Текущий игровой стол. За ним сидят игроки, на нём лежат карты. Почти как в жизни
public class Table : BaseScreen
{
    //То, что управляет картами и игроками. Нельзя конкретно сказать, что за что отвечает
    public CardController _cardController;
    public GameUIs _gameUI;
    public RoomRow _roomRow;
    public Room _room;

    public Transform FoldPlace; //Сюда складываются спрайты карт

    public List<CardPair> TableCardPairs = new List<CardPair>(); //Пары для сортировки карт в руке

    //События, связанные с картами на столе
    #region events_table_cards
    public delegate void BeatCardEvent();
    public static event BeatCardEvent beatCard_event;

    public delegate void ThrowCardEvent();
    public static event ThrowCardEvent throwCard_event;

    public delegate void Events();
    public static event Events folding;
    #endregion

    private void Start()
    {
        //Инициализация того, что управляет картами и игроками
        _cardController = GameObject.FindGameObjectWithTag("Hands").GetComponent<CardController>();
        _gameUI = GetComponent<GameUIs>();
        _roomRow = GetComponent<RoomRow>();
        _room = GetComponent<Room>();

        //Говорим сокету, как нужно дополнительно обработать событие, используя методы из этого класса
        SocketNetwork.placeCard += placeCard;
        SocketNetwork.beatCard += beatCard;
        SocketNetwork.FoldAllCards += foldCards;

        Debug.Log("Table: start;");
    }

    //Ходы игрока
    #region table_player_moves
    public void ThrowCard(GameCard g_card)
    {
        Debug.Log("Table: throw card {");

        CardMath card = g_card.math;
        
        if (Session.role != ERole.main)
        {
            Debug.Log("Table: throw not main");
            if (_roomRow.status == EStatus.Fold || _roomRow.status == EStatus.Pass)
            {
                Debug.Log("Table: throw return (fold or pass)");
                return;
            }
            if (isAbleToThrow(g_card))
            {
                Debug.Log("Table: is able to throw");
                if (!_roomRow.isAlone)
                {
                    m_socketNetwork.EmitThrow(new Card { suit = card.strimg_Suit, nominal = card.str_Nnominal });
                    Debug.Log("Table: room is alone");
                }
                else
                {
                    Debug.Log("Table: place card (not alone)");

                    placeCard(Session.UId, new Card { suit = card.strimg_Suit, nominal = card.str_Nnominal });
                    _cardController.DestroyCard(new Card { suit = card.strimg_Suit, nominal = card.str_Nnominal });
                    Debug.Log("Table: placed and destroyed");

                    Debug.Log("Table: invoke throw card event");
                    throwCard_event.Invoke();
                }
            }
        }

        Debug.Log("}");
    }
    public void BeatCard(GameCard card, GameCard beatingCard)
    {
        Debug.Log("Table: BeatCard {");
        if (Session.role == ERole.main)
        {
            if (_roomRow.status == EStatus.Grab)
            {
                Debug.Log("Table: grab => return");
                return;
            }
            if (isAbleToBeat(card, beatingCard))
            {
                Debug.Log("Table: is able to beat");
                if (!_roomRow.isAlone) {

                    Debug.Log("Table: is not alone");

                    m_socketNetwork.EmitBeat(new Card 
                    { suit = beatingCard.math.strimg_Suit, nominal = beatingCard.math.str_Nnominal }, 
                    new Card { suit = card.math.strimg_Suit, nominal = card.math.str_Nnominal });

                    Debug.Log("Table: socket emit beat");
                }
                else
                {
                    Debug.Log("Table: alone <--> beat card");

                    beatCard(Session.UId, new Card { suit = beatingCard.math.strimg_Suit, nominal = beatingCard.math.str_Nnominal }, new Card { suit = card.math.strimg_Suit, nominal = card.math.str_Nnominal });
                    _cardController.DestroyCard(new Card { suit = beatingCard.math.strimg_Suit, nominal = beatingCard.math.str_Nnominal });

                    Debug.Log("Table: beat card event");
                    beatCard_event.Invoke();
                }
            }


            if(_roomRow.GameType == ETypeGame.Transferable && card.math.Nominal == beatingCard.math.Nominal)
            {
                Debug.Log("Table: game is Transferable. not beat, transfer");
                m_socketNetwork.EmitBeat(new Card { suit = beatingCard.math.strimg_Suit, nominal = beatingCard.math.str_Nnominal }, new Card { suit = card.math.strimg_Suit, nominal = card.math.str_Nnominal });
            }

            Debug.Log("}");
        }
    }

    public void placeCard(uint UserID, Card card)
    {
        Debug.Log("Table: place card");
        if (Session.role == ERole.main)
        {
            Debug.Log("Table: role main");
            if (TableCardPairs.Count == 0)
            {
                Debug.Log("Show grab btn");
                _gameUI.showGrabButton();
            }
        }
        Debug.Log("Table: instantinate prefab card");
        var cardItem = _room.Card.CreateCard(card).GetComponent<GameCard>();
        cardItem.transform.localScale = _cardController.StartOfCards.localScale;
        cardItem.transform.SetParent(gameObject.transform);
        cardItem.tag = "tableBeatingCard";

        Debug.Log("Table: card data init");
        cardItem.Init(card);
        cardItem.isDraggble = false;

        Debug.Log("Table: card pairs");
        CardPair cardPair = new CardPair();
        cardPair.FirstCard = cardItem.gameObject;

        TableCardPairs.Add(cardPair);

        Debug.Log("Table: soft card pairs. set all table card pos");
        SortCardPairs();
        SetAllTableCardsPos();

        if (Session.role == ERole.main)
        {
            Debug.Log("Table: role main. Show grub btn");
            _gameUI.showGrabButton();
            return;
        }

        Debug.Log("}");
    }

    public void placeCard_(uint UserID, string suit, string nominal)
    {
        Debug.Log("Table: placeCard_");

        if (Session.role == ERole.main)
        {
            if (TableCardPairs.Count == 0)
            {
                Debug.Log("Table: shoe game UI");
                _gameUI.showGrabButton();
            }
        }

        Debug.Log("Table: pref card instantinate");

        var card = new Card();
        card.suit = suit;
        card.nominal = nominal;
        var pref_card = _room.Card.CreateCard(card);
        pref_card.transform.localScale = _cardController.StartOfCards.localScale;
        pref_card.transform.SetParent(gameObject.transform);
        pref_card.tag = "tableBeatingCard";

;
        pref_card.isDraggble = false;



        CardPair cardPair = new CardPair();
        cardPair.FirstCard = pref_card.gameObject;

        TableCardPairs.Add(cardPair);

        SortCardPairs();
        SetAllTableCardsPos();

        if (Session.role == ERole.main)
        {
            _gameUI.showGrabButton();
            return;
        }
    }

    public void beatCard(uint UserID, Card beatCard, Card beatingCard)
    {
        Debug.Log("Table: beat card {");

        Debug.Log("Table: instantinate new card");
        var pref_card = _room.Card.CreateCard(beatingCard);
        pref_card.transform.localScale = _cardController.StartOfCards.localScale;
        pref_card.transform.SetParent(gameObject.transform);
        pref_card.tag = "tableNotBeatingCard";

        Debug.Log("Table: init beating card");
        pref_card.isDraggble = false;


        Debug.Log("Table: for (table card pairs)");
        for (int i = 0; i < TableCardPairs.Count; i++)
        {
            if(TableCardPairs[i].FirstCard.GetComponent<GameCard>().math.strimg_Suit == beatCard.suit && TableCardPairs[i].FirstCard.GetComponent<GameCard>().math.str_Nnominal == beatCard.nominal)
            {
                TableCardPairs[i].FirstCard.tag = "tableNotBeatingCard";
                TableCardPairs[i].SecondCard = pref_card.gameObject;
                TableCardPairs[i].isFull = true;
            }
        }

        Debug.Log("Table: sorting and setting pairs and cards");
        SortCardPairs();
        SetAllTableCardsPos();

        if (Session.role == ERole.main)
        {
            Debug.Log("Table: role == main");
            if (TableCardPairs.Count > 0)
            {
                Debug.Log("Table: foreach pair");
                foreach (CardPair pair in TableCardPairs)
                {
                    Debug.Log("<-->");
                    if (!pair.isFull)
                    {
                        _gameUI.showGrabButton();
                        Debug.Log("no grab}");
                        return;
                    }
                }
                _gameUI.hideGrabButton();
            }
        }

        if (Session.role == ERole.firstThrower || Session.role == ERole.thrower)
        {
            Debug.Log("Role == first thrower or thrower");
            foreach (CardPair cardPair in TableCardPairs)
            {
                if (!cardPair.isFull)
                {
                    _gameUI.hideFoldButton();
                    Debug.Log("no fold}");
                    return;
                }
            }
            _gameUI.showFoldButton();
        }

        Debug.Log("}");
    }
    public void foldCards()
    {
        Debug.Log("Table: fold cards {");

        Debug.Log("Table: for [");
        for (int i = 0; i < TableCardPairs.Count; i++)
        {

            Debug.Log("Table: tags");
            TableCardPairs[i].FirstCard.tag = "tableNotBeatingCard";
            if (TableCardPairs[i].SecondCard != null) TableCardPairs[i].SecondCard.tag = "tableNotBeatingCard";

            Debug.Log("Table: move_to, start courutine");
            StartCoroutine(TableCardPairs[i].FirstCard.GetComponent<GameCard>().MoveTo(new Vector3(FoldPlace.position.x, (float)((float)(FoldPlace.position.y) - (float)(i / 10)), FoldPlace.position.z), new Vector3(0, 0, 0), TableCardPairs[i].FirstCard.transform.localScale, 1));
            if (TableCardPairs[i].SecondCard != null) StartCoroutine(TableCardPairs[i].SecondCard.GetComponent<GameCard>().MoveTo(new Vector3(FoldPlace.position.x, (float)((float)(FoldPlace.position.y) - (float)(i / 15)), FoldPlace.position.z), new Vector3(0, 0, 0), TableCardPairs[i].SecondCard.transform.localScale, 1));
        }
        Debug.Log("]... ");

        Debug.Log("Table: card pairs");
        TableCardPairs = new List<CardPair>();

        if (_roomRow.isAlone)
        {
            Debug.Log("Table: room is alone <-> folding invoke");
            folding.Invoke();
        }

        Debug.Log("}");
    }
    #endregion

    //Первичная проверка, можно ли вообще так походить
    #region checkers
    public bool isAbleToBeat(GameCard beatCard, GameCard beatingCard)
    {
        Debug.Log("Table: Is able to beat?;");

        if (beatCard.math.Suit == _roomRow.Trump && beatingCard.math.Suit != _roomRow.Trump)
            return true;

        if (beatingCard.math.Suit == _roomRow.Trump && beatCard.math.Suit != _roomRow.Trump)
            return false;

        if(((int)beatingCard.math.Suit) == ((int)beatCard.math.Suit))
        {
            return ((int)beatCard.math.Nominal) > ((int)beatingCard.math.Nominal);
        }
        else
        {
            return false;
        }
    }
    public bool isAbleToThrow(GameCard card)
    {
        Debug.Log("Table: Is able to throw?;");

        if (TableCardPairs.Count == 0)
        {
            if (Session.role == ERole.firstThrower)
            {
                return true;
            }
        }
        else
        {
            if (Session.role != ERole.main)
            {
                if (isRightCard(card)) return true;
            }
        }

        return false;
    }
    public bool isRightCard(GameCard card)
    {
        Debug.Log("Table: Is right card?;");

        foreach (CardPair cardPair in TableCardPairs)
        {
            if (cardPair.FirstCard.GetComponent<GameCard>().math.str_Nnominal == card.math.str_Nnominal) return true;

            else if (cardPair.SecondCard != null)
            {
                if (cardPair.SecondCard.GetComponent<GameCard>().math.str_Nnominal == card.math.str_Nnominal) return true;
            }
        }
        return false;
    }
    #endregion

    //Отвечает за часть анимаций при распределении карт
    #region card_table_graphics

    Vector3 newPos;
    Vector3 newRotate = new Vector3(0, 0, 0);

    public void SetAllTableCardsPos()
    {
        Debug.Log("Table: set all table card pos: {");

        for (int i = 0; i < TableCardPairs.Count; i++)
        {
            Debug.Log("-pair-");
            switch (i)
            {
                case 0:
                    newPos.x = -3;
                    newPos.y = -0.4f;
                    break;
                case 1:
                    newPos.x = 0;
                    newPos.y = -0.4f;
                    break;
                case 2:
                    newPos.x = 3;
                    newPos.y = -0.4f;
                    break;
                case 3:
                    newPos.x = -3;
                    newPos.y = -1.6f;
                    break;
                case 4:
                    newPos.x = 0;
                    newPos.y = -1.6f;
                    break;
                case 5:
                    newPos.x = 3;
                    newPos.y = -1.6f;
                    break;

                default:
                    newPos.x = 5;

                    newPos.y = (i-5)/ 2;
                    break;
            }

            StartCoroutine(TableCardPairs[i].FirstCard.GetComponent<GameCard>().MoveTo(newPos, newRotate, new Vector3(0.7f, 0.7f, 0.7f), 0.5f));
            newPos.y -= 0.5f;
            if (TableCardPairs[i].SecondCard != null) StartCoroutine(TableCardPairs[i].SecondCard.GetComponent<GameCard>().MoveTo(newPos, newRotate, new Vector3(0.7f, 0.7f, 0.7f), 0.5f));
        }

        Debug.Log("}");
    }

    void SortCardPairs()
    {
        Debug.Log("Table: sort card pairs");
        TableCardPairs.Sort(new CardPairComparer());
    }

    //Пара карт ДЛЯ СРАВНЕНИЯ И СОРТИРОВКИ в руке
    public class CardPair 
    {
        public GameObject FirstCard;
        public GameObject SecondCard;

        public bool isFull = false;
    }
    #endregion
}

//Сортировщик для карт в руке
public class CardPairComparer : IComparer<CardPair>
{
    public int Compare(CardPair x, CardPair y)
    {
        if (x.isFull && !y.isFull)
        {
            return -1; // x is considered smaller, so it will appear before y
        }
        else if (!x.isFull && y.isFull)
        {
            return 1; // x is considered larger, so it will appear after y
        }
        else
        {
            return 0; // x and y have the same isFull value, so their order remains unchanged
        }
    }
}
