using JSON_card;
using System.Collections;
using UnityEngine;

// game card
// the script that is responseble for 
// gaming cards on the table

public class GameCard : MonoBehaviour
{
    public Table _table; //pointer to current game table

    private bool isDragging = false;
    private Vector3 offset;

    public bool isDraggble = true;

    public CardMath math; //special values

    public GameCard nowChoosedCard = null; //????

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "tableBeatingCard")
        {
            GameCard beatingCard = collision.gameObject.GetComponent<GameCard>();

            if (beatingCard != null) nowChoosedCard = beatingCard;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "tableBeatingCard")
        {
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
        _table = GameObject.FindGameObjectWithTag("Room").GetComponent<Table>();
    }

    public void Init(Card card)
    {
        math = new CardMath(card.suit, card.nominal);
    }
    public void Init(string suit, string nominal)
    {
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

    public IEnumerator MoveTo(Vector3 MoveToPoint, Vector3 rotate, Vector3 scale, float Time = 1)
    {
        yield return moveTo(MoveToPoint, rotate, scale, Time);
    }
    private bool moveTo(Vector3 MoveToPoint, Vector3 rotate, Vector3 scale, float Time)
    {
        LeanTween.moveLocal(gameObject, MoveToPoint, Time);
        LeanTween.rotate(gameObject, rotate, Time);
        LeanTween.scale(gameObject, scale, Time);

        return true;
    }

}