using System.Security.Cryptography;

class NoteService
{
    private NoteRepository repository;
    private User? currentUser;
    private const int PasswordIterations = 600000;

    public NoteService(NoteRepository repository)
    {
        this.repository = repository;
    }

    public string? CurrentUsername
    {
        get
        {
            if (currentUser == null)
            {
                return null;
            }
            return currentUser.Username;
        }
    }

    public string Register(string? username, string? password)
    {
        if (currentUser != null)
        {
            return "Log out before registering another user.";
        }
        if (string.IsNullOrWhiteSpace(username))
        {
            return "Username cannot be empty.";
        }
        if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
        {
            return "Password must contain at least 8 characters.";
        }

        username = username.Trim();
        string normalizedUsername = username.ToUpperInvariant();
        if (repository.FindUser(normalizedUsername) != null)
        {
            return "That username is already taken.";
        }

        byte[] salt = RandomNumberGenerator.GetBytes(16);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, PasswordIterations, HashAlgorithmName.SHA256, 32);
        User user = new User();
        user.Username = username;
        user.NormalizedUsername = normalizedUsername;
        user.PasswordSalt = Convert.ToBase64String(salt);
        user.PasswordHash = Convert.ToBase64String(hash);
        repository.AddUser(user);
        return "Registration successful. Please log in.";
    }

    public string Login(string? username, string? password)
    {
        if (currentUser != null)
        {
            return "You are already logged in.";
        }
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            return "Enter your username and password.";
        }

        User? user = repository.FindUser(username.Trim().ToUpperInvariant());
        if (user != null)
        {
            byte[] salt;
            byte[] savedHash;
            try
            {
                salt = Convert.FromBase64String(user.PasswordSalt);
                savedHash = Convert.FromBase64String(user.PasswordHash);
            }
            catch (FormatException error)
            {
                throw new InvalidDataException("The stored password data is invalid.", error);
            }

            byte[] enteredHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, PasswordIterations, HashAlgorithmName.SHA256, 32);
            if (CryptographicOperations.FixedTimeEquals(enteredHash, savedHash))
            {
                currentUser = user;
                return "Login successful.";
            }
        }
        return "Incorrect username or password.";
    }

    public void Logout()
    {
        currentUser = null;
    }

    public List<Note> GetMyNotes()
    {
        return repository.GetNotes(GetCurrentUser().Id);
    }

    public List<Note> FindNotes(string searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
        {
            throw new ArgumentException("Enter text to search for.");
        }

        List<Note> matches = new List<Note>();
        foreach (Note note in GetMyNotes())
        {
            if (note.Title.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                note.Content.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                matches.Add(note);
            }
        }
        return matches;
    }

    public string AddNote(string? title, string? content)
    {
        User owner = GetCurrentUser();
        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(content))
        {
            return "A note needs a title and content.";
        }

        Note note = new Note();
        note.OwnerId = owner.Id;
        note.Title = title;
        note.Content = content;
        repository.AddNote(note);
        return "Note added and saved.";
    }

    public string EditNote(int noteId, string? title, string? content)
    {
        User owner = GetCurrentUser();
        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(content))
        {
            return "A note needs a title and content.";
        }

        Note? note = repository.FindNote(owner.Id, noteId);
        if (note == null)
        {
            return "Note not found.";
        }
        note.Title = title;
        note.Content = content;
        repository.SaveChanges();
        return "Note updated and saved.";
    }

    public string DeleteNote(int noteId)
    {
        User owner = GetCurrentUser();
        Note? note = repository.FindNote(owner.Id, noteId);
        if (note == null)
        {
            return "Note not found.";
        }
        repository.DeleteNote(note);
        return "Note deleted and saved.";
    }

    private User GetCurrentUser()
    {
        if (currentUser == null)
        {
            throw new InvalidOperationException("You must log in first.");
        }
        return currentUser;
    }
}
