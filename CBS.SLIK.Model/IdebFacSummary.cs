using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace SLIK.Model
{
    public class IdebFacSummary
    {
        [JsonProperty(PropertyName = "plafonEfektifKredit")]
        public double PlafonEfektifKredit { get; set; }

        [JsonProperty(PropertyName = "plafonEfektifLc")]
        public double PlafonEfektifLc { get; set; }

        [JsonProperty(PropertyName = "plafonEfektifBg")]
        public double PlafonEfektifBg { get; set; }

        [JsonProperty(PropertyName = "plafonEfektifSec")]
        public double plafonEfektifSec { get; set; }

        [JsonProperty(PropertyName = "plafonEfektifLainnya")]
        public double PlafonEfektifLainnya { get; set; }

        [JsonProperty(PropertyName = "plafonEfektifTotal")]
        public double PlafonEfektifTotal { get; set; }

        [JsonProperty(PropertyName = "bakiDebetKredit")]
        public double BakiDebetKredit { get; set; }

        [JsonProperty(PropertyName = "bakiDebetLc")]
        public double BakiDebetLc { get; set; }

        [JsonProperty(PropertyName = "bakiDebetBg")]
        public double BakiDebetBg { get; set; }

        [JsonProperty(PropertyName = "bakiDebetSec")]
        public double BakiDebetSec { get; set; }

        [JsonProperty(PropertyName = "bakiDebetLainnya")]
        public double BakiDebetLainnya { get; set; }

        [JsonProperty(PropertyName = "bakiDebetTotal")]
        public double BakiDebetTotal { get; set; }

        [JsonProperty(PropertyName = "krediturBankUmum")]
        public int KrediturBankUmum { get; set; }

        [JsonProperty(PropertyName = "krediturBPR/S")]
        public int KrediturBPR_S { get; set; }

        [JsonProperty(PropertyName = "krediturLp")]
        public int KrediturLp { get; set; }

        [JsonProperty(PropertyName = "krediturLainnya")]
        public int KrediturLainnya { get; set; }

        [JsonProperty(PropertyName = "kolekTerburuk")]
        public string KolekTerburuk { get; set; }

        [JsonProperty(PropertyName = "kolekBulanDataTerburuk")]
        public string kolekBulanDataTerburuk { get; set; }
    }
}