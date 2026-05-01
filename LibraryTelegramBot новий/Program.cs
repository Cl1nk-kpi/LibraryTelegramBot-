var builder = WebApplication.CreateBuilder(args); 
builder.Services.AddHttpClient<LibraryTelegramBot_новий.Services.GoogleBooksService>();


builder.Services.AddControllers(); //дає нашому додатку розуміти і обробляти апі контроллери. без цього він не знатиме шо робити з запитами на /api/books

builder.Services.AddEndpointsApiExplorer(); //створення сторінки Swagger
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); // безпека. будь-який запит шо прийшов по незахищеному http:// автоматично переадресовуватись на безпечний https://

app.MapControllers();  //зв'язує конкретні URL-адреси з конкретними методами в коді

app.Run();

