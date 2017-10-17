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
using DMS.Framework;

namespace CBS.Web.UI.Param
{
    public partial class USC_DDL_Cascade : System.Web.UI.UserControl
    {
        public DbConnection conn;
        public int DbTimeOut = 600;

        public string JavaScript = null;
        public bool RunScriptOnEndCallback = false;

        public string QueryString = null;
        public object[] QueryParam = null;

        public string CssClass
        {
            set { DDL.CssClass = value; }
            get { return DDL.CssClass; }
        }

        public string JSClear()
        {
            return "DDL_Cascade_Clear('" + this.ClientID + "');";            
        }

        public string JSGetValue()
        {
            return "DDL_Cascade_GetValue('" + this.ClientID + "');";
        }

        public string JSSetValue(string value)
        {
            return "DDL_Cascade_SetValue('" + this.ClientID + "', " + value + ");";
        }

        public string JSCallback()
        {
            return this.ClientID + "_xPanel.PerformCallback('');";
        }

        public string JSCallback(string value)
        {
            return this.ClientID + "_xPanel.PerformCallback('" + value + "');";
        }

        public object oValue
        {
            get
            {
                return hDDL;
            }

        }

        public string Value
        {
            get
            {
                return hDDL.Value;
            }

            set
            {
                try { DDL.SelectedValue = value; }
                catch { }
                hDDL.Value = value;
            }
        }

        public void FillDDL()
        {
            staticFramework.reff(DDL, QueryString, QueryParam, conn);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string JSCascade = "<script language='javascript'>" +
               "function DDL_Cascade_Clear(ID) " +
               "{" +
               "DDL_Cascade_SetValue(ID, ''); " +
               "}" +
               "function DDL_Cascade_GetValue(ID) " +
               "{" +
               "return document.getElementById(ID + '_hDDL').value;" +
               "}" +
               "function DDL_Cascade_SetValue(ID, value) " +
               "{" +
               "document.getElementById(ID + '_xPanel_DDL').value = value;" +
               "document.getElementById(ID + '_hDDL').value = value;" +
               "}" +
               "</script>";
            
            Page.ClientScript.RegisterStartupScript(this.GetType(), this.ClientID + "_Cascade", JSCascade);


            xPanel.ClientInstanceName = xPanel.ClientID;
            DDL.Attributes["onchange"] = hDDL.ClientID + ".value = " + DDL.ClientID + ".value;";
            if (JavaScript != null)
            {
                if(RunScriptOnEndCallback)
                    xPanel.ClientSideEvents.EndCallback = " function(s,e){ " + JavaScript + "}";
                DDL.Attributes["onchange"] += JavaScript;
            }

            if (!xPanel.IsCallback && (IsPostBack || Page.IsCallback) && QueryParam != null)
            {
                FillDDL();
                try
                {
                    DDL.SelectedValue = Value;
                }
                catch { }
            }
        }

        protected void xPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            FillDDL();
            Value = DDL.SelectedValue;
        }
    }
}