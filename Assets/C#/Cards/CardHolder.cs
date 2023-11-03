using JSON_card;
using UnityEngine;
using System.Collections.Generic;

public class CardHolder : MonoBehaviour
{
    [SerializeField] private Sprite _cardBack;
    [SerializeField] private GameCard _cardPrefab;
    [SerializeField] private GameObject _backPrefab;

    private List<GameCard> _cards = new List<GameCard>();
    private List<GameObject> _backs = new List<GameObject>();


    //Создание новых карт (из колоды) на основе префабов
    #region Card
    public GameCard CreateCard(Card data)
    {
        var card = GetCard();
        Debug.Log("CardController: cardData init");
        card.Init(data);
        Debug.Log("Table: card data suit");
        card.SetSprite(CardStyleHub.Instance.GetCardSprite(card.math.Suit, card.math.Nominal));
        card.gameObject.SetActive(true);
        return card;
    }

    public GameCard CreateCard(CardItem cardData) // карты для игры с ботом
    {
        var card = Create(cardData.suit, cardData.data.Nominal);
        card.Init(cardData);
        return card;
    }

    private GameCard Create(ESuit suit, ENominal nominal)
    {
        var card = GetCard();
        Debug.Log("CardController: cardData init");
        Debug.Log("Table: card data suit");
        card.SetSprite(CardStyleHub.Instance.GetCardSprite(suit, nominal));
        card.gameObject.SetActive(true);
        return card;
    }

    private GameCard GetCard()
    {
        if (_cards.Count > 0)
        {
            var card = _cards[0];
            _cards.Remove(card);
        }
        return Instantiate(_cardPrefab.gameObject).GetComponent<GameCard>();
    }

    public void CardDelete(GameCard card)
    {
        _cards.Add(card);
        card.gameObject.SetActive(false);
    }

    public void CardDelete(GameObject card)
    {
        _cards.Add(card.GetComponent<GameCard>());
        card.SetActive(false);
    }
    #endregion
    #region Back
    public GameObject CreateBack()
    {
        if (_backs.Count > 0)
        {
            var card = _backs[0];
            card.gameObject.SetActive(true);
            _backs.Remove(card);
            return card;
        }
        return Instantiate(_backPrefab);
    }

    public void BackDelete(GameObject card)
    {
        _backs.Add(card);
        card.gameObject.SetActive(false);
    }
    #endregion
}
