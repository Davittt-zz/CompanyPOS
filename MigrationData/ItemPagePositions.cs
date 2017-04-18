namespace MigrationData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ItemPagePositions
    {
        public int ID { get; set; }

        public int? hPos { get; set; }

        public int? vPos { get; set; }

        public int MenuID { get; set; }

        public int StoreID { get; set; }

        public int ItemID { get; set; }

        public int MenuPage_ID { get; set; }

        public virtual Items Items { get; set; }
    }
}
