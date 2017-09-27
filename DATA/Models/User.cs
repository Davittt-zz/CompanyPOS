namespace DATA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class User
    {
        public int ID { get; set; }
		public string UUID { get; set; }

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Phone { get; set; }
        public string Type { get; set; }

        public string Password { get; set; }
        public string UserLevel { get; set; }
        [NotMapped]
		public int UserLevelNum { get; set; }

        public string Username { get; set; }
        public string Email { get; set; }

		public bool Active { get; set; }

		public int ClerkNum { get; set; }

		//If true User is Active, Inactive is false
		public bool Status { get; set; }

        public Nullable<int> CompanyID { get; set; }

        public int StoreID { get; set; }
        [ForeignKey("StoreID")]
        public virtual Store Store { get; set; }

        public virtual ICollection<UserActivity> UserActivity { get; set; }
    }
}
