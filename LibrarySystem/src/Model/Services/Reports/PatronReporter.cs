namespace LibrarySystem.Reports;

public class PatronReporter
{
    private readonly ILoanRepository _loanRepository;
    private readonly IPatronRepository _patronRepository;

    public PatronReporter(ILoanRepository loanRepository, IPatronRepository patronRepository)
    {
        _loanRepository = loanRepository;
        _patronRepository = patronRepository;
    }

    public async Task<List<Patron>> GetPatronsThatBorrowedBooks()
    {
        var currentlyLoans = await _loanRepository.GetCurrentlyLoans();
        var currentlyBorrowedPatrons = new List<Patron>();

        foreach (var loan in currentlyLoans)
        {
            var patron = await _patronRepository.GetById(loan.PatronId);
            if (patron is not null)
            {
                currentlyBorrowedPatrons.Add(patron);
            }
        }

        return currentlyBorrowedPatrons;
    }
}