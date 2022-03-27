using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models.DbEntities;
using Models.Enums;

namespace Services.Interfaces
{
    public interface ICollectionService
    {
        Task AddRestaurantToCollection(string name, List<string> collectionList);
        Task AddRestToCollection(string name, int collectionsItem);
        Task RemoveCollectionItemForRestaurantAsync(string name, string collectionName);

        Task<List<Collection>> GetCollectionByRestaurantNameAsync(string restaurantName);
        List<Collection> GetAllFavCollectionItems();

    }
}
