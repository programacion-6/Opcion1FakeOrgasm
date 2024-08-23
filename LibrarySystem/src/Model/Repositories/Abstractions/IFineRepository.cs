namespace LibrarySystem;

public interface IFineRepository : IRepository<Fine>
{
    Fine? GetByLoan(Loan loan);
    List<Fine> GetFinesByPatron(Patron patron, ILoanRepository loanRepository);
    List<Fine> GetActiveFines();
}
