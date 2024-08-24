namespace LibrarySystem;

public static class PatronValidator
{
    public static void ValidatePatron(Patron patron)
    {
        if (patron == null)
        {
            throw new PatronException(
                "Patron object cannot be null",
                SeverityLevel.Critical,
                "This error occurs when the Patron object is not initialized. " +
                "Ensure that the Patron object is properly instantiated and filled with the necessary data before passing it for validation.");
        }

        ValidateName(patron.Name);
        ValidateMembershipNumber(patron.MembershipNumber);
        ValidateContactDetails(patron.ContactDetails);
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new PatronException(
                "Name cannot be null, empty, or whitespace",
                SeverityLevel.Medium,
                "A patron's name is essential for identification. " +
                "Ensure that a valid name is provided, and it should not be left blank or contain only whitespace.");
        }

        if (!name.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
        {
            throw new PatronException(
                "Name must contain only letters and spaces",
                SeverityLevel.Medium,
                "The patron's name should consist of alphabetic characters and spaces only. " +
                "Please verify the name and ensure it does not contain any special characters or numbers.");
        }
    }

    private static void ValidateMembershipNumber(int membershipNumber)
    {
        if (membershipNumber <= 0)
        {
            throw new PatronException(
                "Membership number must be a positive integer",
                SeverityLevel.Medium,
                "The membership number is a critical identifier for a patron. " +
                "Please provide a positive integer that correctly represents the patron's membership.");
        }
    }

    private static void ValidateContactDetails(string contactDetails)
    {
        if (string.IsNullOrWhiteSpace(contactDetails))
        {
            throw new PatronException(
                "Contact details cannot be null, empty, or whitespace.",
                SeverityLevel.Medium,
                "Accurate contact details are necessary for communication with the patron. " +
                "Ensure that the contact details are filled out and contain valid information.");
        }
    }
}