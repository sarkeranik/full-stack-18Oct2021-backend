using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models.DbEntities;

namespace Services.Interfaces
{
    public interface IRestaurantService
    {
        void InsertRestaurantDetails(RestaurantDetails restaurantDetails);
        void BulkInsertRestaurantDetails(List<RestaurantDetails> restaurantDetails);

        Task<RestaurantDetails> GetRestaurantByName(string name);
        Task<List<string>> GetTop200RestaurantsNames(int itemCount);

        Task<List<RestaurantDetails>> GetAllRestaurantDetailsByName(string name);
        Task<List<RestaurantDetails>> GetAllRestaurantDetailsByWeekday(DateTime? dateTime);
        Task<RestaurantDetails> GetAllRestaurantDetailsByWeekdayAndName(string name, DateTime dateTime);

        Task<List<RestaurantDetails>> GetRestaurantsAvailableWeekDays(string name);
    }
}
