using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Элементы интерфейса внутри игры (комнаты)
public class GameUIs : BaseScreen
{
    //указатели на соответствующую игре комнату
    public RoomRow _roomRow;
    public Room _room;

    //Внутриигровые таймеры (тоже для отображения)
    public float timer;
    public float timeToEnd;

    //Элементы интерфейса (то, зачем вообще этот класс нужен)
    #region UI_room_elements
    [Header("Fold timer")]
    public Image BeatTimerLine;
    public TMP_Text BeatTimerText;

    [Header("Grab Timer")]
    public Image TakeTimerLine;
    public TMP_Text TakeTimerText;

    [Header("Pass Timer")]
    public Image PassTimerLine;
    public TMP_Text PassTimerText;

    [Header("Buttons")]
    public GameObject PassButton;
    public GameObject GrabButton;
    public GameObject FoldButton;

    [Header("role")]
    public GameObject roleObj;
    public TMP_Text roleText;

    [Header("chat")]
    public GameObject chatObject;
    #endregion

    //События, связанные с тем, что игрок профукал время
    public delegate void loseTime();
    public static event loseTime Lose;

    //Стили и текстуры для отображения фона, стола и т.д.
    #region styles

    [Space, Space, Header("styles")]

    [Space, Header("sprites")]
    public Image background_screen_obj;
    public Image table_screen_obj;

    [Space, Space, Header("base")]
    public Texture2D bace_backGround_image;
    public Texture2D bace_table_image;
    public Texture2D bace_back_card_image;
    public Texture2D bace_coloda_image;

    [Space, Header("russisn")]
    public Texture2D russisn_backGround_image;
    public Texture2D russisn_table_image;
    public Texture2D russisn_back_card_image;
    public Texture2D russisn_coloda_image;

    [Space, Header("natureMiddleLine")]
    public Texture2D natureMiddleLine_backGround_image;
    public Texture2D natureMiddleLine_table_image;
    public Texture2D natureMiddleLine_back_card_image;
    public Texture2D natureMiddleLine_coloda_image;

    [Space, Header("fallout")]
    public Texture2D fallout_backGround_image;
    public Texture2D fallout_table_image;
    public Texture2D fallout_back_card_image;
    public Texture2D fallout_coloda_image;

    [Space, Header("natureTropicks")]
    public Texture2D natureTropicks_backGround_image;
    public Texture2D natureTropicks_table_image;
    public Texture2D natureTropicks_back_card_image;
    public Texture2D natureTropicks_coloda_image;

    [Space, Header("herouse")]
    public Texture2D herouse_backGround_image;
    public Texture2D herouse_table_image;
    public Texture2D herouse_back_card_image;
    public Texture2D herouse_coloda_image;

    [Space, Header("cars")]
    public Texture2D cars_backGround_image;
    public Texture2D cars_table_image;
    public Texture2D cars_back_card_image;
    public Texture2D cars_coloda_image;

    [Space, Header("horror")]
    public Texture2D horror_backGround_image;
    public Texture2D horror_table_image;
    public Texture2D horror_back_card_image;
    public Texture2D horror_coloda_image;

    [Space, Header("erotick")]
    public Texture2D erotick_backGround_image;
    public Texture2D erotick_table_image;
    public Texture2D erotick_back_card_image;
    public Texture2D erotick_coloda_image;

    private Texture2D backGround_image;
    private Texture2D table_image;
    public Texture2D back_card_image;
    public Texture2D coloda_image;
    #endregion

