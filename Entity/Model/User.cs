using ModelSecurityRepaso.Entity.Model.Base;

namespace ModelSecurityRepaso.Entity.Model
{
    public class User : BaseModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        // Clave foránea para Person
        public int PersonId { get; set; }
        public virtual Person Person { get; set; }

        // Navegaciones
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}