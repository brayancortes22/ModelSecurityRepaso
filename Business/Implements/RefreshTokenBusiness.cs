using ModelSecurityRepaso.Entity.Model;
using ModelSecurityRepaso.Business.Implements.Base;
using ModelSecurityRepaso.Business.Interface;
using ModelSecurityRepaso.Data.Implements;

namespace ModelSecurityRepaso.Business.Implements
{
    /// <summary>
    /// Lógica de negocio para operaciones con RefreshToken.
    /// Valida reglas de negocio antes de persistir.
    /// </summary>
    public class RefreshTokenBusiness : BaseBusiness<RefreshToken>, IRefreshTokenBusiness
    {
        private readonly RefreshTokenData _refreshTokenData;

        /// <summary>
        /// Constructor con inyección de dependencias.
        /// </summary>
        public RefreshTokenBusiness(RefreshTokenData refreshTokenData) : base(refreshTokenData)
        {
            _refreshTokenData = refreshTokenData;
        }

        // Métodos de validación y lógica pueden agregarse aquí
    }
}
