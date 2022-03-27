using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Models.DbEntities;
using Models.DTOs.Restaurant;
using Models.Enums;
using Models.ResponseModels;
using Services.Interfaces;
using WebApi.Attributes;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectionController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;
        private readonly ICollectionService _collectionService;
        private readonly IImportRestaurantsService _importRestaurantsService;
        private readonly IMapper _mapper;

        public CollectionController(IRestaurantService restaurantService, IMapper mapper, IImportRestaurantsService importRestaurantsService, ICollectionService collectionService)
        {
            _restaurantService = restaurantService;
            _mapper = mapper;
            _importRestaurantsService = importRestaurantsService;
            _collectionService = collectionService;
        }

        [HttpPost("AddRestaurantToCollection")]
        public async Task<IActionResult> AddRestaurantToCollection(AddRestaurantToCollectionModel restaurantToCollectionModel)
        {
            if (string.IsNullOrEmpty(restaurantToCollectionModel.Name))
                throw new ArgumentNullException(nameof(restaurantToCollectionModel.Name));
            if (restaurantToCollectionModel.CollectionList == null
                || !restaurantToCollectionModel.CollectionList.Any())
                throw new ArgumentNullException(nameof(restaurantToCollectionModel.CollectionList));

            await _collectionService.AddRestaurantToCollection(restaurantToCollectionModel.Name, restaurantToCollectionModel.CollectionList);
            return Ok(new BaseResponse<List<string>>(restaurantToCollectionModel.CollectionList, true, $"Successfully added"));
        }
        [HttpPost("RemoveCollectionItemForRestaurant")]
        public async Task<IActionResult> RemoveCollectionItemForRestaurant(string name, string collection)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrEmpty(collection))
                throw new ArgumentNullException(nameof(collection));

            await _collectionService.RemoveCollectionItemForRestaurantAsync(name, collection);
            return Ok(new BaseResponse<string>(collection, true, $"Successfully removed"));
        }

        [HttpGet("GetCollectionByRestaurantName")]
        public async Task<IActionResult> GetCollectionByRestaurantName(string restaurantName)
        {
            var collectionList = await _collectionService.GetCollectionByRestaurantNameAsync(restaurantName);
            var collectionModel = new List<string>();
            if (collectionList?.Any() ?? false)
            {
                collectionModel = (from collection in collectionList
                                   where collection.FavoriteCollectionId != null
                                   select new CollectionModel
                                   {
                                       RestaurantName = collection.RestaurantName,
                                       RestaurantId = collection.RestaurantId,
                                       FavoriteCollectionName = ((FavoriteCollectionsItems)collection.FavoriteCollectionId).ToString()
                                   }).Select(x => x.FavoriteCollectionName).ToList();
            }

            return Ok(new BaseResponse<List<string>>(collectionModel, true, $"Collection List"));
        }

        [HttpGet("GetAllFavCollectionItems")]
        public IActionResult GetAllFavCollectionItems()
        {
            var collectionList = _collectionService.GetAllFavCollectionItems();
            var collectionModel = new List<CollectionModel>();
            if (collectionList?.Any() ?? false)
            {
                collectionModel = (from collection in collectionList
                                   where collection.FavoriteCollectionId != null
                                   select new CollectionModel
                                   {
                                       RestaurantName = collection.RestaurantName,
                                       RestaurantId = collection.RestaurantId,
                                       FavoriteCollectionName = ((FavoriteCollectionsItems)collection.FavoriteCollectionId).ToString()
                                   }).OrderBy(x=>x.RestaurantName).ToList();
            }

            return Ok(new BaseResponse<List<CollectionModel>>(collectionModel, true, $"Collection List"));
        }

    }
}
