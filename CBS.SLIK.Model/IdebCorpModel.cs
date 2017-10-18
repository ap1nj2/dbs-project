using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace SLIK.Model
{
    public class IdebCorp
    {
        [JsonProperty(PropertyName = "parameterPencarian")]
        public IdebCorpSearchVar SearchVar { get; set; }

        [JsonProperty(PropertyName = "dataPokokDebitur")]
        public List<IdebCorpData> CorpDataList { get; set; }

        [JsonProperty(PropertyName = "kelompokPengurusPemilik")]
        public List<IdebCorpGroup> CorpGroupList { get; set; }

        [JsonProperty(PropertyName = "ringkasanFasilitas")]
        public IdebFacSummary RingkasanFasilitas { get; set; }

        [JsonProperty(PropertyName = "fasilitas")]
        public IdebFacSummary Fasilitas { get; set; }
    }

    public class IdebCorpSearchVar
    {
        //public string IdebId { get; set; }
        [JsonProperty(PropertyName = "namaBadanUsaha")]
        public string NamaBadanUsaha { get; set; }

        [JsonProperty(PropertyName = "npwp")]
        public string Npwp { get; set; }

        [JsonProperty(PropertyName = "tempatPendirian")]
        public string TempatPendirian { get; set; }

        [JsonProperty(PropertyName = "tanggalAktaPendirian")]
        private string TanggalAktaPendirianHasil { get; set; }
        public DateTime? TanggalAktaPendirian { get; set; }

        [JsonProperty(PropertyName = "nomorAktaPendirian")]
        public string NomorAktaPendirian { get; set; }
    }

    public class IdebCorpData
    {
        //public string IdebId { get; set; }
        //public int Seq { get; set; }

        /// <summary>
        /// Debitur Corp Unique Id from Debtector
        /// </summary>
        /// 
        [JsonProperty(PropertyName = "debtectorID")]
        public string DebtectorId { get; set; }

        /// <summary>
        /// Foreign Key to IdebMain.DebtectorId
        /// </summary>
        /// 
        [JsonProperty(PropertyName = "debdectorIdebId")]
        public string DebtectorIdebId { get; set; }

        [JsonProperty(PropertyName = "namaDebitur")]
        public string NamaDebitur { get; set; }

        [JsonProperty(PropertyName = "namaLengkap")]
        public string NamaLengkap { get; set; }

        [JsonProperty(PropertyName = "npwp")]
        public string Npwp { get; set; }

        [JsonProperty(PropertyName = "bentukBu")]
        public string BentukBu { get; set; }

        [JsonProperty(PropertyName = "goPublic")]
        public string GoPublic { get; set; }

        [JsonProperty(PropertyName = "tempatPendirian")]
        public string TempatPendirian { get; set; }

        [JsonProperty(PropertyName = "noAktaPendirian")]
        public string NoAktaPendirian { get; set; }


        [JsonProperty(PropertyName = "tglAktaPendirian")]
        private string TglAktaPendirianHasil { get; set; }

        public DateTime? TglAktaPendirian
        {
            get { return DateTime.ParseExact(TglAktaPendirianHasil, "yyyyMMddHHmmss", null); }
            set { TglAktaPendirianHasil = value.GetValueOrDefault().ToString("yyyyMMddHHmmss"); }
        }

        [JsonProperty(PropertyName = "pelapor")]
        public string Pelapor { get; set; }

        [JsonProperty(PropertyName = "tanggalDibentuk")]
        private string TanggalDibentukHasil { get; set; }

        public DateTime? TanggalDibentuk
        {
            get { return DateTime.ParseExact(TanggalDibentukHasil, "yyyyMMddHHmmss", null); }
            set { TanggalDibentukHasil = value.GetValueOrDefault().ToString("yyyyMMddHHmmss"); }
        }


        [JsonProperty(PropertyName = "tanggalUpdate")]
        private string TanggalUpdateHasil { get; set; }

        public DateTime? TanggalUpdate
        {
            get { return DateTime.ParseExact(TanggalUpdateHasil, "yyyyMMddHHmmss", null); }
            set { TanggalUpdateHasil = value.GetValueOrDefault().ToString("yyyyMMddHHmmss"); }
        }

        [JsonProperty(PropertyName = "alamat")]
        public string Alamat { get; set; }

        [JsonProperty(PropertyName = "kelurahan")]
        public string Kelurahan { get; set; }

        [JsonProperty(PropertyName = "kecamatan")]
        public string Kecamatan { get; set; }

        [JsonProperty(PropertyName = "kabKota")]
        public string KabKota { get; set; }

        [JsonProperty(PropertyName = "kodePos")]
        public string KodePos { get; set; }

        [JsonProperty(PropertyName = "negara")]
        public string Negara { get; set; }

        [JsonProperty(PropertyName = "noAktaTerakhir")]
        public string NoAktaTerakhir { get; set; }

        [JsonProperty(PropertyName = "tglAktaTerakhir")]
        private string TglAktaTerakhirHasil { get; set; }

        public DateTime? TglAktaTerakhir
        {
            get { return DateTime.ParseExact(TanggalUpdateHasil, "yyyyMMddHHmmss", null); }
            set { TanggalUpdateHasil = value.GetValueOrDefault().ToString("yyyyMMddHHmmss"); }
        }

        [JsonProperty(PropertyName = "sektorEkonomi")]
        public string SektorEkonomi { get; set; }

        [JsonProperty(PropertyName = "pemeringkat")]
        public string Pemeringkat { get; set; }

        [JsonProperty(PropertyName = "peringkat")]
        public string Peringkat { get; set; }

        [JsonProperty(PropertyName = "tanggalPemeringkatan")]
        private string TanggalPemeringkatanHasil { get; set; }
        public DateTime? TanggalPemeringkatan
        {
            get { return DateTime.ParseExact(TanggalPemeringkatanHasil, "yyyyMMddHHmmss", null); }
            set { TanggalPemeringkatanHasil = value.GetValueOrDefault().ToString("yyyyMMddHHmmss"); }
        }
    }

    public class IdebCorpGroup
    {
        //public string GroupId { get; set; }
        //public string IdebId { get; set; }
        //public int Seq { get; set; }
        [JsonProperty(PropertyName = "kodeLJK")]
        public string KodeLJK { get; set; }

        [JsonProperty(PropertyName = "namaLJK")]
        public string NamaLJK { get; set; }

        [JsonProperty(PropertyName = "pengurusPemilik")]
        public List<IdebCorpGroupOwner> PengurusList { get; set; }
    }

    public class IdebCorpGroupOwner
    {
        //public string GroupId { get; set; }
        //public int Seq { get; set; }
        [JsonProperty(PropertyName = "namaSesuaiIdentitas")]
        public string NamaSesuaiIdentitas { get; set; }

        [JsonProperty(PropertyName = "nomorIdentitas")]
        public string NomorIdentitas { get; set; }

        [JsonProperty(PropertyName = "kodeJenisKelamin")]
        public string KodeJenisKelamin { get; set; }

        [JsonProperty(PropertyName = "kodePosisiPekerjaan")]
        public string KodePosisiPekerjaan { get; set; }

        [JsonProperty(PropertyName = "prosentaseKepemilikan")]
        public double ProsentaseKepemilikan { get; set; }

        [JsonProperty(PropertyName = "kodeStatusPengurusPemilik")]
        public string KodeStatusPengurusPemilik { get; set; }

        [JsonProperty(PropertyName = "alamat")]
        public string Alamat { get; set; }

        [JsonProperty(PropertyName = "kelurahan")]
        public string Kelurahan { get; set; }

        [JsonProperty(PropertyName = "kecamatan")]
        public string Kecamatan { get; set; }

        [JsonProperty(PropertyName = "kodeKota")]
        public string KodeKota { get; set; }
    }

}