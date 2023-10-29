using UnityEngine;
using System.Collections.Generic;

public class BotHolder : MonoBehaviour
{
    [SerializeField] private alone_Game_BOT _botPreffab; //Скрипт бота

    private List<alone_Game_BOT> _pool = new List<alone_Game_BOT>();

    public void AddBot(Room room)
    {
        room._roomRow.isAlone = true;

        Debug.Log("Room: bot init");
        var enemy = CreateBot();

        enemy.Init(room);

        Debug.Log("Room: bot omready finished");

        Debug.Log("}");
    }

    public bool DeleteBot(alone_Game_BOT bot)
    {
        bot.gameObject.SetActive(false);
        if (!_pool.Contains(bot))
        {
            _pool.Add(bot);
            return true;
        }
        return false;
    }

    private alone_Game_BOT CreateBot()
    {
        if (_pool.Count > 0)
        {
            var bot = _pool[0];
            bot.gameObject.SetActive(true);
            bot.transform.parent = transform;
            bot.transform.localPosition = Vector3.zero;
        }
        return Instantiate(_botPreffab, transform).
            GetComponent<alone_Game_BOT>();
    }

}
