using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using Lib = CBS.Library;

namespace CBS.Testing
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            successAlert.Visible = false;
            warningAlert.Visible = false;
        }        

        protected void genBureauReqFileSlik_Click(object sender, EventArgs e)
        {
            Lib.Class1 lib = new Lib.Class1();
            switch (lib.BureauReqFileSlik())
            {
                case 0:
                    warningLabel.Text = "File is available";                    
                    warningAlert.Visible = true;
                    break;
                case 1:
                    successLabel.Text = "Generate file success";
                    successAlert.Visible = true;                    
                    break;
                case 2:
                    warningLabel.Text = "There is an exception";
                    warningAlert.Visible = true;              
                    break;
                default:
                    break;
            }                        
        }

        protected void genBureauReqFileKBIJ_Click(object sender, EventArgs e)
        {

        }

        protected void genBureauReqFilePefindo_Click(object sender, EventArgs e)
        {

        }

        protected void dlReqProcessIwidExspsLogFile_Click(object sender, EventArgs e)
        {

        }

        protected void dlBureauProcessResultLogFile_Click(object sender, EventArgs e)
        {

        }
    }
}