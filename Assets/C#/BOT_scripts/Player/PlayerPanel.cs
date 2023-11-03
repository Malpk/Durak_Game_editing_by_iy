using UnityEngine;

public class PlayerPanel : MonoBehaviour
{
    [SerializeField] private CardHolder _cards;
    [SerializeField] private Transform _cardPoint;
    [SerializeField] private Transform _userHolder;
    [SerializeField] private Transform _cardHolder;

    private Player _bind;

    public Player Bind => _bind;

    public void BindPlayer(Player player)
    {
        _bind = player;
        _bind.user.transform.parent = _userHolder;
    }

    public void AddCard(CardUI card)
    {
        card.Bind(Instantiate(_cardPoint.gameObject, _cardHolder).
            GetComponent<RectTransform>());
    }
}
