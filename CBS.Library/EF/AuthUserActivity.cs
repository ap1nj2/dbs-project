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
    
    public partial class AuthUserActivity
    {
        public string UserId { get; set; }
        public int Seq { get; set; }
        public string TokenId { get; set; }
        public Nullable<System.DateTime> ActTime { get; set; }
        public string ActDesc { get; set; }
        public Nullable<int> ActLevel { get; set; }
        public string Host { get; set; }
        public string PageUrl { get; set; }
        public string PageName { get; set; }
    }
}
