using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.UserName,
                opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.DocumentStatus,
                opt => opt.MapFrom(src => GetStatusDisplayName(src.Document.Status)))
            .ForMember(dest => dest.DocumentTypeName,
                opt => opt.MapFrom(src => src.Document.DocumentType.Name))
            .ForMember(dest => dest.Format,
                opt => opt.MapFrom(src => GetFormatDisplayName(src.Format)))
            .ReverseMap();

            CreateMap<CreateOrderRequest, Order>()
                .ForMember(dest => dest.CreatedDate,
                    opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.CompletedDate,
                    opt => opt.Ignore());
        }

        private string GetFormatDisplayName(DocumentFormat format)
        {
            return format switch
            {
                DocumentFormat.Electronic => "Цифровий",
                DocumentFormat.Paper => "Паперовий",
                _ => format.ToString()
            };
        }

        private string GetStatusDisplayName(DocumentStatus status)
        {
            return status switch
            {
                DocumentStatus.New => "Новий",
                DocumentStatus.InReview => "В обробці",
                DocumentStatus.Signed => "Підписано",
                DocumentStatus.Rejected => "Відхилено",
                DocumentStatus.Completed => "Завершено",
                _ => status.ToString()
            };
        }
    }
}
