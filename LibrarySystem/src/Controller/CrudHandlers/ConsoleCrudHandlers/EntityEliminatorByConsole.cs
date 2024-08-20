namespace LibrarySystem;

public class EntityEliminatorByConsole<T> : IEntityEliminator<T, string> where T : EntityBase
{
    private IRepository<T> _repository;
    private IMessageRenderer _renderer;
    private EntitySelectorByConsole<T> _entitySelector;

    public EntityEliminatorByConsole(IRepository<T> repository, IMessageRenderer renderer, EntitySelectorByConsole<T> entitySelector)
    {
        _repository = repository;
        _renderer = renderer;
        _entitySelector = entitySelector;
    }

    public void TryToDeleteEntity()
    {
        var entities = _repository.GetAll();
        var entity = _entitySelector.TryToSelectAtLeastOne(entities);
        if (entity is not null)
        {
            var wasDeleted = _repository.Delete(entity);
            RenderDeleteStatus(wasDeleted);
        }

        if (entities.Any() && entity is null)
        {
            _renderer.RenderErrorMessage("try again...");
        }
    }

    private void RenderDeleteStatus(bool wasDeleted)
    {
        if (wasDeleted)
        {
            _renderer.RenderSuccessMessage("deleted");
        }
        else
        {
            _renderer.RenderErrorMessage("something bad happened. Please try again.");
        }
    }
}
