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
    
    public partial class IdebCorpGroup
    {
        public string IdebId { get; set; }
        public int Seq { get; set; }
        public string KodeLJK { get; set; }
        public string NamaLJK { get; set; }
        public string NamaSesuaiIdentitas { get; set; }
        public string NomorIdentitas { get; set; }
        public string JenisKelamin { get; set; }
        public string JenisKelaminKet { get; set; }
        public string PosisiPekerjaan { get; set; }
        public string PosisiPekerjaanKet { get; set; }
        public Nullable<double> ProsentaseKepemilikan { get; set; }
        public string StatusPengurusPemilik { get; set; }
        public string StatusPengurusPemilikKet { get; set; }
        public string Alamat { get; set; }
        public string Kelurahan { get; set; }
        public string Kecamatan { get; set; }
        public string Kota { get; set; }
        public string KotaKet { get; set; }
    
        public virtual Ideb Ideb { get; set; }
    }
}
