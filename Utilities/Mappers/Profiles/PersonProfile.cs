using AutoMapper;
using ModelSecurityRepaso.Entity.Dto;
using ModelSecurityRepaso.Entity.Model;
using ModelSecurityRepaso.Entity.Dto.Response;

namespace ModelSecurityRepaso.Utilities.Mappers.Profiles
{
    /// <summary>
    /// Perfil de mapeo para Person ↔ PersonDto ↔ PersonResponseDto.
    /// </summary>
    public class PersonProfile : Profile
    {
        public PersonProfile()
        {
            CreateMap<Person, PersonDto>()
                .ReverseMap();

            // Mapeo a Response DTO
            CreateMap<Person, PersonResponseDto>();
        }
    }
}
