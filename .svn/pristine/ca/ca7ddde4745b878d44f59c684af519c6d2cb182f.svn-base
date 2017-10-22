using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SLIK.Model
{
    public class IdebHeader
    {
        [JsonProperty(PropertyName = "kodeReferensiPengguna")]
        public string KodeReferensiPengguna { get; set; }

        [JsonProperty(PropertyName = "tanggalHasil")]
        private string TanggalHasilString { get; set;}

        public DateTime TanggalHasil {
            get { return DateTime.ParseExact(TanggalHasilString, "yyyyMMddHHmmss", null); }
            set { TanggalHasilString = value.ToString("yyyyMMddHHmmss"); }
        }

        [JsonProperty(PropertyName = "idPermintaan")]
        public string IdPermintaan { get; set; }

        [JsonProperty(PropertyName = "idPenggunaPermintaan")]
        public string IdPenggunaPermintaan { get; set; }

        [JsonProperty(PropertyName = "dibuatOleh")]
        public string DibuatOleh { get; set; }

        [JsonProperty(PropertyName = "kodeLJKPermintaan")]
        public string KodeLJKPermintaan { get; set; }

        [JsonProperty(PropertyName = "kodeCabangPermintaan")]
        public string KodeCabangPermintaan { get; set; }

        [JsonProperty(PropertyName = "kodeTujuanPermintaan")]
        public string KodeTujuanPermintaan { get; set; }

        [JsonProperty(PropertyName = "tanggalPermintaan")]
        public string TanggalPermintaanString { get; set; }
        
        public DateTime TanggalPermintaan
        {
            get { return DateTime.ParseExact(TanggalPermintaanString, "yyyyMMddHHmmss", null); }
            set { TanggalPermintaanString = value.ToString("yyyyMMddHHmmss"); }
        }

        [JsonProperty(PropertyName = "totalBagian")]
        public int? TotalBagian { get; set; }

        [JsonProperty(PropertyName = "nomorBagian")]
        public int? NomorBagian { get; set; }
    }
}
