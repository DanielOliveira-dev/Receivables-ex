using AutoMapper;
using Domain;
using Services.DTO;
using System.Globalization;

namespace ReceivablesAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ReceivableDTO, Debtor>()
             .ForMember(dest => dest.Reference, opt => opt.MapFrom(src => src.DebtorReference))
             .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.DebtorName))
             .ForMember(dest => dest.Address1, opt => opt.MapFrom(src => src.DebtorAddress1))
             .ForMember(dest => dest.Address2, opt => opt.MapFrom(src => src.DebtorAddress2))
             .ForMember(dest => dest.Town, opt => opt.MapFrom(src => src.DebtorTown))
             .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.DebtorState))
             .ForMember(dest => dest.Zip, opt => opt.MapFrom(src => src.DebtorZip))
             .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.DebtorCountryCode))
             .ForMember(dest => dest.RegistrationNumber, opt => opt.MapFrom(src => src.DebtorRegistrationNumber))
             .ForMember(dest => dest.Receivables, opt => opt.Ignore());

            CreateMap<ReceivableDTO, Receivable>()
                .ForMember(dest => dest.Reference, opt => opt.MapFrom(src => src.Reference))
                .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => Enum.Parse<CurrencyCode>(src.CurrencyCode)))
                .ForMember(dest => dest.IssueDate, opt => opt.MapFrom(src => ConvertToDate(src.IssueDate)))
                .ForMember(dest => dest.OpeningValue, opt => opt.MapFrom(src => src.OpeningValue))
                .ForMember(dest => dest.PaidValue, opt => opt.MapFrom(src => src.PaidValue))
                .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => ConvertToDate(src.DueDate)))
                .ForMember(dest => dest.ClosedDate, opt => opt.MapFrom(src => src.ClosedDate != null ? ConvertToDate(src.ClosedDate) : (DateTime?)null))
                .ForMember(dest => dest.Cancelled, opt => opt.MapFrom(src => src.Cancelled));
        }

        private static DateTime ConvertToDate(string source)
        {
            string[] dateFormats = { "dd/MM/yyyy", "dd-MM-yyyy" };

            if (DateTime.TryParseExact(source, dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                return result;
            }

            throw new InvalidOperationException("Invalid date format");
        }
    }
}
