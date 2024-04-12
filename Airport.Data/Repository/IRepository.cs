
namespace Airport.Data.Repository
{
    public interface IRepository<T> where T : class
    {
        T? Get(int id);
        IEnumerable<T> GetAll();
        Task <int> Add(T item);
        Task<int> Update(T item);
        void Delete(int id);
        Task DeleteAll();
    }
}
