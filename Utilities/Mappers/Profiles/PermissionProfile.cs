using AutoMapper;
using ModelSecurityRepaso.Entity.Dto;
using ModelSecurityRepaso.Entity.Model;
using ModelSecurityRepaso.Entity.Dto.Response;

namespace ModelSecurityRepaso.Utilities.Mappers.Profiles
{
    /// <summary>
    /// Perfil de mapeo para Permission ↔ PermissionDto ↔ PermissionResponseDto.
    /// </summary>
    public class PermissionProfile : Profile
    {
        public PermissionProfile()
        {
            CreateMap<Permission, PermissionDto>()
                .ReverseMap();

            // Mapeo a Response DTO
            CreateMap<Permission, PermissionResponseDto>();
        }
    }
}
