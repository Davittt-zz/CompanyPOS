namespace MigrationData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Menus
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int StoreID { get; set; }
    }
}
