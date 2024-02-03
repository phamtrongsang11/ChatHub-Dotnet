using System.Linq.Expressions;

namespace TeamChat.Repositories.BaseRepository
{
    public interface IRepository<T>
        where T : class
    {
        Task<IEnumerable<T>> GetAll(
            string? includeProperties = null,
            int? cursor = null,
            int? page = null
        );

        Task<T?> Get(Expression<Func<T, bool>> filter, string? includeProperties = null);

        T Update(T entity);

        T Create(T entity);

        Boolean Remove(T entity);
    }
}
