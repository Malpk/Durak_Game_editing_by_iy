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
        Debug.LogWarning(players.Length);
        foreach (var player in _players)
        {
            player.OnGrab += () => _list.Remove(player);
        }
        _list.AddRange(_players);
        OnChooseStarted.Invoke(SetStartPlayer());
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

    private Player SetStartPlayer()  // выбираем атакуещего
    {
        var attaked = GetAttakedPlayer();
        while (_list.Count > 0)
        {
            if (_list[0] == attaked)
            {
                _list.Remove(attaked);
                return attaked;
            }
            else
            {
                _list.Remove(_list[0]);
            }
        }
        return attaked;
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

    private void DeleteWinPlayer()  //удаляем победителей из списка тех кто ходит
    {
        foreach (var player in _players)
        {
            if (player.CountCards == 0)
                _list.Remove(player);
        }
    }

    private Player GetTarget()  //Получаем защитника
    {
        UpdateList();
        return _list[0];
    }

    private Player GetAttaked()  //Получаем атакуещего
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
}
