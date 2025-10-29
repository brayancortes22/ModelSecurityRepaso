using ModelSecurityRepaso.Entity.Dto.Base;
    
namespace ModelSecurityRepaso.Entity.Dto
{
    public class UserDto : BaseDto
    {
        // solo se expone lo que le pido al usuario por medio de la interfaz gráfica
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}