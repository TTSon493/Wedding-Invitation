using AutoMapper;
using Wedding.Model.Domain;
using Wedding.Model.DTO;
using Wedding.Utility.Constants;

namespace Wedding.Service.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<UserInfoDTO, ApplicationUser>().ReverseMap();
        CreateMap<EmailTemplateDTO, EmailTemplateDTO>()
            .ForMember(dest => dest.TemplateName, opt
                => opt.MapFrom(src => src.TemplateName))
            .ForMember(dest => dest.SenderName, opt
                => opt.MapFrom(src => src.SenderName))
            .ForMember(dest => dest.SenderEmail, opt
                => opt.MapFrom(src => src.SenderEmail))
            .ForMember(dest => dest.Category, opt
                => opt.MapFrom(src => src.Category))
            .ForMember(dest => dest.SubjectLine, opt
                => opt.MapFrom(src => src.SubjectLine))
            .ForMember(dest => dest.PreHeaderText, opt
                => opt.MapFrom(src => src.PreHeaderText))
            .ForMember(dest => dest.PersonalizationTags, opt
                => opt.MapFrom(src => src.PersonalizationTags))
            .ForMember(dest => dest.BodyContent, opt
                => opt.MapFrom(src => src.BodyContent))
            .ForMember(dest => dest.FooterContent, opt
                => opt.MapFrom(src => src.FooterContent))
            .ForMember(dest => dest.CallToAction, opt
                => opt.MapFrom(src => src.CallToAction))
            .ForMember(dest => dest.Language, opt
                => opt.MapFrom(src => src.Language))
            .ForMember(dest => dest.RecipientType, opt
                => opt.MapFrom(src => src.RecipientType))
            .ReverseMap();

        CreateMap<Customer, CustomerInfoDTO>().ReverseMap();
        CreateMap<Customer, CustomerFullInfoDTO>()
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.ApplicationUser.FullName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.ApplicationUser.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.ApplicationUser.PhoneNumber))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.ApplicationUser.Gender))
            .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.ApplicationUser.BirthDate))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.ApplicationUser.Country))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.ApplicationUser.Address))
            .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.ApplicationUser.AvatarUrl))
            .ReverseMap();

        CreateMap<CartHeader, CartHeaderDTO>().ReverseMap();
        CreateMap<CartDetails, CartDetailsDTO>().ReverseMap();

        CreateMap<OrderHeader, GetOrderHeaderDTO>().ReverseMap();
        CreateMap<OrderDetails, GetOrderDetailsDTO>().ReverseMap();
        CreateMap<OrderStatus, GetOrdersStatusDTO>().ReverseMap();

        CreateMap<Transaction, GetTransactionDTO>().ReverseMap();
        CreateMap<Balance, GetBalanceDTO>().ReverseMap();

        CreateMap<Company, UpdateCompanyDTO>().ReverseMap();
        CreateMap<Privacy, CreatePrivacyDTO>().ReverseMap();
        CreateMap<Privacy, UpdatePrivacyDTO>().ReverseMap();
        CreateMap<TermOfUse, CreateTermOfUseDTO>().ReverseMap();
        CreateMap<TermOfUse, UpdateTermOfUseDTO>().ReverseMap();
    }
}
