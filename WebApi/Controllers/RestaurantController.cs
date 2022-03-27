using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Models.DbEntities;
using Models.DTOs.Account;
using Models.DTOs.Restaurant;
using Models.Enums;
using Models.ResponseModels;
using Services.Interfaces;
using WebApi.Attributes;

namespace WebApi.Controllers
{
    //[Microsoft.AspNetCore.Authorization.Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;
        private readonly ICollectionService _collectionService;
        private readonly IImportRestaurantsService _importRestaurantsService;
        private readonly IMapper _mapper;

        public RestaurantController(IRestaurantService restaurantService, IMapper mapper, IImportRestaurantsService importRestaurantsService, ICollectionService collectionService)
        {
            _restaurantService = restaurantService;
            _mapper = mapper;
            _importRestaurantsService = importRestaurantsService;
            _collectionService = collectionService;
        }

        [Cached(999999999)]
        [HttpGet("GetInitialRestaurantData")]
        public async Task<IActionResult> GetInitialRestaurantData(int itemCount)
        {
            var restaurantNameList = await _restaurantService.GetTop200RestaurantsNames(itemCount);

            return Ok(new BaseResponse<string[]>(restaurantNameList.ToArray(), true, $"The Restaurant lists"));
        }
        [HttpGet("GetRestaurantByName")]
        public async Task<IActionResult> GetAllRestaurantByName(string name)
        {
            var restaurantDetailsList = (await _restaurantService.GetAllRestaurantDetailsByName(name)).ToList();
            //.GroupBy(item => item.Name)
            //.Select(item => item.First()).ToList();
            var data = _mapper
                .Map<IReadOnlyList<RestaurantDetails>, IReadOnlyList<RestaurantDetailsModel>>(restaurantDetailsList);

            var response = new BaseResponse<IReadOnlyList<RestaurantDetailsModel>>(data, true, $"Restaurant List");

            return Ok(response);
        }
        [HttpGet("GetRestaurantByDateTime")]
        public async Task<IActionResult> GetRestaurantByDateTime(DateTime? dateTime)
        {
            if (dateTime == null)
                throw new ArgumentNullException(nameof(dateTime));

            var restaurantDetailsList = (await _restaurantService.GetAllRestaurantDetailsByWeekday(dateTime))
                .GroupBy(item => item.Name)
                .Select(item => item.First()).ToList();

            var data = _mapper
                .Map<IReadOnlyList<RestaurantDetails>, IReadOnlyList<RestaurantDetailsModel>>(restaurantDetailsList);

            return Ok(new BaseResponse<IReadOnlyList<RestaurantDetailsModel>>(data, true, $"Restaurant List"));
        }
        [HttpGet("GetRestaurantByNameAndDatetime")]
        public async Task<IActionResult> GetRestaurantByNameAndDatetime(string name, DateTime? dateTime)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            if (dateTime == null)
                throw new ArgumentNullException(nameof(dateTime));

            //remove this after test
            //var restaurantDetailsList = await _restaurantService.GetTop200RestaurantsNames(2);
            //var restaurantDetailsModels = _mapper
            //    .Map<IReadOnlyList<RestaurantDetails>, IReadOnlyList<RestaurantDetailsModel>>(restaurantDetailsList);
            //return Ok(new BaseResponse<IReadOnlyList<RestaurantDetailsModel>>(restaurantDetailsModels, true, $"The Restaurant lists"));
            var restaurantDetailsList = (await _restaurantService.GetAllRestaurantDetailsByWeekdayAndName(name, dateTime.Value));
            if (restaurantDetailsList == null)
                return Ok(new BaseResponse<RestaurantDetailsModel>(null, true, $"Not available at specified date time"));
            var restaurantDetailsModel = _mapper.Map<RestaurantDetails, RestaurantDetailsModel>(restaurantDetailsList);

            return Ok(new BaseResponse<IReadOnlyList<RestaurantDetailsModel>>(new List<RestaurantDetailsModel> { restaurantDetailsModel }, true, $"Available In Following Time"));
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
        [HttpGet("SyncAllRestaurantsData")]
        public async Task<IActionResult> SyncAllRestaurantsData()
        {
            await _importRestaurantsService.SyncAllRestaurantsFromCsvAsync();

            return Ok(new BaseResponse<object>(null, true, $"Successfully restaurants added"));
        }
    }
}
