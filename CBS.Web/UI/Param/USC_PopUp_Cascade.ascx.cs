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
using DMS.Framework;

namespace CBS.Web.UI.Param
{
    public partial class USC_PopUp_Cascade : System.Web.UI.UserControl
    {

        public int MaxRecord = 10;
        private USC_SearchCascade ddl;
        public string JavaScript = "";
        public bool CustomCallBack = false;
        private DataTable dt;

        protected void Page_Load(object sender, EventArgs e)
        {
            Grid.ClientInstanceName = Grid.ClientID;
            Grid.SettingsPager.PageSize = MaxRecord;
            
            Popup.PopupHorizontalAlign = DevExpress.Web.PopupHorizontalAlign.LeftSides;
            Popup.PopupVerticalAlign = DevExpress.Web.PopupVerticalAlign.Below;


            string JSCascade = 
                "<script language='javascript'>" +
                "function " + this.ClientID + "_Cascade(obj, val) " +
                "{" +
                "document.getElementById('" + oCascade.ClientID + "').value=obj;" +
                Grid.ClientID + ".PerformCallback(val);" +
                "}" +
                "function " + this.ClientID + "_OnGetRowValues(values) " +
                "{" +
                "if(values[0]==null) " +
                "{ " +
                "   document.getElementById(document.getElementById('" + oCascade.ClientID + "').value + '_txtID').value=''; " +
                "   document.getElementById(document.getElementById('" + oCascade.ClientID + "').value + '_lblDESC').innerHTML=''; " +
                "   document.getElementById(document.getElementById('" + oCascade.ClientID + "').value + '_hDesc').value=''; " +
                "   try { window.parent.resizeFrame(); } catch(e) {};" +
                "} " +
                "else " +
                "{ " +
                "   document.getElementById(document.getElementById('" + oCascade.ClientID + "').value + '_txtID').value=values[0]; " +
                "   document.getElementById(document.getElementById('" + oCascade.ClientID + "').value + '_lblDESC').innerHTML=values[1]; " +
                "   document.getElementById(document.getElementById('" + oCascade.ClientID + "').value + '_hDesc').value=values[1]; " +
                "   try { window.parent.resizeFrame(); } catch(e) {};" +
                "}" +
                JavaScript +
                "} " +
                "</script>";
            Page.ClientScript.RegisterStartupScript(this.GetType(), this.ClientID + "_Cascade", JSCascade);


            Grid.ClientSideEvents.RowClick = "function(s,e) {" +
                "s.GetRowValues(e.visibleIndex, s.cp_FieldNames, " + this.ClientID + "_OnGetRowValues);" +
                Popup.ClientID + ".Hide(); }";

            Grid.ClientSideEvents.EndCallback = "function(s,e) {" +
                "if(s.cp_customcallback=='0') "+
                "{ "+
                "if(s.cp_firstcallback=='1') "+Popup.ClientID + ".Show(); " +
                "s.cp_firstcallback='0'; return; " +
                "} " +
                "s.GetRowValues(0, s.cp_FieldNames, " + this.ClientID + "_OnGetRowValues);" +
                "s.cp_customcallback = '0'; try { window.parent.resizeFrame(); } catch(e) {}; }";
        }

        protected void Grid_Load(object sender, EventArgs e)
        {
            if (!Grid.IsCallback) return;

            ddl =(USC_SearchCascade) ModuleSupport.FindControlByClientID(this.Page, oCascade.Value);

            string QueryString = ddl.QueryString;
            object[] QueryParam = ddl.QueryParam;
            
            
            QueryString = QueryString.ToUpper().Replace("SELECT ", "SELECT TOP 0 ");
            
            dt = ddl.conn.GetDataTable(QueryString, QueryParam, ddl.DbTimeOut);

            Grid.AutoGenerateColumns = false;
            

            GridViewCommandColumn cmd = new GridViewCommandColumn();
            cmd.ClearFilterButton.Visible = true;
            cmd.ShowSelectCheckbox = false;
            Grid.Columns.Add(cmd);

            string FieldNames = "";
            string strfilter = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                //if (dt.Columns[i].DataType.ToString() == "System.DateTime")
                //{
                //    GridViewDataDateColumn gvddate = new GridViewDataDateColumn();
                //    gvddate.FieldName = dt.Columns[i].ColumnName;
                //    gvddate.Settings.FilterMode = ColumnFilterMode.Value;
                //    gvddate.Settings.SortMode = DevExpress.XtraGrid.ColumnSortMode.Value;
                //    Grid.Columns.Add(gvddate);
                //    if (gvddate.FilterExpression != "")
                //        strfilter += " AND " + gvddate.FilterExpression;
                //    FieldNames += ";" + dt.Columns[i].ColumnName;
                //}
                //else
                //{
                    GridViewDataTextColumn c = new GridViewDataTextColumn();
                    c.PropertiesTextEdit.EncodeHtml = false;
                    c.FieldName = dt.Columns[i].ColumnName;
                    c.Settings.FilterMode = ColumnFilterMode.DisplayText;
                    c.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                    c.Settings.SortMode = DevExpress.XtraGrid.ColumnSortMode.Value;
                    Grid.Columns.Add(c);
                    if (c.FilterExpression != "")
                        strfilter += " AND " + c.FilterExpression;
                    FieldNames += ";" + dt.Columns[i].ColumnName;
                //}
            }

