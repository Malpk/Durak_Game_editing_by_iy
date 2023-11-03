using UnityEngine;

public class LocalSesion : MonoBehaviour
{
    [Min(0)]
    [SerializeField] private int _maxCards = 6;
    [Header("Reference")]
    [SerializeField] private Caloda _caloda;
    [SerializeField] private SwitchPlayer _switcher;

    private Player _attacked;
    private Player[] _players;

    private void Reset()
    {
        _maxCards = 6;
    }

    private void OnEnable()
    {
        _switcher.OnChooseStarted += (Player attaked) => _attacked = attaked;
    }

    private void OnDisable()
    {
        _switcher.OnChooseStarted -= (Player attaked) => _attacked = attaked;
    }

    public void Play()
    {
        _caloda.MixColode(10);
        SetPlayerCards();
        _switcher.Play(_players);
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
        for (int i = 0; i < _players.Length && _caloda.CountCards > 0; i++)
        {
            var count = _maxCards - _players[i].CountCards;
            if (count > 0)
                _players[i].AddCard(_caloda.GetCards(count));
        }
    }

    private void SetPlayerCards()  // раздача карт
    {
        foreach (var player in _players)
        {
            player.AddCard(_caloda.GetCards(_maxCards));
            player.SetMinTrump(_caloda.Trump.suit);
        }
    }

}
