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

        IBookRepository bookRepository = new BookRepository(databaseConfig.ConnectionString);
        IPatronRepository patronRepository = new PatronRepository(databaseConfig.ConnectionString);
        ILoanRepository loanRepository = new LoanRepository(databaseConfig.ConnectionString);
        IFineRepository fineRepository = new FineRepository(databaseConfig.ConnectionString);

        LenderValidator lenderValidator = new LenderValidator(fineRepository);
        Lender lender = new Lender(loanRepository, lenderValidator);
        DebtManager debtManager = new DebtManager(loanRepository, fineRepository);

        Reporter reporter = new Reporter(loanRepository, bookRepository, patronRepository);
        StatisticsGenerator statisticsGenerator = new StatisticsGenerator(loanRepository, fineRepository, bookRepository, patronRepository);

        IEntityRequester<Book> bookRequester = new BookRequesterByConsole(messageRenderer);
        IEntityRequester<Patron> patronRequester = new PatronRequesterByConsole(messageRenderer);

        IEntityFormatterFactory<Book> bookFormatterFactory = new BookFormatterFactory();
        IEntityFormatterFactory<Patron> patronFormatterFactory = new PatronFormatterFactory();
        IEntityFormatterFactory<Loan> loanFormatterFactory = new LoanFormatterFactory(bookRepository, patronRepository);
        IEntityFormatterFactory<Fine> fineFormatterFactory = new FineFormatterFactory(loanRepository);

        EntitySelectorByConsole<Book> bookSelectorByConsole = new EntitySelectorByConsole<Book>(messageRenderer, bookFormatterFactory);
        EntitySelectorByConsole<Patron> patronSelectorByConsole = new EntitySelectorByConsole<Patron>(messageRenderer, patronFormatterFactory);
        EntitySelectorByConsole<Fine> fineSelectorByConsole = new EntitySelectorByConsole<Fine>(messageRenderer, fineFormatterFactory);

        IEntityCreator<Book, string> bookCreator = new EntityCreatorByConsole<Book>(bookRepository, bookRequester, messageRenderer);
        IEntityUpdater<Book, string> bookUpdater = new EntityUpdaterByConsole<Book>(bookRepository, bookRequester, messageRenderer, bookSelectorByConsole);
        IEntityEliminator<Book, string> bookEliminator = new EntityEliminatorByConsole<Book>(bookRepository, messageRenderer, bookSelectorByConsole);
        IEntityCreator<Patron, string> patronCreator = new EntityCreatorByConsole<Patron>(patronRepository, patronRequester, messageRenderer);
        IEntityUpdater<Patron, string> patronUpdater = new EntityUpdaterByConsole<Patron>(patronRepository, patronRequester, messageRenderer, patronSelectorByConsole);
        IEntityEliminator<Patron, string> patronEliminator = new EntityEliminatorByConsole<Patron>(patronRepository, messageRenderer, patronSelectorByConsole);

        EntityRendererAsConsolePages<Book> bookRendererAsPages = new EntityRendererAsConsolePages<Book>(bookRepository, bookFormatterFactory);
        EntityRendererAsConsolePages<Patron> patronRendererAsPages = new EntityRendererAsConsolePages<Patron>(patronRepository, patronFormatterFactory);

        IExecutableHandler<string> bookController = new BookControllerAsText(bookRepository, bookCreator, bookUpdater, bookEliminator, bookFormatterFactory, messageRenderer, bookRendererAsPages);
        IExecutableHandler<string> patronController = new PatronControllerAsText(patronRepository, patronCreator, patronUpdater, patronEliminator, messageRenderer, patronFormatterFactory, patronRendererAsPages);
        IExecutableHandler<string> lenderController = new LoanControllerAsText(lender, loanRepository, patronRepository, bookRepository, messageRenderer, patronSelectorByConsole, bookSelectorByConsole);
        IExecutableHandler<string> fineController = new FineControllerAsText(debtManager, fineRepository, messageRenderer, fineFormatterFactory, fineSelectorByConsole);
        IExecutableHandler<string> reportController = new ReporterControllerAsText(reporter, statisticsGenerator, patronRepository, bookFormatterFactory, patronFormatterFactory, loanFormatterFactory, messageRenderer, patronSelectorByConsole, fineFormatterFactory);

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