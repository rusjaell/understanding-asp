using API.Services;

namespace API.Tests;

public class UserApiUnitTests
{
    private readonly IUserService _userService = new MockUserService();

    [Fact]
    public void GetAllUsers()
    {
        _userService.Create(new User(0, "seeduser", "seed@example.com"));

        var result = _userService.GetAll();

        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void GetUserById()
    {
        var createdUser = _userService.Create(new User(0, "testuser", "test@example.com"));
        Assert.NotNull(createdUser);

        var user = _userService.GetById(createdUser.Id);

        Assert.NotNull(user);
        Assert.Equal(createdUser.Id, user.Id);
    }

    [Fact]
    public void CreateUser()
    {
        var newUser = new User(0, "newuser", "newuser@example.com");
        var created = _userService.Create(newUser);

        Assert.NotNull(created);
        Assert.True(created.Id > 0);
        Assert.Equal(newUser.Username, created.Username);
    }

    [Fact]
    public void DeleteUser()
    {
        var createdUser = _userService.Create(new User(0, "todelete", "delete@example.com"));
        Assert.NotNull(createdUser);

        var deleted = _userService.Delete(createdUser.Id);

        Assert.True(deleted);
    }
}
