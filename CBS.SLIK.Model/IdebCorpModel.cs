using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLIK.Model
{
    public class IdebCorp
    {
        public IdebCorpSearchVar SearchVar { get; set; }

        public List<IdebCorpData> CorpDataList { get; set; }

        public List<IdebCorpGroup> CorpGroupList { get; set; }
    }

    public class IdebCorpSearchVar
    {
        //public string IdebId { get; set; }
        public string NamaBadanUsaha { get; set; }

        public string Npwp { get; set; }

        public string TempatPendirian { get; set; }

        public DateTime? TanggalAktaPendirian { get; set; }

        public string NomorAktaPendirian { get; set; }
    }

    public class IdebCorpData
    {
        //public string IdebId { get; set; }
        //public int Seq { get; set; }

        /// <summary>
        /// Debitur Corp Unique Id from Debtector
        /// </summary>
        public string DebtectorId { get; set; }

        /// <summary>
        /// Foreign Key to IdebMain.DebtectorId
        /// </summary>
        public string DebtectorIdebId { get; set; }

        public string NamaDebitur { get; set; }

        public string NamaLengkap { get; set; }

        public string Npwp { get; set; }

        public string BentukBu { get; set; }

        public string GoPublic { get; set; }

        public string TempatPendirian { get; set; }

        public string NoAktaPendirian { get; set; }

        public DateTime? TglAktaPendirian { get; set; }

        public string Pelapor { get; set; }

        public DateTime? TanggalDibentuk { get; set; }

        public DateTime? TanggalUpdate { get; set; }

        public string Alamat { get; set; }

        public string Kelurahan { get; set; }

        public string Kecamatan { get; set; }

        public string KabKota { get; set; }

        public string KodePos { get; set; }

        public string Negara { get; set; }

        public string NoAktaTerakhir { get; set; }

        public DateTime? TglAktaTerakhir { get; set; }

        public string SektorEkonomi { get; set; }

        public string Pemeringkat { get; set; }

        public string Peringkat { get; set; }

        public DateTime? TanggalPemeringkatan { get; set; }
    }

    public class IdebCorpGroup
    {
        //public string GroupId { get; set; }
        //public string IdebId { get; set; }
        //public int Seq { get; set; }
        public string KodeLJK { get; set; }

        public string NamaLJK { get; set; }

        public List<IdebCorpGroupOwner> PengurusList { get; set; }
    }

    public class IdebCorpGroupOwner
    {
        //public string GroupId { get; set; }
        //public int Seq { get; set; }
        public string NamaSesuaiIdentitas { get; set; }

        public string NomorIdentitas { get; set; }

        public string KodeJenisKelamin { get; set; }

        public string KodePosisiPekerjaan { get; set; }

        public double ProsentaseKepemilikan { get; set; }

        public string KodeStatusPengurusPemilik { get; set; }

        public string Alamat { get; set; }

        public string Kelurahan { get; set; }

        public string Kecamatan { get; set; }

        public string KodeKota { get; set; }
    }
}