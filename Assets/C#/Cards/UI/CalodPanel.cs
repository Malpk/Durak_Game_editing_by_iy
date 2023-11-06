using UnityEngine;
using UnityEngine.UI;

public class CalodPanel : MonoBehaviour
{
    [SerializeField] private Caloda _caloda;
    [SerializeField] private Image _coloda;
    [SerializeField] private Image _trump;

    private void OnEnable()
    {
        _caloda.OnEmpety += () => _caloda.gameObject.SetActive(false);
        _caloda.OnSetTrump += SetStyle;
    }

    private void OnDisable()
    {
        _caloda.OnEmpety -= () => _caloda.gameObject.SetActive(false);
        _caloda.OnSetTrump -= SetStyle;
    }

    private void SetStyle(CardItem card)
    {
        _trump.sprite = CardStyleHub.Instance.GetCardSprite(card.suit, card.data.Nominal);
    }

}
