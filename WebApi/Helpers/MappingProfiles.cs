using AutoMapper;
using Identity.Models;
using Models.DTOs.Account;
using System.Collections.Generic;
using Models.DbEntities;
using Models.DTOs.Restaurant;

namespace WebApi.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<ApplicationUser, UserDto>()
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.UserName))
                .ForMember(d => d.FirstName, o => o.MapFrom(s => s.FirstName))
                .ForMember(d => d.LastName, o => o.MapFrom(s => s.LastName))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.Email));
            CreateMap<RestaurantDetails, RestaurantDetailsModel>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.ClosingTime, o => o.MapFrom(s => s.ClosingTime))
                .ForMember(d => d.OpeningTime, o => o.MapFrom(s => s.OpeningTime))
                .ForMember(d => d.TimePeriod, o => o.MapFrom(s => s.TimePeriod));
            CreateMap<Collection, CollectionModel>()
                .ForMember(d => d.RestaurantName, o => o.MapFrom(s => s.RestaurantName))
                .ForMember(d => d.RestaurantId, o => o.MapFrom(s => s.RestaurantId));
        }
    }
}
