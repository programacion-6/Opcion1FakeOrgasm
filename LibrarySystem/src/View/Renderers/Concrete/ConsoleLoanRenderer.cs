namespace LibrarySystem;

public class ConsoleLoanRenderer : IResultRenderer<Loan>
{
    private readonly IBookRepository _bookRepository;
    private readonly IPatronRepository _patronRepository;

    public ConsoleLoanRenderer(IBookRepository bookRepository, IPatronRepository patronRepository)
    {
        _bookRepository = bookRepository;
        _patronRepository = patronRepository;
    }

    public void RenderResult(Loan? result)
    {
        if (result is not null)
        {
            var book = _bookRepository.GetById(result.IdBook);
            var patron = _patronRepository.GetById(result.IdPatron);

            Console.WriteLine($"{FormatLoan(result, book, patron)}\n");
        }
        else
        {
            var infoMessage = ConsoleFormatter.AsAnInfo("no loan found");
            Console.WriteLine(infoMessage);
        }
    }

    public void RenderResults(List<Loan> results)
    {
        if (results.Any())
        {
            int index = 0;
            foreach (var result in results)
            {
                var book = _bookRepository.GetById(result.IdBook);
                var patron = _patronRepository.GetById(result.IdPatron);
                Console.WriteLine($"{++index}. {FormatLoan(result, book, patron)}");
            }
        }
        else
        {
            var infoMessage = ConsoleFormatter.AsAnInfo("no loans found");
            Console.WriteLine(infoMessage);
        }
    }

    public void RenderResultWith<S>(Loan? result, S someElse)
    {
        if (result is not null)
        {
            var book = _bookRepository.GetById(result.IdBook);
            var patron = _patronRepository.GetById(result.IdPatron);
            Console.WriteLine($"{FormatLoan(result, book, patron)} : {someElse}");
        }
        else
        {
            var infoMessage = ConsoleFormatter.AsAnInfo("no loans found");
            Console.WriteLine(infoMessage);
        }
    }

    private string FormatLoan(Loan loan, Book book, Patron patron)
    {
        return $"Loan {(loan.WasReturn ? "returned" : "active")} | {loan.LoanDate} - {loan.ReturnDate}" +
               $"\n\tBook: {book.Title}" +
               $"\n\tPatron: {patron.Name}";
    }
}