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
                   .FirstOrDefault(fine => fine.Loan.Id == loan.Id);
    }

    public List<Fine> GetFinesByPatron(Patron patron)
    {
        return Data.Values
                   .Where(fine => fine.Loan.IdPatron == patron.Id)
                   .ToList();
    }
}
