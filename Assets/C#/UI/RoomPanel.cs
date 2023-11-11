using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using TMPro;

public class RoomPanel : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _id;

    public void Initializate(string id, UnityAction action)
    {
        _id.SetText("ID: " + id);
        _button.onClick.AddListener(action);
    }
}
