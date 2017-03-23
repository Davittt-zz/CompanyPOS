
namespace DATA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Menu
    {
        public int ID { get; set; }
		public string Name { get; set; }
        public string Description { get; set; }
		public int StoreID { get; set; }
    }
}
