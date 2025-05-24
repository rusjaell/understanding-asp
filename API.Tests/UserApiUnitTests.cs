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
    public void UpdateUser()
    {
        var originalUser = _userService.Create(new User(0, "beforeupdate", "before@example.com"));
        Assert.NotNull(originalUser);

        var updatedUser = new User(originalUser.Id, "afterupdate", "after@example.com");
        var success = _userService.Update(originalUser.Id, updatedUser);

        Assert.True(success);

        var fetched = _userService.GetById(originalUser.Id);
        Assert.NotNull(fetched);
        Assert.Equal("afterupdate", fetched.Username);
        Assert.Equal("after@example.com", fetched.Email);
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
