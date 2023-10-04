using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Отображение аватарки пользователя
public class AvatarScr : MonoBehaviour
{
    public uint UserID; //айдишник юзера
    public Image avatarImage; //сама картинка

    private void Start()
    {
        //Изображение мы получаем только через сокет и никак иначе
        SocketNetwork.got_avatar += SetAvatar;
        avatarImage = GetComponent<Image>();
    }

    private void OnDestroy()
    {
        //Не забываем, аватарка не всегда от запуска до закрытия. Аватарки других игроков имеют свойство уходить вместе с комнатами в небытие
        SocketNetwork.got_avatar -= SetAvatar;
    }

    //Установка аватара для игрока
    public void SetAvatar(uint ID, Sprite sprite)
    {
        if (UserID == ID)
        {
            avatarImage.sprite = sprite;
            Debug.Log("avatar setted");
        }
    }
}
