using System;
using System.Collections;
using System.Collections.Specialized;
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
using CBS.Web.Common;

namespace CBS.Web.UI.Param
{
    public partial class ParamApprv : System.Web.UI.Page
    {
        DbConnection conn = null;
        int dbtimeout = 600;
        DataTable dtparam = null;
        string TableNm = "";
        DataTable dtparamschema;
        string sqldata, sqlpending1, sqlpending2, sqlsortby;
        int topquery = 1000; 
        public string funcpendCss = "hide";
        public string funcpenddelCss(string Status)
        {
            if (Status == "Delete")
                return "hide";
            else
                return "";
        }

        #region static vars
        private static string Q_PARAMSYS = "SELECT * FROM PARAMETERSYSTEMFIELD WHERE TABLENM = @1 ORDER BY FIELDPOS";
        private static string Q_PARAMTMP = "SELECT * FROM PARAMETERSYSTEM_TEMPORARY WHERE TEMPORARYID = @1";
        private static string Q_PARAMTMPDET = "SELECT * FROM PARAMETERSYSTEM_TEMPORARYDETAIL WHERE TEMPORARYID = @1";
        private static string U_DELPARAMTMP = "DELETE PARAMETERSYSTEM_TEMPORARY WHERE TEMPORARYID = @1";
        private static string SP_AUDITTRAIL = "EXEC USP_AUDITTRAIL @1,@2";
        #endregion

        #region grid

        protected void creategridquery()
        {
            string fieldstr = "", tempfieldstr = "", condstr = "";
            string Key = "''";
            SortedList sortby = new SortedList();
            for (int i = 0; i < dtparam.Rows.Count; i++)
            {
                string FieldNm = dtparam.Rows[i]["FIELDNM"].ToString();
                string FieldDesc = dtparam.Rows[i]["FIELDDESC"].ToString();
                string FieldReff = dtparam.Rows[i]["FIELDREFF"].ToString().ToUpper().Replace(":[", "[X].[");
                bool FieldKey = false;
                if (dtparam.Rows[i]["FIELDKEY"].ToString() != "")
                    FieldKey = (bool)dtparam.Rows[i]["FIELDKEY"];
                string FieldType = dtparam.Rows[i]["FIELDTYPE"].ToString();
                bool isAuto = dtparam.Rows[i]["FIELDAUTO"].ToString().Trim() != "";
                string AutoPrefix = "", AutoSufix = "";
                if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.String"                   //autoprefix and autosuffix must not be used to a non string fieldtype
                        && isAuto == false && FieldReff == "")                                              //and non-auto and non refferential field 
                {
                    AutoPrefix = dtparam.Rows[i]["AUTOPREFIX"].ToString();
                    AutoSufix = dtparam.Rows[i]["AUTOSUFIX"].ToString();
                }
                int SortSeq = 0;
                if (dtparam.Rows[i]["SORTSEQ"].ToString() != "")
                    SortSeq = (int)dtparam.Rows[i]["SORTSEQ"];
                if (SortSeq > 0)
                {
                    if (dtparam.Rows[i]["GROUPBY"].ToString() == "" || (bool)dtparam.Rows[i]["GROUPBY"] == false)   //ignore grouped by columns
                        if (!sortby.ContainsKey(SortSeq))        //ignore repetitive order seq
                        {
                            if (dtparam.Rows[i]["SORTASC"].ToString() != "" && (bool)dtparam.Rows[i]["SORTASC"] == false)
                                sortby.Add(SortSeq, FieldNm + " DESC");
                            else
                                sortby.Add(SortSeq, FieldNm);
                        }
                }
                if (FieldKey)
                {
                    Key += "+' AND [" + FieldNm + "]='''+ CONVERT(VARCHAR(200),[" + FieldNm + "])+''''";
                }
                if (FieldReff != "")
                {
                    int idx = FieldReff.IndexOf(",");
                    int idx2 = FieldReff.LastIndexOf("SELECT ", idx) + 7;
                    int idx3 = FieldReff.IndexOf(" FROM", idx);

                    string FieldReffId = FieldReff.Substring(idx2, idx - idx2).Trim();
                    string FieldReffDesc = FieldReff.Substring(idx + 1, idx3 - (idx + 1)).Trim();
                    string FieldReffFrom = FieldReff.Substring(FieldReff.IndexOf("FROM"));
                    string FieldReffWhere = "";
                    if (FieldReff.IndexOf("WHERE") > 0)
                        FieldReffWhere += " AND ";
                    else
                        FieldReffWhere += " WHERE ";

                    fieldstr += "," + " (SELECT " + FieldReffDesc + " " + FieldReffFrom + FieldReffWhere + FieldReffId + "=[X].[" + FieldNm + "]) AS [" + FieldNm + "_DESC]"; ;
                }
                if (AutoPrefix.Length > 0 || AutoSufix.Length > 0)
                {
                    int prelen, sulen;
                    prelen = 1 + AutoPrefix.Length;
                    sulen = AutoSufix.Length + AutoPrefix.Length;
                    fieldstr += ", substring([" + FieldNm + "], " + prelen.ToString() + ", len(" + FieldNm + ") - " + sulen.ToString() + ") as [" + FieldNm + "]";
                }
                else if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Boolean")
                    fieldstr += ", convert(bit, [" + FieldNm + "]) as [" + FieldNm + "]";
                else
                {
                    fieldstr += ", [" + FieldNm + "]";
                }
                tempfieldstr += ",(SELECT FIELDVALUE FROM PARAMETERSYSTEM_TEMPORARYDETAIL " +
                                "  WHERE FIELDNAME = '" + FieldNm + "' AND TEMPORARYID=Y.TEMPORARYID)[" + FieldNm + "] ";
                if (isAuto && FieldType != "auto")      //auto value but not autogenerated 
                {
                    string thisfieldauto = dtparam.Rows[i]["FIELDAUTO"].ToString().Trim();
                    if (thisfieldauto.StartsWith("q:"))
                    {
                        condstr += "and [X].[" + FieldNm + "] = '" + Request.QueryString[thisfieldauto.Substring(2)] + "' ";
                    }
                    else if (thisfieldauto.StartsWith("s:"))
                    {
                        condstr += "and [X].[" + FieldNm + "] = '" + Session[thisfieldauto.Substring(2)].ToString() + "' ";
                    }
                }
            }
            if (condstr != "")
                condstr = " WHERE " + condstr.Substring(4);

