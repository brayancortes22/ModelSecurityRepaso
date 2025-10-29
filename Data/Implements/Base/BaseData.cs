using ModelSecurityRepaso.Entity.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ModelSecurityRepaso.Data.Implements.Base
{
    /// <summary>
    /// Clase base genérica para operaciones CRUD sobre cualquier entidad.
    /// Permite reutilizar lógica de acceso a datos en los repositorios concretos.
    /// </summary>
    /// porque todas las entidades son clases se usa where T : class
    /// <typeparam name="T">Tipo de entidad sobre la cual se realizarán las operaciones CRUD.</typeparam>
    /// como por ejemplo User, Product, Order, etc.
    /// permitiendo hacer operaciones CRUD genéricas.
    /// y no tener que repetir el mismo código en cada repositorio.
    public class BaseData<T> where T : class
    {
        protected readonly ApplicationDbContext _context;

        public ApplicationDbContext Context => _context;

        public BaseData(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Busca una entidad por su identificador primario.
        /// </summary>
        /// <param name="id">Identificador de la entidad.</param>
        /// <returns>La entidad encontrada o null si no existe.</returns>
        public virtual async Task<T> GetByIdAsync(object id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        /// <summary>
        /// Obtiene todas las entidades del tipo T.
        /// </summary>
        /// <returns>Lista de todas las entidades.</returns>
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        /// <summary>
        /// Agrega una nueva entidad a la base de datos y guarda los cambios.
        /// </summary>
        /// <param name="entity">Entidad a agregar.</param>
        public virtual async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Actualiza una entidad existente en la base de datos y guarda los cambios.
        /// </summary>
        /// <param name="entity">Entidad a actualizar.</param>
        public virtual async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Elimina una entidad de la base de datos y guarda los cambios.
        /// </summary>
        /// <param name="entity">Entidad a eliminar.</param>
        public virtual async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Actualiza parcialmente una entidad (PATCH) aplicando solo los campos modificados.
        /// </summary>
        /// <param name="id">Identificador de la entidad.</param>
        /// <param name="patchValues">Acción que aplica los cambios sobre la entidad encontrada.</param>
        public virtual async Task<bool> PatchAsync(object id, Action<T> patchValues)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null)
                return false;

            patchValues(entity);
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Realiza un borrado lógico de una entidad, marcando una propiedad como eliminada en vez de eliminar físicamente.
        /// La entidad debe tener una propiedad booleana llamada "IsDeleted".
        /// </summary>
        /// <param name="id">Identificador de la entidad.</param>
        public virtual async Task<bool> DeleteLogicalAsync(object id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null)
                return false;

            var prop = entity.GetType().GetProperty("IsDeleted");
            if (prop == null)
                throw new InvalidOperationException("La entidad no tiene la propiedad IsDeleted.");

            prop.SetValue(entity, true);
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}