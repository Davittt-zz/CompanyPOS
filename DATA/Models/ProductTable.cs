namespace DATA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

	public class ProductTable
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double InStock { get; set; }
		public string Unit { get; set; }
        public int StoreID { get; set; }
		public DateTime LastUpdated { get; set; }
    }
}
