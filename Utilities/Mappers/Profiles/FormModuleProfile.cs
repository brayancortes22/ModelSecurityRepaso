using AutoMapper;
using ModelSecurityRepaso.Entity.Dto;
using ModelSecurityRepaso.Entity.Model;

namespace ModelSecurityRepaso.Utilities.Mappers.Profiles
{
    /// <summary>
    /// Perfil de mapeo para FormModule ↔ FormModuleDto.
    /// </summary>
    public class FormModuleProfile : Profile
    {
        public FormModuleProfile()
        {
            CreateMap<FormModule, FormModuleDto>()
                .ReverseMap();
        }
    }
}
