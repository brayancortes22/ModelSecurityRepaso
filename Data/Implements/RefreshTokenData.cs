using ModelSecurityRepaso.Entity.Context;
using ModelSecurityRepaso.Entity.Model;
using ModelSecurityRepaso.Data.Implements.Base;
using ModelSecurityRepaso.Data.Interface;

namespace ModelSecurityRepaso.Data.Implements
{
    /// <summary>
    /// Repositorio específico para operaciones con RefreshToken.
    /// Maneja la persistencia de tokens de renovación.
    /// </summary>
    public class RefreshTokenData : BaseData<RefreshToken>, IRefreshTokenData
    {
        /// <summary>
        /// Constructor que recibe el contexto de BD por inyección.
        /// </summary>
        public RefreshTokenData(ApplicationDbContext context) : base(context) { }

        // Métodos personalizados pueden agregarse aquí
    }
}
