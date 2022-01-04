using DaysCounterService.Builder;

namespace DaysCounterService
{
    public static class DaysCounter
    { 
       public static IDaysCounterBuilder Create(DateTime startDate, DateTime endDate)
       {
            return new DaysCounterBuilder(startDate, endDate);
       }

        public static bool AreOverlapping(this IEnumerable<DateRangeDto> periods)
        {
            return periods.Any(a => periods.Any(b => a != b && a.Overlap(b)));
        }

        public static bool Overlap(this DateRangeDto period1, DateRangeDto period2)
        {
            return period1.StartDate <= period2.EndDate
                         && period1.EndDate >= period2.StartDate;
        }


    }
}