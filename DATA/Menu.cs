//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DATA
{
    using System;
    using System.Collections.Generic;
    
    public partial class Menu
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StoreID { get; set; }
        public Nullable<int> Page { get; set; }
    
        public virtual Store Store { get; set; }
    }
}
