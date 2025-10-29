using AutoMapper;
using ModelSecurityRepaso.Entity.Dto;
using ModelSecurityRepaso.Entity.Model;

namespace ModelSecurityRepaso.Utilities.Mappers.Profiles
{
    /// <summary>
    /// Perfil de mapeo para RoleFormPermission ↔ RolFormPermissionDto.
    /// </summary>
    public class RoleFormPermissionProfile : Profile
    {
        public RoleFormPermissionProfile()
        {
            CreateMap<RoleFormPermission, RolFormPermissionDto>()
                .ReverseMap();
        }
    }
}
