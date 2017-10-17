using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using DMS.Tools;
using CBS.Common;

namespace CBS.Web
{
    public partial class unittest : System.Web.UI.Page
    {
        #region static vars
        private static string Q_LOGINSCR = "select top 1 LOGIN_SCR, DB_IP, DB_NAMA, DB_LOGINID, DB_LOGINPWD from RFMODULE where MODULEID = @1 ";
        private static string Q_USERDATA = "select * from VW_SESSIONLOS where USERID = @1 ";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private bool AddSession(string uid, string connstr, int timeout)
        {
            bool ret = false;
            string connString= EncryptDecrypt.decryptConnStr(ConfigurationManager.AppSettings["AuthConnString"]));
            using (DbConnection locconn = new DbConnection(connString))
            {
                object[] user = new object[1] { uid };
                locconn.ExecReader(Q_USERDATA, user, timeout);
                if (locconn.hasRow())
                {
                    Session.Add("UserID", locconn.GetFieldValue("USERID"));
                    Session.Add("FullName", locconn.GetFieldValue("FULLNAME"));
                    Session.Add("GroupID", locconn.GetFieldValue("GROUPID"));
                    Session.Add("GroupNAME", locconn.GetFieldValue("GROUPNAME"));
                    Session.Add("BranchID", locconn.GetFieldValue("BRANCHID"));
                    Session.Add("BranchNAME", locconn.GetFieldValue("BRANCHNAME"));
                    Session.Add("CCOBRANCH", locconn.GetFieldValue("CCOBRANCH"));
                    Session.Add("USERLEVEL", locconn.GetFieldValue("LEVELID"));
                    Session.Add("LOGDATE", locconn.GetFieldValue("LOGDATE"));

                    Session.Add("ModuleID", ConfigurationManager.AppSettings["ModuleID"]);
                    Session.Add("ConnString", connstr);
                    Session.Add("dbTimeOut", timeout);
                    Session.Add("dbBigTimeOut", int.Parse(ConfigurationManager.AppSettings["dbBigTimeOut"]));
                    Session.Add("LoginTime", System.DateTime.Now);
                    Session.Add("TraceLevel", 20);
                    Session.Add("MaxFileSize", int.Parse(ConfigurationManager.AppSettings["MaxFileSize"]));
                    string luaf_uid = null, luaf_pwd = null, cis_uid = null, cis_pwd = null;
                    try
                    {
                        luaf_uid = ConfigurationManager.AppSettings["LookupAcct_uid"];
                        luaf_pwd = authenticate.decryptConnStr(ConfigurationManager.AppSettings["LookupAcct_pwd"]);
                    }
                    catch { }
                    try
                    {
                        cis_uid = ConfigurationManager.AppSettings["CISAcct_uid"];
                        cis_pwd = authenticate.decryptConnStr(ConfigurationManager.AppSettings["CISAcct_pwd"]);
                    }
                    catch { }
                    Session.Add("LookupAcctUID", luaf_uid);
                    Session.Add("LookupAcctPwd", luaf_pwd);
                    Session.Add("CISAcctUID", cis_uid);
                    Session.Add("CISAcctPwd", cis_pwd);
                    ret = true;
                }
            }
            return ret;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();

            int dbtimeout = int.Parse(ConfigurationManager.AppSettings["dbTimeOut"]);
            string connString = EncryptDecrypt.decryptConnStr(ConfigurationManager.AppSettings["AuthConnString"]);            
            string nexturl = "";
            using (DbConnection conn = new DbConnection(connString))
            {
                conn.ExecReader(Q_LOGINSCR, parmodule, dbtimeout);
                if (conn.hasRow())
                {
                    string uid = TextBox1.Text,
                        dbip = connESecurity.GetFieldValue(1),
                        dbname = connESecurity.GetFieldValue(2),
                        dbuid = connESecurity.GetFieldValue(3),
                        dbpwd = authenticate.decryptMyStr(connESecurity.GetFieldValue(4));

                    if (AddSession(uid, "Data Source=" + dbip + ";Initial Catalog=" + dbname + ";uid=" + dbuid + ";pwd=" + dbpwd + ";Pooling=true", dbtimeout))
                    {
                        FormsAuthentication.SetAuthCookie(uid, false);
                        nexturl = TextBox2.Text;
                    }
                }


                conn.Dispose();
                if (nexturl != "")
                    Response.Redirect(nexturl, true);
                else
                    MyPage.popMessage(this, "User not found");
            }
           
            
        }
    }
}
