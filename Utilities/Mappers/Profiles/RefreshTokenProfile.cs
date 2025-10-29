using AutoMapper;
using ModelSecurityRepaso.Entity.Dto;
using ModelSecurityRepaso.Entity.Model;
using ModelSecurityRepaso.Entity.Dto.Response;

namespace ModelSecurityRepaso.Utilities.Mappers.Profiles
{
    /// <summary>
    /// Perfil de mapeo para RefreshToken ↔ RefreshTokenDto ↔ RefreshTokenResponseDto.
    /// </summary>
    public class RefreshTokenProfile : Profile
    {
        public RefreshTokenProfile()
        {
            CreateMap<RefreshToken, RefreshTokenDto>()
                .ReverseMap();

            // Mapeo a Response DTO
            CreateMap<RefreshToken, RefreshTokenResponseDto>();
        }
    }
}
