//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CBS.Library.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class ParameterSystem_AuditTrail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ParameterSystem_AuditTrail()
        {
            this.ParameterSystem_AuditTrailDetail = new HashSet<ParameterSystem_AuditTrailDetail>();
        }
    
        public System.Guid TemporaryID { get; set; }
        public string TableName { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ApproveDate { get; set; }
        public string ApproveBy { get; set; }
        public string HostIP { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParameterSystem_AuditTrailDetail> ParameterSystem_AuditTrailDetail { get; set; }
    }
}