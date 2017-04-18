namespace MigrationData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Sales
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sales()
        {
            Invoices = new HashSet<Invoices>();
            ItemPurchases = new HashSet<ItemPurchases>();
        }

        public int ID { get; set; }

        public DateTime? Date { get; set; }

        public string Title { get; set; }

        public decimal? DiscountAmount { get; set; }

        public decimal? DiscountRate { get; set; }

        public decimal? TaxAmunt { get; set; }

        public decimal? TaxRate { get; set; }

        public decimal? TotalPrice { get; set; }

        public decimal? SubtotalPrice { get; set; }

        public string Status { get; set; }

        public int UserID { get; set; }

        public int ShiftID { get; set; }

        public int StoreID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Invoices> Invoices { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ItemPurchases> ItemPurchases { get; set; }

        public virtual Stores Stores { get; set; }
    }
}
