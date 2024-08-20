namespace LibrarySystem;

public static class PatronValidator
{
    public static void ValidatePatron(Patron patron)
    {
        if (patron == null)
        {
            throw new PatronException("Patron object cannot be null");
        }

        ValidateName(patron.Name);
        ValidateMembershipNumber(patron.MembershipNumber);
        ValidateContactDetails(patron.ContactDetails);
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new PatronException("Name cannot be null, empty, or whitespace");
        }
    }

    private static void ValidateMembershipNumber(int membershipNumber)
    {
        if (membershipNumber <= 0)
        {
            throw new PatronException("Membership number must be a positive integer");
        }
    }

    private static void ValidateContactDetails(string contactDetails)
    {
        if (string.IsNullOrWhiteSpace(contactDetails))
        {
            throw new PatronException("Contact details cannot be null, empty, or whitespace");
        }
    }
}