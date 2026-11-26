using System.Security.Cryptography;

class NoteService
{
    private JsonRepository repository;
    private AppData data;
    private User? currentUser;
    private const int PasswordIterations = 600000;

    public NoteService(JsonRepository repository)
    {
        this.repository = repository;
        data = repository.Load();
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
        int nextId = 1;
        foreach (User user in data.Users)
        {
            if (string.Equals(user.Username, username, StringComparison.OrdinalIgnoreCase))
            {
                return "That username is already taken.";
            }
            if (user.Id >= nextId)
            {
                nextId = user.Id + 1;
            }
        }

        byte[] salt = RandomNumberGenerator.GetBytes(16);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, PasswordIterations, HashAlgorithmName.SHA256, 32);
        User newUser = new User(nextId, username, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        data.Users.Add(newUser);
        repository.Save(data);
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

        foreach (User user in data.Users)
        {
            if (string.Equals(user.Username, username.Trim(), StringComparison.OrdinalIgnoreCase))
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
        }

        return "Incorrect username or password.";
    }

    public void Logout()
    {
        currentUser = null;
    }

    public List<Note> GetMyNotes()
    {
        User owner = GetCurrentUser();
        List<Note> myNotes = new List<Note>();
        foreach (Note note in data.Notes)
        {
            if (note.OwnerId == owner.Id)
            {
                myNotes.Add(note);
            }
        }
        return myNotes;
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

        int nextId = 1;
        foreach (Note note in data.Notes)
        {
            if (note.Id >= nextId)
            {
                nextId = note.Id + 1;
            }
        }

        data.Notes.Add(new Note(nextId, owner.Id, title, content));
        repository.Save(data);
        return "Note added and saved.";
    }

    public string EditNote(int noteId, string? title, string? content)
    {
        User owner = GetCurrentUser();
        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(content))
        {
            return "A note needs a title and content.";
        }

        foreach (Note note in data.Notes)
        {
            if (note.Id == noteId && note.OwnerId == owner.Id)
            {
                note.Update(title, content);
                repository.Save(data);
                return "Note updated and saved.";
            }
        }
        return "Note not found.";
    }

    public string DeleteNote(int noteId)
    {
        User owner = GetCurrentUser();
        foreach (Note note in data.Notes)
        {
            if (note.Id == noteId && note.OwnerId == owner.Id)
            {
                data.Notes.Remove(note);
                repository.Save(data);
                return "Note deleted and saved.";
            }
        }
        return "Note not found.";
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
