namespace API.Services;

public record User(int Id, string Username, string Email);

public interface IUserService
{
    List<User> GetAll();
    User? GetById(int id);
    User? Create(User user);
    bool Update(int id, User user);
    bool Delete(int id);
}

public sealed class MockUserService : IUserService
{
    private int _nextId = 1;

    // simulates a database
    private readonly List<User> _users = new List<User>();

    public List<User> GetAll() => _users;

    public User? GetById(int id) => _users.FirstOrDefault(user => user.Id == id);

    public User? Create(User user)
    {
        if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Email))
            return null;

        var nextId = _nextId++;

        var newUser = user with { Id = nextId };
        _users.Add(newUser);
        return newUser;
    }

    public bool Update(int id, User modifiedUser)
    {
        var index = _users.FindIndex(user => user.Id == id);
        if (index == -1)
            return false;

        var updated = modifiedUser with { Id = id };
        _users[index] = updated;
        return true;
    }

    public bool Delete(int id)
    {
        var user = GetById(id);
        if(user == null)
            return false;
        return _users.Remove(user);
    }
}