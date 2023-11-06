using UnityEngine;

//Изначальный (стартовый) экран. В нём ничего, связанного с программированием, нет.
public class BaseScreen : MonoBehaviour
{
    protected ScreenDirector m_screenDirector;
    protected SocketNetwork m_socketNetwork;

    protected void Awake()
    {
        var screeen = GameObject.FindGameObjectWithTag("ScreenDirector");
        var socket = GameObject.FindGameObjectWithTag("SocketNetwork");
        if (screeen)
            m_screenDirector = screeen.GetComponent<ScreenDirector>();
        if (socket)
            m_socketNetwork = socket.GetComponent<SocketNetwork>();
    }

    public virtual void SetActiveHandler(bool active)
    { }
}
