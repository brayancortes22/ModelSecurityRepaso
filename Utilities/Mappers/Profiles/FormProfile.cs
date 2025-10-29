using AutoMapper;
using ModelSecurityRepaso.Entity.Dto;
using ModelSecurityRepaso.Entity.Model;

namespace ModelSecurityRepaso.Utilities.Mappers.Profiles
{
    /// <summary>
    /// Perfil de mapeo para Form ↔ FormDto.
    /// </summary>
    public class FormProfile : Profile
    {
        public FormProfile()
        {
            CreateMap<Form, FormDto>()
                .ReverseMap();
        }
    }
}
