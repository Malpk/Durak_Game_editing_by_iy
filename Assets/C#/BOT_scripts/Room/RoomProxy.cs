using UnityEngine;

public class RoomProxy : MonoBehaviour
{
    [Min(2)]
    [SerializeField] private int _countPlayer = 2;
    [SerializeField] private User _prefab;
    [Header("Reference")]
    [SerializeField] private Canvas _table;
    [SerializeField] private LocalSesion _session;

    private void Start()
    {
        for (int i = 0; i < _countPlayer - 1; i++)
        {
            _session.AddEnemy(CreatePlayer());
        }
        _session.AddPlayer(CreatePlayer());
        _session.Play();
    }

    private Player CreatePlayer()
    {
        var player = new Player();
        player.user = Instantiate(_prefab.gameObject, _table.transform).
            GetComponent<User>();
        return player;
    }
}