            sqldata = "SELECT " + Key + " AS __KEY " + fieldstr + " FROM " + TableNm + " [X] " + condstr;
            sqlpending1 = "SELECT "+((topquery>0)?" top "+topquery.ToString():"")+" Y.TEMPORARYID __KEY, Y.STATUS __STATUS, Y.CREATEDBY __CREATEDBY" + tempfieldstr + " FROM PARAMETERSYSTEM_TEMPORARY Y " +
                         "WHERE Y.TableName='" + TableNm + "'";
            sqlpending2 = "SELECT __KEY,__STATUS,__CREATEDBY" + fieldstr + " FROM (" + sqlpending1 + ") [X] " + condstr;
            sqlsortby = "";
            for (int i = 0; i < sortby.Count; i++)
                sqlsortby += (string)sortby.GetByIndex(i) + ", ";
            sortby.Clear();
            if (sqlsortby != "")
                sqlsortby = sqlsortby.Substring(0, sqlsortby.Length - 2);
        }

        public void createGridColumns(DevExpress.Web.ASPxGridView gridpending)
        {
            DevExpress.Web.GridViewDataTextColumn p;
            for (int i = 0; i < dtparam.Rows.Count; i++)
            {
                string FieldNm = dtparam.Rows[i]["FIELDNM"].ToString();
                string FieldDesc = dtparam.Rows[i]["FIELDDESC"].ToString();
                string FieldReff = dtparam.Rows[i]["FIELDREFF"].ToString().ToUpper().Replace(":[", "[X].[");
                //bool FieldKey = (bool)dtparam.Rows[i]["FIELDKEY"];
                bool FieldKey = dtparam.Rows[i]["FIELDKEY"] == System.DBNull.Value ? false : (bool)dtparam.Rows[i]["FIELDKEY"];
                string FieldType = dtparam.Rows[i]["FIELDTYPE"].ToString();
                bool isAuto = dtparam.Rows[i]["FIELDAUTO"].ToString().Trim() != "";
                bool GroupBy = false;
                if (dtparam.Rows[i]["GROUPBY"].ToString() != "")
                    GroupBy = (bool)dtparam.Rows[i]["GROUPBY"];
                switch (FieldType)
                {
                    case "d":
                        break;
                }

                p = new DevExpress.Web.GridViewDataTextColumn();
                if (FieldReff != "")
                    p.FieldName = FieldNm + "_DESC";
                else
                    p.FieldName = FieldNm;
                p.Caption = FieldDesc;
                p.Settings.FilterMode = ColumnFilterMode.Value;
                p.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Guid" || isAuto)
                    p.Visible = false;
                else if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Double" ||
                        dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Decimal")
                    p.PropertiesTextEdit.DisplayFormatString = "###,##0.00";
                else if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.DateTime")
                    p.PropertiesTextEdit.DisplayFormatString = "dd/MM/yyyy";
                gridpending.Columns.Add(p);
                if (GroupBy)
                    gridpending.GroupBy(p);

            }
            p = new DevExpress.Web.GridViewDataTextColumn();
            p.FieldName = "__STATUS";
            p.Caption = "Status";
            p.Settings.FilterMode = ColumnFilterMode.Value;
            p.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            gridpending.Columns.Add(p);
            p = new DevExpress.Web.GridViewDataTextColumn();
            p.FieldName = "__CREATEDBY";
            p.Caption = "Created By";
            p.Settings.FilterMode = ColumnFilterMode.Value;
            p.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            gridpending.Columns.Add(p);

            gridpending.KeyFieldName = "__KEY";
        }

