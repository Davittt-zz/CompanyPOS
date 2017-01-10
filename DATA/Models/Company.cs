namespace DATA.Models
{
    using System;
    using System.Collections.Generic;
    
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Store> Stores { get; set; }
    }
}
