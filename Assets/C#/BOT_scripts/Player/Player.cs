using System.Collections.Generic;
using JSON_card;
using UnityEngine;

[System.Serializable]
public class Player // используется для игры с ботом
{
    public User user;
    public List<Card> cards_old = new List<Card>();

    private ESuit _trumpSuit;
    private List<CardItem> _cards = new List<CardItem>();

    public event System.Action<bool> OnUpdateMode;
    public event System.Action OnGrab;
    public event System.Action<CardItem> OnAttack;
    public event System.Action<CardItem> OnAddCard;
    public event System.Action<CardItem> OnRemoveCard;
    public event System.Action<CardItem, CardItem> OnBeat;

    public int CountCards => _cards.Count;
    public CardItem Trump { get; private set; }
    public Player Target { get; private set; }

    public void SetTarget(Player player)
    {
        if (player != this)
        {
            Target = player;
        }
        else
        {
            Target = null;
        }
    }
    #region Mode
    public void SetMode(bool mode)
    {
        OnUpdateMode?.Invoke(mode);
    }
    #endregion
    #region Attack
    public void Attack()
    {
        var attackSet = GetCards(true);
        if (attackSet.Count > 0)
        {
            var attack = attackSet[Random.Range(0, attackSet.Count)];
            OnAttack?.Invoke(attack);
        }
        else
        {
            var trump = GetMinTrump(_trumpSuit);
            OnAttack?.Invoke(trump);
        }
    }

    public void Attack(CardItem attack)
    {
        Target.TakeAttack(attack);
    }

    public List<CardItem> GetCards(ENominal nominal)
    {
        var list = new List<CardItem>();
        foreach (var card in _cards)
        {
            if (card.data.Nominal == nominal)
                list.Add(card);
        }
        return list;
    }

    #endregion
    #region Beat
    public bool TakeAttack(CardItem attack)
    {
        if (Beat(attack, attack.suit))
        {
            return true;
        }
        else if (attack.suit != _trumpSuit)
        {
            var trump = GetMinTrump(_trumpSuit);
            if (trump != null)
            {
                OnBeat?.Invoke(attack, trump);
                return true;
            }
        }
        return false;
    }

    private bool Beat(CardItem attack, ESuit suit)
    {
        foreach (var card in GetCards(suit))
        {
            if (card > attack)
            {
                OnBeat?.Invoke(attack, card);
                return true;
            }
        }
        return false;
    }
    #endregion

    private List<CardItem> GetCards(bool deleteTrunp)
    {
        var list = new List<CardItem>();
        if (deleteTrunp)
        {
            foreach (var card in _cards)
            {
                if (card.suit != _trumpSuit)
                    list.Add(card);
            }
            return list;
        }
        return _cards;
    }

    private List<CardItem> GetCards(ESuit suit)
    {
        var list = new List<CardItem>();
        foreach (var card in _cards)
        {
            if (card.suit == suit)
                list.Add(card);
        }
        return list;
    }

    #region Card

    public bool Contain(CardItem target)
    {
        foreach (var card in _cards)
        {
            if (card == target)
            {
                return true;
            }
        }
        return false;
    }

    public void SetCard(CardItem[] cards)
    {
        _cards.Clear();
        _cards.AddRange(cards);
    }

    public void GrabCard()
    {
        OnGrab?.Invoke();
    }

    public void AddCard(CardItem card)
    {
        _cards.Add(card);
        OnAddCard?.Invoke(card);
    }

    public void AddCard(CardItem[] cards)
    {
        _cards.AddRange(cards);
        foreach (var card in cards)
        {
            OnAddCard?.Invoke(card);
        }
    }

    public void RemoveCard(CardItem card)
    {
        _cards.Remove(card);
        OnRemoveCard?.Invoke(card);
    }

    public void SetTrump(ESuit suit)
    {
        Trump = GetMinTrump(suit);
        _trumpSuit = suit;

    }

    private CardItem GetMinTrump(ESuit suit)
    {
        CardItem trump = null;
        foreach (var card in _cards)
        {
            if (card.suit == suit)
            {
                if (trump > card)
                    trump = card;
            }
        }
        return trump;
    }
    #endregion
}