using JSON_card;
using System.Collections;
using UnityEngine;

//Карта в игре, содержит все указатели на GameObject, сама им и является
public class GameCard : MonoBehaviour
{
    public Table _table; //pointer to current game table

    private bool isDragging = false;
    private Vector3 offset;

    public bool isDraggble = true;

    public CardMath math; //характеристики игровой карты (масть, достоинство)

    public GameCard nowChoosedCard = null; //????

    //Для обработки того, как игрок походил картой (что чем отбил)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "tableBeatingCard")
        {
            Debug.Log("GameCard: trigger enter + collide");

            GameCard beatingCard = collision.gameObject.GetComponent<GameCard>();

            if (beatingCard != null) nowChoosedCard = beatingCard;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "tableBeatingCard")
        {
            Debug.Log("GameCard: trigger exit 2D (beating card)");

            GameCard beatingCard = collision.gameObject.GetComponent<GameCard>();

            if (beatingCard != null)
            {
                if(beatingCard == nowChoosedCard)
                {
                    nowChoosedCard = null;
                }
            }
        }
    }

    public void Start()
    {
        Debug.Log("GameCard: Start();");
        _table = GameObject.FindGameObjectWithTag("Room").GetComponent<Table>();
    }

    //Инициализация из JSON
    public void Init(Card card)
    {
        Debug.Log("GameCard: Init()");
        math = new CardMath(card.suit, card.nominal);
    }
    //Инициализация по масти и номиналу
    public void Init(string suit, string nominal)
    {
        Debug.Log("GameCard: Init()");
        math = new CardMath(suit, nominal);
    }

    #region Mouse Events
    private void OnMouseDown()
    {
        if (isDraggble)
        {
            offset = transform.position - GetMouseWorldPosition();
            isDragging = true;
        }
    }
    private void OnMouseDrag()
    {
        if (isDraggble)
        {
            if (isDragging)
            {
                transform.position = GetMouseWorldPosition() + offset;
            }
        }
    }

    private void OnMouseUp()
    {
        if (isDraggble)
        {
            isDragging = false;

            if (Session.role == ERole.main)
            {
                Debug.Log("BeatCard");
                if (nowChoosedCard != null) _table.BeatCard(this, nowChoosedCard);
            }

            else if (gameObject.transform.position.y >= 1)
            {
                _table.ThrowCard(this);
            }

            GameObject.Find("Hands").GetComponent<CardController>().SetAllCardsPos();
        }
    }
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -Camera.main.transform.position.z;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }
    #endregion

    //Физическое передвижение GameObject
    public IEnumerator MoveTo(Vector3 MoveToPoint, Vector3 rotate, Vector3 scale, float Time = 1)
    {
        Debug.Log("GameCard: MoveTo (Time)");
        yield return moveTo(MoveToPoint, rotate, scale, Time);
    }
    private bool moveTo(Vector3 MoveToPoint, Vector3 rotate, Vector3 scale, float Time)
    {
        Debug.Log("GameCard: MoveTo (bigger )");

        LeanTween.moveLocal(gameObject, MoveToPoint, Time);
        LeanTween.rotate(gameObject, rotate, Time);
        LeanTween.scale(gameObject, scale, Time);

        return true;
    }

}