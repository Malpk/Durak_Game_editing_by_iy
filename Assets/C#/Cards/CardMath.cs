using System.Collections.Generic;
using JSON_card;

//Содержит основные характеристики карты.
public class CardMath {

    public readonly ESuit suit;
    public readonly ENominal nominal;

    private string _suit; //символ масти
    private string _nominal; //номинал карты

    public CardMath(CardItem card)
    {
        suit = card.suit;
        nominal = card.data.Nominal;
    }

    //Конструктор (классический)
    public CardMath(string _suit, string _nominal) { 
        this._suit = _suit;
        this._nominal = _nominal;
    }

    //Переделываем символ рубашки в соответствующий enum
    public ESuit Suit
    {
        get
        {
            //СОХРАНЯТЬ при копировании ТОЛЬКО НАСИЛЬНО и только В UTF-8
            switch (_suit)
            {
                case "♥":
                    return ESuit.HEART;
                case "♦":
                    return ESuit.TILE;
                case "♣":
                    return ESuit.CLOVERS;
                case "♠":
                    return ESuit.PIKES;

                default: throw new System.Exception("Error: unknown suit");
            }
        }
    }
    //Получаем символ масти
    public string strimg_Suit
    {
        get
        {
            return _suit;
        }
    }

    //Переделываем получаемый с сервера номинал в нужный enum. Словарь вместо огромного switch-case
    Dictionary<string, ENominal> nominalLookup = new Dictionary<string, ENominal>
        {
            { "2 ", ENominal.TWO    },
            { "3 ", ENominal.THREE  },
            { "4 ", ENominal.FOUR   },
            { "5 ", ENominal.FIVE   },
            { "6 ", ENominal.SIX    },
            { "7 ", ENominal.SEVEN  },
            { "8 ", ENominal.EIGHT  },
            { "9 ", ENominal.NINE   },
            { "10", ENominal.TEN    },
            { "В ", ENominal.JACK   },
            { "Д ", ENominal.QUEEN  },
            { "К ", ENominal.KING   },
            { "Т ", ENominal.COUNT  }
        };
    public ENominal Nominal
    {
        get
        {
            if (nominalLookup.TryGetValue(_nominal, out ENominal result))
            {
                return result;
            }
            else
            {
                throw new System.Exception("Error: unknown nominal");
            }
        }
    }

    //Строка номинала
    public string str_Nnominal
    {
        get
        {
            return _nominal;
        }
    }

    //Для переделывания карты в json файл (перед отправкой на сервер)
    public Card json {
        get
        {
            return new Card { suit = _suit, nominal = _nominal};
        }
    }
}
