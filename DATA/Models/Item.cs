namespace DATA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Item
    {
        public int ID { get; set; }
		public string Name { get; set; }
        public Nullable<float> UnitPrice { get; set; }
        public string Description { get; set; }
		public bool ActiveForSale { get; set; }
		public string Color { get; set; } 
		public string Tax {get;set;}

		public bool On { get; set; }

        public int StoreID { get; set; }
        [ForeignKey("StoreID")]
        public virtual Store Store { get; set; }

        public Nullable<int> CategoryID { get; set; }
        [ForeignKey("CategoryID")]
        public virtual Category Category { get; set; }

        public virtual ICollection<ItemPagePosition> ItemPagePositions { get; set; }
        public virtual ICollection<ItemAttribute> ItemAttributes { get; set; }
        //  public virtual ICollection<ItemPurchase>     ItemPurchases { get; set; }
    }
}
