namespace MigrationData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Users
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Users()
        {
            UserActivities = new HashSet<UserActivities>();
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Password { get; set; }

        public string UserLevel { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public int? CompanyID { get; set; }

        public int StoreID { get; set; }

        public virtual Stores Stores { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserActivities> UserActivities { get; set; }
    }
}
