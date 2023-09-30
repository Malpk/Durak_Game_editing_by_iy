//Перевод карты в json для работы с сокетом
namespace JSON_card
{
    //Толко для работы с JSON файлами!
    public class _card
    {
        public uint UId;
        public Card card;
    }

    //Толко для работы с JSON файлами!
    public class Card
    {
        public string suit;
        public string nominal;
    }
}