using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class TrumpHolder : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Image _trumpIcon;
    [SerializeField] private Image _trumpColoda;
    [SerializeField] private Transform _colodaPosition;
    [SerializeField] private TextMeshProUGUI _trumpText;

    private GameCard _trumpCard;

    public void SetTrump(GameCard card)
    {
        Debug.LogWarning("cards");
        Debug.Log("Trump init start");
        Debug.Log("Trump is null? " + card == null);
        SetText(card.math.strimg_Suit);
        Debug.Log("Trump init end");
        _trumpIcon.sprite = card.Sprite;
        _trumpColoda.gameObject.SetActive(true);
        _trumpIcon.gameObject.SetActive(true);
        _trumpCard = card;
        _trumpCard.gameObject.SetActive(false);
        _trumpCard.transform.SetParent(gameObject.transform);
        Debug.Log("Room: card data init (trump)");
    }

    public void HideColode()
    {
        Debug.Log("Room: OnColodaEmpty");
        _trumpColoda.gameObject.SetActive(false);
    }
    public void HideTrump()
    {
        Debug.Log("Room: OnTrumpIsDone");
        _trumpIcon.gameObject.SetActive(false);
    }

    private void SetText(string suit)
    {
        _trumpText.SetText(suit);
        //Задаём символ козыря и цвет масти
        if (suit.Contains('♥') || suit.Contains('♦'))
        {
            _trumpText.color = Color.red;
        }
        else
        {
            _trumpText.color = Color.black;
        }
        Debug.Log(_trumpText.text);
    }
}
