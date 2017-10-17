using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLIK.Model
{
    public class IdebIndiv
    {
        public IdebIndivSearchVar SearchVar { get; set; }

        public List<IdebIndivData> IndivDataList { get; set; }
    }

    public class IdebIndivSearchVar
    {
        //public string IdebId { get; set; }
        public string NamaDebitur { get; set; }

        public string JenisKelamin { get; set; }

        public string JenisKelaminKet { get; set; }

        public string NoIdentitas { get; set; }

        public string Npwp { get; set; }

        public string TempatLahir { get; set; }

        public DateTime? TanggalLahir { get; set; }
    }

    public class IdebIndivData
    {
        //public string IdebId { get; set; }
        //public int Seq { get; set; }

        /// <summary>
        /// Debitur Individual Unique Id from Debtector.
        /// </summary>
        public string DebtectorId { get; set; }

        /// <summary>
        /// Foreign Key to IdebMain.DebtectorId
        /// </summary>
        public string DebtectorIdebId { get; set; }

        public string NamaDebitur { get; set; }

        public string Identitas { get; set; }

        public string NoIdentitas { get; set; }

        public string JenisKelamin { get; set; }

        public string Npwp { get; set; }

        public string TempatLahir { get; set; }

        public DateTime? TanggalLahir { get; set; }

        public string Pelapor { get; set; }

        public DateTime? TanggalDibentuk { get; set; }

        public DateTime? TanggalUpdate { get; set; }

        public string Alamat { get; set; }

        public string Kelurahan { get; set; }

        public string Kecamatan { get; set; }

        public string KabKota { get; set; }

        public string KodePos { get; set; }

        public string Negara { get; set; }

        public string Pekerjaan { get; set; }

        public string TempatBekerja { get; set; }

        public string BidangUsaha { get; set; }

        public string KodeGelarDebitur { get; set; }
    }
}