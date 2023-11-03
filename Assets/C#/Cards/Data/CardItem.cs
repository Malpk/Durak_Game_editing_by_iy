using UnityEngine;

[System.Serializable]
public class CardItem  // тип данных хранящий информацию о карте, используется в колоде
{
    [SerializeField] private string _name;

    public readonly ESuit suit;
    public readonly CardData data;

    public static bool operator >(CardItem first,CardItem second)
    {
        if (first == null)
            return true;
        else
            return first.data.Force > second.data.Force;
    }
    public static bool operator <(CardItem first, CardItem second)
    {
        if (first == null)
            return true;
        else
            return first.data.Force < second.data.Force;
    }

    public CardItem(ESuit suit, CardData data)
    {
        _name = suit.ToString() + "-" + data.Nominal.ToString();
        this.suit = suit;
        this.data = data;
    }
}
