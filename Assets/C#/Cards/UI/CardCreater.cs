using UnityEngine;

public class CardCreater : MonoBehaviour
{
    [SerializeField] private Sprite _back;
    [SerializeField] private CardUI _prefab;
    [Header("Reference")]
    [SerializeField] private Canvas _table;
    [SerializeField] private Transform _caloda;

    public CardUI CreateCard(CardItem cardData, Transform cardHolder ,bool back = false)
    {
        var card = Instantiate(_prefab.gameObject, cardHolder).GetComponent<CardUI>();
        card.transform.position = _caloda.position;
        card.Initializate(cardData, GetSprite(cardData, back), _table);
        return card;
    }

    private Sprite GetSprite(CardItem card, bool back)
    {
        if (!back)
            return CardStyleHub.Instance.GetCardSprite(card.suit, card.data.Nominal);
        else
            return _back;
    }
}
