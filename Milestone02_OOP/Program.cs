using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

class Program
{
    private static List<Note> notes = new List<Note>();
    private static string fileName = "milestone02-notes.json";

    static void Main(string[] args)
    {
        if (LoadNotes() == false)
        {
            return;
        }

        bool isRunning = true;

        while (isRunning)
        {
            ShowMenu();
            Console.Write("Choose an option: ");
            string? choice = Console.ReadLine();

            if (choice == null)
            {
                Console.WriteLine("Input ended. Goodbye!");
                isRunning = false;
            }
            else if (choice == "1")
            {
                AddNote();
            }
            else if (choice == "2")
            {
                ListNotes();
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
            else if (choice == "0")
            {
                Console.WriteLine("Goodbye!");
                isRunning = false;
            }
            else
            {
                Console.WriteLine("Invalid option. Please choose 0 to 5.");
            }
        }
    }

    static void ShowMenu()
    {
        Console.WriteLine();
        Console.WriteLine("=== Note Application ===");
        Console.WriteLine("1. Add a note");
        Console.WriteLine("2. List notes");
        Console.WriteLine("3. Find notes");
        Console.WriteLine("4. Delete a note");
        Console.WriteLine("5. Edit a note");
        Console.WriteLine("0. Exit");
    }

    static void AddNote()
    {
        Console.Write("Enter the title: ");
        string? title = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(title))
        {
            Console.WriteLine("A title cannot be empty.");
            return;
        }

        Console.Write("Enter the content: ");
        string? content = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(content))
        {
            Console.WriteLine("Note content cannot be empty.");
            return;
        }

        Note note = new Note(title, content);
        notes.Add(note);
        if (SaveNotes())
        {
            Console.WriteLine("Note added and saved.");
        }
    }

    static void ShowNote(int index)
    {
        Console.WriteLine((index + 1) + ". " + notes[index].Title);
        Console.WriteLine("   " + notes[index].Content);
    }

    static void ListNotes()
    {
        if (notes.Count == 0)
        {
            Console.WriteLine("There are no notes yet.");
            return;
        }

        Console.WriteLine("Your notes:");
        for (int index = 0; index < notes.Count; index++)
        {
            ShowNote(index);
        }
    }

    static void FindNotes()
    {
        Console.Write("Enter text to search for: ");
        string? searchText = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(searchText))
        {
            Console.WriteLine("Please enter text to search for.");
            return;
        }

        int matchCount = 0;
        for (int index = 0; index < notes.Count; index++)
        {
            if (notes[index].Title.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                notes[index].Content.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                ShowNote(index);
                matchCount++;
            }
        }

        if (matchCount == 0)
        {
            Console.WriteLine("No matching notes found.");
        }
    }

    static void DeleteNote()
    {
        ListNotes();
        if (notes.Count == 0)
        {
            return;
        }

        Console.Write("Enter the number of the note to delete: ");
        string? input = Console.ReadLine();
        int noteNumber;

        if (int.TryParse(input, out noteNumber) &&
            noteNumber >= 1 && noteNumber <= notes.Count)
        {
            notes.RemoveAt(noteNumber - 1);
            if (SaveNotes())
            {
                Console.WriteLine("Note deleted and saved.");
            }
        }
        else
        {
            Console.WriteLine("That note number is not valid.");
        }
    }

    static void EditNote()
    {
        ListNotes();
        if (notes.Count == 0)
        {
            return;
        }

        Console.Write("Enter the number of the note to edit: ");
        string? input = Console.ReadLine();
        int noteNumber;
        if (int.TryParse(input, out noteNumber) == false ||
            noteNumber < 1 || noteNumber > notes.Count)
        {
            Console.WriteLine("That note number is not valid.");
            return;
        }

        Console.Write("Enter the updated title: ");
        string? title = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(title))
        {
            Console.WriteLine("A title cannot be empty.");
            return;
        }

        Console.Write("Enter the updated content: ");
        string? content = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(content))
        {
            Console.WriteLine("Note content cannot be empty.");
            return;
        }

        notes[noteNumber - 1].Update(title, content);
        if (SaveNotes())
        {
            Console.WriteLine("Note updated and saved.");
        }
    }

    static bool LoadNotes()
    {
        if (!File.Exists(fileName))
        {
            return true;
        }

        try
        {
            string jsonText = File.ReadAllText(fileName);
            List<Note>? savedNotes = JsonSerializer.Deserialize<List<Note>>(jsonText);

            if (savedNotes == null)
            {
                Console.WriteLine("Could not load notes: the file must contain a list of notes.");
                return false;
            }

            foreach (Note savedNote in savedNotes)
            {
                if (savedNote == null || string.IsNullOrWhiteSpace(savedNote.Title) ||
                    string.IsNullOrWhiteSpace(savedNote.Content))
                {
                    Console.WriteLine("Could not load notes: each note needs a title and content.");
                    return false;
                }
            }

            notes = savedNotes;
            return true;
        }
        catch (JsonException error)
        {
            Console.WriteLine("Could not load notes: the JSON file is invalid. " + error.Message);
        }
        catch (IOException error)
        {
            Console.WriteLine("Could not read notes: " + error.Message);
        }
        catch (UnauthorizedAccessException error)
        {
            Console.WriteLine("Could not read notes: " + error.Message);
        }

        return false;
    }

    static bool SaveNotes()
    {
        try
        {
            string jsonText = JsonSerializer.Serialize(notes);
            File.WriteAllText(fileName, jsonText);
            return true;
        }
        catch (IOException error)
        {
            Console.WriteLine("Could not save notes: " + error.Message);
        }
        catch (UnauthorizedAccessException error)
        {
            Console.WriteLine("Could not save notes: " + error.Message);
        }

        Console.WriteLine("Your change is only in memory and may be lost when the app closes.");
        return false;
    }
}
