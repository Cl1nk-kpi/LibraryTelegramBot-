namespace LibraryTelegramBot_новий.Models
{
    public class UserBook
    {
        public string Id { get; set; } = Guid.NewGuid().ToString(); // генерує унікальний ID

        public string Title { get; set; } = "";
        public string Authors { get; set; } = "";
        public string ImageUrl { get; set; } = "";

        // Статус за замовчуванням
        public string Status { get; set; } = "⏳ В планах";
    }
}
//Описує як виглядає книга яку користувач зберігає