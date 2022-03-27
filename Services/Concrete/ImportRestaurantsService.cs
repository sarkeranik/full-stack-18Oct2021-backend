using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CsvHelper;
using Microsoft.Extensions.Logging;
using Models.DbEntities;
using Models.ResponseModels;
using Services.Interfaces;

namespace Services.Concrete
{
    public class ImportRestaurantsService : IImportRestaurantsService
    {
        #region Ctor
        private readonly IRestaurantService _restaurantService;
        private readonly ILogger<ImportRestaurantsService> _logger;

        public ImportRestaurantsService(IRestaurantService restaurantService, ILogger<ImportRestaurantsService> logger)
        {
            _restaurantService = restaurantService;
            _logger = logger;
        }
        #endregion

        #region Methods
        public async Task SyncAllRestaurantsFromCsvAsync()
        {
            await DownloadPagesAsync();
        } 
        #endregion

        #region Helpers
        private async Task DownloadPagesAsync()
        {
            for (var i = 1; i < 3; i++)
            {
                var pageToGet = $"https://gist.githubusercontent.com/seahyc/7ee4da8a3fb75a13739bdf5549172b1f/raw/f1c3084250b1cb263198e433ae36ba8d7a0d9ea9/hours.csv";


                using var client = new HttpClient();
                using var response = await client.GetAsync(pageToGet);
                using var content = response.Content;
                await using var stream = (MemoryStream)await content.ReadAsStreamAsync();
                using var reader = new StreamReader(stream);
                using var csv = new CsvReader(reader, CultureInfo.CurrentCulture);

                foreach (var restaurant in csv.GetRecords<RestaurantDetailsResponseModel>().ToList())
                {
                    _restaurantService.BulkInsertRestaurantDetails(PrepareRestaurantDetailsEntity(restaurant));
                }
            }
        }
        private List<RestaurantDetails> PrepareRestaurantDetailsEntity(RestaurantDetailsResponseModel responseModel)
        {
            var restaurantDetailsList = new List<RestaurantDetails>();

            var weekPeriods = responseModel.AvailableWeekPeriods.Split("/");//days to multiple Week days 
            foreach (var weekPeriod in weekPeriods)
            {

                try
                {
                    if (weekPeriod.Contains(","))
                    {
                        var weekDaysAndTime = SplitElementWithSkip(weekPeriod, ' ', 2);
                        var onlyDays = StringToDayOfWeekConvertedValue(weekDaysAndTime[0]);

                        #region Time
                        var onlyTimes = weekDaysAndTime[1].Split("-");
                        var openingTime = DateTime.Parse(onlyTimes[0]).ToString("HH:mm");
                        var closingTime = DateTime.Parse(onlyTimes[1]).ToString("HH:mm");
                        #endregion

                        var splitedOnlyDays = onlyDays.Split(",");//one week period another single week day
                        foreach (var splitedOnlyDay in splitedOnlyDays)
                        {
                            if (splitedOnlyDay.Contains("-"))
                            {
                                var splitedWeekDays = splitedOnlyDay.Split("-");
                                var betweenTwoDayOfWeeks = GetAllWeekDaysBetweenTwoDayOfWeeks(int.Parse(splitedWeekDays[0]),
                                    int.Parse(splitedWeekDays[1]));
                                restaurantDetailsList.AddRange(betweenTwoDayOfWeeks
                                    .Select(dayOfWeek => new RestaurantDetails
                                    {
                                        Name = responseModel.Name,
                                        DayOfWeeKId = (int)dayOfWeek,
                                        OpeningTime = openingTime,
                                        ClosingTime = closingTime,
                                        TimePeriod = weekPeriod
                                    }));
                            }
                            else
                            {
                                restaurantDetailsList.Add(new RestaurantDetails
                                {
                                    Name = responseModel.Name,
                                    DayOfWeeKId = int.Parse(splitedOnlyDay),
                                    OpeningTime = openingTime,
                                    ClosingTime = closingTime,
                                    TimePeriod = weekPeriod
                                });
                            }
                        }
                    }
                    else
                    {
                        var weekDaysAndTime = SplitElementWithSkip(weekPeriod, ' ', 1);
                        var onlyDays = StringToDayOfWeekConvertedValue(weekDaysAndTime[0]);
                        #region Time
                        var onlyTimes = weekDaysAndTime[1].Split("-");
                        var openingTime = DateTime.Parse(onlyTimes[0]).ToString("HH:mm");
                        var closingTime = DateTime.Parse(onlyTimes[1]).ToString("HH:mm");
                        #endregion

                        if (onlyDays.Contains("-"))
                        {
                            var splitedOnlyDays = onlyDays.Split("-");
                            var betweenTwoDayOfWeeks = GetAllWeekDaysBetweenTwoDayOfWeeks(int.Parse(splitedOnlyDays[0]),
                                int.Parse(splitedOnlyDays[1]));
                            restaurantDetailsList.AddRange(betweenTwoDayOfWeeks
                                .Select(dayOfWeek => new RestaurantDetails
                                {
                                    Name = responseModel.Name,
                                    DayOfWeeKId = (int)dayOfWeek,
                                    OpeningTime = openingTime,
                                    ClosingTime = closingTime,
                                    TimePeriod = weekPeriod
                                }));
                        }
                        else
                        {
                            restaurantDetailsList.Add(new RestaurantDetails
                            {
                                Name = responseModel.Name,
                                DayOfWeeKId = int.Parse(onlyDays),
                                OpeningTime = openingTime,
                                ClosingTime = closingTime,
                                TimePeriod = weekPeriod
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError((EventId)0, ex.Message, ex);
                    continue;
                }
            }

            return restaurantDetailsList;
        }
        private List<DayOfWeek> GetAllWeekDaysBetweenTwoDayOfWeeks(int openingWeekday, int closedWeekDay)
        {
            var dayOfWeeks = new List<DayOfWeek>();
            dayOfWeeks.Add((DayOfWeek)openingWeekday);
            while (true)
            {
                openingWeekday += 1;

                dayOfWeeks.Add((DayOfWeek)openingWeekday);
                if (openingWeekday == 7 && openingWeekday != closedWeekDay)
                    openingWeekday = 1;

                if (openingWeekday == closedWeekDay)
                    break;
            }

            return dayOfWeeks;
        }
        private string StringToDayOfWeekConvertedValue(string weekPeriod)
        {
            if (Regex.Match(weekPeriod, @"\bMon\b").Success)
                weekPeriod = Regex.Replace(weekPeriod, @"\bMon\b", "1");
            if (Regex.Match(weekPeriod, @"\bTues\b").Success)
                weekPeriod = Regex.Replace(weekPeriod, @"\bTues\b", "2");

            if (Regex.Match(weekPeriod, @"\bWeds\b").Success)
                weekPeriod = Regex.Replace(weekPeriod, @"\bWeds\b", "3");
            if (Regex.Match(weekPeriod, @"\bWed\b").Success)
                weekPeriod = Regex.Replace(weekPeriod, @"\bWed\b", "3");

            if (Regex.Match(weekPeriod, @"\bThurs\b").Success)
                weekPeriod = Regex.Replace(weekPeriod, @"\bThurs\b", "4");
            if (Regex.Match(weekPeriod, @"\bThu\b").Success)
                weekPeriod = Regex.Replace(weekPeriod, @"\bThu\b", "4");

            if (Regex.Match(weekPeriod, @"\bFri\b").Success)
                weekPeriod = Regex.Replace(weekPeriod, @"\bFri\b", "5");
            if (Regex.Match(weekPeriod, @"\bSat\b").Success)
                weekPeriod = Regex.Replace(weekPeriod, @"\bSat\b", "6");
            if (Regex.Match(weekPeriod, @"\bSun\b").Success)
                weekPeriod = Regex.Replace(weekPeriod, @"\bSun\b", "7");

            return weekPeriod;
        }
        private string[] SplitElementWithSkip(string input, char c, int skip)
        {
            var firstString = string.Empty;
            var secondString = string.Empty;

            var items = input.Replace(" - ", "-").Split(c).Where(x => (x != "")).ToArray();
            for (var i = 0; i < items.Length; i++)
            {
                if (i < skip)//2<=2
                {
                    firstString += items[i] + " ";
                }
                else
                {
                    secondString += items[i] + " ";
                }
            }

            return new string[] { firstString?.Trim(), secondString?.Trim() };
        } 
        #endregion
    }
}
