namespace MigrationData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Shifts
    {
        public int ID { get; set; }

        public string Status { get; set; }

        public DateTime? TimeStart { get; set; }

        public DateTime? TimeEnd { get; set; }

        public double OpeningAmount { get; set; }

        public double ClosingAmount { get; set; }

        public int StoreID { get; set; }

        public virtual Stores Stores { get; set; }
    }
}
