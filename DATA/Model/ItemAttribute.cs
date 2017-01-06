
namespace DATA
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ItemAttribute
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public Nullable<float> Price { get; set; }
        
        public Nullable<bool> Visible { get; set; }

        public int ItemID { get; set; }
        //[ForeignKey("ItemID")]
        //public virtual Item Item { get; set; }

        public int StoreID { get; set; }
        //[ForeignKey("StoreID")]
        //public Store Store { get; set; }
    }
}
