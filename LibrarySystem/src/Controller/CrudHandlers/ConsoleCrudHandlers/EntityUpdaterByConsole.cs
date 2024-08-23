namespace LibrarySystem;

public class EntityUpdaterByConsole<T> : IEntityUpdater<T, string> where T : EntityBase
{
    private IRepository<T> _repository;
    private IEntityRequester<T> _entityRequester;
    private IMessageRenderer _renderer;
    private EntitySelectorByConsole<T> _entitySelector;

    public EntityUpdaterByConsole(IRepository<T> repository, IEntityRequester<T> entityRequester, IMessageRenderer renderer, EntitySelectorByConsole<T> entitySelector)
    {
        _repository = repository;
        _entityRequester = entityRequester;
        _renderer = renderer;
        _entitySelector = entitySelector;
    }

    public async Task TryToUpdateEntity()
    {
        var entities = await _repository.GetAll();
        
        var entity = _entitySelector.TryToSelectAtLeastOne(entities.ToList());
        if (entity is not null)
        {
            var newEntityData = _entityRequester.AskForEntity();
            if (newEntityData is not null)
            {
                newEntityData.Id = entity.Id;
                var wasUpdated = await _repository.Update(newEntityData);
                RenderDeleteStatus(wasUpdated);
            }
        }

        if (entities.Any() && entity is null)
        {
            _renderer.RenderErrorMessage("try again...");
        }
    }

    private void RenderDeleteStatus(bool wasUpdated)
    {
        if (wasUpdated)
        {
            _renderer.RenderSuccessMessage("updated");
        }
        else
        {
            _renderer.RenderErrorMessage("something bad happened. Please try again.");
        }
    }
}
