using System.Linq;

namespace DATA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Invoice
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public string PaymentMethod { get; set; }

        [NotMapped]
        public string errorMessage { get; set; }

        [NotMapped]
        public string _date
        {
            get { return Date.ToString(); }
            set
            {
				 Date = TimeTools.GetDate(value);
            }
        }

        public int StoreID { get; set; }
        //[ForeignKey("StoreID")]
        //public virtual Store Store { get; set; }

        public int SaleID { get; set; }
        //[ForeignKey("SaleID")]
        //public virtual Sale Sale { get; set; }
    }
}
