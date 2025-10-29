using ModelSecurityRepaso.Entity.Model;
using ModelSecurityRepaso.Data.Interface.Base;

namespace ModelSecurityRepaso.Data.Interface
{
    /// <summary>
    /// Interfaz para operaciones con RefreshToken.
    /// Hereda de IBaseData para mantener los métodos CRUD estándar.
    /// </summary>
    public interface IRefreshTokenData : IBaseData<RefreshToken>
    {
        // Métodos personalizados pueden agregarse aquí si es necesario
    }
}
