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
using DevExpress.Web;
using DMS.Tools;

namespace CBS.Web.UI.Param
{
    public partial class USC_SearchCascade : System.Web.UI.UserControl
    {
        public DbConnection conn;
        public int DbTimeOut = 600;
        public DataView DataViewValue;

        public string SearchText = "...";
        public bool NeedFilter = false;
        public bool hideText = false;
        public string btncssclass = null;

        public string QueryString = null;
        public object[] QueryParam = null;

        public string JavaScript;

        public string CssClass
        {
            set { txtID.CssClass = value; }
            get { return txtID.CssClass; }
        }

        public string getJSCallBack(USC_PopUp_Cascade oPopUpCascade)
        {
            return oPopUpCascade.ClientID + "_Cascade" + "('" + this.ClientID + "','');";
        }

        public string getJSCallBack(USC_PopUp_Cascade oPopUpCascade, string Value)
        {
            return oPopUpCascade.ClientID + "_Cascade" + "('" + this.ClientID + "'," + Value + ");";
        }

        public void PopUpCascade(USC_PopUp_Cascade oPopUpCascade, string JavaScript)
        {
            string HRef = "javascript:" + oPopUpCascade.Popup.ClientID + ".ShowAtElementByID('" + btnlink.ClientID + "');";
            HRef += oPopUpCascade.Popup.ClientID + ".Hide();";
            HRef += getJSCallBack(oPopUpCascade, "'__NOTCUSTOMCALLBACK__'");
            btnlink.Attributes.Add("onclick", HRef);

            //txtID.AutoPostBack = false;
            txtID.Attributes["onchange"] += "document.getElementById('" + oPopUpCascade.oCascade.ClientID + "').value='" + this.ClientID + "';";
            txtID.Attributes["onchange"] += getJSCallBack(oPopUpCascade, "this.value");
            if (JavaScript != null && JavaScript != "")
                oPopUpCascade.JavaScript += "if(document.getElementById('" + oPopUpCascade.oCascade.ClientID + "').value == '" + this.ClientID + "' ){ " + JavaScript + " }";
        }

      
        public string Value
        {
            get {
                return txtID.Text;
            }

            set {
                txtID.Text = value;
                setDesc();
            }
        }

        public void setDesc()
        {
            DataTable dt = conn.GetDataTable(QueryString, QueryParam, DbTimeOut);
            DataViewValue = new DataView(dt, dt.Columns[0].ColumnName + " = '" + txtID.Text + "'", "", DataViewRowState.OriginalRows);
            if (DataViewValue.Count > 0)
            {
                lblDESC.Text = DataViewValue[0][1].ToString();
                hDesc.Value = DataViewValue[0][1].ToString();
            }
            else
            {
                txtID.Text = "";
                lblDESC.Text = "";
                hDesc.Value = "";
            }
        }

        public string Desc
        {
            get
            {
                return hDesc.Value;
            }            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack && !this.Page.IsCallback && QueryString == null)
            {
                btnlink.Visible = false;
                txtID.Visible = false;
            }

            if (IsPostBack || this.Page.IsCallback )
                lblDESC.Text = hDesc.Value;

            btnlink.Value = SearchText;
            if (btncssclass != null)
                btnlink.Attributes.Add("class", btncssclass);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (hideText)
            {
                lblDESC.Text = "";
                txtID.Attributes["style"] = "display:none";
            }
        }
    }
}