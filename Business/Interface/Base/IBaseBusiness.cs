using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interface.BaseBusiness
{
    /// <summary>
    /// Interfaz genérica para la lógica de negocio sobre cualquier entidad.
    /// Define los métodos estándar que deben implementar las clases de negocio.
    /// 
    /// ¿Por qué se hace así?
    /// - Permite definir un contrato común para todas las capas de negocio.
    /// - Facilita la reutilización, el mantenimiento y las pruebas unitarias.
    /// - Permite extender la funcionalidad agregando nuevos métodos al contrato común.
    /// </summary>
    /// <typeparam name="T">Tipo de entidad sobre la cual se realizará la lógica de negocio.</typeparam>
    public interface IBaseBusiness<T> where T : class
    {
        /// <summary>
        /// Obtiene una entidad por su identificador.
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

        /// <summary>
        /// Ejecuta una operación de negocio dentro de una transacción atómica.
        /// </summary>
        Task ExecuteInTransactionAsync(Func<Task> operation);

        /// <summary>
        /// Ejecuta una operación de negocio dentro de una transacción atómica y devuelve un resultado.
        /// </summary>
        Task<TResult> ExecuteInTransactionAsync<TResult>(Func<Task<TResult>> operation);
    }
}