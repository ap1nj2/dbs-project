using System;
using System.Collections;
using System.Collections.Generic;
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
using DMS.Framework;
using DMS.Tools;

namespace CBS.Web.UI.Param
{
    public partial class ParamMaker_USC : System.Web.UI.UserControl
    {
        DbConnection conn = null;
        int dbtimeout = 600;
        public USC_PopUp_Cascade ucpuc1;
        DataTable dtparam = null;
        string TableNm = "";
        const string _autogen = "[AUTOGENERATE]";
        DataTable dtparamschema;
        string sqldata, sqlpending1, sqlpending2, sqlsortby;
       
        
        #region static vars

        private static string Q_PARAMSYS = "SELECT * FROM PARAMETERSYSTEMFIELD WHERE TABLENM = @1 ORDER BY FIELDPOS";

        #endregion static vars

        #region grid

        protected void creategridquery()
        {
            string fieldstr = "", pfieldstr = "", tempfieldstr = "", condstr = "";
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
                    if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Double")
                        Key += "+' AND [" + FieldNm + "]='''+ CONVERT(VARCHAR(200), convert(decimal(20,8), [" + FieldNm + "]))+''''";
                    else
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

                    fieldstr += ", " + "(SELECT " + FieldReffDesc + " " + FieldReffFrom + FieldReffWhere + FieldReffId + "=[X].[" + FieldNm + "]) AS [" + FieldNm + "_DESC]"; ;
                    pfieldstr += ", " + "(SELECT " + FieldReffDesc + " " + FieldReffFrom + FieldReffWhere + FieldReffId + "=[X].[" + FieldNm + "]) AS [" + FieldNm + "_DESC]"; ;
                }
                if (AutoPrefix.Length > 0 || AutoSufix.Length > 0)
                {
                    int prelen, sulen;
                    prelen = 1 + AutoPrefix.Length;
                    sulen = AutoSufix.Length + AutoPrefix.Length;
                    fieldstr += ", substring([" + FieldNm + "], " + prelen.ToString() + ", len(" + FieldNm + ") - " + sulen.ToString() + ") as [" + FieldNm + "]";
                    pfieldstr += ", substring([" + FieldNm + "], " + prelen.ToString() + ", len(" + FieldNm + ") - " + sulen.ToString() + ") as [" + FieldNm + "]";
                }
                else
                {
                    if (FieldNm.ToLower() == "active")
                        fieldstr += ", case when " + FieldNm + " = 1 then 'Aktif' else 'No' end as [" + FieldNm + "]";
                    else
                        fieldstr += ", [" + FieldNm + "]";
                    if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Double" ||
                            dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Decimal")
                        pfieldstr += ", convert(float, [" + FieldNm + "]) as [" + FieldNm + "]";
                    else if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.DateTime")
                        pfieldstr += ", convert(datetime, [" + FieldNm + "]) as [" + FieldNm + "]";
                    else if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Boolean")
                        //20141031
                        //pfieldstr += ", convert(bit, [" + FieldNm + "]) as [" + FieldNm + "]";
                        pfieldstr += ", case when " + FieldNm + " = 1 then 'Aktif' else 'No' end as [" + FieldNm + "]";
                    else
                        pfieldstr += ", [" + FieldNm + "]";
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
            sqlpending1 = "SELECT Y.TEMPORARYID __KEY, Y.STATUS __STATUS, Y.CREATEDBY __CREATEDBY" + tempfieldstr + " FROM PARAMETERSYSTEM_TEMPORARY Y " +
                         "WHERE Y.TableName='" + TableNm + "'";
            sqlpending2 = "SELECT __KEY,__STATUS,__CREATEDBY" + pfieldstr + " FROM (" + sqlpending1 + ") [X] " + condstr;
            sqlsortby = "";
            for (int i = 0; i < sortby.Count; i++)
                sqlsortby += (string)sortby.GetByIndex(i) + ", ";
            sortby.Clear();
            if (sqlsortby != "")
                sqlsortby = sqlsortby.Substring(0, sqlsortby.Length - 2);
        }

