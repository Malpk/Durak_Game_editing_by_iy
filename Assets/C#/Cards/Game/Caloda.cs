using System.Collections.Generic;
using UnityEngine;

public class Caloda : MonoBehaviour
{
    private static ESuit[] Suit = { ESuit.CLOVERS, ESuit.HEART, ESuit.PIKES, ESuit.TILE };

    [SerializeField] private CardData[] _cards;
    [SerializeField] private List<CardItem> _colode = new List<CardItem>();

    private List<CardItem> _pool = new List<CardItem>();

    public event System.Action OnEmpety;  //событие срабатывает когда колода пустая
    public event System.Action<CardItem> OnSetTrump;  //событие срабатывает когда устанавливается козырь
    public int CountCards => _colode.Count;
    public CardItem Trump { get; private set; }

    public void MixColode(int count)
    {
        SetColode();
        for (int i = 0; i < count; i++)
        {
            _colode = MixeColode(_colode);
        }
        Trump = _colode[_colode.Count - 1];
        OnSetTrump?.Invoke(Trump);
        _colode.Remove(Trump);
    }

    public CardItem GetCard()
    {
        if (_colode.Count > 0)
        {
            var card = _colode[0];
            _colode.Remove(card);
            if (_colode.Count <= 1)
                OnEmpety?.Invoke();
            return card;
        }
        else
        {
            return null;
        }
    }

    public CardItem[] GetCards(int count)
    {
        count = Mathf.Clamp(count, 0, _colode.Count);
        var cards = new CardItem[count];
        for (int i = 0; i < count; i++)
        {
            cards[i] = _colode[0];
            _colode.Remove(cards[i]);
        }
        if (_colode.Count <= 1)
            OnEmpety?.Invoke();
        return cards;
    }

    #region Colode
    private void SetColode()
    {
        if (_pool.Count > 0)
        {
            _colode.AddRange(_pool);
            _pool.Clear();
        }
        else
        {
            _colode = CreateColode();
        }
    }

    private List<CardItem> CreateColode()
    {
        var list = new List<CardItem>();
        foreach (var card in _cards)
        {
            foreach (var suit in Suit)
            {
                list.Add(new CardItem(suit, card));
            }
        }
        return list;
    }
    #endregion
    #region Mixe
    private List<CardItem> MixeColode(List<CardItem> colode)
    {
        var newColode = new List<CardItem>();
        while (colode.Count > 0)
        {
            var card = colode[Random.Range(0, colode.Count)];
            colode.Remove(card);
            newColode.Add(card);
        }
        return newColode;
    }
    #endregion
}
