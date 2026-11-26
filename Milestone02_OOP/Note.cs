class Note
{
    public string Title { get; private set; }
    public string Content { get; private set; }

    public Note(string title, string content)
    {
        Title = title;
        Content = content;
    }

    public void Update(string title, string content)
    {
        Title = title;
        Content = content;
    }
}
