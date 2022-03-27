using System;
using System.Collections.Generic;
using System.Text;
using CsvHelper.Configuration.Attributes;

namespace Models.ResponseModels
{
    public class RestaurantDetailsResponseModel
    {
        [Index(0)]
        public string Name { get; set; }

        [Index(1)]
        public string AvailableWeekPeriods { get; set; }
    }
}
