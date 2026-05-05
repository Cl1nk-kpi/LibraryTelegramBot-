using System.Text.Json;
using LibraryTelegramBot_новий.Models;

namespace LibraryTelegramBot_новий.Storage
{
    public class LibraryStorageService
    {
        private readonly string _filePath = "books_library.json";

        public async Task<List<UserBook>> GetAllBooksAsync()
        {
            if (!File.Exists(_filePath))
            {
                return new List<UserBook>();
            }

            var json = await File.ReadAllTextAsync(_filePath); //await ставить на паузу поточний метод і звільняє потік для інших запитів, коли він допрацює то продовжить з цього місця
            return JsonSerializer.Deserialize<List<UserBook>>(json) ?? new List<UserBook>(); // ?? працює як перевірка на нулл , якщо він там є т овін створює порожній список
        }

        public async Task SaveAllBooksAsync(List<UserBook> books)
        {
            var json = JsonSerializer.Serialize(books, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_filePath, json);
        }
    }
}