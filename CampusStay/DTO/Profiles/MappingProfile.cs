using AutoMapper;
using CampusStay.DTO.Apartments;
using CampusStay.DTO.CartItem;
using CampusStay.DTO.Users;
using CampusStay.Models.ApartmentModels;
using CampusStay.Models.CartModels;
using CampusStay.Models.UserModels;

namespace CampusStay.DTO.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Apartment, ApartmentDto>().ReverseMap();
            CreateMap<ApartmentPhotos, ApartmentPhotoDto>().ReverseMap();
            CreateMap<CartItems, CartItemDto>().ReverseMap();
            CreateMap<Favorite, FavoriteDto>().ReverseMap();
            CreateMap<Like, LikeDto>().ReverseMap();
        }
    }

}