    //Инициализация всех текстур в зависимости от стиля + элементов интерфейса
    private void Start()
    {
        _roomRow = GetComponent<RoomRow>();
        _room = GetComponent<Room>();

        Session.roleChanged += onRoleChange;

        string style = PlayerPrefs.GetString("Style");
        //Получаем стиль и загружаем нужные текстуры. ЕСЛИ ВЫЛЕТАЕТ НА ЭТОМ МОМЕНТЕ, значит, что=то неверно указано тут или в файлах игры проблемки
        switch (style)
        {
            case "Russian":
                backGround_image = russisn_backGround_image;
                table_image = russisn_table_image;
                back_card_image = russisn_back_card_image;
                coloda_image = russisn_coloda_image;
                break;

            case "nature_middleLine":
                backGround_image = natureMiddleLine_backGround_image;
                table_image = natureMiddleLine_table_image;
                back_card_image = natureMiddleLine_back_card_image;
                coloda_image = natureMiddleLine_coloda_image;
                break;

            case "Fallout":
                backGround_image = fallout_backGround_image;
                table_image = fallout_table_image;
                back_card_image = fallout_back_card_image;
                coloda_image = fallout_coloda_image;
                break;

            case "nature_tropicks":
                backGround_image = natureTropicks_backGround_image;
                table_image = natureTropicks_table_image;
                back_card_image = natureTropicks_back_card_image;
                coloda_image = natureTropicks_coloda_image;
                break;

            case "herouse":
                backGround_image = herouse_backGround_image;
                table_image = herouse_table_image;
                back_card_image = herouse_back_card_image;
                coloda_image = herouse_coloda_image;
                break;

            case "cars":
                backGround_image = cars_backGround_image;
                table_image = cars_table_image;
                back_card_image = cars_back_card_image;
                coloda_image = cars_coloda_image;
                break;

            case "horror":
                backGround_image = horror_backGround_image;
                table_image = horror_table_image;
                back_card_image = horror_back_card_image;
                coloda_image = horror_coloda_image;
                break;

            case "erotick":
                backGround_image = erotick_backGround_image;
                table_image = erotick_table_image;
                back_card_image = erotick_back_card_image; 
                coloda_image = erotick_coloda_image;
                break;

            default:
                backGround_image = bace_backGround_image;
                table_image = bace_table_image;
                back_card_image = bace_back_card_image;
                coloda_image = bace_coloda_image;
                break;
        }


        Sprite background_sprite = Sprite.Create(backGround_image, new Rect(0, 0, backGround_image.width, backGround_image.height), Vector2.zero);
        background_screen_obj.sprite = background_sprite;
        //Установка спрайтов для фона с соответствующей стилю текстурой
        Sprite table_sprite = Sprite.Create(table_image, new Rect(0, 0, table_image.width, table_image.height), Vector2.zero);
        table_screen_obj.sprite = table_sprite;
    }

    private void OnDestroy()
    {
        Session.roleChanged -= onRoleChange; //Не забываем, комната не вечна, а этот объект пропадает вместе с комнатой
    }

    //Появление кнопок, соответствующих ходу (беру, пас и т.п.)
    #region Buttons_hide_show
    public void hideGrabButton()
    {
        GrabButton.SetActive(false);
        timer = 0;
    }
    public void showGrabButton()
    {
        hidePassButton();
        hideFoldButton();

        GrabButton.SetActive(true);
        timer = timeToEnd;
    }

    public void hidePassButton()
    {
        PassButton.SetActive(false);
        timer = 0;
    }
    public void showPassButton()
    {
        hideGrabButton();
        hideFoldButton();

        PassButton.SetActive(true);
        timer = timeToEnd;
    }

    public void hideFoldButton()
    {
        FoldButton.SetActive(false);
        timer = 0;
    }
    public void showFoldButton()
    {
        hidePassButton();
        hideGrabButton();

        FoldButton.SetActive(true);
        timer = timeToEnd;
    }
    #endregion

    //Обновление таймера (чтобы игроки знали, сколько времени даётся на ход)
    private void FixedUpdate()
    {
        timer += Time.deltaTime;

        if (timer < 0)
        {
            hide_all_buttons();

            if (Session.role == ERole.main)
            {
                _room.Grab();
            }
            else
            {
                _room.Fold();
            }
        }
        else
        {

        }
    }

    //Когда роль изменилась (после завершения хода) пишем игроку, что ему нужно делать
    private void onRoleChange(ERole role)
    {
        roleObj.SetActive(true);

        string message = "";

        switch (role)
        {
            case ERole.main:
                message = "You are need to struggle.";
                break;
            case ERole.thrower:
                message = "you need to throw";
                break;
            case ERole.firstThrower:
                message = "You beginning, throw!";
                break;
        }

        roleText.text = message;
    }

    //Показываем/скрываем окошко с чатом
    public void chatButton()
    {
        chatObject.SetActive(!chatObject.activeSelf);
    }

    //Прячем все кнопки
    public void hide_all_buttons()
    {
        hideGrabButton();
        hideFoldButton();
        hidePassButton();
    }
}
