namespace LibrarySystem;

public class LenderValidator
{
    private readonly IFineRepository fineRepository;
    private const int MIN_LOAN_TIME = 1;
    private const int MAX_LOAN_TIME = 30;

    public LenderValidator(IFineRepository fineRepository)
    {
        this.fineRepository = fineRepository;
    }

    public bool HasFine(Patron patron)
    {
        var activeFines = fineRepository.GetFinesByPatron(patron).Where(fine => !fine.WasPayed);
        return activeFines.Any();
    }

    public void ValidateLoanTime(int loanTimeInDays)
    {
        if (loanTimeInDays < MIN_LOAN_TIME)
        {
            throw new LoanException($"The loan period must be {MIN_LOAN_TIME} or more days");
        }

        if (loanTimeInDays > MAX_LOAN_TIME)
        {
            throw new LoanException($"The loan period cannot be longer than {MAX_LOAN_TIME} days.");
        }
    }
}