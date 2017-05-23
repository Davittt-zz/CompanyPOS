namespace DATA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Associate
    {
        public int ID { get; set; }
		public string Name { get; set; }

		public string Address { get; set; }
		public string City { get; set; }
		public string Country { get; set; }
        public string PostalCode { get; set; }

        public string PhoneNumber { get; set; }
        public string Bulstat { get; set; }
        public string Email { get; set; }

        public int StoreID { get; set; }
        [ForeignKey("StoreID")]
        public virtual Store Store { get; set; }
    }
}
