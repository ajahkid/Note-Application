class AppData
{
    public List<User> Users { get; private set; }
    public List<Note> Notes { get; private set; }

    public AppData(List<User> users, List<Note> notes)
    {
        Users = users;
        Notes = notes;
    }
}
