namespace LibrarySystem;

public class DebtManager
{
    private readonly ILoanRepository loanRepository;
    private readonly IFineRepository fineRepository;

    public DebtManager(ILoanRepository loanRepository, IFineRepository fineRepository)
    {
        this.loanRepository = loanRepository;
        this.fineRepository = fineRepository;
    }

    public async Task MarkAsPaid(Fine fine)
    {
        fine.WasPayed = true;
        await fineRepository.Update(fine);
    }

    private async Task<Fine> CreateFine(Loan loan)
    {
        var fineAmount = FineCalculator.GetFineAmountByDate(loan.LoanDate, loan.ReturnDate);
        var fine = new Fine
        {
            Id = Guid.NewGuid(),
            LoanId = loan.Id,
            FineAmount = fineAmount,
            WasPayed = false
        };
        await fineRepository.Save(fine);
        return fine;
    }

    public async void CreateDebtsAutomatically()
    {
        var overdueLoans = await loanRepository.GetOverdueLoans();
        foreach (var loan in overdueLoans)
        {
            await CreateFine(loan);
        }
    }
}