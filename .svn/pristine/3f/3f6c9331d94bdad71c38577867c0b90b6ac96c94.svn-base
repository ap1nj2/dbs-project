using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SLIK.Model;
using Newtonsoft.Json;

namespace CBS.ImportExportSLIK
{
    public class SLIKViewModel
    {
        public IdebHeader header { get; set; }

        public IdebCorpSlik perusahaan { get; set; }
    }

    public class IdebCorpSlik
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
        public IdebFacilities Fasilitas { get; set; }
    }

    public class IdebFacilities
    {
        [JsonProperty(PropertyName = "suratBerharga")]
        public List<IdebSuratBerharga> SuratBerharga { get; set; }

        [JsonProperty(PropertyName = "kredit")]
        public List<IdebKredit> Kredit { get; set; }

        [JsonProperty(PropertyName = "lc")]
        public List<IdebLc> Lc { get; set; }

        [JsonProperty(PropertyName = "bankGaransi")]
        public List<IdebBankGaransi> BankGarasi { get; set; }

        [JsonProperty(PropertyName = "fasilitasLainnya")]
        public List<IdebFasilitasLainnya> FasilitasLainnya { get; set; }
    }

}
