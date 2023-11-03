using System.Collections.Generic;
using UnityEngine;

public class AiPlayerCreater : MonoBehaviour
{
    [SerializeField] private AIPlayer _prefab;

    private List<AIPlayer> _pool;

    public AIPlayer Create(Player bind)
    {
        var aiPlayer = GetPlayer();
        aiPlayer.BindPlayer(bind);
        return aiPlayer;
    }

    private AIPlayer GetPlayer()
    {
        if (_pool.Count > 0)
        {
            var aiPlayer = _pool[0];
            aiPlayer.gameObject.SetActive(true);
            _pool.Remove(aiPlayer);
            return aiPlayer;
        }
        return Instantiate(_prefab.gameObject).GetComponent<AIPlayer>();
    }
}
