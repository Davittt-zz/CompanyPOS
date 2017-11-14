using System.Linq;

namespace DATA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;
    using System.Runtime.Serialization;

    [DataContract]
    public class Invoice
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public DateTime Date { get; set; }
        [DataMember]
        public string PaymentMethod { get; set; }

        [NotMapped]
        public string errorMessage { get; set; }

        [NotMapped]
        [DataMember]
        public string _date
        {
            get { return Date.ToString(); }
            set
            {
				 Date = TimeTools.GetDate(value);
            }
        }
        [DataMember]
        public int StoreID { get; set; }
        //[ForeignKey("StoreID")]
        //public virtual Store Store { get; set; }
        [DataMember]
        public int SaleID { get; set; }
     
        [JsonIgnore]
        public virtual Sale Sale { get; set; }
        [DataMember]
        public decimal? AmountPaid { get; set; }
  
    }
}
