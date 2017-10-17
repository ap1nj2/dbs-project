using System;
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

namespace CBS.Web.Common
{
    public class MasterPageX : System.Web.UI.Page
    {
        protected DbConnection conn;
        protected int dbtimeout;
        protected string USERID, GROUPID;

        protected override void OnInit(EventArgs e)
        {
            dbtimeout = (int)Session["DbTimeOut"];
            conn = new DbConnection((string)Session["ConnString"]);
            USERID = (string)Session["UserID"];
            GROUPID = (string)Session["GroupID"];

            base.OnInit(e);
        }

        static public void logactivity(Page page, string sender, string desc, DbConnection conn, int dbtimeout)
        {
            int tracelevel = 10;
            if (page.Request.RawUrl.ToLower().IndexOf("screenmenu.aspx") > 0)
                tracelevel = 20;

            if ((int)page.Session["TraceLevel"] >= tracelevel)
            {
                string url = page.Request.RawUrl;
                object[] loguser = new object[]
                {
                    page.Session["UserID"], null, desc, tracelevel
                    , page.Request.UserHostAddress, sender, url
                };
                string qrystr, appid, tokenId;
                if (url.IndexOf("?") > 0)
                {
                    qrystr = url.Substring(url.IndexOf("?"));
                    url = url.Substring(0, url.IndexOf("?"));
                    appid = page.Request.QueryString["appid"];
                    tokenId= page.Request.QueryString["tokenid"];
                    loguser = new object[]
                    {
                        page.Session["UserID"], tokenId, desc, tracelevel
                            , page.Request.UserHostAddress, sender , url
                    };
                }
                conn.ExecNonQuery("exec dbo.usp_InsUserActivity", loguser, dbtimeout);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            //conn.ExecReader("EXEC USP_VW_LASUSERACT @1, @2", new object[] { Session["UserID"], Request.UserHostAddress }, dbtimeout);
            //if (conn.hasRow())
            //{
            //    Response.Redirect(ConfigurationSettings.AppSettings["logoutscr"]);
            //}

            if (!IsPostBack)
            {
                //logactivity(this, "MasterPage", "Accessing Page", conn, dbtimeout);
            }

            base.OnLoad(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            try
            {
                conn.Dispose();
            }
            catch { }
        }
    }

    public class DataPage : MasterPage
    {
        protected bool allowViewState = false;

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (this.Request.QueryString["readonly"] != null)
                ModuleSupport.DisableControls(this, allowViewState);
        }
    }
}