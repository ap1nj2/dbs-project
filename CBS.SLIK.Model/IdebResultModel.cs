using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SLIK.Model
{
    /// <summary>
    /// Represent one pdf/text result (not one *.ideb file)
    /// </summary>
    public class IdebResultModel
    {
        

        public IdebIndiv Individual { get; set; }

        public IdebCorp Corporate { get; set; }

        public IdebFacSummary FacilitySummary { get; set; }

        public List<IdebKredit> CreditList { get; set; }

        public List<IdebBankGaransi> BankGaransiList { get; set; }

        public List<IdebLc> IrrecovaleLCList { get; set; }

        //public List<IdebSbModel> SuratBerhargaList { get; set; }

        public List<IdebFasilitasLainnya> FasilitasLainList { get; set; }

        public List<IdebCollateral> CollateralList { get; set; }

        public List<IdebPenjamin> PenjaminList { get; set; }

        public List<IdebPengurusModel> PengurusList { get; set; }
    }
}