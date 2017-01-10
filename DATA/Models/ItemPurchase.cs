
namespace DATA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ItemPurchase
    {
        public int ID { get; set; }
        public int Quantity { get; set; }
        public string Discount { get; set; }
        public string TotalPrice { get; set; }

        public int ItemID { get; set; }
        //  [ForeignKey("ItemID")]
        //  public virtual Item Item { get; set; }
        //
        public int StoreID { get; set; }
        //  [ForeignKey("StoreID")]
        //  public virtual Store Store { get; set; }
        //
        public int SaleID { get; set; }
        //  [ForeignKey("SaleID")]
        //  public virtual Sale Sale { get; set; }
    }
}
