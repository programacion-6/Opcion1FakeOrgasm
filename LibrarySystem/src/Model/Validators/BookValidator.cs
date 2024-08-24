using System.Text.RegularExpressions;

namespace LibrarySystem;

public class BookValidator
{
    public void ValidateBook(Book book)
    {
        if (book == null)
        {
            throw new BookException(
                "Book object cannot be null.",
                SeverityLevel.Critical,
                "This error indicates that the Book object was not initialized before being passed for validation. " +
                "Ensure that the Book object is properly instantiated and contains all required fields before attempting to validate it.");
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
                "Title cannot be null, empty, or whitespace.",
                SeverityLevel.Medium,
                "A book title is mandatory and must contain meaningful text. " +
                "Ensure that the title field is populated with a valid, non-empty string.");
        }
    }

    private void ValidateAuthor(string author)
    {
        if (string.IsNullOrWhiteSpace(author))
        {
            throw new BookException(
                "Author cannot be null, empty, or whitespace.",
                SeverityLevel.Medium,
                "The author's name is required for proper identification of the book. " +
                "Please provide a valid author's name, ensuring that it is not empty or composed solely of whitespace.");
        }

        if (!author.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
        {
            throw new BookException(
                "Author name must contain only letters.",
                SeverityLevel.Medium,
                "The author's name should consist of alphabetic characters only. " +
                "Please verify the author's name and ensure it does not contain any special characters or numbers.");
        }
    }

    private void ValidateISBN(string isbn)
    {
        if (string.IsNullOrWhiteSpace(isbn))
        {
            throw new BookException(
                "ISBN cannot be null, empty, or whitespace.",
                SeverityLevel.Medium,
                "The ISBN is essential for cataloging and uniquely identifying the book. " +
                "Make sure to input a valid ISBN number, and verify that it is not left blank.");
        }

        string isbnPattern = @"^(97(8|9))?\d{9}(\d|X)$";
        if (!Regex.IsMatch(isbn, isbnPattern))
        {
            throw new BookException(
                "ISBN format is invalid. It should be in the format 978-3-16-148410-0 or similar.",
                SeverityLevel.High,
                "This error suggests that the ISBN entered does not match the standard format. " +
                "Please verify the ISBN and ensure it conforms to the standard format, such as '978-3-16-148410-0'.");
        }
    }

    private void ValidateGenreBook(string genre)
    {
        if (string.IsNullOrWhiteSpace(genre))
        {
            throw new BookException(
                "Genre cannot be null, empty, or whitespace.",
                SeverityLevel.Medium,
                "The genre field is important for classifying the book. " +
                "Ensure that a valid genre is provided, and it should not be an empty or whitespace-only string.");
        }

        if (!genre.All(char.IsLetter))
        {
            throw new BookException(
                "Genre must contain only letters.",
                SeverityLevel.Medium,
                "The genre field should consist of alphabetic characters only. " +
                "Please verify the genre field and ensure it does not contain any special characters or numbers");
        }
    }

    private void ValidatePublicationYear(int year)
    {
        if (year < 0)
        {
            throw new BookException(
                "Publication year must be a positive integer.",
                SeverityLevel.Medium,
                "The publication year should represent a valid year in history. " +
                "Please provide a positive integer that reflects the correct publication year of the book.");
        }

        if (year > DateTime.Now.Year)
        {
            throw new BookException(
                "Publication year cannot be in the future.",
                SeverityLevel.High,
                "This error indicates that the publication year provided is set in the future, which is not possible. " +
                "Please ensure that the year is accurate and does not exceed the current year.");
        }
    }
}