using JSON_card;
using UnityEngine;
using System.Collections.Generic;

public class CardHolder : MonoBehaviour
{
    [SerializeField] private Sprite _cardBack;
    [SerializeField] private GameCard _cardPrefab;
    [SerializeField] private GameObject _backPrefab;
    [Header("Reference")]
    [SerializeField] private CardStyleHub _styles;

    private List<GameCard> _cards = new List<GameCard>();
    private List<GameObject> _backs = new List<GameObject>();

    public CardStyleHub Style => _styles;

    //�������� ����� ���� (�� ������) �� ������ ��������
    #region Card
    public GameCard CreateCard(Card data)
    {
        if (_cards.Count > 0)
        {
            var card = _cards[0];
            Debug.Log("CardController: cardData init");
            card.Init(data);
            Debug.Log("Table: card data suit");
            card.SetSprite(_styles.GetCardSprite(card.math));
            card.gameObject.SetActive(true);
            _cards.Remove(card);
            return card;
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