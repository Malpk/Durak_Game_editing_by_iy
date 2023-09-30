using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//Реализация внутриигрового чата
public class Chat : BaseScreen
{
    [Header("prefabs")]
    public GameObject messagePrefab; //Создание объекта сообщения внутри окошка

    [Header("UI")] //Элементы интерфейса
    public GameObject Chat_obj;
    public Transform chat_container;
    public TMP_InputField chat_input;

    //Задаём сокету обработку получения сообщения
    void Start()
    {
        SocketNetwork.got_message += gotMessage;
    }
    private void OnDestroy()
    {
        SocketNetwork.got_message -= gotMessage; //Не забываем открепить. Чат не вечен и удаляется вместе с комнатой.
    }

    //Отправка сообщения в чат
    public void sendMessage()
    {
        m_socketNetwork.Emit_sendMessage(chat_input.text);
    }

    //Получение нового сообщения из чата. Обновляем интерфейс
    public void gotMessage(uint ID, string message)
    {
        Chat_obj.SetActive(true);

        GameObject message_obj = Instantiate(messagePrefab, chat_container);

        message_obj.GetComponent<TMP_Text>().text = ID.ToString() + ": " + message;
    }
}
