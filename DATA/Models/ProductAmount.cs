namespace DATA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ProductAmount
    {
        public int ID { get; set; }
        public string Name { get; set; }
		public string Amount { get; set; }
		public string Unit { get; set; }

		public int ProductID { get; set; }
        public int ItemID { get; set; }
        public int StoreID { get; set; }
    }
}
