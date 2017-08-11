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
		public double Amount { get; set; }
		public string Color { get; set; }
		public string Units { get; set; }
		public bool Visible { get; set; }

		public double Cost { get; set; }

		public string Tax { get; set; }
        public int ItemID { get; set; }
        public int StoreID { get; set; }

		//Optional
		public int ProductID { get; set; }
    }
}
