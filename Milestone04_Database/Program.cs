using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            using (NoteDbContext database = new NoteDbContext())
            {
                database.Database.EnsureCreated();
                NoteRepository repository = new NoteRepository(database);
                NoteService service = new NoteService(repository);
                ConsoleMenu menu = new ConsoleMenu(service);
                menu.Run();
            }
        }
        catch (SqliteException error)
        {
            Console.WriteLine("Database error: " + error.Message);
        }
        catch (DbUpdateException error)
        {
            Console.WriteLine("Could not save to the database: " + error.Message);
        }
        catch (InvalidDataException error)
        {
            Console.WriteLine("Invalid stored data: " + error.Message);
        }
        catch (IOException error)
        {
            Console.WriteLine("Could not access the database: " + error.Message);
        }
        catch (UnauthorizedAccessException error)
        {
            Console.WriteLine("Could not access the database: " + error.Message);
        }
    }
}
