namespace DaysCounterService.Builder
{
    public interface ITotalDaysCounterBuilder
    {
        int TotalDays();
        IDaysCounterBuilder ExcludeAllVacationDays(IEnumerable<DateRangeDto> vacationDates);

    }
}
