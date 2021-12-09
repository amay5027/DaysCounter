namespace DaysCounterService
{
    public static class DateTimeExtensions
    {
       public static IEnumerable<DateTime> GetAllDaysUntil(this DateTime startDate, DateTime endDate)
        {
            return Enumerable.Range(0, (endDate - startDate).Days + 1).Select(d => startDate.AddDays(d));
        }
    }
}
