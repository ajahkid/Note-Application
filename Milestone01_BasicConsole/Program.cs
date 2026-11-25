using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

class Program
{
    private static List<string> notes = new List<string>();
    private static string fileName = "notes.json";

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
            else if (choice == "0")
            {
                isRunning = false;
                Console.WriteLine("Goodbye!");
            }
            else
            {
                Console.WriteLine("Invalid option. Please choose 0 to 4.");
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
        Console.WriteLine("0. Exit");
    }

    static void AddNote()
    {
        Console.Write("Enter your note: ");
        string? note = Console.ReadLine();

        if (note == null)
        {
            note = "";
        }

        notes.Add(note);
        if (SaveNotes())
        {
            Console.WriteLine("Note added and saved.");
        }
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
            Console.WriteLine((index + 1) + ". " + notes[index]);
        }
    }

    static void FindNotes()
    {
        Console.Write("Enter text to search for: ");
        string? searchText = Console.ReadLine();

        if (searchText == null)
        {
            searchText = "";
        }

        int matchCount = 0;

        for (int index = 0; index < notes.Count; index++)
        {
            if (notes[index].IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                Console.WriteLine((index + 1) + ". " + notes[index]);
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
            noteNumber >= 1 &&
            noteNumber <= notes.Count)
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

    static bool LoadNotes()
    {
        if (!File.Exists(fileName))
        {
            return true;
        }

        try
        {
            string jsonText = File.ReadAllText(fileName);
            // Convert the JSON text back into a list of notes.
            List<string>? savedNotes = JsonSerializer.Deserialize<List<string>>(jsonText);

            if (savedNotes == null)
            {
                Console.WriteLine("Could not load notes: the JSON file must contain a list of notes.");
                return false;
            }

            foreach (string savedNote in savedNotes)
            {
                if (savedNote == null)
                {
                    Console.WriteLine("Could not load notes: a note cannot be null.");
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
            // Convert the list of notes into JSON text before saving it.
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
