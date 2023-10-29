using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeButton : MonoBehaviour
{
    [SerializeField] private Image _field;
    [SerializeField] private TMP_Text _timer;

    public void SetActive(bool mode)
    {
        gameObject.SetActive(mode);
    }

    public void UpdateTimer(int time, float progress)
    {
        _timer.SetText($"{(int)time}");
        _field.fillAmount = progress;
    }
}
