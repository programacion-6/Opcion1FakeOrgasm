namespace LibrarySystem;

public class StatisticsGenerator
{
    private const int TOP_NUMBER_OF_ENTITIES = 3;
    private readonly ILoanRepository loanRepository;
    private readonly IFineRepository fineRepository;

    public StatisticsGenerator(ILoanRepository loanRepository, IFineRepository fineRepository)
    {
        this.loanRepository = loanRepository;
        this.fineRepository = fineRepository;
    }

    public List<Book> GetMostBorrowedBooks()
    {
        var loans = loanRepository.GetAll();
        var mostBorrowedBooks = loans.GroupBy(loan => loan.Book)
                                      .OrderByDescending(group => group.Count())
                                      .Select(group => group.Key)
                                      .Take(TOP_NUMBER_OF_ENTITIES)
                                      .ToList();
        return mostBorrowedBooks;
    }

    public List<Patron> GetMostActivePatrons()
    {
        var loans = loanRepository.GetAll();
        var mostActivePatrons = loans.GroupBy(loan => loan.Patron)
                                     .OrderByDescending(group => group.Count())
                                     .Select(group => group.Key)
                                     .Take(TOP_NUMBER_OF_ENTITIES)
                                     .ToList();
        return mostActivePatrons;
    }

    public List<Tuple<Patron, List<Fine>>> GetPatronsFines()
    {
        var fines = fineRepository.GetAll();
        var patronsFines = fines.GroupBy(fine => fine.Loan.Patron)
                                .Select(group => new Tuple<Patron, List<Fine>>(group.Key, group.ToList()))
                                .ToList();
        return patronsFines;
    }
}
