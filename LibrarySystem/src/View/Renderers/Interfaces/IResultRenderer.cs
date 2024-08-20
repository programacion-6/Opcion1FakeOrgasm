namespace LibrarySystem;

public interface IResultRenderer<R>
{
    public void RenderResult(R? result);
    public void RenderResults(List<R> results);
    public void RenderResultWith<S>(R? result, S someElse);
}
