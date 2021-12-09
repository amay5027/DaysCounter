namespace DaysCounterService.Builder
{
    public interface ICountrySpecificDaysCounterBuilder : ITotalDaysCounterBuilder
    {
        ICountrySpecificDaysCounterBuilder ExcludePublicHolidays();
        ICountrySpecificDaysCounterBuilder ExcludeVacationDaysThatFallOnBusinessDays(IEnumerable<DateRangeDto> vacationDates);
        ICountrySpecificDaysCounterBuilder ExcludeWeekends();
    }
}
