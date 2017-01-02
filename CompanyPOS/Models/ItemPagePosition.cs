
namespace CompanyPOS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ItemPagePosition
    {
        public int ID { get; set; }
        public Nullable<int> hPos { get; set; }
        public Nullable<int> vPos { get; set; }
        public Nullable<int> Page { get; set; }

      // public int MenuID { get; set; }
      // [ForeignKey("MenuID")]
      // public virtual Menu Menu { get; set; }
      //
      // public int StoreID { get; set; }
      // [ForeignKey("StoreID")]
      //  public virtual Store Store { get; set; }
      //
       public int ItemID { get; set; }
      // [ForeignKey("ItemID")]
      // public virtual Item Item { get; set; }
    }
}
