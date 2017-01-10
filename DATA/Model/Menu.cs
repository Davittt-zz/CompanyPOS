
namespace DATA
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    
    public class Menu
    {
        //public Menu()
        //{
        //    this.ItemPagePosition = new HashSet<ItemPagePosition>();
        //}
    
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StoreID { get; set; }
        public Nullable<int> Page { get; set; }

        //[ForeignKey("StoreID")]
        //public virtual Store Store { get; set; }

        public virtual ICollection<ItemPagePosition> ItemPagePositions { get; set; }
    }
}
