using ClothingStore.Core.Entities;
using ClothingStore.Core.Interfaces;
using ClothingStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.Infrastructure.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly DbSet<T> _entities;

        public BaseRepository(ClothingStoreContext context)
        {
            _entities = context.Set<T>();
        }
        public async Task<IEnumerable<T>> GetAll()
        {         
            return await _entities.ToListAsync();
        }        
        public async Task<T?> GetById(int id)
        {
            return await _entities.FindAsync(id);
        }
        public async Task Add(T entity)
        {
            await _entities.AddAsync(entity);
        }
        public void Update(T entity)
        {
            _entities.Update(entity);
        }
        public async Task Delete(int id)
        {
            T? entity = await GetById(id);
            if (entity != null) 
             _entities.Remove(entity);
        }
    }

}
