using API.Services;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Adds my custom service
builder.Services.AddSingleton<IUserService, MockUserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Use((context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("Request from {IP} to {Path}", context.Connection.RemoteIpAddress?.ToString() ?? "Unknown", context.Request.Path);

    return next.Invoke();
});

app.MapGet("/users", (IUserService userService) => userService.GetAll()).WithName("GetAllUsers");

app.MapGet("/users/{id:int}", (int id, IUserService userService) =>
{
    var user = userService.GetById(id);
    if (user != null)
        return Results.Ok(user);
    return Results.NotFound();
}).WithName("GetUserById");

app.MapPost("/users", (User user, IUserService userService) =>
{
    if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Email))
        return Results.BadRequest("Username and Email are required.");

    var createdUser = userService.Create(user);
    if (createdUser == null)
        return Results.Problem("User creation failed.");

    return Results.Created($"/users/{createdUser.Id}", createdUser);
}).WithName("CreateUser");

app.MapPut("/users/{id:int}", (int id, User user, IUserService userService) =>
{
    var successful = userService.Update(id, user);
    if (successful)
        return Results.Ok(successful);
    
    return Results.NotFound();
}).WithName("UpdateUser");

app.MapDelete("/users/{id:int}", (int id, IUserService userService) =>
{
    var deleted = userService.Delete(id);
    if (deleted)
        return Results.NoContent();
    
    return Results.NotFound();
}).WithName("DeleteUser");

app.Run();
