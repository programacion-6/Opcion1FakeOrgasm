namespace LibrarySystem;

class Program
{
    public static async Task Main(string[] args)
    {
        var databaseConfig = new DatabaseConfig();
        using var connection = databaseConfig.CreateConnection();

        await connection.OpenAsync();

        await DatabaseConfig.CreateDatabaseSchema(connection);

        IReceiver<string> receiver = new ConsoleReceiver();
        IMessageRenderer messageRenderer = new ConsoleMessageRenderer();
        AbstractViewChanger<string> viewChanger = new ConsoleViewChanger();

        IResultRenderer<Book> bookRenderer = new ConsoleBookRenderer();
        IResultRenderer<Patron> patronRenderer = new ConsolePatronRenderer();
        IResultRenderer<Fine> fineRenderer = new ConsoleFineRenderer();
        IResultRenderer<Loan> loanRenderer = new ConsoleLoanRenderer();

        IBookRepository bookRepository = new BookRepository(connection);
        IPatronRepository patronRepository = new PatronRepository(connection);
        ILoanRepository loanRepository = new LoanRepository(connection);
        IFineRepository fineRepository = new FineRepository(connection);

        LenderValidator lenderValidator = new LenderValidator(fineRepository);
        Lender lender = new Lender(loanRepository, lenderValidator);
        DebtManager debtManager = new DebtManager(loanRepository, fineRepository);
        Reporter reporter = new Reporter(loanRepository, bookRepository, patronRepository);
        StatisticsGenerator statisticsGenerator = new StatisticsGenerator(loanRepository, fineRepository, bookRepository, patronRepository);

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

        IAppController app = new AppControllerAsText(viewChanger, receiver, controllers, messageRenderer);
        await app.ExecuteInfinitelyAsync();
    }
}