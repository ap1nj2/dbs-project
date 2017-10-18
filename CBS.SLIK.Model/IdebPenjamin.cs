﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace SLIK.Model
{
    public class IdebPenjamin
    {
        //public string FacId { get; set; } Foreign key to IdebFac
        //public int Seq { get; set; } generate by system

        /// <summary>
        /// Penjamin Unique Id from Debtector.
        /// </summary>
        /// 
        [JsonProperty(PropertyName = "debtectorId")]
        public string DebtectorId { get; set; }

        /// <summary>
        /// Foreign Key to object facility(Credit, BankGaransi, LC, SuratBerharga, Lainnya).DebtectorId
        /// </summary>
        [JsonProperty(PropertyName = "debtectorFacId")]
        public string DebtectorFacId { get; set; }

        [JsonProperty(PropertyName = "facCat")]
        public string FacCat { get; set; }

        [JsonProperty(PropertyName = "namaPenjamin")]
        public string NamaPenjamin { get; set; }

        [JsonProperty(PropertyName = "nomorIdentitas")]
        public string NomorIdentitas { get; set; }

        [JsonProperty(PropertyName = "tanggalUpdate")]
        private string TanggalUpdateHasil { get; set; }
        public DateTime? TanggalUpdate {
            get { return DateTime.ParseExact(TanggalUpdateHasil, "yyyyMMddMMssdd", null); }
            set { TanggalUpdateHasil = value.GetValueOrDefault().ToString("yyyyMMdd"); }
        }

        [JsonProperty(PropertyName = "tanggalBuat")]
        private string TanggalBuatHasil { get; set; }
        public DateTime? TanggalBuat
        {
            get { return DateTime.ParseExact(TanggalBuatHasil, "yyyyMMddMMssdd", null); }
            set { TanggalBuatHasil = value.GetValueOrDefault().ToString("yyyyMMdd"); }
        }

        [JsonProperty(PropertyName = "kodeJenisPenjamin")]
        public string JenisPenjamin { get; set; }

        [JsonProperty(PropertyName = "alamatPenjamin")]
        public string AlamatPenjamin { get; set; }

        [JsonProperty(PropertyName = "keterangan")]
        public string Keterangan { get; set; }
    }
}