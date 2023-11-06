using UnityEngine;
using System.Collections.Generic;

public class PlayerPanel : MonoBehaviour
{
    [SerializeField] private bool _back;
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
            _bind.OnAddCard -= GetCard;
        _bind = player;
        if (_bind != null)
        {
            _bind.OnAddCard += GetCard;
            gameObject.SetActive(true);
            _bind.user.transform.parent = _userHolder;
            _bind.user.transform.localPosition = Vector3.zero;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void GetCard(CardItem data)
    {
        var card = _caloda.CreateCard(data, transform ,_back);
        card.Bind(_cardHolder.GetFollow());
        _cards.Add(card);
    }
}
