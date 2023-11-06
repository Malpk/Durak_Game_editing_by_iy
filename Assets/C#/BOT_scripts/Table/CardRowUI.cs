using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CardRowUI : MonoBehaviour
{
    [SerializeField] private float _maxCountCards;
    [SerializeField] private RectTransform _cardPoint;
    [Header("Reference")]
    [SerializeField] private GridLayoutGroup _layout;

    private List<RectTransform> _points = new List<RectTransform>();

    public RectTransform GetFollow()
    {
        var point = Instantiate(_cardPoint, transform).
            GetComponent<RectTransform>();
        _points.Add(point);
        UpdateRow();
        return point;
    }

    private void UpdateRow()
    {
        //Обрабатываем изменение карт
    }
}
