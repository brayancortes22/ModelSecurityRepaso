using AutoMapper;
using ModelSecurityRepaso.Entity.Dto;
using ModelSecurityRepaso.Entity.Model;
using ModelSecurityRepaso.Entity.Dto.Response;

namespace ModelSecurityRepaso.Utilities.Mappers.Profiles
{
    /// <summary>
    /// Perfil de mapeo para FormModule ↔ FormModuleDto ↔ FormModuleResponseDto.
    /// </summary>
    public class FormModuleProfile : Profile
    {
        public FormModuleProfile()
        {
            CreateMap<FormModule, FormModuleDto>()
                .ReverseMap();

            // Mapeo a Response DTO
            CreateMap<FormModule, FormModuleResponseDto>();
        }
    }
}
