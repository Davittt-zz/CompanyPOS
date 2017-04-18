namespace MigrationData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Items
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Items()
        {
            ItemAttributes = new HashSet<ItemAttributes>();
            ItemPagePositions = new HashSet<ItemPagePositions>();
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public float? UnitPrice { get; set; }

        public string Description { get; set; }

        public int StoreID { get; set; }

        public int? CategoryID { get; set; }

        public virtual Categories Categories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ItemAttributes> ItemAttributes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ItemPagePositions> ItemPagePositions { get; set; }

        public virtual Stores Stores { get; set; }
    }
}
