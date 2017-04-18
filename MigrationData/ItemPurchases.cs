namespace MigrationData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ItemPurchases
    {
        public int ID { get; set; }

        public int Quantity { get; set; }

        public string Discount { get; set; }

        public string TotalPrice { get; set; }

        public int ItemID { get; set; }

        public int StoreID { get; set; }

        public int SaleID { get; set; }

        public virtual Sales Sales { get; set; }
    }
}
