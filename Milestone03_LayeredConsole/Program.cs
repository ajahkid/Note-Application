using System.Text.Json;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            JsonRepository repository = new JsonRepository();
            NoteService service = new NoteService(repository);
            ConsoleMenu menu = new ConsoleMenu(service);
            menu.Run();
        }
        catch (JsonException error)
        {
            Console.WriteLine("Could not load the JSON data: " + error.Message);
        }
        catch (InvalidDataException error)
        {
            Console.WriteLine("Could not load the data: " + error.Message);
        }
        catch (IOException error)
        {
            Console.WriteLine("Could not access the data file: " + error.Message);
        }
        catch (UnauthorizedAccessException error)
        {
            Console.WriteLine("Could not access the data file: " + error.Message);
        }
    }
}
