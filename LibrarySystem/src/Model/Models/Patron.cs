namespace LibrarySystem;

public class Patron : EntityBase
{
    public required string Name { get; set; }
    public required int MembershipNumber { get; set; }
    public required string ContactDetails { get; set; }

    public override string ToString()
    {
        return Name +
                    "\n\tmembership number: " + MembershipNumber +
                    "\n\tcontact: " + ContactDetails;
    }
}