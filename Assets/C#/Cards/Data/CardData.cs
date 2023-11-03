using UnityEngine;

[CreateAssetMenu(menuName ="Card/CardData")]
public class CardData : ScriptableObject  //Информация о карте
{
    [SerializeField] private int _force;
    [SerializeField] private ENominal _nominal;  

    public int Force => _force;
    public ENominal Nominal => _nominal;

}
