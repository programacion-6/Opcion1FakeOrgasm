namespace LibrarySystem;

public class FineRepository : BaseRepository<Fine>, IFineRepository
{
    public List<Fine> GetActiveFines()
    {
        return Data.Values
                    .Where(fine => !fine.WasPayed)
                    .ToList();
    }

    public Fine? GetByLoan(Loan loan)
    {
        return Data.Values
                   .FirstOrDefault(fine => fine.IdLoan == loan.Id);
    }

    public List<Fine> GetFinesByPatron(Patron patron, ILoanRepository loanRepository)
    {
        var loans = loanRepository.GetAll();

        var patronLoanIds = loans
            .Where(loan => loan.IdPatron == patron.Id)
            .Select(loan => loan.Id)
            .ToHashSet();

        return Data.Values
                   .Where(fine => patronLoanIds.Contains(fine.IdLoan))
                   .ToList();
    }
}
