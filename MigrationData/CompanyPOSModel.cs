namespace MigrationData
{
	using System;
	using System.Data.Entity;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;

	public partial class CompanyPOSModel : DbContext
	{
		public CompanyPOSModel()
			: base("name=CompanyPOSModel")
		{
		}

		public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
		public virtual DbSet<Categories> Categories { get; set; }
		public virtual DbSet<Companies> Companies { get; set; }
		public virtual DbSet<Invoices> Invoices { get; set; }
		public virtual DbSet<ItemAttributes> ItemAttributes { get; set; }
		public virtual DbSet<ItemPagePositions> ItemPagePositions { get; set; }
		public virtual DbSet<ItemPurchases> ItemPurchases { get; set; }
		public virtual DbSet<Items> Items { get; set; }
		public virtual DbSet<MenuPages> MenuPages { get; set; }
		public virtual DbSet<Menus> Menus { get; set; }
		public virtual DbSet<Sales> Sales { get; set; }
		public virtual DbSet<Sessions> Sessions { get; set; }
		public virtual DbSet<Shifts> Shifts { get; set; }
		public virtual DbSet<Stores> Stores { get; set; }
		public virtual DbSet<UserActivities> UserActivities { get; set; }
		public virtual DbSet<Users> Users { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Categories>()
				.HasMany(e => e.Items)
				.WithOptional(e => e.Categories)
				.HasForeignKey(e => e.CategoryID);

			modelBuilder.Entity<Companies>()
				.HasMany(e => e.Stores)
				.WithRequired(e => e.Companies)
				.HasForeignKey(e => e.CompanyID);

			modelBuilder.Entity<Items>()
				.HasMany(e => e.ItemAttributes)
				.WithRequired(e => e.Items)
				.HasForeignKey(e => e.ItemID);

			modelBuilder.Entity<Items>()
				.HasMany(e => e.ItemPagePositions)
				.WithRequired(e => e.Items)
				.HasForeignKey(e => e.ItemID);

			modelBuilder.Entity<Sales>()
				.HasMany(e => e.Invoices)
				.WithRequired(e => e.Sales)
				.HasForeignKey(e => e.SaleID);

			modelBuilder.Entity<Sales>()
				.HasMany(e => e.ItemPurchases)
				.WithRequired(e => e.Sales)
				.HasForeignKey(e => e.SaleID);

			modelBuilder.Entity<Stores>()
				.HasMany(e => e.Categories)
				.WithRequired(e => e.Stores)
				.HasForeignKey(e => e.StoreID);

			modelBuilder.Entity<Stores>()
				.HasMany(e => e.Items)
				.WithRequired(e => e.Stores)
				.HasForeignKey(e => e.StoreID);

			modelBuilder.Entity<Stores>()
				.HasMany(e => e.Sales)
				.WithRequired(e => e.Stores)
				.HasForeignKey(e => e.StoreID);

			modelBuilder.Entity<Stores>()
				.HasMany(e => e.Shifts)
				.WithRequired(e => e.Stores)
				.HasForeignKey(e => e.StoreID);

			modelBuilder.Entity<Stores>()
				.HasMany(e => e.Users)
				.WithRequired(e => e.Stores)
				.HasForeignKey(e => e.StoreID);

			modelBuilder.Entity<Users>()
				.HasMany(e => e.UserActivities)
				.WithRequired(e => e.Users)
				.HasForeignKey(e => e.UserID);
		}
	}
}
