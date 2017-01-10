namespace DATA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class User
    {
        // public User()
        //{
        //    this.UserActivity = new HashSet<UserActivity>();
        //    this.Sale = new HashSet<Sale>();
        //}

        public int ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        // public string TypeID { get; set; }
        public string Password { get; set; }
        public string UserLevel { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public Nullable<int> CompanyID { get; set; }

        public int StoreID { get; set; }
        [ForeignKey("StoreID")]
        public virtual Store Store { get; set; }

        public virtual ICollection<UserActivity> UserActivity { get; set; }
        //public virtual ICollection<Sale> Sale { get; set; }
    }
}