        public void dtbindpending(DevExpress.Web.ASPxGridView gridpending)
        {
            DataTable dt = conn.GetDataTable(sqlpending2, null, dbtimeout);
            DataView dv = new DataView(dt);
            dv.Sort = sqlsortby;
            gridpending.DataSource = dv;
            gridpending.DataBind();

            hALL.Value = "";
            for (int i = 0; i < dt.Rows.Count; i++)
                hALL.Value += dt.Rows[i]["__KEY"].ToString() + ",";
        }

        //public void dtbindpending(DevExpress.Web.ASPxGridView gridpending)
        //{
        //    DataTable dt = conn.GetDataTable(sqlpending2, null, dbtimeout);
        //    DataView dv = new DataView(dt);
        //    dv.Sort = sqlsortby;
        //    gridpending.DataSource = dv;
        //    gridpending.DataBind();
        //}

        #endregion

        #region init page
        protected override void OnLoad(EventArgs e)
        {
            conn = new DbConnection((string)Session["ConnString"]);
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
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["top"] != null)
            {
                topquery = int.Parse(Request.QueryString["top"]);
            }
            else
            {
                topquery = 1000;
            }
            TableNm = Request.QueryString["TBLNM"];
            object[] param = new object[] { TableNm };
            dtparam = conn.GetDataTable(Q_PARAMSYS, param, dbtimeout);
            dtparamschema = conn.GetDataTable("SELECT TOP 0 * FROM " + TableNm, null, dbtimeout, true, true);
            creategridquery();

