namespace LibrarySystem;

class Program
{
    public static void Main(string[] args)
    {
        IReceiver<string> receiver = new ConsoleReceiver();
        IMessageRenderer messageRenderer = new ConsoleMessageRenderer();
        AbstractViewChanger<string> viewChanger = new ConsoleViewChanger();

        IResultRenderer<Book> bookRenderer = new ConsoleBookRenderer();
        IResultRenderer<Patron> patronRenderer = new ConsolePatronRenderer();
        IResultRenderer<Fine> fineRenderer = new ConsoleFineRenderer();
        IResultRenderer<Loan> loanRenderer = new ConsoleLoanRenderer();

        IBookRepository bookRepository = new BookRepository();
        IPatronRepository patronRepository = new PatronRepository();
        ILoanRepository loanRepository = new LoanRepository();
        IFineRepository fineRepository = new FineRepository();

        LenderValidator lenderValidator = new LenderValidator(fineRepository);
        Lender lender = new Lender(loanRepository, lenderValidator);
        DebtManager debtManager = new DebtManager(loanRepository, fineRepository);
        Reporter reporter = new Reporter(loanRepository);
        StatisticsGenerator statisticsGenerator = new StatisticsGenerator(loanRepository, fineRepository);

        IEntityRequester<Book> bookRequester = new BookRequesterByConsole(messageRenderer, receiver);
        IEntityRequester<Patron> patronRequester = new PatronRequesterByConsole(receiver, messageRenderer);

        EntitySelectorByConsole<Book> bookSelectorByConsole = new EntitySelectorByConsole<Book>(bookRenderer, messageRenderer, receiver);
        EntitySelectorByConsole<Patron> patronSelectorByConsole = new EntitySelectorByConsole<Patron>(patronRenderer, messageRenderer, receiver);
        EntitySelectorByConsole<Fine> fineSelectorByConsole = new EntitySelectorByConsole<Fine>(fineRenderer, messageRenderer, receiver);

        IEntityCreator<Book, string> bookCreator = new EntityCreatorByConsole<Book>(bookRepository, bookRequester, messageRenderer);
        IEntityUpdater<Book, string> bookUpdater = new EntityUpdaterByConsole<Book>(bookRepository, bookRequester, messageRenderer, bookSelectorByConsole);
        IEntityEliminator<Book, string> bookEliminator = new EntityEliminatorByConsole<Book>(bookRepository, messageRenderer, bookSelectorByConsole);

        IEntityCreator<Patron, string> patronCreator = new EntityCreatorByConsole<Patron>(patronRepository, patronRequester, messageRenderer);
        IEntityUpdater<Patron, string> patronUpdater = new EntityUpdaterByConsole<Patron>(patronRepository, patronRequester, messageRenderer, patronSelectorByConsole);
        IEntityEliminator<Patron, string> patronEliminator = new EntityEliminatorByConsole<Patron>(patronRepository, messageRenderer, patronSelectorByConsole);

        IExecutableHandler<string> bookController = new BookControllerAsText(bookRepository, bookCreator, bookUpdater, bookEliminator, bookRenderer, receiver, messageRenderer);
        IExecutableHandler<string> patronController = new PatronControllerAsText(patronRepository, patronCreator, patronUpdater, patronEliminator, receiver, messageRenderer, patronRenderer);
        IExecutableHandler<string> lenderController = new LoanControllerAsText(lender, loanRepository, patronRepository, bookRepository, receiver, messageRenderer, patronSelectorByConsole, bookSelectorByConsole);
        IExecutableHandler<string> fineController = new FineControllerAsText(debtManager, fineRepository, messageRenderer, fineRenderer, fineSelectorByConsole);
        IExecutableHandler<string> reportController = new ReporterControllerAsText(reporter, statisticsGenerator, patronRepository, bookRenderer, patronRenderer, loanRenderer, messageRenderer, patronSelectorByConsole);

        Dictionary<MenuView, IExecutableHandler<string>> controllers = new Dictionary<MenuView, IExecutableHandler<string>>
        {
            {MenuView.BOOK_HANDLER, bookController},
            {MenuView.PATRON_HANDLER, patronController},
            {MenuView.LOAN_MANAGER, lenderController},
            {MenuView.DEBT_MANAGER, fineController},
            {MenuView.REPORT_MANAGER, reportController},
        };

        FillSampleData(bookRepository, patronRepository, loanRepository, fineRepository);
        IAppController app = new AppControllerAsText(viewChanger, receiver, controllers, messageRenderer);
        app.ExecuteInfinitely();
    }

