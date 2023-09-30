using System;
using System.Collections.Generic;
using UnityEngine;

//��� ������� �������� � ������������, ������������ � ��������� ������, ������������� ����� lock
public class MainThreadDispatcher : MonoBehaviour
{
    private static MainThreadDispatcher instance;
    private Queue<Action> actions = new Queue<Action>(); //������� �����

    private void Awake()
    {
        instance = this;
    }

    //��������� ���������� �������� � ���� ������. ����� ����, ��������� ���� (��. �������� lock)
    private void Update()
    {
        lock (actions)
        {
            while (actions.Count > 0)
            {
                actions.Dequeue()?.Invoke();
            }
        }
    }

    //������ � ������� ������. ��������� ����.
    public static void RunOnMainThread(Action action)
    {
        lock (instance.actions)
        {
            instance.actions.Enqueue(action);
        }
    }
}
