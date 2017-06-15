namespace DATA.Models
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
		public string Tax { get; set; }

        public int ItemID { get; set; }
        //[ForeignKey("ItemID")]
        //public virtual Item Item { get; set; }
        //
        public int StoreID { get; set; }
        // [ForeignKey("StoreID")]
        //public virtual Store Store { get; set; }

		//Optional
		public int ProductID { get; set; }
    }
}
