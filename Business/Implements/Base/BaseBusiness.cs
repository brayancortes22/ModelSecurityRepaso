using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace Business.Implements.BaseBusiness
{
    /// <summary>
    /// Clase base genérica para la lógica de negocio sobre cualquier entidad.
    /// Permite reutilizar lógica de negocio en las capas concretas.
    /// Incluye soporte para transacciones atómicas.
    /// </summary>
    /// <typeparam name="T">Tipo de entidad sobre la cual se realizará la lógica de negocio.</typeparam>
    public class BaseBusiness<T> where T : class
    {
        protected readonly Data.Implements.Base.BaseData<T> _data;

        public BaseBusiness(Data.Implements.Base.BaseData<T> data)
        {
            _data = data;
        }

        /// <summary>
        /// Obtiene una entidad por su identificador.
        /// </summary>
        public virtual async Task<T> GetByIdAsync(object id)
        {
            // Aquí podrías agregar validaciones o reglas de negocio antes de obtener la entidad
            if (id == null)
                throw new ArgumentNullException(nameof(id), "El identificador no puede ser nulo.");

            return await _data.GetByIdAsync(id);
        }

        /// <summary>
        /// Obtiene todas las entidades del tipo T.
        /// </summary>
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            // Aquí podrías filtrar, ordenar o aplicar reglas de negocio antes de devolver los datos
            return await _data.GetAllAsync();
        }

        /// <summary>
        /// Agrega una nueva entidad a la base de datos.
        /// </summary>
        public virtual async Task AddAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "La entidad no puede ser nula.");

            // Aquí podrías validar reglas de negocio antes de agregar
            await _data.AddAsync(entity);
        }

        /// <summary>
        /// Actualiza una entidad existente en la base de datos.
        /// </summary>
        public virtual async Task UpdateAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "La entidad no puede ser nula.");

            // Aquí podrías validar reglas de negocio antes de actualizar
            await _data.UpdateAsync(entity);
        }

        /// <summary>
        /// Elimina una entidad de la base de datos.
        /// </summary>
        public virtual async Task DeleteAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "La entidad no puede ser nula.");

            // Aquí podrías validar reglas de negocio antes de eliminar
            await _data.DeleteAsync(entity);
        }

        /// <summary>
        /// Actualiza parcialmente una entidad (PATCH) aplicando solo los campos modificados.
        /// </summary>
        public virtual async Task<bool> PatchAsync(object id, Action<T> patchValues)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id), "El identificador no puede ser nulo.");
            if (patchValues == null)
                throw new ArgumentNullException(nameof(patchValues), "La acción de patch no puede ser nula.");

            // Aquí podrías validar reglas de negocio antes de aplicar el patch
            return await _data.PatchAsync(id, patchValues);
        }

        /// <summary>
        /// Realiza un borrado lógico de una entidad, marcando una propiedad como eliminada en vez de eliminar físicamente.
        /// </summary>
        public virtual async Task<bool> DeleteLogicalAsync(object id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id), "El identificador no puede ser nulo.");

            // Aquí podrías validar reglas de negocio antes de borrar lógicamente
            return await _data.DeleteLogicalAsync(id);
        }

        /// <summary>
        /// Ejecuta una operación de negocio dentro de una transacción atómica.
        /// Si ocurre una excepción, la transacción se revierte.
        /// </summary>
        /// <param name="operation">Función asíncrona que contiene la lógica a ejecutar dentro de la transacción.</param>
        public async Task ExecuteInTransactionAsync(Func<Task> operation)
        {
            var context = _data._context;
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    await operation();
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        /// <summary>
        /// Ejecuta una operación de negocio dentro de una transacción atómica y devuelve un resultado.
        /// Si ocurre una excepción, la transacción se revierte.
        /// </summary>
        /// <typeparam name="TResult">Tipo del resultado a devolver.</typeparam>
        /// <param name="operation">Función asíncrona que contiene la lógica a ejecutar dentro de la transacción.</param>
        /// <returns>El resultado de la operación.</returns>
        public async Task<TResult> ExecuteInTransactionAsync<TResult>(Func<Task<TResult>> operation)
        {
            var context = _data._context;
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var result = await operation();
                    await transaction.CommitAsync();
                    return result;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}