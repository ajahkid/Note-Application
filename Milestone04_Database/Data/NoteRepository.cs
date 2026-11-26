using Microsoft.EntityFrameworkCore;

class NoteRepository
{
    private NoteDbContext database;

    public NoteRepository(NoteDbContext database)
    {
        this.database = database;
    }

    public User? FindUser(string normalizedUsername)
    {
        return database.Users
            .FromSqlInterpolated($"SELECT * FROM Users WHERE NormalizedUsername = {normalizedUsername}")
            .FirstOrDefault();
    }

    public void AddUser(User user)
    {
        database.Users.Add(user);
        database.SaveChanges();
    }

    public List<Note> GetNotes(int ownerId)
    {
        return database.Notes
            .FromSqlInterpolated($"SELECT * FROM Notes WHERE OwnerId = {ownerId} ORDER BY Id")
            .ToList();
    }

    public Note? FindNote(int ownerId, int noteId)
    {
        return database.Notes
            .FromSqlInterpolated($"SELECT * FROM Notes WHERE OwnerId = {ownerId} AND Id = {noteId}")
            .FirstOrDefault();
    }

    public void AddNote(Note note)
    {
        database.Notes.Add(note);
        database.SaveChanges();
    }

    public void SaveChanges()
    {
        database.SaveChanges();
    }

    public void DeleteNote(Note note)
    {
        database.Notes.Remove(note);
        database.SaveChanges();
    }
}
