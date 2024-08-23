namespace LibrarySystem;

public class SimplePatronDetailsFormatter : IEntityFormatter<Patron>
{
    public Patron? Entity { get; set; }

    public override string ToString()
    {
        if (Entity is null) {
            return string.Empty;
        }

        return Entity.Name;
    }
}
