using DaysCounterService;
using DaysCounterService.Builder;
using System;
using System.Collections.Generic;
using Xunit;

namespace DaysCounterTests
{
    public class DaysCounterTests
    {
        [Fact]
        public void GivenDaysCounter_WhenCalculatingTotalDaysInDecember_CalculatesCorrecly()
        {
            var startDate = new DateTime(2021, 12, 01);
            var endDate = new DateTime(2021, 12, 31);

            var count = DaysCounter
                                .Create(startDate, endDate)
                                .TotalDays();
            Assert.Equal(31, count);
        }

        [Fact]
        public void GivenDaysCounter_WhenCalculatingTotalDaysExcludingVacationDays_CalculatesCorrecly()
        {
            var startDate = new DateTime(2021, 12, 01);
            var endDate = new DateTime(2021, 12, 31);

            var count = DaysCounter
                                .Create(startDate, endDate)
                                .ExcludeAllVacationDays(new List<DateRangeDto>{new DateRangeDto(new DateTime(2021, 12, 10), new DateTime(2021, 12, 15))})
                                .TotalDays();
            Assert.Equal(25, count);
        }

        [Fact]
        public void GivenDaysCounter_WhenCalculatingTotalDaysExcludingPublicHolidaysButNotConsideringRegionalVariations_CalculatesCorrecly()
        {
            var startDate = new DateTime(2022, 01, 01);
            var endDate = new DateTime(2022, 01, 31);

            var count = DaysCounter
                                .Create(startDate, endDate)
                                .ForCountry("GB")
                                .ExcludePublicHolidays()
                                .TotalDays();
            Assert.Equal(31, count);
        }

        [Fact]
        public void GivenDaysCounter_WhenCalculatingTotalDaysExcludingPublicHolidaysConsideringRegionalVariations_CalculatesCorrecly()
        {
            var startDate = new DateTime(2022, 01, 01);
            var endDate = new DateTime(2022, 01, 31);

            var count = DaysCounter
                                .Create(startDate, endDate)
                                .ForCountry("GB", "GB-ENG")
                                .ExcludePublicHolidays()
                                .TotalDays();
            Assert.Equal(30, count);
        }

        [Fact]
        public void GivenDaysCounter_ExcludingWeekendsAndPublicHolidays_CalculatesCorrecly()
        {
            var startDate = new DateTime(2022, 01, 01);
            var endDate = new DateTime(2022, 01, 31);

            var count = DaysCounter
                                .Create(startDate, endDate)
                                .ForCountry("GB", "GB-ENG")
                                .ExcludePublicHolidays()
                                .ExcludeWeekends()
                                .TotalDays();
            Assert.Equal(20, count);
        }
    }
}