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
            var results = await _paginable.GetByPage(pageSize, currentPage * pageSize);

            if (!results.Any() || results.Count() == _results.Count())
            {
                exit = true;
                break;
            }

            AnsiConsole.Clear();
            foreach (var result in results)
            {
                if (!_results.Any(r => r.Id == result.Id))
                {
                    _results.Add(result);
                }
            }

            await RenderVerboseBooksFormatted(results.ToList());

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .AddChoices(["Next", "Stop"])
            );

            switch (choice)
            {
                case "Next":
                    currentPage++;
                    break;

                case "Stop":
                    exit = true;
                    break;
            }
        }
    }

    private async Task RenderVerboseBooksFormatted(List<T> books)
    {
        var booksFormated = await Task.WhenAll(books.Select(async book =>
                                    await _formatterFactory
                                    .CreateVerboseFormatter(book)));

        ResultRenderer.RenderResults(booksFormated.ToList());
    }
}
