namespace MigrationData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserActivities
    {
        public int ID { get; set; }

        public string Username { get; set; }

        public string Activity { get; set; }

        public int StoreID { get; set; }

        public int UserID { get; set; }

        public virtual Users Users { get; set; }
    }
}
