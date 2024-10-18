using Microsoft.EntityFrameworkCore;
using StreamingAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Adicionando o DbContext com a string de conexão
builder.Services.AddDbContext<StreamingDbContext>(options =>
    options.UseSqlite("Data Source=Data/streaming.db"));
    
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuração padrão do Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
