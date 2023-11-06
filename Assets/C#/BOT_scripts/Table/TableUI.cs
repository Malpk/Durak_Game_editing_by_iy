using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TableUI : MonoBehaviour, IDropHandler  //стол, который отображает ходы игроков
{
    [Header("Reference")]
    [SerializeField] private GameObject _proxyCard;
    [SerializeField] private PlayerPanel _player;
    [SerializeField] private PlayerPanel[] _enemy;
    [SerializeField] private Transform _tableHolder;
    [Header("Game")]
    [SerializeField] private PlayerPanel _attaked;
    [SerializeField] private PlayerPanel _defender;

    private List<CardUI> _attacks = new List<CardUI>();
    private List<CardUI> _beat = new List<CardUI>();

    public void BindPlayer(Player player)
    {
        _player.BindPlayer(player);
    }

    public bool BindEnemy(Player player)
    {
        foreach (var panel in _enemy)
        {
            if (panel.Bind == null)
            {
                panel.BindPlayer(player);
                return true;
            }
        }
        return false;
    }

    public void SetSteap(Player attaked, Player defender)
    {
        if(_attaked != null)
            _attaked.Bind.OnAttack -= OnThrowCard;
        if(_defender!= null)
            _defender.Bind.OnBeat -= OnBeatCard;
        _attaked = GetPanel(attaked);
        _defender = GetPanel(defender);
        _attaked.Bind.OnAttack += OnThrowCard;
        _defender.Bind.OnBeat += OnBeatCard;
    }

    private PlayerPanel GetPanel(Player player)
    {
        foreach (var panel in _enemy)
        {
            if (panel.Bind == player)
                return panel;
        }
        if (_player.Bind == player)
            return _player;
        else
            return null;
    }


    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent(out CardUI card))
        {
            if (_attaked.Bind.Contain(card.Data))
            {
                _attaked.Bind.RemoveCard(card.Data);
                ThrowCard(card);
                _attacks.Add(card);
            }
            else if(TryBeat(card, out CardUI beat))
            {
                _attacks.Remove(beat);
                _beat.Add(beat);
            }
        }
    }

    private bool TryBeat(CardUI card, out CardUI result)
    {
        result = null;
        foreach (var attack in _attacks)
        {
            if (attack.Data > card.Data)
            {
                if (result == null)
                {
                    result = attack;
                }
                else
                {
                    var temp = Vector2.Distance(card.transform.localPosition,
                        attack.transform.localPosition);
                    var curret = Vector2.Distance(card.transform.localPosition,
                        result.transform.localPosition);
                    result = curret > temp ? attack : result;
                }
            }
        }
        return result;
    }

    public void ThrowCard(CardUI card)
    {
        var point = Instantiate(_proxyCard, _tableHolder).
            GetComponent<RectTransform>();
        card.Bind(point);
    }

    private void OnThrowCard(CardItem data)
    {
        var card = _attaked.GetCard(data);
        card.transform.parent = transform;
        ThrowCard(card);
    }

    private void OnBeatCard(CardItem data, CardItem target)
    {

    }
}
