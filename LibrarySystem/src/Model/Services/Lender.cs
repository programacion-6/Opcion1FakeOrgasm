namespace LibrarySystem;

public class Lender
{
    private readonly ILoanRepository loanRepository;
    private readonly LenderValidator lenderValidator;

    public Lender(ILoanRepository loanRepository, LenderValidator lenderValidator)
    {
        this.loanRepository = loanRepository;
        this.lenderValidator = lenderValidator;
    }

    public void ReturnBook(Loan loan)
    {
        loan.WasReturn = true;
        loanRepository.Update(loan);
    }

    public void LendBook(Book book, Patron patron, int loanTimeInDays)
    {
        lenderValidator.ValidateLoanTime(loanTimeInDays);
        if (!lenderValidator.HasFine(patron, loanRepository))
        {
            var loan = new Loan()
            {
                Id = Guid.NewGuid(),
                IdBook = book.Id,
                IdPatron = patron.Id,
                ReturnDate = DateTime.Now.AddDays(loanTimeInDays)
            };
            loanRepository.Save(loan);
        }
        else
        {
            throw new Exception("The user has active fines.");
        }
    }
}