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
    
    public partial class ParameterSystem
    {
        public string SetId { get; set; }
        public string TableNM { get; set; }
        public Nullable<bool> IsMaker { get; set; }
        public Nullable<bool> ISApprv { get; set; }
        public string ParamDesc { get; set; }
        public string ParamLinkMaker { get; set; }
        public string ParamLinkApprv { get; set; }
        public Nullable<int> ParamPos { get; set; }
        public Nullable<bool> ShowPending { get; set; }
    }
}