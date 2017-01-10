namespace CompanyPOS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class UserActivity
    {
        public int ID { get; set; }

        public string Username { get; set; }
        public string Activity { get; set; }

       public int StoreID { get; set; }
       //[ForeignKey("StoreID")]
       //public virtual Store Store { get; set; }

        public int UserID { get; set; }
        //[ForeignKey("UserID")]
        //public virtual User User { get; set; }
    }
}
