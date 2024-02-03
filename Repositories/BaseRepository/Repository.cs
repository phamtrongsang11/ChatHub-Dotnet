using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TeamChat.Database;
using TeamChat.Models;

namespace TeamChat.Repositories.BaseRepository
{
    public class Repository<T> : IRepository<T>
        where T : BaseModel
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            dbSet = _db.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAll(
            string? includeProperties = null,
            int? cursor = null,
            int? page = null
        )
        {
            IQueryable<T> query = dbSet;
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (
                    var includeProp in includeProperties.Split(
                        new char[] { ',' },
                        StringSplitOptions.RemoveEmptyEntries
                    )
                )
                {
                    query = query.Include(includeProp);
                }
            }
            if (cursor != null && page != null)
            {
                int skip = (int)(cursor - 1);
                int take = (int)page;

                query.Skip((skip - 1) * take).Take(take);
            }

            var servers = await query.AsNoTracking().ToListAsync();
            servers.Reverse();
            return servers;
        }

        public async Task<T?> Get(
            Expression<Func<T, bool>> filter,
            string? includeProperties = null
        )
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (
                    var includeProp in includeProperties.Split(
                        new char[] { ',' },
                        StringSplitOptions.RemoveEmptyEntries
                    )
                )
                {
                    query = query.Include(includeProp);
                }
            }

            var server = await query.AsNoTracking().FirstOrDefaultAsync();

            return server;
        }

        public T Create(T entity)
        {
            entity.createdAt = DateTime.UtcNow;
            return dbSet.Add(entity).Entity;
        }

        public T Update(T entity)
        {
            entity.updatedAt = DateTime.UtcNow;
            return dbSet.Update(entity).Entity;
        }

        public Boolean Remove(T entity)
        {
            T result = dbSet.Remove(entity).Entity;

            if (result == null)
                return false;

            return true;
        }
    }
}
