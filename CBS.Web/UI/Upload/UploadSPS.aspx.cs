﻿using System;
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
			string path = Server.MapPath("~/Uploads/") + UploadSPS.UploadedFiles.First().FileName;
            UploadSPS.UploadedFiles.First().SaveAs(path, true);
			
			int TimeOut = 10;
			string connStr = CBSDBSConstr;
			
			//string spsFilePath, string connString, int dbTimeOut
            Importer.UploadSPSApp(path, ConnStr, TimeOut);
            Importer.UploadSPSAppDoc(path, ConnStr, TimeOut);
            Importer.UploadSPSAppBillPmt(path, ConnStr, TimeOut);
            Importer.UploadSPSAppExLoan(path, ConnStr, TimeOut);
            Importer.UploadAppFlag(path, ConnStr, TimeOut);
			
            Importer.UploadAppFac(path, ConnStr, TimeOut);
            Importer.UploadSPSAppMemo(path, ConnStr, TimeOut);
            Importer.UploadSPSSupp(path, ConnStr, TimeOut);
            Importer.UploadSPSCustExCc(path, ConnStr, TimeOut);
            Importer.UploadSPSCustFin(path, ConnStr, TimeOut);
			
            Importer.UploadSPSCustJob(path, ConnStr, TimeOut);
            Importer.UploadSPSCustJobInfo(path, ConnStr, TimeOut);
            Importer.UploadSPSCustPersonal(path, ConnStr, TimeOut);
            Importer.UploadSPSCustRel(path, ConnStr, TimeOut);
        }
    }
}