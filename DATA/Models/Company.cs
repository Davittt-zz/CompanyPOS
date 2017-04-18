namespace DATA.Models
{
    using System;
    using System.Collections.Generic;
    
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }

		public string Address	  { get; set; }
		public string Email		  { get; set; }
		public string Phone		  { get; set; }
		public string BankAccount { get; set; }
		public string Bulstat_Eik { get; set; }

        public virtual ICollection<Store> Stores { get; set; }
    }
}
