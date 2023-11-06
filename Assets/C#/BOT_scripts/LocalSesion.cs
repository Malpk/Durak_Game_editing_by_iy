using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class LocalSesion : MonoBehaviour
{
    [Min(0)]
    [SerializeField] private int _maxCards = 6;
    [Header("Reference")]
    [SerializeField] private Caloda _caloda;
    [SerializeField] private TableUI _table;
    [SerializeField] private SwitchPlayer _switcher;
    [SerializeField] private AiPlayerCreater _playerHolder;

    private Room _room;
    private Player _attacked;
    private List<Player> _players = new List<Player>();

    public Caloda Caloda => _caloda;

    private void Reset()
    {
        _maxCards = 6;
    }

    private void OnEnable()
    {
        _switcher.OnChooseStarted += SetAttacket;
    }

    private void OnDisable()
    {
        _switcher.OnChooseStarted -= SetAttacket;
    }

    public void AddEnemy(Player player)
    {
        _players.Add(player);
        _table.BindEnemy(player);
        _playerHolder.Create(player);
        Debug.Log("new player: " + _players.Count);
    }

    public void AddPlayer(Player player)
    {
        _players.Add(player);
        _table.BindPlayer(player);
        Debug.Log("new player: " + _players.Count);
    }

    public void Play()
    {
        _caloda.MixColode(10);
        StartCoroutine(SetPlayerCards());
        _switcher.Play(_players.ToArray());
    }

    public void Next()
    {
        UpdatePlayerCards();
        _attacked?.SetMode(false);
        _attacked = _switcher.GetNextPlayer();
        _attacked.SetMode(true);
    }

    private void UpdatePlayerCards()  //Раздача карт
    {
        for (int i = 0; i < _players.Count && _caloda.CountCards > 0; i++)
        {
            var count = _maxCards - _players[i].CountCards;
            if (count > 0)
                _players[i].AddCard(_caloda.GetCards(count));
        }
    }

    private IEnumerator SetPlayerCards()  // раздача карт
    {
        foreach (var player in _players)
        {
            var cards = _caloda.GetCards(_maxCards);
            foreach (var card in cards)
            {
                player.AddCard(card);
                yield return new WaitForSeconds(0.2f);
            }
            player.SetTrump(_caloda.Trump.suit);
        }
    }

    private void SetAttacket(Player player)
    {
        _attacked = player;
        _table.SetAttacked(_attacked);
    }

}
