using Spectre.Console;

namespace LibrarySystem;

public class EntityRendererAsConsolePages<T> where T : EntityBase
{
    private readonly List<T> _results;
    private readonly IPaginable<T> _paginable;
    private readonly IEntityFormatterFactory<T> _formatterFactory;

    public EntityRendererAsConsolePages(IPaginable<T> paginable, IEntityFormatterFactory<T> formatterFactory)
    {
        _results = new List<T>();
        _paginable = paginable;
        _formatterFactory = formatterFactory;
    }

    public async Task RenderByPagination()
    {
        int pageSize = 1;
        int currentPage = 1;
        bool exit = false;

        while (!exit)
        {
            var results = await FetchPageResults(pageSize, currentPage);

            if (IsEndOfResults(results))
            {
                exit = true;
                break;
            }

            AddNewResultsToCollection(results);
            await RenderPage(results.ToList());
            exit = HandleUserChoice(ref currentPage);
        }
    }

    private async Task<IEnumerable<T>> FetchPageResults(int pageSize, int currentPage)
    {
        return await _paginable.GetByPage(pageSize, currentPage * pageSize);
    }

    private bool IsEndOfResults(IEnumerable<T> results)
    {
        return !results.Any() || results.Count() == _results.Count;
    }

    private void AddNewResultsToCollection(IEnumerable<T> results)
    {
        foreach (var result in results)
        {
            if (!_results.Any(r => r.Id == result.Id))
            {
                _results.Add(result);
            }
        }
    }

    private async Task RenderPage(List<T> results)
    {
        AnsiConsole.Clear();
        await RenderVerboseBooksFormatted(results);
    }

    private bool HandleUserChoice(ref int currentPage)
    {
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .AddChoices(new[] { "Next", "Stop" })
        );

        switch (choice)
        {
            case "Next":
                currentPage++;
                return false;
            case "Stop":
                return true;
            default:
                return false;
        }
    }

    private async Task RenderVerboseBooksFormatted(List<T> books)
    {
        var booksFormated = await Task.WhenAll(books.Select(async book =>
            await _formatterFactory.CreateVerboseFormatter(book)));

        ResultRenderer.RenderResults(booksFormated.ToList());
    }
}
