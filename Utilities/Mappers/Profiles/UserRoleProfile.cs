using AutoMapper;
using ModelSecurityRepaso.Entity.Dto;
using ModelSecurityRepaso.Entity.Model;
using ModelSecurityRepaso.Entity.Dto.Response;

namespace ModelSecurityRepaso.Utilities.Mappers.Profiles
{
    /// <summary>
    /// Perfil de mapeo para UserRole ↔ UserRolDto ↔ UserRoleResponseDto.
    /// </summary>
    public class UserRoleProfile : Profile
    {
        public UserRoleProfile()
        {
            CreateMap<UserRole, UserRolDto>()
                .ReverseMap();

            // Mapeo a Response DTO
            CreateMap<UserRole, UserRoleResponseDto>();
        }
    }
}
