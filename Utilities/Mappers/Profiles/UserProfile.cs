using AutoMapper;
using ModelSecurityRepaso.Entity.Model;
using ModelSecurityRepaso.Entity.Model.Base;
using ModelSecurityRepaso.Entity.Dto;
using ModelSecurityRepaso.Entity.Dto.Base;

namespace ModelSecurityRepaso.Utilities.Mappers.Profiles
{
    /// <summary>
    /// Perfil de AutoMapper para mapear entre User y UserDto en ambos sentidos.
    /// </summary>
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // Mapeo de User a UserDto y viceversa (ReverseMap)
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}