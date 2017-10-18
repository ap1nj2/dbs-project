using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace SLIK.Model
{
    public class IdebCollateral
    {
        //public string FacId { get; set; } Foreign key to IdebFac
        //public int Seq { get; set; } generate by system

        /// <summary>
        /// Collateral Unique Id from Debtector.
        /// </summary>
        [JsonProperty(PropertyName = "debtectorId")]
        public string DebtectorId { get; set; }

        /// <summary>
        /// Foreign Key to object facility(Credit, BankGaransi, LC, SuratBerharga, Lainnya).DebtectorId
        /// </summary>
        [JsonProperty(PropertyName = "debtectorFacId")]
        public string DebtectorFacId { get; set; }

        [JsonProperty(PropertyName = "facCat")]
        public string FacCat { get; set; }

        [JsonProperty(PropertyName = "jenisAgunanKet")]
        public string JenisAgunanKet { get; set; }

        [JsonProperty(PropertyName = "nilaiAgunanMenurutLJK")]
        public string NilaiAgunanMenurutLJK { get; set; }

        [JsonProperty(PropertyName = "prosentaseParipasu")]
        public double ProsentaseParipasu { get; set; }

        [JsonProperty(PropertyName = "tanggalUpdate")]
        private string TanggalUpdateHasil { get; set; }
        public DateTime? TanggalUpdate
        {
            get { return DateTime.ParseExact(TanggalUpdateHasil, "yyyyMMddMMssdd", null); }
            set { TanggalUpdateHasil = value.GetValueOrDefault().ToString("yyyyMMddMMssdd"); }
        }

        [JsonProperty(PropertyName = "nomorAgunan")]
        public string NomorAgunan { get; set; }

        [JsonProperty(PropertyName = "jenisPengikatan")]
        public string JenisPengikatan { get; set; }

        [JsonProperty(PropertyName = "tanggalPengikatan")]
        private string TanggalPengikatanHasil { get; set; }
        public DateTime? TanggalPengikatan {
            get { return DateTime.ParseExact(TanggalPengikatanHasil, "yyyyMMdd", null); }
            set { TanggalPengikatanHasil = value.GetValueOrDefault().ToString("yyyyMMdd"); }
        }

        [JsonProperty(PropertyName = "namaPemilikAgunan")]
        public string NamaPemilikAgunan { get; set; }

        [JsonProperty(PropertyName = "alamatAgunan")]
        public string AlamatAgunan { get; set; }

        [JsonProperty(PropertyName = "lokasiAgunan")]
        public string LokasiAgunan { get; set; }


        [JsonProperty(PropertyName = "tglPenilaianLjk")]
        private string TglPenilaianLjkHasil { get; set; }
        
        public DateTime? TglPenilaianLjk
        {
            get { return DateTime.ParseExact(TglPenilaianLjkHasil, "yyyyMMdd", null); }
            set { TglPenilaianLjkHasil = value.GetValueOrDefault().ToString("yyyyMMdd"); }
        }

        [JsonProperty(PropertyName = "peringkatAgunan")]
        public string PeringkatAgunan { get; set; }

        [JsonProperty(PropertyName = "kodeLembagaPemeringkat")]
        public string LembagaPemeringkat { get; set; }

        [JsonProperty(PropertyName = "buktiKepemilikan")]
        public string BuktiKepemilikan { get; set; }

        [JsonProperty(PropertyName = "nilaiAgunanNjop123")]
        public double NilaiAgunanNjop { get; set; }

        [JsonProperty(PropertyName = "nilaiAgunanIndep")]
        public double NilaiAgunanIndep { get; set; }

        [JsonProperty(PropertyName = "namaPenilaiIndep")]
        public string NamaPenilaiIndep { get; set; }

        [JsonProperty(PropertyName = "asuransi")]
        public string Asuransi { get; set; }

        [JsonProperty(PropertyName = "tanggalPenilaianPenilaiIndependen")]
        public string TanggalPenilaianPenilaiIndependen { get; set; }

        [JsonProperty(PropertyName = "keterangan")]
        public string Keterangan { get; set; }
    }
}