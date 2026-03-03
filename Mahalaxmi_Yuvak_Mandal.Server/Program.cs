using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();

// ✅ CORS Configuration for Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.WithOrigins("https://localhost:51281") // Angular default
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// HTTPS redirection
app.UseHttpsRedirection();

// ✅ Use CORS (MUST be before MapControllers)
app.UseCors("AllowAll");

// Swagger (Development only)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

// Map controllers
app.MapControllers();

app.Run();