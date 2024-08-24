using LibrarySystem;

public class BookTests
    {
        private Book CreateValidBook()
        {
            return new Book
            {
                Id = Guid.NewGuid(),
                Title = "Test Book",
                Author = "Test Author",
                ISBN = "1234567890",
                Genre = "Test Genre"
            };
        }

        [Fact]
        public void Constructor_ShouldSetDefaultPublicationYear()
        {
            var book = CreateValidBook();
            Assert.Equal(DateTime.Now.Year, book.PublicationYear);
        }

        [Fact]
        public void Title_SetAndGetPropertyCorrectly()
        {
            var book = CreateValidBook();
            book.Title = "New Test Book";
            Assert.Equal("New Test Book", book.Title);
        }

        [Fact]
        public void Author_SetAndGetPropertyCorrectly()
        {
            var book = CreateValidBook();
            book.Author = "Jane Doe";
            Assert.Equal("Jane Doe", book.Author);
        }

        [Fact]
        public void ISBN_SetAndGetPropertyCorrectly()
        {
            var book = CreateValidBook();
            book.ISBN = "0987654321";
            Assert.Equal("0987654321", book.ISBN);
        }

        [Fact]
        public void Genre_SetAndGetPropertyCorrectly()
        {
            var book = CreateValidBook();
            book.Genre = "Non-Fiction";
            Assert.Equal("Non-Fiction", book.Genre);
        }

        [Fact]
        public void PublicationYear_SetAndGetPropertyCorrectly()
        {
            var book = CreateValidBook();
            book.PublicationYear = 2020;
            Assert.Equal(2020, book.PublicationYear);
        }

        [Fact]
        public void ToString_ReturnsCorrectlyFormattedString()
        {
            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = "The Great Gatsby",
                Author = "F. Scott Fitzgerald",
                ISBN = "9780743273565",
                Genre = "Classic",
                PublicationYear = 1925
            };

            var expected = "The Great Gatsby\n\tauthor: F. Scott Fitzgerald\n\tISBN: 9780743273565\n\tgenre: Classic\n\tyear: 1925";
            Assert.Equal(expected, book.ToString());
        }

        [Fact]
        public void Id_SetAndGetPropertyCorrectly()
        {
            var id = Guid.NewGuid();
            var book = CreateValidBook();
            book.Id = id;
            Assert.Equal(id, book.Id);
        }
    }