class User
{
    public int Id { get; private set; }
    public string Username { get; private set; }
    public string PasswordSalt { get; private set; }
    public string PasswordHash { get; private set; }

    public User(int id, string username, string passwordSalt, string passwordHash)
    {
        Id = id;
        Username = username;
        PasswordSalt = passwordSalt;
        PasswordHash = passwordHash;
    }
}
