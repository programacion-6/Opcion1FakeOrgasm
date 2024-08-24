namespace LibrarySystem;

public interface IFineRepository : IRepository<Fine>
{
    Task<Fine?> GetByLoan(Guid loanId);
    Task<IEnumerable<Fine>> GetFinesByPatron(Guid patronId);
    Task<IEnumerable<Fine>> GetActiveFines();
}
