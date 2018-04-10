using System.Data.Entity;
using System.Linq;
using radisutm.DataAccess.Interface;
using radisutm.DataContext;

namespace radisutm.DataAccess
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class, new()
    {
        private RadisDataContext radisDbContext;
        private DbSet<T> _innerDbSet;
        public BaseRepository()
        {
            radisDbContext = new RadisDataContext();
            _innerDbSet = radisDbContext.Set<T>();
        }
        public void Add(T model)
        {
            _innerDbSet.Add(model);
            Save();
        }
        public void Update(T model)
        {
            radisDbContext.Entry(model).State = EntityState.Modified;
            radisDbContext.Configuration.ValidateOnSaveEnabled = false;
            Save();
            radisDbContext.Configuration.ValidateOnSaveEnabled = true;
        }
        public void Remove(int id)
        {
            var entity = Find(id);
            _innerDbSet.Remove(entity);
            Save();
        }
        public IQueryable<T> All()
        {
            return radisDbContext.Set<T>();
        }
        public T Find(int id)
        {
            return radisDbContext.Set<T>().Find(id);
        }

        private void Save()
        {
            radisDbContext.SaveChanges();
        }
    }
}
