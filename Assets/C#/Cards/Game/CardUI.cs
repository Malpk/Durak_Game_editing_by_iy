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
    [SerializeField] private Canvas _canvas;
    [SerializeField] private RectTransform _rect;

    private Vector2 _velocityMove;
    private Coroutine _move;
    private Transform _player;

    public CardItem Data { get; private set; }

    public void Initializate(CardItem item ,Sprite icon)
    {
        Data = item;
        _icon.sprite = icon;
    }

    public void Bind(RectTransform point)
    {
        if (_move != null)
            StopCoroutine(_move);
        _move = StartCoroutine(MoveTo(point));
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
        if (_move != null)
            StopCoroutine(_move);
        _player = transform.parent;
        transform.parent = _canvas.transform;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rect.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_move == null)
            transform.parent = _player;
    }

}
