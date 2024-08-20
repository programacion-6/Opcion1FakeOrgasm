namespace LibrarySystem;

public class EntityCreatorByConsole<T> : IEntityCreator<T, string> where T : EntityBase
{
    private IRepository<T> _repository;
    private IMessageRenderer _renderer;
    private IEntityRequester<T> _requester;

    public EntityCreatorByConsole(IRepository<T> repository, IEntityRequester<T> requester, IMessageRenderer renderer)
    {
        _repository = repository;
        _requester = requester;
        _renderer = renderer;
    }

    public void TryToCreateEntity()
    {
        _renderer.RenderIndicatorMessage("new");
        var newEntity = _requester.AskForEntity();
        if (newEntity is not null)
        {
            var wasSaved = _repository.Save(newEntity);
            RenderSaveStatus(wasSaved);
        }
        else
        {
            _renderer.RenderErrorMessage("please try again...");
        }
    }

    private void RenderSaveStatus(bool wasSaved)
    {
        if (wasSaved)
        {
            _renderer.RenderSuccessMessage("created");
        }
        else
        {
            _renderer.RenderErrorMessage("something bad happened. Please try again");
        }
    }
}
