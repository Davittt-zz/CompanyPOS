
using DATA.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA
{
    public class CompanyPosDBContext : DbContext
    {
        public CompanyPosDBContext()
        { 
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;

			//Database.SetInitializer<CompanyPosDBContext>(null);
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemAttribute> ItemAttributes { get; set; }
        public DbSet<ItemPagePosition> ItemPagePositions { get; set; }
        public DbSet<ItemPurchase> ItemPurchases { get; set; }

		public DbSet<MenuPage> MenuPages { get; set; }
        public DbSet<Menu> Menues { get; set; }

        public DbSet<Sale> Sales { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserActivity> UserActivities { get; set; }
    }
}