            Grid.JSProperties["cp_FieldNames"] = FieldNames.Substring(1);


            for (int i = 0; i < Grid.Columns.Count; i++)
            {
                if (Grid.Columns[i] is GridViewDataTextColumn)
                {
                    GridViewDataTextColumn c = (GridViewDataTextColumn)Grid.Columns[i];
                    if (c.FieldName != "")
                    {
                        switch (dt.Columns[c.FieldName].DataType.ToString())
                        {
                            case "System.Decimal":
                            case "System.Double":
                                c.PropertiesTextEdit.DisplayFormatString = "###,##0.00";
                                c.CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
                                break;
                            case "System.Int16":
                            case "System.Int32":
                            case "System.Int64":
                            case "System.int":
                                c.PropertiesTextEdit.DisplayFormatString = "###,##0";
                                c.CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
                                break;
                            case "System.DateTime":
                                c.PropertiesTextEdit.DisplayFormatString = "dd MMMM yyyy";
                                c.CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
                                break;
                            case "System.String":
                                if (dt.Columns[0].MaxLength <= 50)
                                    c.CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
                                break;
                        }
                    }
                }
            }            
        }

        protected void Grid_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            if (!CustomCallBack)
            {
                string QueryString = ddl.QueryString;
                object[] QueryParam = ddl.QueryParam;
                if (ddl.NeedFilter)
                {
                    string strfilter = "";
                    for (int i = 1; i < Grid.Columns.Count; i++)
                    {
                        GridViewDataColumn ColumnId = (GridViewDataColumn)Grid.Columns[i];
                        if (ColumnId.FilterExpression != "")
                        {
                            if (ColumnId is GridViewDataDateColumn)
                                strfilter += " AND " + ColumnId.FilterExpression.Replace("#","'");
                            else
                                strfilter += " AND " + ColumnId.FilterExpression;
                        }
                    }

                    if (strfilter == "")
                        strfilter = " AND 1=0";

                    if (QueryString.Trim().ToLower().IndexOf(" where ") == -1)
                        strfilter = " WHERE 1=1 " + strfilter;

                    QueryString = QueryString.ToUpper().Replace("SELECT ", "SELECT TOP 100 ");

                    Grid.DataSource = ddl.conn.GetDataTable(QueryString + strfilter, QueryParam, ddl.DbTimeOut);
                    Grid.DataBind();
                }
                else
                {
                    if (QueryString.ToLower().IndexOf("select") == 0)
                        dt = ddl.conn.GetDataTable(QueryString, QueryParam, ddl.DbTimeOut);
                    Grid.DataSource = dt;
                    Grid.DataBind();
                }
                Grid.JSProperties["cp_customcallback"] = "0";

            }
        }


        

        protected void Grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string QueryString = ddl.QueryString;
            object[] QueryParam = ddl.QueryParam;
            if (QueryString.ToLower().IndexOf("select") == 0)
                dt = ddl.conn.GetDataTable(QueryString, QueryParam, ddl.DbTimeOut);
            if (e.Parameters != "__NOTCUSTOMCALLBACK__")
            {
                CustomCallBack = true;
                if (ddl.QueryString.Trim().ToLower().IndexOf("select") == 1)
                {
                    string strfilter = " AND " + (Grid.Columns[1] as GridViewDataColumn).FieldName + " = '" + e.Parameters + "'";

                    if (QueryString.ToLower().IndexOf(" where ") == -1)
                        strfilter = " WHERE 1=1 " + strfilter;

                    Grid.DataSource = ddl.conn.GetDataTable(QueryString + strfilter, QueryParam, ddl.DbTimeOut);
                    Grid.DataBind();
                }
                else
                {
                    DataView dv = new DataView(dt, (Grid.Columns[1] as GridViewDataColumn).FieldName + " = '" + e.Parameters + "'", null, DataViewRowState.CurrentRows);
                    Grid.DataSource = dv;
                    Grid.DataBind();
                }
                Grid.JSProperties["cp_customcallback"] = "1";
            }
            else
            {
                Grid.FilterExpression = "";
                Grid.DataSource = dt;
                Grid.DataBind(); 
                
                Grid.JSProperties["cp_customcallback"] = "0";
                Grid.JSProperties["cp_firstcallback"] = "1";
            }
        }
    }
}