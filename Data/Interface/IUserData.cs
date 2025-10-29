using ModelSecurityRepaso.Entity.Model;
using ModelSecurityRepaso.Data.Interface.Base;

namespace ModelSecurityRepaso.Data.Interface
{
    /// <summary>
    /// Interfaz para operaciones con User.
    /// Hereda de IBaseData para mantener los métodos CRUD estándar.
    /// </summary>
    public interface IUserData : IBaseData<User>
    {
        // Métodos personalizados pueden agregarse aquí si es necesario
    }
}
