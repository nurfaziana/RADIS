using System.Data.Entity;
using Oracle.ManagedDataAccess.EntityFramework;
using radisutm.Models;

namespace radisutm.DataContext
{
    public class RadisDataContext : DbContext
    {
        public RadisDataContext()
            : base("radisDbContext")
        {

        }
        public DbSet<UserModel> User { get; set; }


    }
}
