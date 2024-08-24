namespace LibrarySystem;

public interface IPatronRepository : IRepository<Patron>, IPaginable<Patron>
{
    Task<Patron?> GetByName(string name);
    Task<Patron?> GetByMembershipNumber(int membershipNumber);
}