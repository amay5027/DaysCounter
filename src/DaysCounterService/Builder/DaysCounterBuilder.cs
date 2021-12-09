using Nager.Date;


namespace DaysCounterService.Builder
{
    public class DaysCounterBuilder : IDaysCounterBuilder, ICountrySpecificDaysCounterBuilder
    {
        private DateTime? _startDate;
        private DateTime? _endDate;
        private IEnumerable<DateTime> _days;
        private string? _countryCode;
        private string? _iso3166_2_Code;

        public DaysCounterBuilder(DateTime startDate, DateTime endDate)
        {
            _startDate = startDate;
            _endDate = endDate;
            _days = startDate.GetAllDaysUntil(endDate);

        }

        public IDaysCounterBuilder ExcludeAllVacationDays(IEnumerable<DateRangeDto> vacationDates)
        {
            return ExcludeVacactionDates(vacationDates);
        }

        public int TotalDays()
        {
            return _days.Count();
        }

        public ICountrySpecificDaysCounterBuilder ForCountry(string countryCode, string iso3166_2_code = "")
        {
            _countryCode = countryCode;
            _iso3166_2_Code = iso3166_2_code;
            return this;
        }

        public ICountrySpecificDaysCounterBuilder ExcludePublicHolidays()
        {
            _days = _days.Where(i => !IsPublicHoliday(i, _countryCode, _iso3166_2_Code));
            return this;
        }

        public ICountrySpecificDaysCounterBuilder ExcludeVacationDaysThatFallOnBusinessDays(IEnumerable<DateRangeDto> vacationDates)
        {
            return ExcludeVacactionDates(vacationDates, _countryCode, _iso3166_2_Code, true);
        }

        public ICountrySpecificDaysCounterBuilder ExcludeWeekends()
        {

            _days = _days.Where(i => !IsWeekend(i, _countryCode));
            return this;
        }

        private DaysCounterBuilder ExcludeVacactionDates(IEnumerable<DateRangeDto> vacationDates, string? countryCode = null, string? iso3166_2_Code = null, bool includeVacationDatesThatAreBusinessWorkingDaysOnly = false)
        {
            var vacationDays = vacationDates
                                .SelectMany(i => i.StartDate.GetAllDaysUntil(i.EndDate))
                                .Where(i => i.Date >= _startDate && i.Date <= _endDate)
                                .Distinct();
            if (includeVacationDatesThatAreBusinessWorkingDaysOnly)
            {
                vacationDays = vacationDays.Where(i => IsWorkingDay(i.Date, countryCode, iso3166_2_Code));
            }
            _days = _days.Where(i => !vacationDays.Contains(i));
            return this;
        }

        private bool IsPublicHoliday(DateTime date, string countryCode, string? iso3166_2_Code = null)
        {
            if (!CountrySupported(countryCode))
            {
                return false; // most prudent to assume not a public holiday
            }

            if (string.IsNullOrEmpty(iso3166_2_Code))
            {
                // until we store sub (e.g England or Scotland for the UK), if none is provided then exclude any public holidays with an empty counties list
                // IsPublicHoliday in this instance will return true for any dates that DONT have state/sub-division variations
                return DateSystem.IsPublicHoliday(date, GetCountryCodeEnum(countryCode));
            }
            else
            {
                return DateSystem.IsPublicHoliday(date, GetCountryCodeEnum(countryCode), iso3166_2_Code);
            }
        }

        private bool IsWeekend(DateTime date, string countryCode)
        {
            if (CountrySupported(countryCode))
            {
                return DateSystem.IsWeekend(date, GetCountryCodeEnum(countryCode));

            }
            else
            {
                return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
            }
        }

        private bool IsWorkingDay(DateTime date, string countryCode, string? iso3166_2_Code)
        {
            return !IsWeekend(date, countryCode)
                   && !IsPublicHoliday(date, countryCode, iso3166_2_Code);
        }

        private CountryCode GetCountryCodeEnum(string countryCode)
        {
            return (CountryCode)Enum.Parse(typeof(CountryCode), countryCode);
        }

        private bool CountrySupported(string countryCode)
        {
            return Enum.TryParse(countryCode, out CountryCode code);
        }
       
    }
}
