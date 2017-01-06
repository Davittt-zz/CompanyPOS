namespace DATA
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Invoice
    {
        public int ID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string PaymentMethod { get; set; }

        public int StoreID { get; set; }
        [ForeignKey("StoreID")]
        public virtual Store Store { get; set; }

        public int SaleID { get; set; }
        [ForeignKey("SaleID")]
        public virtual Sale Sale { get; set; }
    }
}
