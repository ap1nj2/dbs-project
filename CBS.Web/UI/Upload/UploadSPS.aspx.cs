using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CBS.ExportImport;

namespace CBS.Web.UI.Upload
{
    public partial class UploadSPS : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //SPS upload button :  Moch Robi Setiawan 
        protected void btnUploadSPS_Click(object sender, EventArgs e)
        {
            Importer.UploadSPSApp();
            Importer.UploadSPSAppBDoc();
            Importer.UploadSPSAppBillPmt();
            Importer.UploadSPSAppExLoan();
            Importer.UploadAppFlag();
            Importer.UploadAppFac();
            Importer.UploadSPSAppMemo();
            Importer.UploadSPSSupp();

            Importer.UploadSPSCustExCc();
            Importer.UploadSPSCustFin();
            Importer.UploadSPSCustJob();
            Importer.UploadSPSCustJobInfo();
            Importer.UploadSPSCustPersonal();
            Importer.UploadSPSCustRel();
        }
    }
}