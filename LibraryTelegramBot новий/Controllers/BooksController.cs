using Microsoft.AspNetCore.Mvc;
using LibraryTelegramBot_новий.Services;

namespace LibraryTelegramBot_новий.Controllers
{
    [ApiController]// атрибут
    [Route("api/[controller]")] //базову адресу api/books
    public class BooksController : ControllerBase //ControllerBase базовий класн , методи Ok(), можливість читати URL-адреси
    {
        private readonly GoogleBooksService _googleService;

        public BooksController(GoogleBooksService googleService)
        {
            _googleService = googleService;
        }

        [HttpGet("search")] // Повна адреса: api/books/search
        public async Task<IActionResult> Search(string query)
        {
            var result = await _googleService.SearchBooksAsync(query);
            return Ok(result);
        }
    }
}