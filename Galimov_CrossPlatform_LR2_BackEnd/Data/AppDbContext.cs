using Microsoft.EntityFrameworkCore;
using Galimov_CrossPlatform_LR2_BackEnd.Models;
using System.Collections.Generic;

namespace Galimov_CrossPlatform_LR2_BackEnd.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Таблицы в базе данных
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
