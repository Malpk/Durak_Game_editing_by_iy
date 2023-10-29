using UnityEngine;
using TMPro;

public class TrumpHolder : MonoBehaviour
{
    [SerializeField] private GameObject _colodaPrefab;
    [Header("Reference")]
    [SerializeField] private Transform _trumpPosition;
    [SerializeField] private Transform _colodaPosition;
    [SerializeField] private TextMeshProUGUI _trumpText;

    private GameCard _trump;
    private GameObject _coloda;

    public void SetTrump(GameCard card)
    {
        Debug.Log("Trump init start");
        Debug.Log("Trump is null? " + card == null);
        SetText(card.math.strimg_Suit);
        Debug.Log("Trump init end");
        _coloda = Instantiate(_colodaPrefab, _colodaPosition);

        _trump = card;
        _trump.transform.localScale = _trumpPosition.localScale;
        _trump.transform.position = _trumpPosition.position;
        _trump.transform.SetParent(gameObject.transform);
        Debug.Log("Room: card data init (trump)");
    }

    public void HideColode()
    {
        Debug.Log("Room: OnColodaEmpty");
        _trump.gameObject.SetActive(false);
    }
    public void HideTrump()
    {
        Debug.Log("Room: OnTrumpIsDone");
        _coloda.SetActive(false);
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
