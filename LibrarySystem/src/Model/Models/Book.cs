namespace LibrarySystem;

public class Book : EntityBase
{
    private string _title = string.Empty;
    private string _author = string.Empty;
    private string _isbn = string.Empty;
    private string _genre = string.Empty;
    private int _publicationYear;

    public Book()
    {
        _publicationYear = DateTime.Now.Year;
    }

    public required string Title
    {
        get => _title;
        set => _title = value;
    }

    public required string Author
    {
        get => _author;
        set => _author = value;
    }

    public required string ISBN
    {
        get => _isbn;
        set => _isbn = value;
    }

    public required string Genre
    {
        get => _genre;
        set => _genre = value;
    }

    public int PublicationYear
    {
        get => _publicationYear;
        set => _publicationYear = value;
    }

    public override string ToString()
    {
        return Title +
                    "\n\tauthor: " + Author +
                    "\n\tISBN: " + ISBN +
                    "\n\tgenre: " + Genre +
                    "\n\tyear: " + PublicationYear;
    }
}