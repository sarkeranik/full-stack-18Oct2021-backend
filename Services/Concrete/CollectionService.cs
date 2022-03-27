using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Repos;
using Models.DbEntities;
using Models.Enums;
using Services.Interfaces;

namespace Services.Concrete
{
    public class CollectionService : ICollectionService
    {
        private readonly IGenericRepository<Collection> _collectionRepository;
        private readonly IRestaurantService _restaurantService;

        public CollectionService(IGenericRepository<Collection> collectionRepository, IRestaurantService restaurantService)
        {
            _collectionRepository = collectionRepository;
            _restaurantService = restaurantService;
        }

        public async Task AddRestaurantToCollection(string name, List<string> collectionList)
        {
            var existingCollection = await _collectionRepository
                .FindAllAsync(x => x.RestaurantName.Equals(name));
            foreach (var collection in existingCollection)
            {
                _collectionRepository.Delete(collection);
            }

            foreach (var favCollectionName in collectionList)
            {
                var collectionFound = Enum.TryParse(favCollectionName, out FavoriteCollectionsItems collectionsItem); // check if collection item exist
                if (!collectionFound)
                    throw new Exception("No Collection Item Found with the specified Id");

                await AddRestToCollection(name, (int)collectionsItem);
            }
        }

        public async Task AddRestToCollection(string name, int collectionId)
        {
            var restaurant = await _restaurantService.GetRestaurantByName(name);

            if (restaurant == null)
                throw new Exception("No Restaurant Found with the specified Name");

            var existingCollection = (await _collectionRepository.FindAllAsync(x => x.RestaurantName.Equals(name)
                && x.FavoriteCollectionId != null
                && x.FavoriteCollectionId == collectionId)).FirstOrDefault();
            if (existingCollection != null)
                return;

            _collectionRepository.Insert(new Collection
            {
                FavoriteCollectionId = collectionId,
                RestaurantId = restaurant.Id,
                RestaurantName = restaurant.Name
            });
        }
        public async Task RemoveCollectionItemForRestaurantAsync(string name, string collectionName)
        {
            var collectionFound = Enum.TryParse(collectionName, out FavoriteCollectionsItems collectionsItem); // check if collection item exist
            if (!collectionFound)
                throw new Exception("No Collection Item Found with the specified Id");

            var collectionId = (int)collectionsItem;

            var existingCollection = (await _collectionRepository.FindAllAsync(x => x.RestaurantName.Equals(name)
                && x.FavoriteCollectionId != null
                && x.FavoriteCollectionId == collectionId)).FirstOrDefault();
            if (existingCollection == null)
                return;
            _collectionRepository.Delete(existingCollection);
        }
        public async Task<List<Collection>> GetCollectionByRestaurantNameAsync(string restaurantName)
        {
            return await _collectionRepository.FindAllAsync(x => x.RestaurantName.Equals(restaurantName));
        }
        public List<Collection> GetAllFavCollectionItems()
        {
            return _collectionRepository.GetAll();
        }
    }
}
