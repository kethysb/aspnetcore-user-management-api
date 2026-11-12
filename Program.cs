using UserManagementAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Ordered custom middleware pipeline (guided & debugged with Microsoft Copilot)
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<TokenAuthenticationMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
