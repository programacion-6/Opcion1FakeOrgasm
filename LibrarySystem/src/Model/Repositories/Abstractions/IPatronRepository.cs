namespace LibrarySystem;

public interface IPatronRepository : IRepository<Patron>
{
    Patron? GetByName(string name);
    Patron? GetByMembershipNumber(int membershipNumber);
}