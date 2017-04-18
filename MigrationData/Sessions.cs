namespace MigrationData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Sessions
    {
        public int ID { get; set; }

        public int UserID { get; set; }

        public int StoreID { get; set; }

        public string TokenID { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? LastUpdate { get; set; }
    }
}
