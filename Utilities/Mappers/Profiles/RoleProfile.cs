using AutoMapper;
using ModelSecurityRepaso.Entity.Dto;
using ModelSecurityRepaso.Entity.Model;
using ModelSecurityRepaso.Entity.Dto.Response;

namespace ModelSecurityRepaso.Utilities.Mappers.Profiles
{
    /// <summary>
    /// Perfil de mapeo para Role ↔ RoleDto ↔ RoleResponseDto.
    /// </summary>
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, RoleDto>()
                .ReverseMap();

            // Mapeo a Response DTO
            CreateMap<Role, RoleResponseDto>();
        }
    }
}
