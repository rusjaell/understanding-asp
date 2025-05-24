using Microsoft.AspNetCore.Mvc.Testing;
using API.Services;
using System.Net;
using System.Net.Http.Json;

namespace API.Tests;

public class UserApiSimpleTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public UserApiSimpleTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllUsers()
    {
        var response = await _client.GetAsync("/users");
        response.EnsureSuccessStatusCode();

        var users = await response.Content.ReadFromJsonAsync<List<User>>();
        Assert.NotNull(users);
        Assert.Empty(users);
    }

    [Fact]
    public async Task CreateUser()
    {
        var newUser = new User(0, "testuser", "test@example.com");

        var response = await _client.PostAsJsonAsync("/users", newUser);
        response.EnsureSuccessStatusCode();

        var createdUser = await response.Content.ReadFromJsonAsync<User>();
        Assert.NotNull(createdUser);
        Assert.True(createdUser.Id > 0);
        Assert.Equal(newUser.Username, createdUser.Username);
    }

    [Fact]
    public async Task GetUserById()
    {
        var newUser = new User(0, "testuser", "test@example.com");
        var createResponse = await _client.PostAsJsonAsync("/users", newUser);
        createResponse.EnsureSuccessStatusCode();

        var createdUser = await createResponse.Content.ReadFromJsonAsync<User>();
        Assert.NotNull(createdUser);

        var getResponse = await _client.GetAsync($"/users/{createdUser.Id}");
        getResponse.EnsureSuccessStatusCode();

        var fetchedUser = await getResponse.Content.ReadFromJsonAsync<User>();
        Assert.NotNull(fetchedUser);
        Assert.Equal(createdUser.Id, fetchedUser.Id);
    }

    [Fact]
    public async Task DeleteUser()
    {
        var newUser = new User(0, "todelete", "delete@example.com");
        var createResponse = await _client.PostAsJsonAsync("/users", newUser);
        createResponse.EnsureSuccessStatusCode();

        var createdUser = await createResponse.Content.ReadFromJsonAsync<User>();
        Assert.NotNull(createdUser);

        var deleteResponse = await _client.DeleteAsync($"/users/{createdUser.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var getResponse = await _client.GetAsync($"/users/{createdUser.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }
}
