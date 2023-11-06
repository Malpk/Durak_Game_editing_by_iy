using UnityEngine;

[System.Serializable]
public class CardItem  // тип данных хранящий информацию о карте, используется для игры с ботом
{
    [SerializeField] private string _name;

    public readonly ESuit suit;
    public readonly CardData data;
    public CardItem(ESuit suit, CardData data)
    {
        _name = suit.ToString() + "-" + data.Nominal.ToString();
        this.suit = suit;
        this.data = data;
    }

    public static bool operator >(CardItem first,CardItem second)
    {
        if (first == second)
            return false;
        else if (first == null)
            return true;
        else
            return first.data.Force > second.data.Force;
    }
    public static bool operator <(CardItem first, CardItem second)
    {
        if (first == second)
            return false;
        else if (first == null)
            return true;
        else
            return first.data.Force < second.data.Force;
    }


}
