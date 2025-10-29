using AutoMapper;
using ModelSecurityRepaso.Entity.Dto;
using ModelSecurityRepaso.Entity.Model;
using ModelSecurityRepaso.Entity.Dto.Response;

namespace ModelSecurityRepaso.Utilities.Mappers.Profiles
{
    /// <summary>
    /// Perfil de mapeo para RoleFormPermission ↔ RolFormPermissionDto ↔ RoleFormPermissionResponseDto.
    /// </summary>
    public class RoleFormPermissionProfile : Profile
    {
        public RoleFormPermissionProfile()
        {
            CreateMap<RoleFormPermission, RolFormPermissionDto>()
                .ReverseMap();

            // Mapeo a Response DTO
            CreateMap<RoleFormPermission, RoleFormPermissionResponseDto>();
        }
    }
}
