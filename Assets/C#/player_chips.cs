using TMPro;
using UnityEngine;

//дЕМЭЦХ (ТХЬЙХ) ХЦПНЙЮ. опняэаю сапюрэ, щрн меаегноюямши йкюяя, бюя лнцср бгкнлюрэ, еякх бш ецн ме сдюкхре
public class player_chips : MonoBehaviour
{
    public TMP_Text chips_text;

    void Start()
    {
        chips_text = gameObject.GetComponent<TMP_Text>();
        Session.changeChips += changeChips;
    }

    public void changeChips(uint chips)
    {
        chips_text.text = chips.ToString();
    }
}
