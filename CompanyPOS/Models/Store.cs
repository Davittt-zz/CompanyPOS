namespace CompanyPOS.Models
{
    using System;
    using System.Collections.Generic;

    public class Store
    {
        //public Store()
        //{
        //    this.UserActivity = new HashSet<UserActivity>();
        //    this.Users = new HashSet<User>();
        //    this.Menu = new HashSet<Menu>();
        //    this.ItemPagePosition = new HashSet<ItemPagePosition>();
        //    this.Item = new HashSet<Item>();
        //    this.Category = new HashSet<Category>();
        //    this.ItemAttribute = new HashSet<ItemAttribute>();
        //    this.Shift = new HashSet<Shift>();
        //    this.ItemPurchase = new HashSet<ItemPurchase>();
        //    this.Invoice = new HashSet<Invoice>();
        //    this.Sale = new HashSet<Sale>();
        //}

        public int ID { get; set; }
        public string Name { get; set; }

        public int CompanyID { get; set; }
       // public virtual Company Company { get; set; }
        //public virtual ICollection<Menu> Menues { get; set; }
        //public virtual ICollection<ItemPagePosition> ItemPagePositions { get; set; }
    
        //public virtual ICollection<Shift> Shifts { get; set; }
        //public virtual ICollection<ItemPurchase> ItemPurchases { get; set; }
        //public virtual ICollection<Invoice> Invoices { get; set; }
        
        //public virtual ICollection<UserActivity> UserActivities { get; set; }
        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<ItemAttribute> ItemAttributes { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
        

    }
}
