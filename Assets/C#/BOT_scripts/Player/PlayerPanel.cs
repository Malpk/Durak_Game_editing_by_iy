using UnityEngine;
using System.Collections.Generic;

public class PlayerPanel : MonoBehaviour
{
    [SerializeField] private bool _back;  //режим отображение карты
    [SerializeField] private bool _blockController;  //блокируем доступ игрока к управлениею картами
    [Header("Reference")]
    [SerializeField] private CardCreater _caloda;
    [SerializeField] private CardRowUI _cardHolder;
    [SerializeField] private Transform _userHolder;

    private Player _bind = null;
    private List<CardUI> _cards = new List<CardUI>();

    public Player Bind => _bind;

    public void BindPlayer(Player player)
    {
        if (_bind != null)
        {
            _bind.OnAddCard -= AddCard;
        }
        _bind = player;
        if (_bind != null)
        {
            _bind.OnAddCard += AddCard;
            gameObject.SetActive(true);
            _bind.user.transform.parent = _userHolder;
            _bind.user.transform.localPosition = Vector3.zero;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public CardUI GetCard(CardItem target)
    {
        var card = GetCardFromList(target);
        _cards.Remove(card);
        if (_back)
        {
            var attack = _caloda.CreateCard(target, transform);
            attack.transform.position = card.transform.position;
            card.UnBind();
            Destroy(card);
            return attack;
        }
        else
        {
            return card;
        }
    }

    private CardUI GetCardFromList(CardItem data)
    {
        foreach (var card in _cards)
        {
            if (card.Data == data)
                return card;
        }
        return null;
    }

    private void AddCard(CardItem data)
    {
        var card = _caloda.CreateCard(data, transform ,_back);
        card.Bind(_cardHolder.GetFollow());
        card.SetBlock(_blockController);
        _cards.Add(card);
    }
}