        public void createGridColumns(DevExpress.Web.ASPxGridView grid, DevExpress.Web.ASPxGridView gridpending)
        {
            DevExpress.Web.GridViewDataTextColumn p;
            DevExpress.Web.GridViewDataTextColumn c;
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
                bool GroupBy = false;
                if (dtparam.Rows[i]["GROUPBY"].ToString() != "")
                    GroupBy = (bool)dtparam.Rows[i]["GROUPBY"];
                switch (FieldType)
                {
                    case "d":
                        break;
                }

                c = new DevExpress.Web.GridViewDataTextColumn();
                if (FieldReff != "")
                    c.FieldName = FieldNm + "_DESC";
                else
                    c.FieldName = FieldNm;
                c.Caption = FieldDesc;
                c.Settings.FilterMode = ColumnFilterMode.Value;
                c.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                if ((dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Guid" && FieldReff.Trim() == "") || isAuto)
                    c.Visible = false;
                else if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Double")
                    c.PropertiesTextEdit.DisplayFormatString = "###,##0.####";
                else if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Decimal")
                    c.PropertiesTextEdit.DisplayFormatString = "###,##0.####";
                else if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.DateTime")
                    c.PropertiesTextEdit.DisplayFormatString = "dd/MM/yyyy";
                grid.Columns.Add(c);
                if (GroupBy)
                    grid.GroupBy(c);

                p = new DevExpress.Web.GridViewDataTextColumn();
                if (FieldReff != "")
                    p.FieldName = FieldNm + "_DESC";
                else
                    p.FieldName = FieldNm;
                p.Caption = FieldDesc;
                p.Settings.FilterMode = ColumnFilterMode.Value;
                p.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                if ((dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Guid" && FieldReff.Trim() == "") || isAuto)
                    p.Visible = false;
                else if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Double")
                    c.PropertiesTextEdit.DisplayFormatString = "###,##0.####";
                else if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Decimal")
                    c.PropertiesTextEdit.DisplayFormatString = "###,##0.####";
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

            grid.KeyFieldName = "__KEY";
            gridpending.KeyFieldName = "__KEY";
        }

        public void dtbinddata(DevExpress.Web.ASPxGridView grid)
        {
            DataTable dt = conn.GetDataTable(sqldata, null, dbtimeout);
            DataView dv = new DataView(dt);
            dv.Sort = sqlsortby;
            grid.DataSource = dv;
            grid.DataBind();
        }

        public void dtbindpending(DevExpress.Web.ASPxGridView gridpending)
        {
            DataTable dt = conn.GetDataTable(sqlpending2, null, dbtimeout);
            DataView dv = new DataView(dt);
            dv.Sort = sqlsortby;
            gridpending.DataSource = dv;
            gridpending.DataBind();
        }

        #endregion grid

        #region data

        public void savepending(string _keyfield)
        {
            dynamicFramework dyn = null;
            NameValueCollection Fields = new NameValueCollection();
            string fieldstr = "", keyfield = "", tempfieldstr = "T2.TEMPORARYID", tempkeyfield = "", fieldauto = "";
            bool insertnew = _keyfield == null;
            for (int i = 0; i < dtparam.Rows.Count; i++)
            {
                string FieldNm = dtparam.Rows[i]["FIELDNM"].ToString();
                string FieldDesc = dtparam.Rows[i]["FIELDDESC"].ToString();
                string FieldType = dtparam.Rows[i]["FIELDTYPE"].ToString();
                string FieldReff = dtparam.Rows[i]["FIELDREFF"].ToString();
                bool FieldKey = false;
                if (dtparam.Rows[i]["FIELDKEY"].ToString() != "")
                    FieldKey = (bool)dtparam.Rows[i]["FIELDKEY"];
                string FormulaType = dtparam.Rows[i]["FormulaType"].ToString();
                bool isAuto = dtparam.Rows[i]["FIELDAUTO"].ToString().Trim() != "";
                string AutoPrefix = "", AutoSufix = "";
                if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.String"                   //autoprefix and autosuffix must not be used to a non string fieldtype
                        && isAuto == false && FieldReff == "")                                              //and non-auto and non refferential field
                {
                    AutoPrefix = dtparam.Rows[i]["AUTOPREFIX"].ToString();
                    AutoSufix = dtparam.Rows[i]["AUTOSUFIX"].ToString();
                }
                bool locked = false;
                if (dtparam.Rows[i]["LOCKED"].ToString() != "")
                    locked = (bool)dtparam.Rows[i]["LOCKED"];

                fieldstr += ",[" + FieldNm + "]";
                tempfieldstr += ",(SELECT FIELDVALUE FROM PARAMETERSYSTEM_TEMPORARYDETAIL " +
                                "  WHERE FIELDNAME = '" + FieldNm + "' AND TEMPORARYID=T2.TEMPORARYID)[" + FieldNm + "] ";

                if (_keyfield == null)
                {
                    Control oCtrl = this.FindControl(FieldNm);
                    if (locked)
                        oCtrl = this.FindControl("l_" + FieldNm);          //overwrite values with original locked field value from the hidden "l_" prefix controls
                    if (FieldKey)
                    {
                        HtmlInputHidden hCtrl = (HtmlInputHidden)this.FindControl("h_" + FieldNm);
                        if (hCtrl.Value.Trim() != "")       //hidden control set during retrieve
                        {
                            //oCtrl = hCtrl;                  //always use key value from edited keys... oh well, maybe dont force edit if user want to change the key field value.. ease creation of large param with little differences on the param fields..
                            if (staticFramework.getvalue(oCtrl) == null || staticFramework.getvalue(oCtrl).ToString().Trim() == "")
                                oCtrl = hCtrl;              //in disabled mode, sometimes the control doesnt pass the value
                            if (staticFramework.getvalue(oCtrl).ToString() == staticFramework.getvalue(hCtrl).ToString())       //falsefy only if oCtrl value not changed.. meaning: dont do insertion cek..
                                insertnew = false;
                        }
                    }
                    Fields[FieldNm] = staticFramework.toSql(staticFramework.getvalue(oCtrl));
                    if (AutoPrefix.Length > 0 || AutoSufix.Length > 0)
                    {
                        Fields[FieldNm] = staticFramework.toSql(AutoPrefix + staticFramework.getvalue(oCtrl) + AutoSufix);
                    }
                    if (FieldType == "auto" && Fields[FieldNm].IndexOf(_autogen) >= 0)
                    {
                        string thisfieldauto = dtparam.Rows[i]["FIELDAUTO"].ToString();
                        if (thisfieldauto.StartsWith("q:") ||
                            thisfieldauto.StartsWith("s:") ||
                            thisfieldauto.StartsWith("f:"))
                        {
                            string thisautovalue = "", thisautotype = "";

                            if (thisfieldauto.StartsWith("q:"))
                                thisautovalue = "'" + Request.QueryString[thisfieldauto.Substring(2)] + "'";
                            else if (thisfieldauto.StartsWith("s:"))
                                thisautovalue = "'" + Session[thisfieldauto.Substring(2)].ToString() + "'";
                            else if (thisfieldauto.StartsWith("f:"))
                                thisautovalue = thisfieldauto.Substring(2);

                            if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.String")
                                thisautotype = "varchar(" + dtparamschema.Columns[FieldNm].MaxLength.ToString() + ")";
                            else if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Guid")
                                thisautotype = "uniqueidentifier";
                            else if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Boolean")
                                thisautotype = "bit";
                            else if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.DateTime")
                                thisautotype = "datetime";
                            else if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Int16" ||
                                dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Int32" ||
                                dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Int64")
                                thisautotype = "int";
                            else if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Double" ||
                                dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Decimal")
                                thisautotype = "float";

                            thisfieldauto = "DECLARE @" + FieldNm + " " + thisautotype + " SET @" + FieldNm + " = " + thisautovalue;
                        }

                        Fields[FieldNm] = "@" + FieldNm;
                        fieldauto += thisfieldauto + Environment.NewLine;
                    }
                    if (FieldReff.IndexOf(":[") > 0 && FieldReff.Trim().ToLower().IndexOf("select") == 0)
                    {   //special check for cascading refferencial FieldType
                        string qry = FieldReff.Trim();
                        qry = qry.Substring(qry.IndexOf(" ") + 1).Trim();
                        string fldkey = qry.Substring(0, qry.IndexOf(",")).Trim();
                        qry = qry.Substring(qry.IndexOf(",") + 1).Trim();
                        int cekpos = qry.IndexOf(",");
                        if (cekpos == -1 || cekpos > qry.IndexOf(" "))
                            cekpos = qry.IndexOf(" ");
                        string flddesc = qry.Substring(0, cekpos).Trim();
                        qry = qry.Substring(qry.ToLower().IndexOf("from") + 5).Trim();
                        string tblname = qry.Substring(0, qry.ToLower().IndexOf("where")).Trim();
                        qry = "select " + fldkey + " from " + tblname + " where " + fldkey + " = " + Fields[FieldNm];
                        object[] cekpar = new object[] { Fields[FieldNm] };
                        conn.ExecReader(qry, cekpar, dbtimeout);                //cek if Fields[FieldNm] contains correct key field
                        if (!conn.hasRow())
                        {                                                       //if not, cek if Fields[FieldNm] contains desc field
                            qry = "select " + fldkey + " from " + tblname + " where " + flddesc + " = " + Fields[FieldNm];
                            conn.ExecReader(qry, cekpar, dbtimeout);
                            if (conn.hasRow())
                            {                                                   //if so, renew Fields[FieldNm] to have value from key field
                                Fields[FieldNm] = staticFramework.toSql(conn.GetNativeFieldValue(fldkey));
                            }
                        }
                    }
                    /* */
                    if (staticFramework.getvalue(oCtrl) != null && staticFramework.getvalue(oCtrl).ToString() != "")
                        switch (FormulaType)
                        {
                            case "1":
                                if (dyn == null)
                                    dyn = new dynamicFramework(conn);
                                try
                                {
                                    string fieldvalue = staticFramework.getvalue(oCtrl).ToString();
                                    string formula = dyn.retrvCondFW(fieldvalue);
                                    string formulaquery = dyn.Retrieve(formula, "").ToLower().Replace("select ", "select top 0 ");
                                    conn.ExecNonQuery(formulaquery, null, dbtimeout);
                                }
                                catch (Exception exd)
                                {
                                    string errmsg = exd.Message;
                                    if (errmsg.IndexOf("Last Query") > 0)
                                        errmsg = errmsg.Substring(0, errmsg.IndexOf("Last Query"));
                                    string msg = "Invalid formula in " + FieldDesc + "\nErrMsg: " + errmsg;
                                    throw new Exception(msg);
                                }
                                break;
                            case "2":
                                if (dyn == null)
                                    dyn = new dynamicFramework(conn);
                                try
                                {
                                    string[] strsep = new string[] { "&&", "||" };
                                    string[] fieldvalues = staticFramework.getvalue(oCtrl).ToString().Split(strsep, StringSplitOptions.RemoveEmptyEntries);
                                    foreach (string fieldvalue in fieldvalues)
                                    {
                                        string formulacond = " and " + dyn.retrvCondFW(fieldvalue);
                                        string formulaquery = dyn.Retrieve("1", formulacond).ToLower().Replace("select ", "select top 0 ").Replace("@value", "null");
                                        conn.ExecNonQuery(formulaquery, null, dbtimeout);
                                    }
                                }
                                catch (Exception exd)
                                {
                                    string errmsg = exd.Message;
                                    if (errmsg.IndexOf("Last Query") > 0)
                                        errmsg = errmsg.Substring(0, errmsg.IndexOf("Last Query"));
                                    string msg = "Invalid formula in " + FieldDesc + "\nErrMsg: " + errmsg;
                                    throw new Exception(msg);
                                }
                                break;
                            case "3":
                                if (dyn == null)
                                    dyn = new dynamicFramework(conn);
                                try
                                {
                                    string fieldvalue = staticFramework.getvalue(oCtrl).ToString();
                                    string formulacond = " and " + dyn.retrvCondFW(fieldvalue);
                                    string formulaquery = dyn.Retrieve("1", formulacond).ToLower().Replace("select ", "select top 0 ");
                                    conn.ExecNonQuery(formulaquery, null, dbtimeout);
                                }
                                catch (Exception exd)
                                {
                                    string errmsg = exd.Message;
                                    if (errmsg.IndexOf("Last Query") > 0)
                                        errmsg = errmsg.Substring(0, errmsg.IndexOf("Last Query"));
                                    string msg = "Invalid formula in " + FieldDesc + "\nErrMsg: " + errmsg;
                                    throw new Exception(msg);
                                }
                                break;
                            default:
                                break;
                        }
                    /* */

                    if (FieldKey)
                    {
                        keyfield += " AND [" + FieldNm + "]=" + Fields[FieldNm];
                        tempkeyfield += " AND [" + FieldNm + "]=" + Fields[FieldNm];
                        tempkeyfield += " AND [" + FieldNm + "]<>" + staticFramework.toSql(_autogen);
                    }
                }
            }
            fieldstr = fieldstr.Substring(1);

            if (_keyfield != null)
            {
                keyfield = _keyfield;
                tempkeyfield = _keyfield;
            }

            string tempparamsql =
                fieldauto +
                "SELECT " +
                "(SELECT TOP 1 TEMPORARYID FROM " +
                "	(SELECT " + tempfieldstr + " FROM PARAMETERSYSTEM_TEMPORARY T1 " +
                "	JOIN PARAMETERSYSTEM_TEMPORARYDETAIL T2 ON T2.TEMPORARYID=T1.TEMPORARYID " +
                "	where T1.TableName='" +TableNm+"' "+
                "	GROUP BY T2.TEMPORARYID " +
                "	)X WHERE 1=1 " + keyfield +
                ") as col0, NEWID() as col1 ";
            conn.ExecReader(tempparamsql, null, dbtimeout);
            conn.hasRow();

            string TemporarID = conn.GetFieldValue(0);
            if (TemporarID == "")
                TemporarID = conn.GetFieldValue(1);
            else
                if (insertnew)
                    throw new Exception("Parameter with that key value had already been in the Pending Approval!");

            string SavingType = "Insert";
            DataTable dt = conn.GetDataTable(
                    fieldauto +
                    " SELECT " + fieldstr + " FROM " + TableNm +
                    " WHERE 1=1 " + keyfield
                    , null, dbtimeout);
            if (dt.Rows.Count > 0)
            {
                SavingType = "Update";
                if (insertnew)
                    throw new Exception("Parameter with that key value already exists!");
            }

            if (_keyfield != null)
                SavingType = "Delete";

            //validasi jika klik delete 2 kali
            DataTable dt2 = conn.GetDataTable("SELECT Status FROM PARAMETERSYSTEM_TEMPORARY WHERE TemporaryID = '" + TemporarID + "'", null, dbtimeout);
            if (dt2.Rows.Count > 0)
            {
                if (dt2.Rows[0][0].ToString() == "Delete")
                    throw new Exception("Parameter with that key value had already been in the Pending Approval!");
            }

            conn.ExecuteNonQuery(
                    "DELETE PARAMETERSYSTEM_TEMPORARY WHERE TEMPORARYID=@1"
                    , new object[] { TemporarID }, dbtimeout);
            conn.ExecuteNonQuery(
                "INSERT INTO PARAMETERSYSTEM_TEMPORARY " +
                "(TemporaryID,TableName,Status,CreatedBy) VALUES " +
                "(@1,@2,@3,@4)"
                , new object[] { TemporarID, TableNm, SavingType, Session["UserID"] }, dbtimeout);

            for (int col = 0; col < dt.Columns.Count; col++)
            {
                string valuebefore = staticFramework.toSql(null), value = staticFramework.toSql(null);
                if (dt.Rows.Count > 0)
                    valuebefore = staticFramework.toSql(dt.Rows[0][col]);
                if (Fields.Count > col)
                    value = Fields[col];
                else
                    value = valuebefore;

                conn.ExecuteNonQuery(
                    fieldauto +
                    "INSERT INTO PARAMETERSYSTEM_TEMPORARYDETAIL " +
                    "(TemporaryID,FieldName,FieldValue,FieldValueBefore) VALUES " +
                    "(" + staticFramework.toSql(TemporarID) + "," +
                         staticFramework.toSql(dt.Columns[col].ColumnName) + "," +
                         "CONVERT(VARCHAR(8000)," + value + ")," +
                         "CONVERT(VARCHAR(8000)," + valuebefore + ")" +
                    ")"
                    , null, dbtimeout);
            }
        }

        public void deletepending(string TemporaryID)
        {
            conn.ExecuteNonQuery(
                "DELETE PARAMETERSYSTEM_TEMPORARY WHERE TemporaryID=@1"
                , new object[] { TemporaryID }, dbtimeout);
        }

        public void retrieve(string keyvalue)
        {
            DataTable dtretrieve = conn.GetDataTable("SELECT * FROM " + TableNm + " WHERE 1=1 " + keyvalue, null, dbtimeout);
            for (int i = 0; i < dtparam.Rows.Count; i++)
            {
                string FieldNm = dtparam.Rows[i]["FIELDNM"].ToString();
                string FieldReff = dtparam.Rows[i]["FIELDREFF"].ToString();
                bool FieldKey = false;
                if (dtparam.Rows[i]["FIELDKEY"].ToString() != "")
                    FieldKey = (bool)dtparam.Rows[i]["FIELDKEY"];
                bool isAuto = dtparam.Rows[i]["FIELDAUTO"].ToString().Trim() != "";
                string AutoPrefix = "", AutoSufix = "";
                if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.String"                   //autoprefix and autosuffix must not be used to a non string fieldtype
                        && isAuto == false && FieldReff == "")                                              //and non-auto and non refferential field
                {
                    AutoPrefix = dtparam.Rows[i]["AUTOPREFIX"].ToString();
                    AutoSufix = dtparam.Rows[i]["AUTOSUFIX"].ToString();
                }
                bool locked = false;
                if (dtparam.Rows[i]["LOCKED"].ToString() != "")
                    locked = (bool)dtparam.Rows[i]["LOCKED"];

                Control oCtrl = (Control)TableInput.FindControl(FieldNm);

                if (oCtrl is USC_DDL_Cascade && (oCtrl as USC_DDL_Cascade).QueryString.IndexOf("@") != -1)
                    (oCtrl as USC_DDL_Cascade).FillDDL();
                if (AutoPrefix.Length > 0 || AutoSufix.Length > 0)
                {
                    string value = null;
                    if (dtretrieve.Rows.Count > 0)
                        value = dtretrieve.Rows[0][FieldNm].ToString();
                    if (value.Length > AutoPrefix.Length + AutoSufix.Length)
                        value = value.Substring(AutoPrefix.Length, value.Length - AutoSufix.Length - AutoPrefix.Length);
                    staticFramework.retrieve(value, oCtrl);
                }
                else
                    staticFramework.retrieve(dtretrieve, FieldNm, oCtrl);

                if (FieldKey)
                {
                    HtmlInputHidden hCtrl = (HtmlInputHidden)TableInput.FindControl("h_" + FieldNm);
                    staticFramework.retrieve(dtretrieve, FieldNm, hCtrl);
                }

                if (locked)
                {
                    HtmlInputHidden lCtrl = (HtmlInputHidden)TableInput.FindControl("l_" + FieldNm);
                    staticFramework.retrieve(dtretrieve, FieldNm, lCtrl);
                }
            }
        }

        public void retrievepending(string TemporaryID)
        {
            DataTable dtretrieve = conn.GetDataTable(
                sqlpending1 + " AND Y.TEMPORARYID=@1"
                , new object[] { TemporaryID }, dbtimeout);
            for (int i = 0; i < dtparam.Rows.Count; i++)
            {
                string FieldNm = dtparam.Rows[i]["FIELDNM"].ToString();
                string FieldReff = dtparam.Rows[i]["FIELDREFF"].ToString();
                bool FieldKey = false;
                if (dtparam.Rows[i]["FIELDKEY"].ToString() != "")
                    FieldKey = (bool)dtparam.Rows[i]["FIELDKEY"];
                bool isAuto = dtparam.Rows[i]["FIELDAUTO"].ToString().Trim() != "";
                string AutoPrefix = "", AutoSufix = "";
                if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.String"                   //autoprefix and autosuffix must not be used to a non string fieldtype
                        && isAuto == false && FieldReff == "")                                              //and non-auto and non refferential field
                {
                    AutoPrefix = dtparam.Rows[i]["AUTOPREFIX"].ToString();
                    AutoSufix = dtparam.Rows[i]["AUTOSUFIX"].ToString();
                }
                bool locked = false;
                if (dtparam.Rows[i]["LOCKED"].ToString() != "")
                    locked = (bool)dtparam.Rows[i]["LOCKED"];

                Control oCtrl = (Control)TableInput.FindControl(FieldNm);

                if (oCtrl is USC_DDL_Cascade && (oCtrl as USC_DDL_Cascade).QueryString.IndexOf("@") != -1)
                    (oCtrl as USC_DDL_Cascade).FillDDL();
                
                if (AutoPrefix.Length > 0 || AutoSufix.Length > 0)
                {
                    string value = null;
                    if (dtretrieve.Rows.Count > 0)
                        value = dtretrieve.Rows[0][FieldNm].ToString();
                    if (value.Length > AutoPrefix.Length + AutoSufix.Length)
                        value = value.Substring(AutoPrefix.Length, value.Length - AutoSufix.Length - AutoPrefix.Length);
                    staticFramework.retrieve(value, oCtrl);
                }
                else
                {
                    if (oCtrl is DMSControls.TXT_DECIMAL)
                    {
                        if (Convert.ToString((double)12.5).IndexOf(",") > 0)
                            dtretrieve.Rows[0][FieldNm] = dtretrieve.Rows[0][FieldNm].ToString().Replace(".", ",");
                    }
                    staticFramework.retrieve(dtretrieve, FieldNm, oCtrl);
                }
                if (FieldKey)
                {
                    HtmlInputHidden hCtrl = (HtmlInputHidden)TableInput.FindControl("h_" + FieldNm);
                    staticFramework.retrieve(dtretrieve, FieldNm, hCtrl);
                }

                if (locked)
                {
                    HtmlInputHidden lCtrl = (HtmlInputHidden)TableInput.FindControl("l_" + FieldNm);
                    staticFramework.retrieve(dtretrieve, FieldNm, lCtrl);
                }
            }
        }

        #endregion data

        #region control

        protected void recursive_USC_DDL_Cascade_callback(object[] ParamFilter, int i, ref string JavaScript)
        {
            for (int j = 0; j < ParamFilter.Length; j++)
            {
                if (ParamFilter[j] is USC_DDL_Cascade)
                {
                    string QueryString = (ParamFilter[j] as USC_DDL_Cascade).QueryString;
                    if (QueryString.IndexOf("@" + (i + 1).ToString()) != -1)
                    {
                        if (!JavaScript.Contains((ParamFilter[j] as USC_DDL_Cascade).JSCallback()))
                        {
                            JavaScript += (ParamFilter[j] as USC_DDL_Cascade).JSCallback();
                            recursive_USC_DDL_Cascade_callback(ParamFilter, j, ref JavaScript);
                        }
                    }
                }
            }
        }

        protected void recursive_USC_SearchCascade_callback(object[] ParamFilter, int i, ref string JavaScript)
        {
            for (int j = 0; j < ParamFilter.Length; j++)
            {
                if (ParamFilter[j] is USC_SearchCascade)
                {
                    string QueryString = (ParamFilter[j] as USC_SearchCascade).QueryString;
                    if (QueryString.IndexOf("@") != -1)
                    {
                        if (!JavaScript.Contains((ParamFilter[j] as USC_SearchCascade).getJSCallBack(ucpuc)))
                        {
                            JavaScript += (ParamFilter[j] as USC_SearchCascade).getJSCallBack(ucpuc);
                            recursive_USC_SearchCascade_callback(ParamFilter, j, ref JavaScript);
                        }
                    }
                }
            }
        }

        protected void initcontrol()
        {
            for (int i = 0; i < dtparam.Rows.Count; i++)
            {
                string FieldNm = dtparam.Rows[i]["FIELDNM"].ToString();
                string FieldType = dtparam.Rows[i]["FIELDTYPE"].ToString();
                string FieldReff = dtparam.Rows[i]["FIELDREFF"].ToString();
                Control oCtrl = (Control)TableInput.FindControl(FieldNm);
                if (dtparamschema.Columns[FieldNm].DataType.Name.ToLower() == "string")
                    staticFramework.retrieveschema(dtparamschema, FieldNm, oCtrl);

                if (oCtrl is DropDownList)
                    staticFramework.reff(oCtrl, FieldReff, null, conn);

                if (oCtrl is USC_SearchCascade)
                {
                    (oCtrl as USC_SearchCascade).conn = conn;
                    (oCtrl as USC_SearchCascade).QueryString = FieldReff;
                    (oCtrl as USC_SearchCascade).PopUpCascade(ucpuc, null);
                }

            }
        }

        protected string createcontrol()
        {
            List<object> ParamList = new List<object>();
            List<object> ParamListPop = new List<object>();
            List<object> AllCtrl = new List<object>();

            HtmlTableRow TableFilterRow;
            HtmlTableCell TableFilterDesc, TableFilterControl, TableFilterSeprtr;
            string clearjs = "";
            for (int i = 0; i < dtparam.Rows.Count; i++)
            {
                string FieldNm = dtparam.Rows[i]["FIELDNM"].ToString();
                string FieldDesc = dtparam.Rows[i]["FIELDDESC"].ToString();
                string FieldType = dtparam.Rows[i]["FIELDTYPE"].ToString();
                string FieldReff = dtparam.Rows[i]["FIELDREFF"].ToString();
                bool FieldKey = false;
                if (dtparam.Rows[i]["FIELDKEY"].ToString() != "")
                    FieldKey = (bool)dtparam.Rows[i]["FIELDKEY"];
                bool FieldMand = false;
                if (dtparam.Rows[i]["FIELDMAN"].ToString() != "")
                    FieldMand = (bool)dtparam.Rows[i]["FIELDMAN"];
                bool isAuto = dtparam.Rows[i]["FIELDAUTO"].ToString().Trim() != "";
                string FieldAuto = dtparam.Rows[i]["FIELDAUTO"].ToString().Trim();
                bool locked = false;
                if (dtparam.Rows[i]["LOCKED"].ToString() != "")
                    locked = (bool)dtparam.Rows[i]["LOCKED"];
                locked = locked || isAuto;

                TableFilterRow = new HtmlTableRow();
                TableInput.Rows.Add(TableFilterRow);
                if ((dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Guid" && FieldReff.Trim() == "") || isAuto)
                    TableFilterRow.Style["display"] = "none";
                TableFilterDesc = new HtmlTableCell();
                TableFilterSeprtr = new HtmlTableCell();
                TableFilterControl = new HtmlTableCell();
                TableFilterRow.Cells.Add(TableFilterDesc);
                TableFilterRow.Cells.Add(TableFilterSeprtr);
                TableFilterRow.Cells.Add(TableFilterControl);

                if (!IsPostBack && !Page.IsCallback)
                {
                    TableFilterDesc.InnerText = FieldDesc;
                    TableFilterDesc.Attributes.Add("Class", "B01");

                    TableFilterSeprtr.InnerText = ":";
                    TableFilterSeprtr.Attributes.Add("Class", "BS");

                    TableFilterControl.Attributes.Add("Class", "B11");
                }
                WebControl oCtrl = null;
                string devexpresseditorlib = this.Page.MapPath("~/bin") + "\\DevExpress.Web.v12.2.dll";
                string dmscontrollib = this.Page.MapPath("~/bin") + "\\DMSControls.dll";
                if (FieldType == "")
                {
                    if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Boolean")
                        FieldType = "chk";
                    else if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.DateTime")
                        FieldType = "date";
                    else if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Int16" ||
                        dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Int32" ||
                        dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Int64")
                        FieldType = "int";
                    else if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Double")
                        FieldType = "float";
                    else if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Decimal")
                        FieldType = "decimal";
                    //if (FieldReff != "")
                    //    FieldType = "ddl";
                    else if (FieldReff != "")
                        FieldType = "search";
                }
                string initvalue = "";
                if (FieldAuto.StartsWith("q:"))
                    initvalue = Request.QueryString[FieldAuto.Substring(2)];
                else if (FieldAuto.StartsWith("s:"))
                    initvalue = Session[FieldAuto.Substring(2)].ToString();
                switch (FieldType.ToLower())
                {
                    case "auto":
                        oCtrl = (WebControl)Reflection.CreateControl(TableFilterControl, FieldNm, "System.Web.UI.WebControls", "System.Web.UI.WebControls.TextBox");
                        oCtrl.BorderStyle = BorderStyle.None;
                        clearjs += "document.getElementById('" + oCtrl.ClientID + "').value = '" + _autogen + "';";
                        if (FieldKey)
                            clearjs += "document.getElementById('" + oCtrl.ClientID + "').disabled = false;";
                        AllCtrl.Add(oCtrl);
                        break;
                    case "float":
                        oCtrl = (WebControl)Reflection.CreateControl(TableFilterControl, FieldNm, dmscontrollib, "DMSControls.TXT_CURRENCY");
                        clearjs += "document.getElementById('" + oCtrl.ClientID + "').value = '" + initvalue + "';";
                        if (FieldKey)
                            clearjs += "document.getElementById('" + oCtrl.ClientID + "').disabled = false;";
                        AllCtrl.Add(oCtrl);
                        break;
                    case "decimal":
                        oCtrl = (WebControl)Reflection.CreateControl(TableFilterControl, FieldNm, dmscontrollib, "DMSControls.TXT_DECIMAL");
                        clearjs += "document.getElementById('" + oCtrl.ClientID + "').value = '" + initvalue + "';";
                        if (FieldKey)
                            clearjs += "document.getElementById('" + oCtrl.ClientID + "').disabled = false;";
                        AllCtrl.Add(oCtrl);
                        break;
                    case "int":
                        oCtrl = (WebControl)Reflection.CreateControl(TableFilterControl, FieldNm, dmscontrollib, "DMSControls.TXT_NUMBER");
                        clearjs += "document.getElementById('" + oCtrl.ClientID + "').value = '" + initvalue + "';";
                        if (FieldKey)
                            clearjs += "document.getElementById('" + oCtrl.ClientID + "').disabled = false;";
                        AllCtrl.Add(oCtrl);
                        break;
                    case "date":
                        oCtrl = (WebControl)Reflection.CreateControl(TableFilterControl, FieldNm, devexpresseditorlib, "DevExpress.Web.ASPxDateEdit");
                        (oCtrl as DevExpress.Web.ASPxDateEdit).ClientInstanceName = oCtrl.ClientID;
                        clearjs += oCtrl.ClientID + ".SetDate(null);";
                        if (FieldKey)
                            clearjs += oCtrl.ClientID + ".SetEnabled(true);";
                        AllCtrl.Add(oCtrl);
                        break;
                    case "chk":
                        oCtrl = (WebControl)Reflection.CreateControl(TableFilterControl, FieldNm, "System.Web.UI.WebControls", "System.Web.UI.WebControls.CheckBox");
                        if (FieldNm.ToUpper() == "ACTIVE")
                            clearjs += "document.getElementById('" + oCtrl.ClientID + "').checked = true;";
                        else
                            clearjs += "document.getElementById('" + oCtrl.ClientID + "').checked = false;";
                        if (FieldKey)
                            clearjs += "document.getElementById('" + oCtrl.ClientID + "').disabled = false;";
                        AllCtrl.Add(oCtrl);
                        break;
                    case "ddl":
                        USC_DDL_Cascade uCtrl = (USC_DDL_Cascade)this.Page.LoadControl("~\\UserControl\\USC_DDL_Cascade.ascx");
                        uCtrl.ID = FieldNm;
                        uCtrl.QueryString = FieldReff;
                        uCtrl.conn = conn;
                        uCtrl.CssClass = "inlinectrl";
                        uCtrl.xPanel.CssClass = "inlinectrl";
                        uCtrl.PanelContent1.CssClass = "inlinectrl";
                        uCtrl.DDL.CssClass = "inlinectrl";

                        TableFilterControl.Controls.Add(uCtrl);
                        AllCtrl.Add(uCtrl);
                        ParamList.Add(uCtrl);
                        if (FieldKey || FieldMand)
                            uCtrl.CssClass = "mandatory";

                        break;

                    case "search":

                        USC_SearchCascade pCtrl = (USC_SearchCascade)this.Page.LoadControl("USC_SearchCascade.ascx");
                        pCtrl.ID = FieldNm;
                        pCtrl.QueryString = FieldReff;
                        pCtrl.conn = conn;
                        if (FieldKey || FieldMand)
                            pCtrl.CssClass = "mandatory";
                        TableFilterControl.Controls.Add(pCtrl);
                        pCtrl.txtID.Enabled = false;
                        pCtrl.PopUpCascade(ucpuc, null);
                        AllCtrl.Add(pCtrl);
                        ParamListPop.Add(pCtrl);
                        

                        break;

                    default:
                        oCtrl = (WebControl)Reflection.CreateControl(TableFilterControl, FieldNm, "System.Web.UI.WebControls", "System.Web.UI.WebControls.TextBox");
                        clearjs += "document.getElementById('" + oCtrl.ClientID + "').value = '" + initvalue + "';";
                        if (FieldKey)
                            clearjs += "document.getElementById('" + oCtrl.ClientID + "').disabled = false;";
                        AllCtrl.Add(oCtrl);
                        break;
                }

                if (initvalue != "")
                    staticFramework.retrieve(initvalue, oCtrl);

                if (oCtrl != null && (FieldKey || FieldMand))
                    oCtrl.CssClass = "mandatory";

                if (FieldKey)
                {
                    HtmlInputHidden hCtrl = (HtmlInputHidden)Reflection.CreateControl(TableFilterControl, "h_" + FieldNm, "System.Web.UI.WebControls", "System.Web.UI.HtmlControls.HtmlInputHidden");
                    clearjs += "document.getElementById('" + hCtrl.ClientID + "').value = '" + initvalue + "';";
                }

                if (locked)
                {
                    oCtrl.Enabled = false;
                    HtmlInputHidden lCtrl = (HtmlInputHidden)Reflection.CreateControl(TableFilterControl, "l_" + FieldNm, "System.Web.UI.WebControls", "System.Web.UI.HtmlControls.HtmlInputHidden");
                }
            }

            object[] ParamFilter = new object[AllCtrl.Count];

            for (int i = 0; i < AllCtrl.Count; i++)
                ParamFilter[i] = AllCtrl[i];

            for (int i = 0; i < ParamList.Count; i++)
            {
                (ParamList[i] as USC_DDL_Cascade).QueryParam = ParamFilter;
                string QueryString = (ParamList[i] as USC_DDL_Cascade).QueryString;
                clearjs += (ParamList[i] as USC_DDL_Cascade).JSSetValue("''");

                if (QueryString.IndexOf(":[") != -1)
                    clearjs += (ParamList[i] as USC_DDL_Cascade).JSCallback();

                for (int j = ParamFilter.Length; j >= 1; j--)
                {
                    if (QueryString.IndexOf(":[" + (ParamFilter[j - 1] as Control).ID.ToString() + "]") != -1)
                    {
                        if ((ParamFilter[j - 1]) is USC_DDL_Cascade)
                        {
                            (ParamFilter[j - 1] as USC_DDL_Cascade).JavaScript += (ParamList[i] as USC_DDL_Cascade).JSCallback();
                            QueryString = QueryString.Replace(":[" + (ParamFilter[j - 1] as Control).ID.ToString() + "]", "@" + j);
                        }
                        else
                        {
                            throw new Exception("Parent reff must be of type USC_DDL_Cascade");
                        }
                    }
                }
                (ParamList[i] as USC_DDL_Cascade).QueryString = QueryString;
                if (!IsPostBack && !this.Page.IsCallback)
                    (ParamList[i] as USC_DDL_Cascade).FillDDL();
            }

            for (int i = 0; i < ParamListPop.Count; i++)
            {
                (ParamListPop[i] as USC_SearchCascade).QueryParam = ParamFilter;
                string QueryString = (ParamListPop[i] as USC_SearchCascade).QueryString;
                if (QueryString.IndexOf(":[") != -1)
                    clearjs += (ParamListPop[i] as USC_SearchCascade).getJSCallBack(ucpuc);

                for (int j = ParamFilter.Length; j >= 1; j--)
                {
                    if (QueryString.IndexOf(":[" + (ParamFilter[j - 1] as Control).ID.ToString() + "]") != -1)
                    {
                        if ((ParamFilter[j - 1]) is USC_SearchCascade)
                        {
                            (ParamFilter[j - 1] as USC_SearchCascade).PopUpCascade(ucpuc, (ParamListPop[i] as USC_SearchCascade).getJSCallBack(ucpuc));
                            QueryString = QueryString.Replace(":[" + (ParamFilter[j - 1] as Control).ID.ToString() + "]", "@" + j);
                        }
                        else
                        {
                            throw new Exception("Parent reff must be of type USC_SearchCascade");
                        }
                    }
                }
                (ParamListPop[i] as USC_SearchCascade).QueryString = QueryString;
                (ParamListPop[i] as USC_SearchCascade).PopUpCascade(ucpuc, null);
            }

            for (int i = 0; i < ParamFilter.Length; i++)
            {
                if (ParamFilter[i] is USC_DDL_Cascade)
                {
                    string JavaScript = "";
                    recursive_USC_DDL_Cascade_callback(ParamFilter, i, ref JavaScript);
                    (ParamFilter[i] as USC_DDL_Cascade).JavaScript = JavaScript;
                }

                if (ParamFilter[i] is USC_SearchCascade)
                {
                    string JavaScript = "";
                    recursive_USC_SearchCascade_callback(ParamFilter, i, ref JavaScript);
                    (ParamFilter[i] as USC_SearchCascade).JavaScript = JavaScript;
                }
            }
            return clearjs;
        }

        #endregion control

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            conn = new DbConnection((string)Session["ConnString"]);
            TableNm = Request.QueryString["TBLNM"];

            object[] param = new object[] { TableNm };

            dtparam = conn.GetDataTable(
                "SELECT * FROM PARAMETERSYSTEMFIELD WHERE TABLENM = @1 ORDER BY FIELDPOS"
                , param, dbtimeout);
            dtparamschema = conn.GetDataTable("SELECT TOP 0 * FROM " + TableNm, null, dbtimeout, true, true);
            for (int i = 0; i < dtparam.Rows.Count; i++)
            {
                object Field = dtparam.Rows[i]["FIELDNM"].ToString();

                //staticFramework.retrieveschema(dtparamschema, Field);
            }

            clrButton.Attributes["onclick"] = createcontrol();
            if (Request.QueryString["readonly"] != null)
            {
                btnSave.Visible = false;
            }

            creategridquery();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && !Page.IsCallback)
                initcontrol();
        }
    }
}