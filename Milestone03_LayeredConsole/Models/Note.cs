class Note
{
    public int Id { get; private set; }
    public int OwnerId { get; private set; }
    public string Title { get; private set; }
    public string Content { get; private set; }

    public Note(int id, int ownerId, string title, string content)
    {
        Id = id;
        OwnerId = ownerId;
        Title = title;
        Content = content;
    }

    public void Update(string title, string content)
    {
        Title = title;
        Content = content;
    }
}
