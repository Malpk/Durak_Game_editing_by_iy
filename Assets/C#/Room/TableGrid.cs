using UnityEngine;
using System.Collections.Generic;

public class TableGrid : MonoBehaviour
{
    [SerializeField] private GridRow[] _rows;

    private List<GameCard> _cards = new List<GameCard>();

    public int CountCard => _cards.Count;

    public bool AddCard(GameCard card)
    {
        Debug.LogWarning("add");
        foreach (var row in _rows)
        {
            if (row.IsReady)
            {
                _cards.Add(card);
                card.BindPoint(row.GetPoint());
                return true;
            }
        }
        return false;
    }

    public void Clear()
    {
        _cards.Clear();
        foreach (var row in _rows)
        {
            row.Clear();
        }
    }

}
