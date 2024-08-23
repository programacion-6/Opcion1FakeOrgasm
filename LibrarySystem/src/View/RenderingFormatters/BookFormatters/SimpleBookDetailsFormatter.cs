namespace LibrarySystem;

public class SimpleBookDetailsFormatter : IEntityFormatter<Book>
{
    public Book? Entity { get; set; }

    public override string ToString()
    {
        if (Entity is null) {
            return string.Empty;
        }

        return Entity.Title + " | " + Entity.Genre;
    }
}
