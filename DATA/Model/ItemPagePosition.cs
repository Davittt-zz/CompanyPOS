
namespace DATA
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ItemPagePosition
    {
        public int ID { get; set; }
        public Nullable<int> hPos { get; set; }
        public Nullable<int> vPos { get; set; }
        public int StoreID { get; set; }
        public int ItemID { get; set; }
        public int MenuID { get; set; }
        public Nullable<int> Page { get; set; }

        //[ForeignKey("MenuID")]
        //public virtual Menu Menu { get; set; }

        //[ForeignKey("StoreID")]
        //public virtual Store Store { get; set; }

        //[ForeignKey("ItemID")]
        //public virtual Item Item { get; set; }
    }
}
