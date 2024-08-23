namespace LibrarySystem;

public class SimpleFineFormatter : IEntityFormatter<Fine>
{
    private readonly Fine _entity;

    public SimpleFineFormatter(Fine entity)
    {
        _entity = entity;
    }

    public override string ToString()
    {
        return "Fine: " + _entity.FineAmount + "$ | "
                + (_entity.WasPayed ? "paid" : "active");
    }

    public Fine Entity
    {
        get => _entity;
    }
}