namespace LibrarySystem;

public interface IViewRenderer<I>
{
    void Render(I item);
    void RenderAsContainer(List<I> items);
}