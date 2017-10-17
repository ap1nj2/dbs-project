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
using CBS.Web.Common;

namespace CBS.Web.UI.Param
{
    public partial class ParamMaker : System.Web.UI.Page
    {
        public string funcCss = "hide";
        public string funcpendCss = "hide";

        public string funcpenddelCss(string Status)
        {
            if (Status == "Delete")
                return "hide";
            else
                return "";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && !IsCallback)
            {
                title.Text = Request.QueryString["title"];
                using (DbConnection conn = new DbConnection((string)Session["ConnString"]))
                {
                    MasterPageX.logactivity(this, "ParamMaker", "Accessing Parameter (" + Request.QueryString["TBLNM"] + ")", conn, 600);
                }
                USC_paraminput.createGridColumns(grid, gridpending);
            }
        }

        protected void panel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            try
            {
                panel.JSProperties["cp_action"] = "";
                if (e.Parameter.StartsWith("s:"))
                {
                    USC_paraminput.savepending(null);
                    panel.JSProperties["cp_action"] = "s";
                }
                else if (e.Parameter.StartsWith("r:"))
                {
                    USC_paraminput.retrieve(e.Parameter.Substring(2));
                    panel.JSProperties["cp_action"] = "r";
                }
                else if (e.Parameter.StartsWith("rp:"))
                {
                    USC_paraminput.retrievepending(e.Parameter.Substring(3));
                    panel.JSProperties["cp_action"] = "rp";
                }
            }
            catch(Exception ex) 
            {
                panel.JSProperties["cp_alert"] = ex.Message;
            }
        }

        protected void grid_Load(object sender, EventArgs e)
        {
            USC_paraminput.dtbinddata(grid);
            if (grid.GroupCount > 0)
                funcCss = "";
        }
        protected void gridpending_Load(object sender, EventArgs e)
        {
            USC_paraminput.dtbindpending(gridpending);
            if (gridpending.GroupCount > 0)
                funcpendCss = "";
        }

        protected void grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {            
        }

        protected void gridpending_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters.StartsWith("d:"))
            {
                USC_paraminput.savepending(e.Parameters.Substring(2));
                USC_paraminput.dtbindpending(gridpending);
                //gridpending.JSProperties["cp_alert"] = "data deleted moved to pending";
            }
            if (e.Parameters.StartsWith("dp:"))
            {
                USC_paraminput.deletepending(e.Parameters.Substring(3));
                USC_paraminput.dtbindpending(gridpending);
            }

        }

        protected void grid_BeforeColumnSortingGrouping(object sender, DevExpress.Web.ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            if (grid.GroupCount > 0)
                funcCss = "";
            else
                funcCss = "hide";

        }

        protected void gridpending_BeforeColumnSortingGrouping(object sender, DevExpress.Web.ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            if (gridpending.GroupCount > 0)
                funcpendCss = "";
            else
                funcpendCss = "hide";
        }        
    }
}
