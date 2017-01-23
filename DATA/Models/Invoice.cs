using System.Linq;

namespace DATA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Invoice
    {
        public int ID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string PaymentMethod { get; set; }

        [NotMapped]
        public string errorMessage { get; set; }

        [NotMapped]
        public string _date
        {
            get { return Date.ToString(); }
            set
            {
                try
                {
                    var inputDate = string.Join("/", value.Split(new Char[] { '-' }).Take(3).Reverse());
                    var inputTime = string.Join(":", value.Split(new Char[] { '-' }).Skip(3));
                    Date = Convert.ToDateTime(inputDate + " " + inputTime);

                    errorMessage = " Todo piola";
                }
                catch (Exception Ex)
                {
                    errorMessage = Ex.Message;
                    Date = null;
                }
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
