namespace LibraryTelegramBot_новий.Models
{
    public class UserBook
    {
        // Унікальний ID щоб потім розуміти яку саме книгу видаляти
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Title { get; set; } = "";
        public string Authors { get; set; } = "";
        public string ImageUrl { get; set; } = "";

        // Статус за замовчуванням
        public string Status { get; set; } = "⏳ В планах";
    }
}