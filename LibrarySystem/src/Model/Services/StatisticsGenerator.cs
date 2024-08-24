namespace LibrarySystem;

public class StatisticsGenerator
{
    private const int TOP_NUMBER_OF_ENTITIES = 3;
    private readonly ILoanRepository loanRepository;
    private readonly IFineRepository fineRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IPatronRepository _patronRepository;

    public StatisticsGenerator(ILoanRepository loanRepository, IFineRepository fineRepository, IBookRepository bookRepository, IPatronRepository patronRepository)
    {
        this.loanRepository = loanRepository;
        this.fineRepository = fineRepository;
        _bookRepository = bookRepository;
        _patronRepository = patronRepository;
    }

    public async Task<List<Book>> GetMostBorrowedBooks()
    {
        var loans = await loanRepository.GetAll();

        var bookIds = loans.Select(loan => loan.BookId).Distinct().ToList();
        var books = new List<Book>();

        foreach (var bookId in bookIds)
        {
            var book = await _bookRepository.GetById(bookId);
            if (book is not null)
            {
                books.Add(book);
            }
        }

        var mostBorrowedBooks = loans
            .GroupBy(loan => books.First(book => book.Id == loan.BookId))
            .OrderByDescending(group => group.Count())
            .Select(group => group.Key)
            .Take(TOP_NUMBER_OF_ENTITIES)
            .ToList();

        return mostBorrowedBooks;
    }

    public async Task<List<Patron>> GetMostActivePatrons()
    {
        var loans = await loanRepository.GetAll();

        var patronsIds = loans.Select(loan => loan.PatronId).Distinct().ToList();
        var patrons = new List<Patron>();

        foreach (var patronId in patronsIds)
        {
            var patron = await _patronRepository.GetById(patronId);
            if (patron is not null)
            {
                patrons.Add(patron);
            }
        }

        var mostActivePatrons = loans.GroupBy(loan => patrons.First(patron => patron.Id == loan.PatronId))
                                     .OrderByDescending(group => group.Count())
                                     .Select(group => group.Key)
                                     .Take(TOP_NUMBER_OF_ENTITIES)
                                     .ToList();
        return mostActivePatrons;
    }

    public async Task<List<Tuple<Patron, List<Fine>>>> GetPatronsFines()
    {
        var fines = await fineRepository.GetAll();

        var loanIds = fines.Select(fine => fine.LoanId).Distinct().ToList();
        var loans = new List<Loan>();

        foreach (var loanId in loanIds)
        {
            var loan = await loanRepository.GetById(loanId);
            if (loan is not null)
            {
                loans.Add(loan);
            }
        }

        var patrons = new List<Patron>();

        foreach (var loan in loans)
        {
            var patron = await _patronRepository.GetById(loan.PatronId);
            if (patron is not null && !patrons.Any(p => p.Id == patron.Id))
            {
                patrons.Add(patron);
            }
        }

        var patronsFines = fines
            .GroupBy(fine => patrons.First(patron => patron.Id == loans.First(loan => loan.Id == fine.LoanId).PatronId))
            .Select(group => new Tuple<Patron, List<Fine>>(group.Key, group.ToList()))
            .ToList();

        return patronsFines;
    }
}
