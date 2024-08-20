namespace LibrarySystem;

public class PatronRepository : BaseRepository<Patron>, IPatronRepository
{
    public Patron? GetByName(string name)
    {
        return Data.Values
                   .FirstOrDefault(patron =>
                   patron.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public Patron? GetByMembershipNumber(int membershipNumber)
    {
        return Data.Values
                   .FirstOrDefault(patron =>
                   patron.MembershipNumber == membershipNumber);
    }
}
