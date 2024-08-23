namespace LibrarySystem;

public class AllBookDetailsFormatter : IEntityFormatter<Book>
{
    public Book? Entity { get; set; }

    public override string ToString()
    {
        if (Entity is null)
        {
            return string.Empty;
        }

        return Entity.Title +
               "\n\tauthor: " + Entity.Author +
               "\n\tISBN: " + Entity.ISBN +
               "\n\tgenre: " + Entity.Genre +
               "\n\tyear: " + Entity.PublicationYear;
    }
}
