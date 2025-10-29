using ModelSecurityRepaso.Entity.Model;
using ModelSecurityRepaso.Data.Interface.Base;

namespace ModelSecurityRepaso.Data.Interface
{
    /// <summary>
    /// Interfaz para operaciones con Module.
    /// Hereda de IBaseData para mantener los métodos CRUD estándar.
    /// </summary>
    public interface IModuleData : IBaseData<Module>
    {
        // Métodos personalizados pueden agregarse aquí si es necesario
    }
}
