using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JSON_card;

public class CardMath {

    private string _suit;
    private string _nominal;

    public CardMath(string _suit, string _nominal) { 
        this._suit = _suit;
        this._nominal = _nominal;
    }

    public ESuit Suit
    {
        get
        {
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
    public string strimg_Suit
    {
        get
        {
            return _suit;
        }
    }

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
    public string str_Nnominal
    {
        get
        {
            return _nominal;
        }
    }

    public Card json {
        get
        {
            return new Card { suit = _suit, nominal = _nominal};
        }
    }
}
