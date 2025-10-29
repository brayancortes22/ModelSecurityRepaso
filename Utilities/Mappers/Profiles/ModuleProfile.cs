using AutoMapper;
using ModelSecurityRepaso.Entity.Dto;
using ModelSecurityRepaso.Entity.Model;
using ModelSecurityRepaso.Entity.Dto.Response;

namespace ModelSecurityRepaso.Utilities.Mappers.Profiles
{
    /// <summary>
    /// Perfil de mapeo para Module ↔ ModuleDto ↔ ModuleResponseDto.
    /// </summary>
    public class ModuleProfile : Profile
    {
        public ModuleProfile()
        {
            CreateMap<Module, ModuleDto>()
                .ReverseMap();

            // Mapeo a Response DTO
            CreateMap<Module, ModuleResponseDto>();
        }
    }
}
