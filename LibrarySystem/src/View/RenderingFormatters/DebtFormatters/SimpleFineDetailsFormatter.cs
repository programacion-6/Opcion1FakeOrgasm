namespace LibrarySystem;

public class SimpleFineDetailsFormatter : IEntityFormatter<Fine>
{
    public Fine? Entity { get; set; }

    public override string ToString()
    {
        if (Entity is null)
        {
            return string.Empty;
        }

        return "Fine: " + Entity.FineAmount + "$ | " 
                + (Entity.WasPayed ? "paid" : "active");
    }
}