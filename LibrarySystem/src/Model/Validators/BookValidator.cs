using System.Text.RegularExpressions;

namespace LibrarySystem;

public class BookValidator
{
    public void ValidateBook(Book book)
    {
        if (book == null)
        {
            throw new BookException(
                "Book object cannot be null",
                SeverityLevel.Critical,
                "Ensure that the book object is initialized before passing it for validation.");
        }

        ValidateTitle(book.Title);
        ValidateAuthor(book.Author);
        ValidateISBN(book.ISBN);
        ValidateGenreBook(book.Genre);
        ValidatePublicationYear(book.PublicationYear);
    }

    private void ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new BookException(
                "Title cannot be null, empty, or whitespace",
                SeverityLevel.Medium,
                "Provide a valid title for the book.");
        }
    }

    private void ValidateAuthor(string author)
    {
        if (string.IsNullOrWhiteSpace(author))
        {
            throw new BookException(
                "Author cannot be null, empty, or whitespace",
                SeverityLevel.Medium,
                "Provide a valid author name for the book.");
        }
    }

    private void ValidateISBN(string isbn)
    {
        if (string.IsNullOrWhiteSpace(isbn))
        {
            throw new BookException(
                "ISBN cannot be null, empty, or whitespace",
                SeverityLevel.Medium,
                "Provide a valid ISBN for the book.");
        }

        string isbnPattern = @"^(97(8|9))?\d{9}(\d|X)$";
        if (!Regex.IsMatch(isbn, isbnPattern))
        {
            throw new BookException(
                "ISBN format is invalid. It should be in the format 978-3-16-148410-0 or similar.",
                SeverityLevel.High,
                "Ensure that the ISBN is in the correct format before proceeding.");
        }
    }

    private void ValidateGenreBook(string genre)
    {
        if (string.IsNullOrWhiteSpace(genre))
        {
            throw new BookException(
                "Genre cannot be null, empty, or whitespace",
                SeverityLevel.Medium,
                "Provide a valid genre for the book.");
        }
    }

    private void ValidatePublicationYear(int year)
    {
        if (year < 0)
        {
            throw new BookException(
                "Publication year must be a positive integer",
                SeverityLevel.Medium,
                "Provide a valid publication year for the book.");
        }

        if (year > DateTime.Now.Year)
        {
            throw new BookException(
                "Publication year cannot be in the future",
                SeverityLevel.High,
                "Ensure the publication year is not set to a future date.");
        }
    }
}