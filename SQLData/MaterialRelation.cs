//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Subnautica.SQLData
{
    using System;
    using System.Collections.Generic;
    
    public partial class MaterialRelation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MaterialRelation()
        {
            this.MaterialRelation1 = new HashSet<MaterialRelation>();
        }
    
        public System.Guid ID { get; set; }
        public System.Guid ResourceID { get; set; }
        public Nullable<System.Guid> ProductID { get; set; }
        public int Quantity { get; set; }
    
        public virtual Material Material { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MaterialRelation> MaterialRelation1 { get; set; }
        public virtual MaterialRelation MaterialRelation2 { get; set; }
    }
}
