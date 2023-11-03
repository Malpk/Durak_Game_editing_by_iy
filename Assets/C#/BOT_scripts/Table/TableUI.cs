using UnityEngine;
using UnityEngine.EventSystems;

public class TableUI : MonoBehaviour, IDropHandler  //стол, который отображает ходы игроков
{
    [Header("Reference")]
    [SerializeField] private GameObject _proxyCard;
    [SerializeField] private PlayerPanel _player;
    [SerializeField] private PlayerPanel[] _enemy;
    [SerializeField] private Transform _tableHolder;

    private Player _attaked;

    public void BindPlayer(Player player)
    {
        _player.BindPlayer(player);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent(out CardUI card))
        {
            _attaked.RemoveCard(card.Data);
            if (_attaked.Contain(card.Data))
            {
                ThrowCard(card);
            }
        }
    }

    private void ThrowCard(CardUI card)
    {
        var point = Instantiate(_proxyCard, _tableHolder).GetComponent<RectTransform>();
        card.Bind(point);
    }

}
