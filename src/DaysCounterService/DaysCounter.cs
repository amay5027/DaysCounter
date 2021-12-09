using DaysCounterService.Builder;

namespace DaysCounterService
{
    public static class DaysCounter
    { 
       public static IDaysCounterBuilder Create(DateTime startDate, DateTime endDate)
        {
            return new DaysCounterBuilder(startDate, endDate);
        }
    }
}