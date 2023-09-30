using UnityEngine;

//Пустой скрипт. Делает так, чтобы игровой объект никогда не уничтожался силами движка. Это единственное его применение
public class DontDestroy : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
