﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace SLIK.Model
{
    public class IdebSuratBerharga
    {
        [JsonProperty(PropertyName = "ljk")]
        public string Ljk { get; set; }

        [JsonProperty(PropertyName = "ljkKet")]
        public string LjkKet { get; set; }

        [JsonProperty(PropertyName = "cabang")]
        public string Cabang { get; set; }

        [JsonProperty(PropertyName = "cabangKet")]
        public string CabangKet { get; set; }

        [JsonProperty(PropertyName = "nominalSb")]
        public double NominalSb { get; set; }

        [JsonProperty(PropertyName = "tanggalDibentuk")]
        private string TanggalDibentukString { get; set; }

        public DateTime TanggalDibentuk {
            get { return DateTime.ParseExact(TanggalDibentukString, "yyyyMMddHHmmss", null); }
            set { TanggalDibentukString = value.ToString("yyyyMMddHHmmss"); }
        }


        [JsonProperty(PropertyName = "tanggalUpdate")]
        private string TanggalUpdateString { get; set; }

        public DateTime TanggalUpdate {
            get { return DateTime.ParseExact(TanggalUpdateString, "yyyyMMddHHmmss", null); }
            set { TanggalUpdateString = value.ToString("yyyyMMddHHmmss"); }
        }

        [JsonProperty(PropertyName = "bulan")]
        public int Bulan { get; set; }

        [JsonProperty(PropertyName = "tahun")]
        public int Tahun { get; set; }

        [JsonProperty(PropertyName = "noSuratBeharga")]
        public string NoSuratBeharga { get; set; }

        [JsonProperty(PropertyName = "jenisSuratBeharga")]
        public string JenisSuratBeharga { get; set; }

        [JsonProperty(PropertyName = "sovereignRate")]
        public double SovereignRate { get; set; }

        [JsonProperty(PropertyName = "listing")]
        public string Listing { get; set; }

        [JsonProperty(PropertyName = "peringkatSuratBeharga")]
        public string PeringkatSuratBeharga { get; set; }

        [JsonProperty(PropertyName = "tujuanKepemilikan")]
        public string TujuanKepemilikan { get; set; }

        [JsonProperty(PropertyName = "tujuanKepemilikanKet")]
        public string TujuanKepemilikanKet { get; set; }

        [JsonProperty(PropertyName = "tanggalTerbit")]
        private string TanggalDiterbitkanString { get; set; }

        public DateTime TanggalDiterbitkan {
            get { return DateTime.ParseExact(TanggalDiterbitkanString, "yyyyMMdd", null); }
            set { TanggalDiterbitkanString = value.ToString("yyyyMMdd"); }
        }

        [JsonProperty(PropertyName = "tanggalJatuhTempo")]
        private string TanggalJatuhTempoString { get; set; }

        public DateTime TanggalJatuhTempo {
            get { return DateTime.ParseExact(TanggalJatuhTempoString, "yyyyMMdd", null); }
            set { TanggalJatuhTempoString = value.ToString("yyyyMMdd"); }
        }

        [JsonProperty(PropertyName = "sukuBunga")]
        public double SukuBunga { get; set; }

        [JsonProperty(PropertyName = "kodeValuta")]
        public string KodeValuta { get; set; }

        [JsonProperty(PropertyName = "kolektibilitas")]
        public string Kolektibilitas { get; set; }

        [JsonProperty(PropertyName = "jumlahHariTunggakan")]
        public int JumlahHariTunggakan { get; set; }

        [JsonProperty(PropertyName = "nilaiDalamMataUangAsal")]
        public double NilaiDalamMataUangAsal { get; set; }

        [JsonProperty(PropertyName = "nilaiPasar")]
        public double NilaiPasar { get; set; }

        [JsonProperty(PropertyName = "nilaiPerolehan")]
        public double NilaiPerolehan { get; set; }

        [JsonProperty(PropertyName = "tunggakan")]
        public double Tunggakan { get; set; }

        [JsonProperty(PropertyName = "tanggalMacet")]
        private string TanggalMacetString { get; set; }

        public DateTime TanggalMacet {
            get { return DateTime.ParseExact(TanggalMacetString, "yyyyMMdd", null); }
            set { TanggalMacetString = value.ToString("yyyyMMdd"); }
        }

        [JsonProperty(PropertyName = "kodeSebabMacet")]
        public string SebabMacet { get; set; }

        [JsonProperty(PropertyName = "sebabMacetKet")]
        public string SebabMacetKet { get; set; }

        [JsonProperty(PropertyName = "kondisi")]
        public string Kondisi { get; set; }

        [JsonProperty(PropertyName = "kondisiKet")]
        public string KondisiKet { get; set; }

        [JsonProperty(PropertyName = "tanggalKondisi")]
        private string TanggalKondisiString { get; set; }

        public DateTime TanggalKondisi
        {
            get { return DateTime.ParseExact(TanggalKondisiString, "yyyyMMdd", null); }
            set { TanggalKondisiString = value.ToString("yyyyMMdd"); }
        }

        [JsonProperty(PropertyName = "keterangan")]
        public string Keterangan { get; set; }

        [JsonProperty(PropertyName = "tahunBulan01Ht")]
        public string TahunBulan01Ht { get; set; }

        [JsonProperty(PropertyName = "tahunBulan01")]
        public string TahunBulan01 { get; set; }

        [JsonProperty(PropertyName = "tahunBulan01Kol")]
        public string TahunBulan01Kol { get; set; }

        [JsonProperty(PropertyName = "tahunBulan02Ht")]
        public string TahunBulan02Ht { get; set; }

        [JsonProperty(PropertyName = "tahunBulan02")]
        public string TahunBulan02 { get; set; }

        [JsonProperty(PropertyName = "tahunBulan02Kol")]
        public string TahunBulan02Kol { get; set; }

        [JsonProperty(PropertyName = "tahunBulan03Ht")]
        public string TahunBulan03Ht { get; set; }

        [JsonProperty(PropertyName = "tahunBulan03")]
        public string TahunBulan03 { get; set; }

        [JsonProperty(PropertyName = "tahunBulan03Kol")]
        public string TahunBulan03Kol { get; set; }

        [JsonProperty(PropertyName = "tahunBulan04Ht")]
        public string TahunBulan04Ht { get; set; }

        [JsonProperty(PropertyName = "tahunBulan04")]
        public string TahunBulan04 { get; set; }

        [JsonProperty(PropertyName = "tahunBulan04Kol")]
        public string TahunBulan04Kol { get; set; }

        [JsonProperty(PropertyName = "tahunBulan05Ht")]
        public string TahunBulan05Ht { get; set; }

        [JsonProperty(PropertyName = "tahunBulan05")]
        public string TahunBulan05 { get; set; }

        [JsonProperty(PropertyName = "tahunBulan05Kol")]
        public string TahunBulan05Kol { get; set; }

        [JsonProperty(PropertyName = "tahunBulan06Ht")]
        public string TahunBulan06Ht { get; set; }

        [JsonProperty(PropertyName = "tahunBulan06")]
        public string TahunBulan06 { get; set; }

        [JsonProperty(PropertyName = "tahunBulan06Kol")]
        public string TahunBulan06Kol { get; set; }

        [JsonProperty(PropertyName = "tahunBulan07Ht")]
        public string TahunBulan07Ht { get; set; }

        [JsonProperty(PropertyName = "tahunBulan07")]
        public string TahunBulan07 { get; set; }

        [JsonProperty(PropertyName = "tahunBulan07Kol")]
        public string TahunBulan07Kol { get; set; }

        [JsonProperty(PropertyName = "tahunBulan08Ht")]
        public string TahunBulan08Ht { get; set; }

        [JsonProperty(PropertyName = "tahunBulan08")]
        public string TahunBulan08 { get; set; }

        [JsonProperty(PropertyName = "tahunBulan08Kol")]
        public string TahunBulan08Kol { get; set; }

        [JsonProperty(PropertyName = "tahunBulan09Ht")]
        public string TahunBulan09Ht { get; set; }

        [JsonProperty(PropertyName = "tahunBulan09")]
        public string TahunBulan09 { get; set; }

        [JsonProperty(PropertyName = "tahunBulan09Kol")]
        public string TahunBulan09Kol { get; set; }

        [JsonProperty(PropertyName = "tahunBulan10Ht")]
        public string TahunBulan10Ht { get; set; }

        [JsonProperty(PropertyName = "tahunBulan10")]
        public string TahunBulan10 { get; set; }

        [JsonProperty(PropertyName = "tahunBulan10Kol")]
        public string TahunBulan10Kol { get; set; }

        [JsonProperty(PropertyName = "tahunBulan11Ht")]
        public string TahunBulan11Ht { get; set; }

        [JsonProperty(PropertyName = "tahunBulan11")]
        public string TahunBulan11 { get; set; }

        [JsonProperty(PropertyName = "tahunBulan11Kol")]
        public string TahunBulan11Kol { get; set; }

        [JsonProperty(PropertyName = "tahunBulan12Ht")]
        public string TahunBulan12Ht { get; set; }

        [JsonProperty(PropertyName = "tahunBulan12")]
        public string TahunBulan12 { get; set; }

        [JsonProperty(PropertyName = "tahunBulan12Kol")]
        public string TahunBulan12Kol { get; set; }

        [JsonProperty(PropertyName = "agunan")]
        public IdebCollateral[] Agunan { get; set; }

        [JsonProperty(PropertyName = "penjamin")]
        public IdebPenjamin[] Penjamin { get; set; }

    }
}