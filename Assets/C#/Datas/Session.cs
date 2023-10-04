using UnityEngine;

//������ ���������� � ��������. ����� ������ �����
public static class Session
{

    //����������� �������. �������� ����� ���������������� ��������
    public delegate void chipsChangeEvent(uint chips);
    public static event chipsChangeEvent changeChips;  
    public delegate void UIdChangeEvent(uint UId);
    public static event UIdChangeEvent changeUId;
    public delegate void roleChange(ERole role);
    public static event roleChange roleChanged;

    public static string Token; //����� ������. ��� ���� ������ �� ��������� �������

    private static uint m_id; //ID ������
    public static uint UId
    {
        get { return m_id; }
        set
        {
            m_id = value;
            changeUId?.Invoke((uint)value);
        }
    }

    public static string Name; //���

    private static int m_chips; //������ (�����) ������
    public static int Chips
    {
        get { return m_chips; }
        set { 
            m_chips = value;

            changeChips?.Invoke((uint)value);
        }
    }

    public static int PlayedGames; //��� ������ �������


    //������ �������
    public static uint RoomID;

    public static uint Bet; //������

    private static ERole _role; //���� ������
    public static ERole role
    {
        set
        {
            Debug.Log("set role of main: " + value.ToString());
            _role = value;
            roleChanged?.Invoke(value);
        }
        get { return _role; }
    }
}
