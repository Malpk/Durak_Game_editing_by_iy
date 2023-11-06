using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class CardUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [Range(0,1f)]
    [SerializeField] private float _smoothTime;
    [Header("Referemce")]
    [SerializeField] private Image _icon;
    [SerializeField] private Canvas _table;
    [SerializeField] private RectTransform _rect;

    private bool _block;
    private Vector2 _velocityMove;
    private Coroutine _move;
    private Transform _parent;
    private RectTransform _point;

    public CardItem Data { get; private set; }

    public void Initializate(CardItem item ,Sprite icon, Canvas table)
    {
        Data = item;
        _table = table;
        _icon.sprite = icon;
    }

    public void SetBlock(bool mode)
    {
        _block = mode;
    }

    public void Bind(RectTransform point)
    {
        UnBind();
        _point = point;
        _move = StartCoroutine(MoveTo(point));
    }

    public void UnBind()
    {
        if (_move != null)
            StopCoroutine(_move);
    }

    private IEnumerator MoveTo(RectTransform target)
    {
        while (enabled)
        {
            _rect.localPosition = Vector2.SmoothDamp(_rect.localPosition,
                target.localPosition, ref _velocityMove, _smoothTime);
            yield return null;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!_block)
        {
            if (_move != null)
                StopCoroutine(_move);
            _parent = transform.parent;
            transform.parent = _table.transform;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_block)
        {
            _rect.anchoredPosition += eventData.delta / _table.scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!_block)
        {
            if (transform.parent == _table.transform)
            {
                transform.parent = _parent;
                _move = StartCoroutine(MoveTo(_point));
            }
        }
    }

}
