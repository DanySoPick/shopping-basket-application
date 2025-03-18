using AutoMapper;
using shopping.basket.api.DTO;
using shopping.basket.core.Domain.ShoppingBasket.Models;

namespace shopping.basket.api.Profiler
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            // Define mappings
            CreateMap<BasketDTO, Transaction>()
                .ForMember(dest => dest.TransactionItems, opt => opt.MapFrom(src => src.Items))
                .ForMember(dest => dest.TransactionDiscounts, opt => opt.MapFrom(src => src.Discounts));

            CreateMap<SelectedItemsDTO, TransactionItem>();
            CreateMap<SelectedDiscountDTO, TransactionDiscount>();
            CreateMap<Transaction, TransactionDTO>();
            CreateMap<Discount, DiscountDTO>();
        }
    }
}
