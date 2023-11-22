using JSON_card;
using System.Collections;
using UnityEngine;

//Карта в игре, содержит все указатели на GameObject, сама им и является
public class GameCard : MonoBehaviour
{
    public bool isDraggble = true;
    [SerializeField] private float _smoothTime;
    private Vector3 offset;
    [Header("Reference")]
    public Table _table; //pointer to current game table
    public CardMath math; //характеристики игровой карты (масть, достоинство)
    public GameCard nowChoosedCard = null; //????

    [SerializeField] private SpriteRenderer _body;

    private int _sortLayer;
    private bool _isDragging = false;
    private Coroutine _corotune;

    public Sprite Sprite => _body.sprite;

    //Для обработки того, как игрок походил картой (что чем отбил)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "tableBeatingCard")
        {
            Debug.Log("GameCard: trigger enter + collide");
            GameCard beatingCard = collision.gameObject.GetComponent<GameCard>();
            if (beatingCard != null)
                nowChoosedCard = beatingCard;
        }
        else if (collision.TryGetComponent(out Table table))
        {
            _table = table;
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
                if (beatingCard == nowChoosedCard)
                {
                    nowChoosedCard = null;
                }
            }
        }
    }

    public void Start()
    {
        _sortLayer = _body.sortingOrder;
        Debug.Log("GameCard: Start();");
    }

    //Инициализация из JSON
    public void Init(Card card)
    {
        Debug.Log($"GameCard: Init({card})");
        math = new CardMath(card.suit, card.nominal);
    }

    public void SetLayer(int layer)
    {
        _body.sortingOrder = _sortLayer + layer;
    }

    public void SetSprite(Sprite sprite)
    {
        _body.sprite = sprite;
    }

    public void BindPoint(Transform point)
    {
        StartMoveTo(point);
    }

    public void Init(string suit, string nominal)
    {
        Debug.Log("GameCard: Init()");
        math = new CardMath(suit, nominal);
    }

    public void StartMoveTo(Transform target)
    {
        if (_corotune != null)
            StopCoroutine(_corotune);
        _corotune = StartCoroutine(MoveTo(target));
    }
    public void StartMoveTo(Vector3 MoveToPoint, Vector3 rotate, Vector3 scale, float Time = 1)
    {
        if (_corotune != null)
            StopCoroutine(_corotune);
        _corotune = StartCoroutine(MoveTo(MoveToPoint, rotate, scale, Time));
    }

    //Инициализация по масти и номиналу

    #region Mouse Events
    private void OnMouseDown()
    {
        if (isDraggble)
        {
            offset = transform.position - GetMouseWorldPosition();
            _isDragging = true;
        }
    }
    private void OnMouseDrag()
    {
        if (isDraggble)
        {
            if (_isDragging)
            {
                transform.position = GetMouseWorldPosition() + offset;
            }
        }
    }

    private void OnMouseUp()
    {
        if (isDraggble)
        {
            _isDragging = false;
            if (Session.role == ERole.main)
            {
                Debug.Log("BeatCard");
                if (nowChoosedCard != null)
                    _table.BeatCard(this, nowChoosedCard);
            }
            else if (_table)
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
    private IEnumerator MoveTo(Vector3 MoveToPoint, Vector3 rotate, Vector3 scale, float Time = 1)
    {
        Debug.Log("GameCard: MoveTo (Time)");
        yield return moveTo(MoveToPoint, rotate, scale, Time);
    }

    private IEnumerator MoveTo(Transform target)
    {
        var velocity = Vector3.zero;
        while (enabled && target)
        {
            transform.position = Vector3.SmoothDamp(transform.position,
                target.position, ref velocity, _smoothTime);
            yield return null;
        }
        _corotune = null;
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