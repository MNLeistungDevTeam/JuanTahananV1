using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Infrastructure.Persistence
{
    public class HangFireDBContext : DbContext
    {

        public HangFireDBContext(DbContextOptions<HangFireDBContext> options) : base(options)
        {
        }

        public void InitializeDatabase()
        {
            Database.EnsureCreated();

            SaveChanges();
        }
    }
}
