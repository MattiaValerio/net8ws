using Microsoft.AspNetCore.Http.HttpResults;
using wws.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ChatService>();

//builder.WebHost.UseUrls("http://*:1434");

var app = builder.Build();
app.UseWebSockets();

// Set the cors for all origins
app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
);

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


app.UseCors();


app.MapGet("/", async (HttpContext context, ChatService chatService) =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        await chatService.HandleWebSocketConnection(webSocket);
    }
    else
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync("Expected a WebSocket request");
    }
});

app.MapGet("/test", () =>
{
    return "HELLO"; 
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
