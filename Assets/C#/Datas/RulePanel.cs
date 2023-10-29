using UnityEngine;
using TMPro;

public class RulePanel : MonoBehaviour
{
    //Отображение текущих правил игры, количества игроков и т.п.
    public static string CurrentRules = "Error: no rules defined";
    public static string CurrentPlayers = "Error: no players defined";
    public static string CurrentTableID = "Table: local (with bot)";

    [SerializeField] private RoomRow _row;
    [SerializeField] private TextMeshProUGUI _playerCount;
    [SerializeField] private TextMeshProUGUI _rule;
    [SerializeField] private TextMeshProUGUI _table;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    //UI Rules update
    public void UpdateData()
    {
        Debug.Log("Room: upd_rules {");

        bool screen_active = gameObject.activeInHierarchy;
        gameObject.SetActive(true);

        CurrentRules = $"Rules: {_row.GameType}";
        CurrentPlayers = $"Players: {_row.maxPlayers_number} / {_row.roomPlayers.Count}";
        CurrentTableID = $"ID Room: {_row.RoomID}";

        _rule.SetText(CurrentRules);
        _table.SetText(CurrentTableID);
        _playerCount.SetText(CurrentPlayers);

        gameObject.SetActive(screen_active);

        Debug.Log("}");
    }
}
