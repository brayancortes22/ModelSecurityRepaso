using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ModelSecurityRepaso.Data.Interface.Base
{
    /// <summary>
    /// Interfaz genérica para operaciones CRUD y utilidades sobre cualquier entidad.
    /// 
    /// ¿Por qué se hace así?
    /// - Permite definir un contrato común para todos los repositorios, asegurando que todos implementen los mismos métodos básicos.
    /// - Facilita la reutilización y el mantenimiento del código, ya que cualquier clase que implemente esta interfaz tendrá una estructura estándar.
    /// - Hace posible el uso de inyección de dependencias y pruebas unitarias, ya que puedes programar contra la interfaz y no contra una implementación concreta.
    /// - Permite extender fácilmente la funcionalidad agregando nuevos métodos al contrato común.
    /// 
    /// Así, cualquier repositorio concreto (por ejemplo, UserData, ProductData) puede implementar esta interfaz y heredar estos métodos, evitando duplicar lógica y asegurando coherencia en el acceso a datos.
    /// </summary>
    /// <typeparam name="T">Tipo de entidad sobre la cual se realizarán las operaciones.</typeparam>
    public interface IBaseData<T> where T : class
    {
        /// <summary>
        /// Busca una entidad por su identificador primario.
        /// </summary>
        Task<T> GetByIdAsync(object id);

        /// <summary>
        /// Obtiene todas las entidades del tipo T.
        /// </summary>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Agrega una nueva entidad a la base de datos.
        /// </summary>
        Task AddAsync(T entity);

        /// <summary>
        /// Actualiza una entidad existente en la base de datos.
        /// </summary>
        Task UpdateAsync(T entity);

        /// <summary>
        /// Elimina una entidad de la base de datos.
        /// </summary>
        Task DeleteAsync(T entity);

        /// <summary>
        /// Actualiza parcialmente una entidad (PATCH) aplicando solo los campos modificados.
        /// </summary>
        Task<bool> PatchAsync(object id, Action<T> patchValues);

        /// <summary>
        /// Realiza un borrado lógico de una entidad, marcando una propiedad como eliminada en vez de eliminar físicamente.
        /// </summary>
        Task<bool> DeleteLogicalAsync(object id);
    }
}
