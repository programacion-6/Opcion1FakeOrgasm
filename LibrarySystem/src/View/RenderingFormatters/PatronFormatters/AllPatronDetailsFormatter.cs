namespace LibrarySystem;

public class AllPatronDetailsFormatter : IEntityFormatter<Patron>
{
    public Patron? Entity { get; set; }

    public override string ToString()
    {
        if (Entity is null)
        {
            return string.Empty;
        }

        return Entity.Name +
                    "\n\tmembership number: " + Entity.MembershipNumber +
                    "\n\tcontact: " + Entity.ContactDetails;
    }
}
