namespace MigrationData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Invoices
    {
        public int ID { get; set; }

        public DateTime? Date { get; set; }

        public string PaymentMethod { get; set; }

        public int StoreID { get; set; }

        public int SaleID { get; set; }

        public virtual Sales Sales { get; set; }
    }
}
