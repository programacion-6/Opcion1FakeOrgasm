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

    public async Task ReturnBook(Loan loan)
    {
        loan.WasReturn = true;
        await loanRepository.Update(loan);
    }

    public async Task LendBook(Book book, Patron patron, int loanTimeInDays)
    {
        lenderValidator.ValidateLoanTime(loanTimeInDays);
        if (!await lenderValidator.HasFine(patron))
        {
            var loan = new Loan()
            {
                Id = Guid.NewGuid(),
                IdBook = book.Id,
                IdPatron = patron.Id,
                ReturnDate = DateTime.Now.AddDays(loanTimeInDays)
            };
            await loanRepository.Save(loan);
        }
        else
        {
            throw new Exception("The user has active fines.");
        }
    }
}