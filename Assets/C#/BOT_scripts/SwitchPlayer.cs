using UnityEngine;
using System.Collections.Generic;

public class SwitchPlayer : MonoBehaviour
{
    private Player[] _players;
    private List<Player> _list = new List<Player>();

    public event System.Action<Player> OnChooseStarted;

    #region Play
    public void Play(Player[] players)
    {
        _players = players;
        foreach (var player in _players)
        {
            player.OnGrab += () => _list.Remove(player);
        }
        _list.AddRange(_players);
        SetStartPlayer();
    }

    public void Stop()
    {
        foreach (var player in _players)
        {
            player.OnGrab -= () => _list.Remove(player);
        }
        _list.Clear();
        _players = null;
    }

    private void SetStartPlayer()  // �������� ����������
    {
        var attaked = GetAttakedPlayer();
        while (_list[0] != attaked || _list.Count > 0)
        {
            _list.Remove(_list[0]);
        }
        _list.Remove(attaked);
        OnChooseStarted.Invoke(attaked);
    }

    private Player GetAttakedPlayer()
    {
        var attacked = _players[0];
        foreach (var player in _players)
        {
            if (attacked.Trump > player.Trump)
                attacked = player;
        }
        return attacked;
    }
    #endregion

    public Player GetNextPlayer()
    {
        DeleteWinPlayer();
        var attaked = GetAttaked();
        attaked.SetTarget(GetTarget());
        return attaked;
    }

    private void DeleteWinPlayer()  //������� ����������� �� ������ ��� ��� �����
    {
        foreach (var player in _players)
        {
            if (player.CountCards == 0)
                _list.Remove(player);
        }
    }

    private Player GetTarget()  //�������� ���������
    {
        UpdateList();
        return _list[0];
    }

    private Player GetAttaked()  //�������� ����������
    {
        UpdateList();
        var attaked = _list[0];
        _list.Remove(attaked);
        return attaked;
    }

    private void UpdateList()
    {
        if (_list.Count == 0)
        {
            foreach (var player in _players)
            {
                if (player.CountCards > 0)
                    _list.Add(player);
            }
        }
    }
    private int CountActivePlayer() //������� ���������� ������� � ������� ���� �����
    {
        var count = 0;
        foreach (var player in _players)
        {
            if (player.CountCards > 0)
                count++;
        }
        return count;
    }
}