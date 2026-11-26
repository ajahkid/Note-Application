class ConsoleMenu
{
    private NoteService service;

    public ConsoleMenu(NoteService service)
    {
        this.service = service;
    }

    public void Run()
    {
        bool isRunning = true;
        while (isRunning)
        {
            Console.WriteLine();
            if (service.CurrentUsername == null)
            {
                Console.WriteLine("=== Note Application ===");
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Log in");
                Console.WriteLine("0. Exit");
                Console.Write("Choose an option: ");
                string? choice = Console.ReadLine();

                if (choice == null || choice == "0")
                {
                    isRunning = false;
                }
                else if (choice == "1")
                {
                    Register();
                }
                else if (choice == "2")
                {
                    Login();
                }
                else
                {
                    Console.WriteLine("Invalid option. Please choose 0 to 2.");
                }
            }
            else
            {
                Console.WriteLine("=== Notes for " + service.CurrentUsername + " ===");
                Console.WriteLine("1. Add a note");
                Console.WriteLine("2. List my notes");
                Console.WriteLine("3. Find my notes");
                Console.WriteLine("4. Delete a note");
                Console.WriteLine("5. Edit a note");
                Console.WriteLine("6. Log out");
                Console.WriteLine("0. Exit");
                Console.Write("Choose an option: ");
                string? choice = Console.ReadLine();

                if (choice == null || choice == "0")
                {
                    isRunning = false;
                }
                else if (choice == "1")
                {
                    AddNote();
                }
                else if (choice == "2")
                {
                    ShowNotes(service.GetMyNotes());
                }
                else if (choice == "3")
                {
                    FindNotes();
                }
                else if (choice == "4")
                {
                    DeleteNote();
                }
                else if (choice == "5")
                {
                    EditNote();
                }
                else if (choice == "6")
                {
                    service.Logout();
                    Console.WriteLine("Logged out.");
                }
                else
                {
                    Console.WriteLine("Invalid option. Please choose 0 to 6.");
                }
            }
        }
        Console.WriteLine("Goodbye!");
    }

    private void Register()
    {
        Console.Write("Choose a username: ");
        string? username = Console.ReadLine();
        Console.Write("Choose a password (visible as you type): ");
        string? password = Console.ReadLine();
        Console.WriteLine(service.Register(username, password));
    }

    private void Login()
    {
        Console.Write("Username: ");
        string? username = Console.ReadLine();
        Console.Write("Password (visible as you type): ");
        string? password = Console.ReadLine();
        Console.WriteLine(service.Login(username, password));
    }

    private void AddNote()
    {
        Console.Write("Enter the title: ");
        string? title = Console.ReadLine();
        Console.Write("Enter the content: ");
        string? content = Console.ReadLine();
        Console.WriteLine(service.AddNote(title, content));
    }

    private void ShowNotes(List<Note> notes)
    {
        if (notes.Count == 0)
        {
            Console.WriteLine("No notes found.");
            return;
        }

        foreach (Note note in notes)
        {
            Console.WriteLine("ID " + note.Id + ": " + note.Title);
            Console.WriteLine("   " + note.Content);
        }
    }

    private void FindNotes()
    {
        Console.Write("Enter text to search for: ");
        string? searchText = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(searchText))
        {
            Console.WriteLine("Please enter text to search for.");
            return;
        }
        ShowNotes(service.FindNotes(searchText));
    }

    private void DeleteNote()
    {
        ShowNotes(service.GetMyNotes());
        Console.Write("Enter the ID of the note to delete: ");
        string? input = Console.ReadLine();
        int noteId;
        if (int.TryParse(input, out noteId) == false)
        {
            Console.WriteLine("That note ID is not valid.");
            return;
        }
        Console.WriteLine(service.DeleteNote(noteId));
    }

    private void EditNote()
    {
        ShowNotes(service.GetMyNotes());
        Console.Write("Enter the ID of the note to edit: ");
        string? input = Console.ReadLine();
        int noteId;
        if (int.TryParse(input, out noteId) == false)
        {
            Console.WriteLine("That note ID is not valid.");
            return;
        }

        Console.Write("Enter the updated title: ");
        string? title = Console.ReadLine();
        Console.Write("Enter the updated content: ");
        string? content = Console.ReadLine();
        Console.WriteLine(service.EditNote(noteId, title, content));
    }
}
