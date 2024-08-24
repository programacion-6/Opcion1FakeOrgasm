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

    public async Task<bool> HasFine(Patron patron)
    {
        var activeFines = (await fineRepository.GetFinesByPatron(patron.Id)).Where(fine => !fine.WasPayed);
        return activeFines.Any();
    }

    public void ValidateLoanTime(int loanTimeInDays)
    {
        if (loanTimeInDays < MIN_LOAN_TIME)
        {
            throw new LoanException(
                $"The loan period must be {MIN_LOAN_TIME} or more days",
                SeverityLevel.Medium,
                $"The loan period specified ({loanTimeInDays} days) is too short. " +
                $"Please ensure the loan period is at least {MIN_LOAN_TIME} day(s) to meet the minimum borrowing requirements.");
        }

        if (loanTimeInDays > MAX_LOAN_TIME)
        {
            throw new LoanException(
                $"The loan period cannot be longer than {MAX_LOAN_TIME} days.",
                SeverityLevel.Medium,
                $"The loan period specified ({loanTimeInDays} days) exceeds the maximum allowed duration of {MAX_LOAN_TIME} days. " +
                $"Adjust the loan period to a duration within the acceptable range.");
        }
    }
}