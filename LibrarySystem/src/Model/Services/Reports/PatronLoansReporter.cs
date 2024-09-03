namespace LibrarySystem.Reports;

public class PatronLoansReporter
{
    private readonly ILoanRepository _loanRepository;

    public PatronLoansReporter(ILoanRepository loanRepository)
    {
        _loanRepository = loanRepository;
    }

    public async Task<List<Loan>> GetLoansByPatron(Patron patron)
    {
        return (await _loanRepository.GetLoansByPatron(patron.Id)).ToList();
    }
}