    public static void FillSampleData(IBookRepository bookRepository, IPatronRepository patronRepository, ILoanRepository loanRepository, IFineRepository fineRepository)
    {
        var random = new Random();

        var sampleBooks = new List<Book>
        {
            new Book { Id = Guid.NewGuid(), Title = "1984", Author = "George Orwell", ISBN = "978-0-452-28423-4", Genre = "Dystopian", PublicationYear = 1949 },
            new Book { Id = Guid.NewGuid(), Title = "To Kill a Mockingbird", Author = "Harper Lee", ISBN = "978-0-06-112008-4", Genre = "Fiction", PublicationYear = 1960 },
            new Book { Id = Guid.NewGuid(), Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", ISBN = "978-0-7432-7356-5", Genre = "Classic", PublicationYear = 1925 },
            new Book { Id = Guid.NewGuid(), Title = "One Hundred Years of Solitude", Author = "Gabriel Garcia Marquez", ISBN = "978-0-06-088328-7", Genre = "Magical Realism", PublicationYear = 1967 },
            new Book { Id = Guid.NewGuid(), Title = "Moby-Dick", Author = "Herman Melville", ISBN = "978-0-14-243724-7", Genre = "Adventure", PublicationYear = 1851 },
            new Book { Id = Guid.NewGuid(), Title = "War and Peace", Author = "Leo Tolstoy", ISBN = "978-0-14-303999-0", Genre = "Historical", PublicationYear = 1869 },
            new Book { Id = Guid.NewGuid(), Title = "Pride and Prejudice", Author = "Jane Austen", ISBN = "978-0-19-953556-9", Genre = "Romance", PublicationYear = 1813 },
            new Book { Id = Guid.NewGuid(), Title = "The Catcher in the Rye", Author = "J.D. Salinger", ISBN = "978-0-316-76948-0", Genre = "Fiction", PublicationYear = 1951 },
            new Book { Id = Guid.NewGuid(), Title = "The Hobbit", Author = "J.R.R. Tolkien", ISBN = "978-0-618-00221-3", Genre = "Fantasy", PublicationYear = 1937 },
            new Book { Id = Guid.NewGuid(), Title = "Crime and Punishment", Author = "Fyodor Dostoevsky", ISBN = "978-0-14-305814-4", Genre = "Philosophical", PublicationYear = 1866 },
            new Book { Id = Guid.NewGuid(), Title = "Brave New World", Author = "Aldous Huxley", ISBN = "978-0-06-085052-4", Genre = "Dystopian", PublicationYear = 1932 },
            new Book { Id = Guid.NewGuid(), Title = "The Iliad", Author = "Homer", ISBN = "978-0-14-027536-0", Genre = "Epic", PublicationYear = -800 },
            new Book { Id = Guid.NewGuid(), Title = "Don Quixote", Author = "Miguel de Cervantes", ISBN = "978-0-06-093434-7", Genre = "Adventure", PublicationYear = 1605 },
            new Book { Id = Guid.NewGuid(), Title = "The Brothers Karamazov", Author = "Fyodor Dostoevsky", ISBN = "978-0-374-52935-9", Genre = "Philosophical", PublicationYear = 1880 },
            new Book { Id = Guid.NewGuid(), Title = "Anna Karenina", Author = "Leo Tolstoy", ISBN = "978-0-14-303500-8", Genre = "Romance", PublicationYear = 1877 }
        };


        foreach (var book in sampleBooks)
        {
            bookRepository.Save(book);
        }

        var samplePatrons = new List<Patron>
        {
            new Patron { Id = Guid.NewGuid(), Name = "John Doe", MembershipNumber = 1, ContactDetails = "john.doe@example.com" },
            new Patron { Id = Guid.NewGuid(), Name = "Jane Smith", MembershipNumber = 2, ContactDetails = "jane.smith@example.com" },
            new Patron { Id = Guid.NewGuid(), Name = "Alice Johnson", MembershipNumber = 3, ContactDetails = "alice.johnson@example.com" },
            new Patron { Id = Guid.NewGuid(), Name = "Bob Brown", MembershipNumber = 4, ContactDetails = "bob.brown@example.com" },
            new Patron { Id = Guid.NewGuid(), Name = "Charlie Davis", MembershipNumber = 5, ContactDetails = "charlie.davis@example.com" },
            new Patron { Id = Guid.NewGuid(), Name = "Diana Evans", MembershipNumber = 6, ContactDetails = "diana.evans@example.com" },
            new Patron { Id = Guid.NewGuid(), Name = "Edward Wilson", MembershipNumber = 7, ContactDetails = "edward.wilson@example.com" },
            new Patron { Id = Guid.NewGuid(), Name = "Fiona Harris", MembershipNumber = 8, ContactDetails = "fiona.harris@example.com" },
            new Patron { Id = Guid.NewGuid(), Name = "George Clark", MembershipNumber = 9, ContactDetails = "george.clark@example.com" },
            new Patron { Id = Guid.NewGuid(), Name = "Helen Martinez", MembershipNumber = 10, ContactDetails = "helen.martinez@example.com" }
        };

        foreach (var patron in samplePatrons)
        {
            patronRepository.Save(patron);
        }

        var borrowedBooksIds = loanRepository.GetCurrentlyLoans()
                                            .Select(loan => loan.Book.Id)
                                            .ToList();
        var unloanedBooks = bookRepository.GetAll()
                            .Where(book => !borrowedBooksIds.Contains(book.Id))
                            .ToList();

        var sampleLoans = new List<Loan>();
        var sampleFines = new List<Fine>();

        foreach (var book in unloanedBooks)
        {
            var patron = samplePatrons[random.Next(samplePatrons.Count)];
            var loanDate = DateTime.Now.AddDays(-random.Next(60));
            var returnDate = loanDate.AddDays(random.Next(1, 31));

            var loan = new Loan
            {
                Id = Guid.NewGuid(),
                Book = book,
                Patron = patron,
                LoanDate = loanDate,
                ReturnDate = returnDate
            };

            loanRepository.Save(loan);
            sampleLoans.Add(loan);

            if (returnDate < DateTime.Now && random.Next(2) == 0)
            {
                var fineAmount = FineCalculator.GetFineAmountByDate(loanDate, returnDate);

                var fine = new Fine
                {
                    Id = Guid.NewGuid(),
                    Loan = loan,
                    FineAmount = fineAmount
                };

                fineRepository.Save(fine);
                sampleFines.Add(fine);
            }
        }
    }
}