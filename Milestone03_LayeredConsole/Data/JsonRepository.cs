using System.Text.Json;

class JsonRepository
{
    private string fileName = "milestone03-data.json";

    public AppData Load()
    {
        if (File.Exists(fileName) == false)
        {
            return new AppData(new List<User>(), new List<Note>());
        }

        string jsonText = File.ReadAllText(fileName);
        AppData? data = JsonSerializer.Deserialize<AppData>(jsonText);

        if (data == null || data.Users == null || data.Notes == null)
        {
            throw new InvalidDataException("The data file must contain users and notes.");
        }

        foreach (User user in data.Users)
        {
            if (user == null || user.Id < 1 || string.IsNullOrWhiteSpace(user.Username) ||
                string.IsNullOrWhiteSpace(user.PasswordSalt) || string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                throw new InvalidDataException("The data file contains an invalid user.");
            }
        }

        foreach (Note note in data.Notes)
        {
            if (note == null || note.Id < 1 || note.OwnerId < 1 ||
                string.IsNullOrWhiteSpace(note.Title) || string.IsNullOrWhiteSpace(note.Content))
            {
                throw new InvalidDataException("The data file contains an invalid note.");
            }
        }

        return data;
    }

    public void Save(AppData data)
    {
        string jsonText = JsonSerializer.Serialize(data);
        File.WriteAllText(fileName, jsonText);
    }
}
