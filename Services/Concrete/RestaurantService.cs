using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Contexts;
using Data.Repos;
using Microsoft.EntityFrameworkCore;
using Models.DbEntities;
using Models.DTOs.Restaurant;
using Models.Enums;
using Services.Interfaces;

namespace Services.Concrete
{
    public class RestaurantService : IRestaurantService
    {
        #region Ctor
        private readonly IGenericRepository<RestaurantDetails> _restaurantRepository;
        private readonly ApplicationDbContext _context;

        public RestaurantService(IGenericRepository<RestaurantDetails> restaurantRepository, ApplicationDbContext context = null)
        {
            _restaurantRepository = restaurantRepository;
            _context = context;
        }
        #endregion

        #region CRUD
        public void InsertRestaurantDetails(RestaurantDetails restaurantDetails)
        {
            _restaurantRepository.Insert(restaurantDetails);
        }
        public void BulkInsertRestaurantDetails(List<RestaurantDetails> restaurantDetails)
        {
            _restaurantRepository.BulkInsert(restaurantDetails);
        }

        public async Task<RestaurantDetails> GetRestaurantByName(string name)
        {
            var res = (await _restaurantRepository.FindAllAsync(item => item.Name == name)).FirstOrDefault();
            return res;
        }
        #endregion

        #region Methods
        public async Task<List<string>> GetTop200RestaurantsNames(int itemCount)
        {
            return await _restaurantRepository.GetAllIQueryable().Select(x => x.Name).Distinct().Take(itemCount).ToListAsync();
        }
        public async Task<List<RestaurantDetails>> GetAllRestaurantDetailsByName(string name)
        {
            return await _restaurantRepository.FindAllAsync(item => item.Name.Contains(name));
        }
        public async Task<List<RestaurantDetails>> GetAllRestaurantDetailsByWeekday(DateTime? dateTime)
        {
            throw new NotImplementedException();
        }
        public async Task<RestaurantDetails> GetAllRestaurantDetailsByWeekdayAndName(string name, DateTime dateTime)
        {
            var resAvailWeekDays = await GetRestaurantsAvailableWeekDays(name);

            if (!resAvailWeekDays.Any())
                return null;

            var restaurantTiming = resAvailWeekDays.Select(res => new RestaurantTiming(
                res.Id,
                (DayOfWeek)res.DayOfWeeKId,
                res.OpeningTime,
                res.ClosingTime)).ToList();
            var restaurantTimingList = new RestaurantTimingList(restaurantTiming);

            var isOpen = restaurantTimingList.CheckIsOpen(dateTime.DayOfWeek, dateTime.ToString("HH:mm"));

            return isOpen ? resAvailWeekDays.First(item => item.Name.Equals(name) && item.DayOfWeeKId == (int)dateTime.DayOfWeek) : null;
        }
        public async Task<List<RestaurantDetails>> GetRestaurantsAvailableWeekDays(string name)
        {
            return await _restaurantRepository.FindAllAsync(item => item.Name.Equals(name));
        }
        #endregion
    }
}
