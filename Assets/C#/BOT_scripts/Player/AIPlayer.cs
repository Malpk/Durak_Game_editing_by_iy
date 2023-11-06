using System.Collections;
using UnityEngine;

public class AIPlayer : MonoBehaviour
{
    [SerializeField] private Vector2 _timeThink;

    private Player _bind;

    private Coroutine _state;

    private void OnDestroy()
    {
        if (_bind != null)
            _bind.OnUpdateMode -= SetMode;
    }

    public void BindPlayer(Player player)
    {
        if (_bind != null)
        {
            _bind.OnUpdateMode -= SetMode;
        }
        _bind = player;
        _bind.OnUpdateMode += SetMode;
    }

    private void SetMode(bool mode)
    {
        enabled = mode;
        if (mode)
        {
            if (_state == null)
                _state = StartCoroutine(Attacking());
        }
        else if (!mode)
        {
            if (_state!= null)
            {
                StopCoroutine(_state);
                _state = null;
            }
        }
    }

    private IEnumerator Attacking()
    {
        yield return new WaitForSeconds(Random.Range(_timeThink.x, _timeThink.y));
        _bind.Attack();
        yield return new WaitWhile(() => enabled);
        _state = null;
    }


}