            if (!IsPostBack && !IsCallback)
            {
                lblError.Text = lblErrorDet.Text = ""; btn_showdet.Attributes["style"] = "display:none";
                title.Text = Request.QueryString["title"]+" Top "+topquery.ToString()+" of row";
                MasterPageX.logactivity(this, "ParamApprv", "Accessing Parameter (" + Request.QueryString["TBLNM"] + ")", conn, dbtimeout);
                createGridColumns(gridpending);
                //ASPxGridView grid = new ASPxGridView();
                //USC_paraminput.createGridColumns(grid, gridpending2);
            }
        }

        /*
        protected void gridpending2_Load(object sender, EventArgs e)
        {
            dtbindpending(gridpending2);
            //USC_paraminput.dtbindpending(gridpending2);
            if (gridpending2.GroupCount > 0)
                funcpendCss = "";
        }
        protected void gridpending2_BeforeColumnSortingGrouping(object sender, DevExpress.Web.ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            if (gridpending2.GroupCount > 0)
                funcpendCss = "";
            else
                funcpendCss = "hide";
        }        
        */

        protected void gridpending_Load(object sender, EventArgs e)
        {
            dtbindpending(gridpending);
            //USC_paraminput.dtbindpending(gridpending);
            if (gridpending.GroupCount > 0)
                funcpendCss = "";
        }

        protected void gridpending_BeforeColumnSortingGrouping(object sender, DevExpress.Web.ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            if (gridpending.GroupCount > 0)
                funcpendCss = "";
            else
                funcpendCss = "hide";
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            lblError.Text = lblErrorDet.Text = ""; btn_showdet.Attributes["style"] = "display:none";
            string[] keyREJCT = hREJCT.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] keyAPPRV = hAPPRV.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < keyREJCT.Length; i++)
            {
                if (keyREJCT[i] != "") 
                    conn.ExecNonQuery(U_DELPARAMTMP, new object[] { keyREJCT[i] }, dbtimeout);
            }

            for (int i = 0; i < keyAPPRV.Length; i++)
            {
                if (keyAPPRV[i] != "")
                {
                    conn.ExecReader(Q_PARAMTMP, new object[] { keyAPPRV[i] }, dbtimeout);
                    if (conn.hasRow())
                    {
                        switch (conn.GetFieldValue("Status"))
                        {
                            case "Insert" :
                            case "Update" :
                                save(keyAPPRV[i]);
                                break;
                            case "Delete":
                                delete(keyAPPRV[i]);
                                break;
                        }
                    }
                }
            }
            dtbindpending(gridpending);
            if (gridpending.GroupCount > 0)
                funcpendCss = "";
            hAPPRV.Value = "";
            hREJCT.Value = "";
            hCANCL.Value = "";
        }

        public void delete(string temporaryid)
        {
            string createdBy = "", status = "";
            DateTime dateTime = DateTime.Now;
            string Cond = " Where 1=1 ";
            DataTable dtTemp = conn.GetDataTable(Q_PARAMTMP, new object[] { temporaryid }, dbtimeout);
            if (dtTemp.Rows.Count > 0)
            {
                createdBy = dtTemp.Rows[0]["CreatedBy"].ToString();
                if (dtTemp.Rows[0]["CreatedDate"].ToString() != "")
                    dateTime = Convert.ToDateTime(dtTemp.Rows[0]["CreatedDate"].ToString());
                status = dtTemp.Rows[0]["Status"].ToString();
            }

            NameValueCollection Keys = new NameValueCollection();
            NameValueCollection Fields = new NameValueCollection();
            
            DataTable dtdata = conn.GetDataTable(Q_PARAMTMPDET, new object[] { temporaryid }, dbtimeout);
            NameValueCollection nvcData = new NameValueCollection();
            for (int j = 0; j < dtdata.Rows.Count; j++)
                nvcData[dtdata.Rows[j]["FIELDNAME"].ToString().ToUpper()] = dtdata.Rows[j]["FIELDVALUE"].ToString();

            for (int i = 0; i < dtparam.Rows.Count; i++)
            {
                string FieldNm = dtparam.Rows[i]["FIELDNM"].ToString().ToUpper();
                if (nvcData[FieldNm] == null)
                    continue;
                string FieldType = dtparam.Rows[i]["FIELDTYPE"].ToString();

                bool FieldKey = (bool)dtparam.Rows[i]["FIELDKEY"];
                string FieldValue = nvcData[FieldNm];
                if (FieldKey)
                {
                    Cond += " AND " + FieldNm + " = '" + FieldValue+"'";
                    staticFramework.saveNVC(Keys, FieldNm, FieldValue);
                }
            }

            if (nvcData["ACTIVE"] == null)
            {
                staticFramework.Delete(Keys, TableNm, conn);
                DataTable dt = conn.GetDataTable(@"Exec USP_CekParameter @1", new object[] { TableNm }, dbtimeout);
                if (dt.Rows.Count > 0 && Request.QueryString["moduleid"].ToString() == "40")
                {
                     conn.ExecNonQuery("Update " + TableNm + " Set UpdatedDate=Getdate(),UpdatedBy=@1 " + Cond, new object[] { createdBy }, dbtimeout);
                }
            }
            else
            {
                staticFramework.saveNVC(Fields, "ACTIVE", "0");
                staticFramework.save(Fields, Keys, TableNm, conn);
                DataTable dt = conn.GetDataTable(@"Exec USP_CekParameter @1", new object[] { TableNm }, dbtimeout);
                if (Request.QueryString["moduleid"].ToString() == "40" && dt.Rows.Count > 0)
                {
                    conn.ExecNonQuery("Update " + TableNm + " Set UpdatedDate=Getdate(),UpdatedBy=@1 " + Cond, new object[] { createdBy }, dbtimeout);

                }
            }
            conn.ExecuteNonQuery(SP_AUDITTRAIL, new object[] { temporaryid, (string)Session["UserID"], Request.UserHostAddress }, dbtimeout);
        }

        public void save(string temporaryid)
        {
            try
            {
                NameValueCollection Keys = new NameValueCollection();
                NameValueCollection Fields = new NameValueCollection();
                string keyauotgen = "";

                DateTime dateTime = DateTime.Now;
                string createdBy = "", status = "";
                string cond = " where 1=1 ";
                DataTable dtTemp = conn.GetDataTable(Q_PARAMTMP, new object[] { temporaryid }, dbtimeout);
                if (dtTemp.Rows.Count > 0)
                {
                    createdBy = dtTemp.Rows[0]["CreatedBy"].ToString();
                    if (dtTemp.Rows[0]["CreatedDate"].ToString() != "")
                        dateTime = Convert.ToDateTime(dtTemp.Rows[0]["CreatedDate"].ToString());
                    status = dtTemp.Rows[0]["Status"].ToString();
                }

                DataTable dtdata = conn.GetDataTable(Q_PARAMTMPDET, new object[] { temporaryid }, dbtimeout);
                NameValueCollection nvcData = new NameValueCollection();
                for (int j = 0; j < dtdata.Rows.Count; j++)
                    nvcData[dtdata.Rows[j]["FIELDNAME"].ToString()] = dtdata.Rows[j]["FIELDVALUE"].ToString();

                for (int i = 0; i < dtparam.Rows.Count; i++)
                {
                    string FieldNm = dtparam.Rows[i]["FIELDNM"].ToString();
                    if (nvcData[FieldNm] == null)
                        continue;
                    string FieldType = dtparam.Rows[i]["FIELDTYPE"].ToString();
                    bool FieldKey = dtparam.Rows[i]["FIELDKEY"] == System.DBNull.Value ? false : (bool)dtparam.Rows[i]["FIELDKEY"];
                    //bool FieldKey = (bool)dtparam.Rows[i]["FIELDKEY"];
                    string FieldValue = nvcData[FieldNm];
                    if (FieldKey)
                    {
                        staticFramework.saveNVC(Keys, FieldNm, FieldValue);
                        if (FieldType == "auto" && nvcData[FieldNm] == "[AUTOGENERATE]")
                        {
                            Keys[FieldNm] = null;
                            keyauotgen = keyauotgen + dtparam.Rows[i]["FIELDAUTO"].ToString() + "\n";
                        }

                    }
                    else
                        staticFramework.saveNVC(Fields, FieldNm, FieldValue);
                }
                if (Fields.Count > 0)
                {

                    DataTable dt = conn.GetDataTable(@"Exec USP_CekParameter @1", new object[] { TableNm }, dbtimeout);
                    if (dt.Rows.Count > 0)
                    {

                        if (status.ToUpper() == "INSERT" && Request.QueryString["moduleid"].ToString() == "40")
                        {
                            staticFramework.saveNVC(Fields, "CreatedBy", createdBy);
                            staticFramework.saveNVC(Fields, "CreatedDate", dateTime);
                        }
                        else if (status.ToUpper() == "UPDATE" && Request.QueryString["moduleid"].ToString() == "40")
                        {
                            staticFramework.saveNVC(Fields, "UpdatedBy", createdBy);
                            staticFramework.saveNVC(Fields, "UpdatedDate", dateTime);
                        }
                    }

                    staticFramework.save(Fields, Keys, TableNm, keyauotgen, conn);
                }
                else
                {
                    staticFramework.insert(Fields, Keys, TableNm, keyauotgen, conn);
                    for (int j = 1; j < Keys.Count; j++)
                    {
                        if (nvcData[Keys.GetKey(j)] == "[AUTOGENERATE]")
                        {
                            conn.ExecNonQuery("UPDATE PARAMETERSYSTEM_TEMPORARYDETAIL SET FIELDVALUE=@3 WHERE TEMPORARYID=@1 AND FIELDNAME=@2"
                                , new object[] { temporaryid, Keys.GetKey(j), keyauotgen[j] }, dbtimeout);
                        }
                    }
                }

                //saveCreateUpdate(temporaryid, TableNm, dateTime, createdBy, status);

                conn.ExecuteNonQuery(SP_AUDITTRAIL, new object[] { temporaryid, (string)Session["UserID"], Request.UserHostAddress }, dbtimeout);
            }
            catch (Exception ex) 
            {
                string errmsg = "";
                if (ex.Message.IndexOf("Last Query:") > 0)
                    errmsg = ex.Message.Substring(0, ex.Message.IndexOf("Last Query:"));
                else
                    errmsg = ex.Message;

                lblErrorDet.Text = ex.Message;
                lblError.Text = errmsg;
                btn_showdet.Attributes["style"] = "display:''";
            }
        }

        //public void save(string temporaryid, string TableNm, DateTime CreatedDate, string CreatedBy, string Status)
        protected void saveCreateUpdate(string temporaryid, string TableNm, DateTime dateTime, string createdBy, string status)
        {
            DataTable dtParam = conn.GetDataTable("SELECT * FROM PARAMETERSYSTEM_TEMPORARYDETAIL WHERE TABLENM=@1", new object[]{TableNm},dbtimeout);
            if(dtparam.Rows.Count>0)
            {
                
            }


            NameValueCollection Keys =new  NameValueCollection();
            NameValueCollection Fields =new NameValueCollection();

            
            if (status.ToUpper() == "INSERT")
            {
                
            }
            else if(status.ToUpper() =="UPDATE")
            { 
            
            }
        }
    }
}
