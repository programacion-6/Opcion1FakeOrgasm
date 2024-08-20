﻿namespace LibrarySystem;

public class DebtManager
{
    private readonly ILoanRepository loanRepository;
    private readonly IFineRepository fineRepository;

    public DebtManager(ILoanRepository loanRepository, IFineRepository fineRepository)
    {
        this.loanRepository = loanRepository;
        this.fineRepository = fineRepository;
    }

    public void MarkAsPaid(Fine fine)
    {
        fine.WasPayed = true;
        fineRepository.Update(fine);
    }

    private Fine CreateFine(Loan loan)
    {
        var fineAmount = FineCalculator.GetFineAmountByDate(loan.LoanDate, loan.ReturnDate);
        var fine = new Fine
        {
            Id = Guid.NewGuid(),
            Loan = loan,
            FineAmount = fineAmount,
            WasPayed = false
        };
        fineRepository.Save(fine);
        return fine;
    }

    public void CreateDebtsAutomatically()
    {
        var overdueLoans = loanRepository.GetOverdueLoans();
        foreach (var loan in overdueLoans)
        {
            CreateFine(loan);
        }
    }
}