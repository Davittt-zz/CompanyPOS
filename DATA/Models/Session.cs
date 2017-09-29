
namespace DATA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Session
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int StoreID { get; set; }
        public string TokenID { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public Nullable<System.DateTime> LastUpdate { get; set; }
        [NotMapped]
        public int? PinNumber { get; set; }
    }
}
