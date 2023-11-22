using System.Collections.Generic;
using UnityEngine;

public class GridRow : MonoBehaviour
{
    [SerializeField] private int _maxSizeRow;
    [SerializeField] private float _sizeX;
    [Header("Referenece")]
    [SerializeField] private Transform _pointPrefab;

    private List<Transform> _points = new List<Transform>();
    private List<Transform> _pool = new List<Transform>();
    public bool IsReady => _maxSizeRow > _points.Count;

    public void Clear() 
    {
        _pool.AddRange(_points);
        _points.Clear();
    }

    public Transform GetPoint()
    {
        if (IsReady)
        {
            var point = CreatePoint();
            _points.Add(point);
            UpdateContent();
            return point;
        }
        return null;
    }

    private void UpdateContent()
    {
        if (_points.Count == 1)
        {
            _points[0].localPosition = Vector3.zero;
        }
        else
        {
            var start = (_points.Count - 1) * _sizeX;
            start /= 2;
            for (int i = 0; i < _points.Count; i++)
            {
                var position = -start + _sizeX * i;
                _points[i].localPosition = Vector3.right * position;
            }
        }
    }

    private Transform CreatePoint()
    {
        if (_pool.Count > 0)
        {
            var point = _pool[0];
            _pool.Remove(point);
            return point;
        }
        return Instantiate(_pointPrefab.gameObject, transform).transform;
    }

}
