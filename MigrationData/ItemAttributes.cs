namespace MigrationData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ItemAttributes
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public float? Price { get; set; }

        public bool? Visible { get; set; }

        public int ItemID { get; set; }

        public int StoreID { get; set; }

        public virtual Items Items { get; set; }
    }
}
