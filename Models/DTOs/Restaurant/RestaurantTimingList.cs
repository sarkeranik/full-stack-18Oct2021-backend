using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.DTOs.Restaurant
{
    public class RestaurantTimingList
    {
        private List<RestaurantTiming> Timings { get; set; }

        public RestaurantTimingList(List<RestaurantTiming> timings)
        {
            Timings = timings;
        }

        public bool CheckIsOpen(DayOfWeek weekDay, string time)
        {
            return Timings.Any(x => x.IsMatch(weekDay, TimeSpan.Parse(time)));
        }

    }
}
