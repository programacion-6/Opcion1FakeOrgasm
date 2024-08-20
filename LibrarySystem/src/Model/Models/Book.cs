namespace LibrarySystem;

public class Book : EntityBase
{
    public required string Title { get; set; }
    public required string Author { get; set; }
    public required string ISBN { get; set; }
    public required string Genre { get; set; }
    public int PublicationYear { get; set; } = DateTime.Now.Year;

    public override string ToString()
    {
        return Title +
                    "\n\tauthor: " + Author +
                    "\n\tISBN: " + ISBN +
                    "\n\tgenre: " + Genre +
                    "\n\tyear: " + PublicationYear;
    }
}