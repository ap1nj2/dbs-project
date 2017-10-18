using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace SLIK.Model
{
    public class IdebKredit
    {
        [JsonProperty(PropertyName = "ljk")]
        public string Ljk { get; set; }

        [JsonProperty(PropertyName = "ljkKet")]
        public string LjkKet { get; set; }

        [JsonProperty(PropertyName = "cabang")]
        public string Cabang { get; set; }

        [JsonProperty(PropertyName = "cabangKet")]
        public string CabangKet { get; set; }

        [JsonProperty(PropertyName = "bakiDebet")]
        public double BakiDebet { get; set; }

        [JsonProperty(PropertyName = "tanggalDibentuk")]
        private string TanggalDibentukString { get; set; }

        public DateTime TanggalDibentuk
        {
            get { return DateTime.ParseExact(TanggalDibentukString, "yyyyMMddHHmmss", null); }
            set { TanggalDibentukString = value.ToString("yyyyMMddHHmmss"); }
        }

        [JsonProperty(PropertyName = "tanggalUpdate")]
        private string TanggalUpdateString { get; set; }

        public DateTime TanggalUpdate
        {
            get { return DateTime.ParseExact(TanggalUpdateString, "yyyyMMddHHmmss", null); }
            set { TanggalUpdateString = value.ToString("yyyyMMddHHmmss"); }
        }

        [JsonProperty(PropertyName = "bulan")]
        public int Bulan { get; set; }

        [JsonProperty(PropertyName = "tahun")]
        public int Tahun { get; set; }

        [JsonProperty(PropertyName = "sifatKredit")]
        public string SifatKredit { get; set; }

        [JsonProperty(PropertyName = "sifatKreditKet")]
        public string SifatKreditKet { get; set; }

        [JsonProperty(PropertyName = "jenisKredit")]
        public string JenisKredit { get; set; }

        [JsonProperty(PropertyName = "jenisKreditKet")]
        public string JenisKreditKet { get; set; }

        [JsonProperty(PropertyName = "akadPembiayaan")]
        public string AkadPembiayaan { get; set; }

        [JsonProperty(PropertyName = "akadPembiayaanKet")]
        public string AkadPembiayaanKet { get; set; }

        [JsonProperty(PropertyName = "noRekening")]
        public string NoRekening { get; set; }

        [JsonProperty(PropertyName = "baruPerpanjangan")]
        private string BaruPerpanjanganHasil { get; set; }

        public bool BaruPerpanjangan {
            get { return (BaruPerpanjanganHasil == "1") ? true : false; }
            set { }
        }

        [JsonProperty(PropertyName = "noAkadAwal")]
        public string NoAkadAwal { get; set; }

        [JsonProperty(PropertyName = "tanggalAkadAwal")]
        private string TanggalAkadAwalString { get; set; }

        public DateTime TanggalAkadAwal {
            get { return DateTime.ParseExact(TanggalAkadAwalString, "yyyyMMdd", null); }
            set { TanggalAkadAwalString = value.ToString("yyyyMMdd"); }
        }

        [JsonProperty(PropertyName = "noAkadAkhir")]
        public string NoAkadAkhir { get; set; }

        [JsonProperty(PropertyName = "tanggalAkadAkhir")]
        private string TanggalAkadAkhirString { get; set; }

        public DateTime TanggalAkadAkhir {
            get { return DateTime.ParseExact(TanggalAkadAkhirString, "yyyyMMdd", null); }
            set { TanggalAkadAkhirString = value.ToString("yyyyMMdd"); }
        }

        [JsonProperty(PropertyName = "tanggalAwalKredit")]
        private string TanggalAwalKreditString { get; set; }

        public DateTime TanggalAwalKredit
        {
            get { return DateTime.ParseExact(TanggalAwalKreditString, "yyyyMMdd", null); }
            set { TanggalAwalKreditString = value.ToString("yyyyMMdd"); }
        }

        [JsonProperty(PropertyName = "tanggalMulai")]
        private string TanggalMulaiString { get; set; }

        public DateTime TanggalMulai
        {
            get { return DateTime.ParseExact(TanggalMulaiString, "yyyyMMdd", null); }
            set { TanggalMulaiString = value.ToString("yyyyMMdd"); }
        }

        [JsonProperty(PropertyName = "tanggalJatuhTempo")]
        private string TanggalJatuhTempoString { get; set; }

        public DateTime TanggalJatuhTempo
        {
            get { return DateTime.ParseExact(TanggalJatuhTempoString, "yyyyMMdd", null); }
            set { TanggalJatuhTempoString = value.ToString("yyyyMMdd"); }
        }

        [JsonProperty(PropertyName = "kategoriDebiturKode")]
        public string KategoriDebiturKode { get; set; }

        [JsonProperty(PropertyName = "kategoriDebiturKet")]
        public string KategoriDebiturKet { get; set; }

        [JsonProperty(PropertyName = "jenisPenggunaan")]
        public string JenisPenggunaan { get; set; }

        [JsonProperty(PropertyName = "jenisPenggunaanKet")]
        public string JenisPenggunaanKet { get; set; }

        [JsonProperty(PropertyName = "sektorEkonomi")]
        public string SektorEkonomi { get; set; }

        [JsonProperty(PropertyName = "sektorEkonomiKet")]
        public string sektorEkonomiKet { get; set; }

        [JsonProperty(PropertyName = "kreditProgramPemerintah")]
        public string KreditProgramPemerintah { get; set; }

        [JsonProperty(PropertyName = "kreditProgramPemerintahKet")]
        public string KreditProgramPemerintahKet { get; set; }

        [JsonProperty(PropertyName = "lokasiProyek")]
        public string LokasiProyek { get; set; }

        [JsonProperty(PropertyName = "lokasiProyekKet")]
        public string LokasiProyekKet { get; set; }

        [JsonProperty(PropertyName = "valutaKode")] 
        public string KodeValuta { get; set; }

        [JsonProperty(PropertyName = "sukuBunga")]
        public double SukuBunga { get; set; }

        [JsonProperty(PropertyName = "jenisSukuBunga")]
        public string JenisSukuBunga { get; set; }

        [JsonProperty(PropertyName = "jenisSukuBungaKet")]
        public string JenisSukuBungaKet { get; set; }

        [JsonProperty(PropertyName = "kolektibilitas")]
        public string Kolektibilitas { get; set; }

        [JsonProperty(PropertyName = "jumlahHariTunggakan")]
        public int JumlahHariTunggakan { get; set; }

        [JsonProperty(PropertyName = "nilaiProyek")]
        public double NilaiProyek { get; set; }

        [JsonProperty(PropertyName = "plafonAwal")]
        public double PlafonAwal { get; set; }

        [JsonProperty(PropertyName = "plafon")]
        public double Plafon { get; set; }

        [JsonProperty(PropertyName = "realisasiBulanBerjalan")]
        public double RealisasiBulanBerjalan { get; set; }

        [JsonProperty(PropertyName = "nilaiDalamMataUangAsal")]
        public double NilaiDalamMataUangAsal { get; set; }

        [JsonProperty(PropertyName = "kodeSebabMacet")]
        public string SebabMacet { get; set; }

        [JsonProperty(PropertyName = "sebabMacetKet")]
        public string SebabMacetKet { get; set; }

        [JsonProperty(PropertyName = "tanggalMacet")]
        private string TanggalMacetString { get; set; }

        public DateTime TanggalMacet
        {
            get { return DateTime.ParseExact(TanggalMacetString, "yyyyMMdd", null); }
            set { TanggalMacetString = value.ToString("yyyyMMdd"); }
        }

        [JsonProperty(PropertyName = "tunggakanPokok")]
        public double TunggakanPokok { get; set; }

        [JsonProperty(PropertyName = "tunggakanBunga")]
        public double TunggakanBunga { get; set; }

        [JsonProperty(PropertyName = "frekuensiTunggakan")]
        public double FrekuensiTunggakan { get; set; }

        [JsonProperty(PropertyName = "denda")]
        public double Denda { get; set; }

        [JsonProperty(PropertyName = "frekuensiRestrukturisasi")]
        public double FrekuensiRestrukturisasi { get; set; }


        [JsonProperty(PropertyName = "tanggalRestrukturisasiAkhir")]
        private string TanggalRestrukturisasiAkhirString { get; set; }

        public DateTime TanggalRestrukturisasiAkhir
        {
            get { return DateTime.ParseExact(TanggalRestrukturisasiAkhirString, "yyyyMMdd", null); }
            set { TanggalRestrukturisasiAkhirString = value.ToString("yyyyMMdd"); }
        }

        [JsonProperty(PropertyName = "kodeCaraRestrukturisasi")]
        public string KodeCaraRestrukturisasi { get; set; }

        [JsonProperty(PropertyName = "restrukturisasiKet")]
        public string RestrukturisasiKet { get; set; }

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