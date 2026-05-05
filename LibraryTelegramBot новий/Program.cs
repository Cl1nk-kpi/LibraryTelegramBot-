var builder = WebApplication.CreateBuilder(args);

// Реєструємо клієнт для Google API
builder.Services.AddHttpClient<LibraryTelegramBot_новий.Services.GoogleBooksService>();

// Реєструємо наш новий сервіс для роботи з JSON файлом
builder.Services.AddScoped<LibraryTelegramBot_новий.Storage.LibraryStorageService>();

builder.Services.AddControllers(); //дає нашому додатку розуміти і обробляти апі контроллери. без цього він не знатиме шо робити з запитами на /api/books
builder.Services.AddEndpointsApiExplorer(); //створення сторінки Swagger
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();  //зв'язує конкретні URL-адреси з конкретними методами в коді

app.Run();