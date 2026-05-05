using System.Text.Json;
using LibraryTelegramBot_новий.Models;

namespace LibraryTelegramBot_новий.Services
{
    public class GoogleBooksService
    {
        private readonly HttpClient _httpClient;

        private readonly string _apiKey = "Апішка гугла";

        public GoogleBooksService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<BookInfo>> SearchBooksAsync(string query)
        {
            var url = $"https://www.googleapis.com/books/v1/volumes?q={Uri.EscapeDataString(query)}&key={_apiKey}";    //метод перетворює пробіли та спецсимволи у зрозумілий для браузерів формат 
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode) return new List<BookInfo>(); // якщо код не 200 то ми повертаєм пустий рядок

            var content = await response.Content.ReadAsStringAsync(); //витягує сирий текст відповіді (JSON) з HTTP-пакета
            using var doc = JsonDocument.Parse(content);

            if (!doc.RootElement.TryGetProperty("items", out var items))
                return new List<BookInfo>();

            var books = new List<BookInfo>();
            foreach (var item in items.EnumerateArray())
            {
                var info = item.GetProperty("volumeInfo"); //У структурі Google API майже вся корисна інформація (назва, автор) лежить всередині  об'єкта volumeInfo
                books.Add(new BookInfo
                {
                    Title = info.TryGetProperty("title", out var t) ? t.GetString() : "Без назви",
                    Authors = info.TryGetProperty("authors", out var a) ? string.Join(", ", a.EnumerateArray()) : "Невідомо",
                    Description = info.TryGetProperty("description", out var d) ? d.GetString() : "Опису немає",
                    PreviewLink = info.TryGetProperty("infoLink", out var l) ? l.GetString() : "",
                    ImageUrl = info.TryGetProperty("imageLinks", out var imgs) && imgs.TryGetProperty("thumbnail", out var thumb) ? thumb.GetString() : "" // якщо нема хочаб одного то виводим пустий рядок
                });
            }
            return books;
        }
    }
}
