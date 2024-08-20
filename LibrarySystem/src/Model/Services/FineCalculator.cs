namespace LibrarySystem;

public static class FineCalculator
{
    private const int MIN_DAYS_OVERDUE = 0;
    private const double DAILY_FINE_RATE = 0.5;
    private const int WEEK_THRESHOLD = 7;
    private const int MONTH_THRESHOLD = 30;
    private const double WEEKLY_MULTIPLIER = 1.5;
    private const double MONTHLY_MULTIPLIER = 2;

    public static double GetFineAmountByDate(DateTime loanDate, DateTime returnDate)
    {
        var currentDate = DateTime.Now;
        var daysOverdue = (currentDate - returnDate).Days;

        if (daysOverdue == MIN_DAYS_OVERDUE)
        {
            var withoutFine = 0;
            return withoutFine;
        }

        var loanDays = (returnDate - loanDate).Days + 1;
        var loanMultiplier = GetMultiplierForLoanDays(loanDays);
        var fine = daysOverdue * DAILY_FINE_RATE * loanMultiplier;

        return fine;
    }

    private static double GetMultiplierForLoanDays(int totalDays)
    {
        double loanMultiplier = 1.0;
        if (totalDays > WEEK_THRESHOLD)
        {
            loanMultiplier *= WEEKLY_MULTIPLIER;
        }
        if (totalDays > MONTH_THRESHOLD)
        {
            loanMultiplier *= MONTHLY_MULTIPLIER;
        }

        return loanMultiplier;
    }
}
