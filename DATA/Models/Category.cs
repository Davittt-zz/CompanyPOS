namespace DATA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Category
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public int StoreID { get; set; }
        [ForeignKey("StoreID")]
        public virtual Store Store { get; set; }

        public virtual ICollection<Item> Items { get; set; }
    }
}
