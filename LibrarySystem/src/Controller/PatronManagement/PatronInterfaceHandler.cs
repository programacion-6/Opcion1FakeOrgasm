namespace LibrarySystem.Controller.PatronManagement;

public class PatronInterfaceHandler
{
    private IEntityCreator<Patron, string> _patronCreator;
    private IEntityUpdater<Patron, string> _patronUpdater;
    private IEntityEliminator<Patron, string> _patronEliminator;

    public PatronInterfaceHandler(IEntityCreator<Patron, string> patronCreator,
        IEntityUpdater<Patron, string> patronUpdater, IEntityEliminator<Patron, string> patronEliminator)
    {
        _patronCreator = patronCreator;
        _patronUpdater = patronUpdater;
        _patronEliminator = patronEliminator;
    }


    public async Task HandleCreateEntity()
    {
        await _patronCreator.TryToCreateEntity();
    }

    public async Task HandleUpdateEntity()
    {
        await _patronUpdater.TryToUpdateEntity();
    }

    public async Task HandleDeleteEntity()
    {
        await _patronEliminator.TryToDeleteEntity();
    }
}
