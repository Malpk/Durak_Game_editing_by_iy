using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardStyleHub : MonoBehaviour
{
    public static CardStyleHub Instance;

    [Header("base")]
    [SerializeField] private List<Sprite> BaseCardsHeartsTexturies;
    [SerializeField] private List<Sprite> BaseCardsDiamondsTexturies;
    [SerializeField] private List<Sprite> BaseCardsClubsTexturies;
    [SerializeField] private List<Sprite> BaseCardsSpadesTexturies;

    [Header("russisn")]
    [SerializeField] private List<Sprite> RussisnCardsHeartsTexturies;
    [SerializeField] private List<Sprite> RussisnCardsDiamondsTexturies;
    [SerializeField] private List<Sprite> RussisnCardsClubsTexturies;
    [SerializeField] private List<Sprite> RussisnCardsSpadesTexturies;

    [Header("natureMiddleLine")]
    [SerializeField] private List<Sprite> natureMiddleLineCardsHeartsTexturies;
    [SerializeField] private List<Sprite> natureMiddleLineCardsDiamondsTexturies;
    [SerializeField] private List<Sprite> natureMiddleLineCardsClubsTexturies;
    [SerializeField] private List<Sprite> natureMiddleLineCardsSpadesTexturies;

    [Header("fallout")]
    [SerializeField] private List<Sprite> falloutCardsHeartsTexturies;
    [SerializeField] private List<Sprite> falloutCardsDiamondsTexturies;
    [SerializeField] private List<Sprite> falloutCardsClubsTexturies;
    [SerializeField] private List<Sprite> falloutCardsSpadesTexturies;

    [Header("natureTropicks")]
    [SerializeField] private List<Sprite> natureTropicksCardsHeartsTexturies;
    [SerializeField] private List<Sprite> natureTropicksCardsDiamondsTexturies;
    [SerializeField] private List<Sprite> natureTropicksCardsClubsTexturies;
    [SerializeField] private List<Sprite> natureTropicksCardsSpadesTexturies;

    [Header("herouse")]
    [SerializeField] private List<Sprite> herouseCardsHeartsTexturies;
    [SerializeField] private List<Sprite> herouseCardsDiamondsTexturies;
    [SerializeField] private List<Sprite> herouseCardsClubsTexturies;
    [SerializeField] private List<Sprite> herouseCardsSpadesTexturies;

    [Header("cars")]
    [SerializeField] private List<Sprite> carsCardsHeartsTexturies;
    [SerializeField] private List<Sprite> carsCardsDiamondsTexturies;
    [SerializeField] private List<Sprite> carsCardsClubsTexturies;
    [SerializeField] private List<Sprite> carsCardsSpadesTexturies;

    [Header("horror")]
    [SerializeField] private List<Sprite> horrorCardsHeartsTexturies;
    [SerializeField] private List<Sprite> horrorCardsDiamondsTexturies;
    [SerializeField] private List<Sprite> horrorCardsClubsTexturies;
    [SerializeField] private List<Sprite> horrorCardsSpadesTexturies;

    [Header("erotick")]
    [SerializeField] private List<Sprite> erotickCardsHeartsTexturies;
    [SerializeField] private List<Sprite> erotickCardsDiamondsTexturies;
    [SerializeField] private List<Sprite> erotickCardsClubsTexturies;
    [SerializeField] private List<Sprite> erotickCardsSpadesTexturies;

    private List<Sprite> cards_texturies_Hearts = new List<Sprite>();
    private List<Sprite> cards_texturies_Diamonds = new List<Sprite>();
    private List<Sprite> cards_texturies_Clubs = new List<Sprite>();
    private List<Sprite> cards_texturies_Spades = new List<Sprite>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            Debug.LogWarning("Destroy cardStyle");
        }
    }

    public void Load(string style)
    {
        switch (style)
        {
            case "Russian":
                cards_texturies_Hearts = RussisnCardsHeartsTexturies;
                cards_texturies_Diamonds = RussisnCardsDiamondsTexturies;
                cards_texturies_Clubs = RussisnCardsClubsTexturies;
                cards_texturies_Spades = RussisnCardsSpadesTexturies;
                break;

            case "nature_middleLine":
                cards_texturies_Hearts = natureMiddleLineCardsHeartsTexturies;
                cards_texturies_Diamonds = natureMiddleLineCardsDiamondsTexturies;
                cards_texturies_Clubs = natureMiddleLineCardsClubsTexturies;
                cards_texturies_Spades = natureMiddleLineCardsSpadesTexturies;
                break;

            case "Fallout":
                cards_texturies_Hearts = falloutCardsHeartsTexturies;
                cards_texturies_Diamonds = falloutCardsDiamondsTexturies;
                cards_texturies_Clubs = falloutCardsClubsTexturies;
                cards_texturies_Spades = falloutCardsSpadesTexturies;
                break;

            case "nature_tropicks":
                cards_texturies_Hearts = natureTropicksCardsHeartsTexturies;
                cards_texturies_Diamonds = natureTropicksCardsDiamondsTexturies;
                cards_texturies_Clubs = natureTropicksCardsClubsTexturies;
                cards_texturies_Spades = natureTropicksCardsSpadesTexturies;
                break;

            case "herouse":
                cards_texturies_Hearts = herouseCardsHeartsTexturies;
                cards_texturies_Diamonds = herouseCardsDiamondsTexturies;
                cards_texturies_Clubs = herouseCardsClubsTexturies;
                cards_texturies_Spades = herouseCardsSpadesTexturies;
                break;

            case "cars":
                cards_texturies_Hearts = carsCardsHeartsTexturies;
                cards_texturies_Diamonds = carsCardsDiamondsTexturies;
                cards_texturies_Clubs = carsCardsClubsTexturies;
                cards_texturies_Spades = carsCardsSpadesTexturies;
                break;

            case "horror":
                cards_texturies_Hearts = horrorCardsHeartsTexturies;
                cards_texturies_Diamonds = horrorCardsDiamondsTexturies;
                cards_texturies_Clubs = horrorCardsClubsTexturies;
                cards_texturies_Spades = horrorCardsSpadesTexturies;
                break;

            case "erotick":
                cards_texturies_Hearts = erotickCardsHeartsTexturies;
                cards_texturies_Diamonds = erotickCardsDiamondsTexturies;
                cards_texturies_Clubs = erotickCardsClubsTexturies;
                cards_texturies_Spades = erotickCardsSpadesTexturies;
                break;

            default:
                cards_texturies_Hearts = BaseCardsHeartsTexturies;
                cards_texturies_Diamonds = BaseCardsDiamondsTexturies;
                cards_texturies_Clubs = BaseCardsClubsTexturies;
                cards_texturies_Spades = BaseCardsSpadesTexturies;
                break;
        }
    }

    public Sprite GetCardSprite(CardMath math)
    {
        Debug.Log("CardController: card_data_math_suit");
        switch (math.Suit)
        {
            case ESuit.CLOVERS:
                return chooseCardNumber(cards_texturies_Clubs, math.Nominal);
            case ESuit.TILE:
                return chooseCardNumber(cards_texturies_Diamonds, math.Nominal);
            case ESuit.PIKES:
                return chooseCardNumber(cards_texturies_Spades, math.Nominal);
            default:
                return chooseCardNumber(cards_texturies_Hearts, math.Nominal);
        }
    }

    private Sprite chooseCardNumber(List<Sprite> Cards, ENominal nominal)
    {
        Debug.Log("CardController: choose card number;");
        switch (nominal)
        {
            case ENominal.TWO:
                return Cards[1];
            case ENominal.THREE:
                return Cards[2];
            case ENominal.FOUR:
                return Cards[3];
            case ENominal.FIVE:
                return Cards[4];
            case ENominal.SIX:
                return Cards[5];
            case ENominal.SEVEN:
                return Cards[6];
            case ENominal.EIGHT:
                return Cards[7];
            case ENominal.NINE:
                return Cards[8];
            case ENominal.TEN:
                return Cards[9];
            case ENominal.JACK:
                return Cards[10];
            case ENominal.QUEEN:
                return Cards[11];
            case ENominal.KING:
                return Cards[12];
            default:
                return Cards[0];
        }
    }


}
