using Microsoft.AspNetCore.Mvc;
using LibraryTelegramBot_новий.Services;
using LibraryTelegramBot_новий.Models;
using LibraryTelegramBot_новий.Storage;

namespace LibraryTelegramBot_новий.Controllers
{
    [ApiController]
    [Route("api/[controller]")] //адреса api/books
    public class BooksController : ControllerBase //базовий клас який дає методи типу Ok(), NotFound(), BadRequest()
    {
        private readonly GoogleBooksService _googleService;
        private readonly LibraryStorageService _storageService;

        public BooksController(GoogleBooksService googleService, LibraryStorageService storageService)
        {
            _googleService = googleService;
            _storageService = storageService;
        }

        //Пошук по публічному API
        [HttpGet("search")]
        public async Task<IActionResult> Search(string query)
        {
            var result = await _googleService.SearchBooksAsync(query); // просто перенаправляєм запид до сервісу гугла і віддаєм те ш овін нам найшов
            return Ok(result);
        }

        //Отримати всі збережені книги
        [HttpGet("library")]
        public async Task<IActionResult> GetSavedBooks()
        {
            var books = await _storageService.GetAllBooksAsync(); 
            return Ok(books);
        }

        //Отримати конкретну збережену книгу за ID
        [HttpGet("library/{id}")]
        public async Task<IActionResult> GetSavedBookById(string id)
        {
            var books = await _storageService.GetAllBooksAsync();
            var book = books.FirstOrDefault(b => b.Id == id); // перевіряєм чи для книги b або її ід лорівнює тому ід який написав користувач

            if (book == null)
            {
                return NotFound($"Книгу з ID {id} не знайдено"); //404
            }

            return Ok(book);
        }

        //Зберегти нову книгу в бібліотеку
        [HttpPost("library")]
        public async Task<IActionResult> SaveBookToLibrary([FromBody] UserBook? newBook) // ? - може бути нулл 
        {
            if (newBook == null || string.IsNullOrWhiteSpace(newBook.Title)) // перевірка на нулл, чи порожні я чи не складаєтсья тільки з пробілів. || - якшо хотяб одна умова
            {
                return BadRequest("Неправильні дані книги");//400
            }

            var books = await _storageService.GetAllBooksAsync();
            books.Add(newBook);
            await _storageService.SaveAllBooksAsync(books);

            return CreatedAtAction(nameof(GetSavedBookById), new { id = newBook.Id }, newBook); //201 Created
        }

        //Оновити інформацію про збережену книгу
        [HttpPut("library/{id}")]
        public async Task<IActionResult> UpdateBookStatus(string id, [FromBody] UserBook updatedBook)
        {
            var books = await _storageService.GetAllBooksAsync();
            var existingBook = books.FirstOrDefault(b => b.Id == id);

            if (existingBook == null)
            {
                return NotFound($"Книгу з ID {id} не знайдено");
            }

            existingBook.Title = updatedBook.Title;
            existingBook.Authors = updatedBook.Authors;
            existingBook.ImageUrl = updatedBook.ImageUrl;
            existingBook.Status = updatedBook.Status;

            await _storageService.SaveAllBooksAsync(books);

            return NoContent();//204 нема шо відправляти але все гуд
        }

        //Видалити книгу з бібліотеки
        [HttpDelete("library/{id}")]
        public async Task<IActionResult> DeleteBookFromLibrary(string id)
        {
            var books = await _storageService.GetAllBooksAsync();
            var bookToRemove = books.FirstOrDefault(b => b.Id == id);

            if (bookToRemove == null)
            {
                return NotFound($"Книгу з ID {id} не знайдено");
            }

            books.Remove(bookToRemove);
            await _storageService.SaveAllBooksAsync(books);

            return NoContent();
        }
    }
}