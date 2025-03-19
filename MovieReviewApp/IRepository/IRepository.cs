namespace MovieReviewApp.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        Task<T> Add(T Entity);
        Task<T> Update(T Entity);
        Task<T> DeleteById(int id);

    }
}
