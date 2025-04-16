using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MovieReviewApp.Data;

namespace MovieReviewApp.IRepository.Repository
{
    public class Repository<T> : IRepository<T> where T : class       // where T can be reference type like class, interface, delegate
    {
        private readonly ApplicationDbContext _db;
        private readonly DbSet<T> _DbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            _DbSet = _db.Set<T>();
        }

        public async Task<T> Add(T Entity)
        {
            var data = _DbSet.Add(Entity);
            await _db.SaveChangesAsync();
            return data.Entity;
        }

        public async Task<T> DeleteById(int id)
        {
            if (id == 0)
            {
                return null;
            }
            var GetData = await _DbSet.FindAsync(id);
            if(GetData == null)
            {
                return null;
            }
            _DbSet.Remove(GetData);
            await _db.SaveChangesAsync();
            return GetData;

        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _DbSet.ToListAsync();  
        }

        public async Task<T> GetById(int id) { 
        
            var data =  await _DbSet.FindAsync(id);
            if (data == null)
            {
                return null;
            }
            return data;
        }

        public async Task<T> Update(T Entity)
        {
            if (Entity == null)
            {
                return null;
            }
            var UpdatedData =  _DbSet.Update(Entity);
            await _db.SaveChangesAsync();
            return UpdatedData.Entity;

        }
    }
}
