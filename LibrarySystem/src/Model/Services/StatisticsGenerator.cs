namespace LibrarySystem;

public class StatisticsGenerator
{
    private const int TOP_NUMBER_OF_ENTITIES = 3;
    private readonly ILoanRepository _loanRepository;
    private readonly IFineRepository _fineRepository;

    private readonly IBookRepository _bookRepository;

    private readonly IPatronRepository _patronRepository;

    public StatisticsGenerator(ILoanRepository loanRepository, IFineRepository fineRepository,
                               IBookRepository bookRepository, IPatronRepository patronRepository)
    {
        _loanRepository = loanRepository;
        _fineRepository = fineRepository;
        _bookRepository = bookRepository;
        _patronRepository = patronRepository;
    }

    public List<Book> GetMostBorrowedBooks()
    {
        var loans = _loanRepository.GetAll();

        var bookIds = loans
            .GroupBy(loan => loan.IdBook)
            .Select(group => new
            {
                IdBook = group.Key,
                Count = group.Count()
            })
            .OrderByDescending(bookGroup => bookGroup.Count)
            .Take(TOP_NUMBER_OF_ENTITIES)
            .ToList();

        var mostBorrowedBooks = bookIds
            .Select(bookGroup => _bookRepository.GetById(bookGroup.IdBook))
            .Where(book => book != null)
            .ToList();

        return mostBorrowedBooks;
    }


    public List<Patron> GetMostActivePatrons()
    {
        var loans = _loanRepository.GetAll();

        var patronIds = loans
            .GroupBy(loan => loan.IdPatron)
            .Select(group => new
            {
                IdPatron = group.Key,
                Count = group.Count()
            })
            .OrderByDescending(patronGroup => patronGroup.Count)
            .Take(TOP_NUMBER_OF_ENTITIES)
            .ToList();

        var mostActivePatrons = patronIds
            .Select(patronGroup => _patronRepository.GetById(patronGroup.IdPatron))
            .Where(patron => patron != null)
            .ToList();

        return mostActivePatrons;
    }


    public List<Tuple<Patron, List<Fine>>> GetPatronsFines()
    {
        var fines = _fineRepository.GetAll();
        var loans = _loanRepository.GetAll();

        var loanIdToPatronId = loans.ToDictionary(loan => loan.Id, loan => loan.IdPatron);

        var patronIds = fines
            .GroupBy(fine => loanIdToPatronId.GetValueOrDefault(fine.IdLoan))
            .Select(group => new
            {
                IdPatron = group.Key,
                Fines = group.ToList()
            })
            .ToList();

        var patronsFines = patronIds
            .Select(patronGroup =>
            {
                var patron = _patronRepository.GetById(patronGroup.IdPatron);
                return new Tuple<Patron, List<Fine>>(patron, patronGroup.Fines);
            })
            .Where(tuple => tuple.Item1 != null)
            .ToList();

        return patronsFines;
    }
}
