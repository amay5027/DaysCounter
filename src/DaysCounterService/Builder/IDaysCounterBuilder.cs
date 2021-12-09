namespace DaysCounterService.Builder
{
    public interface IDaysCounterBuilder : ITotalDaysCounterBuilder
    {
        ICountrySpecificDaysCounterBuilder ForCountry(string countryCode, string iso3166_2_code = "");
    }
}
