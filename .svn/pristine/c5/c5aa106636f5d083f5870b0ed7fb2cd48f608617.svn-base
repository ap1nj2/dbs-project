using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using ClosedXML.Excel;

namespace CBS.Testing
{
    public partial class Bureau_Recheck : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string appid = "";
            string name = "";
            string id = "";

            if (!IsPostBack)
            {
                DataTable dt = GetData(appid, name, id);
                GenerateTable(dt);
            }
        }

        private void GenerateTable(DataTable data)
        {
            DataTable dt = data;
            //Building an HTML string.
            StringBuilder html = new StringBuilder();
            //Table start.
            html.Append("<table class='table table-bordered'>");
            //Building the Header row.
            html.Append("<tr>");
            foreach (DataColumn column in dt.Columns)
            {
                html.Append("<th>");
                html.Append(column.ColumnName);
                html.Append("</th>");
            }
            html.Append("</tr>");
            //Building the Data rows.
            foreach (DataRow row in dt.Rows)
            {
                html.Append("<tr>");
                foreach (DataColumn column in dt.Columns)
                {
                    html.Append("<td>");
                    html.Append(row[column.ColumnName]);
                    html.Append("</td>");
                }
                //html.Append("<td><input runat='server' type='checkbox' name='"+ row[0] + "' id='" + row[0] + "' /></td>");
                html.Append("</tr>");
            }
            //Table end.
            html.Append("</table>");
            string strText = html.ToString();
            ////Append the HTML string to Placeholder.
            placeholder.Controls.Add(new Literal { Text = html.ToString() });
        }

        public DataTable GetData(string appid,string cust_name,string id_number)
        {
            string constr = ConfigurationManager.ConnectionStrings["DBSCBSConnectionString"].ConnectionString;
            string query = @"SELECT 
                    AppId as 'APP ID',
                    Name as 'Customer Name',
                    ReqTime as 'Receipt Date',
                    IDNumber as 'Customer Number',
                    IDType as 'Mother NM',
                    DateOfBirth as 'Date of Birth',
                    ReqType as 'Bureau Type' 
                    from AppBureauReq
                    where AppId like '%"+ appid + @"%'
                    and Name like '%"+ cust_name + @"%'
                     ";
            using (SqlConnection conn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = conn;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
        }

        protected void view_button_Click(object sender, EventArgs e)
        {
            string appid = app_id.Value;
            string name = cust_name.Value;
            string id = id_number.Value;
            DataTable dt = GetData(appid, name, id);
            GenerateTable(dt);
        }

        public void CreateExcelFile(DataTable Excel)
        {

            //Clears all content output from the buffer stream.  
            Response.ClearContent();
            //Adds HTTP header to the output stream  
            DateTime dta = DateTime.Now;
            string _time_ = String.Format("{0:ddMMyyyyHHmm}", dta);
            string filename = "Bureau_Checker_Generate_"+_time_;
            Response.AddHeader("content-disposition", string.Format("attachment; filename="+filename+".xls"));

            // Gets or sets the HTTP MIME type of the output stream  
            Response.ContentType = "application/vnd.ms-excel";
            string space = "";

            foreach (DataColumn dcolumn in Excel.Columns)
            {

                Response.Write(space + dcolumn.ColumnName);
                space = "\t";
            }
            Response.Write("\n");
            int countcolumn;
            foreach (DataRow dr in Excel.Rows)
            {
                space = "";
                for (countcolumn = 0; countcolumn < Excel.Columns.Count; countcolumn++)
                {
                    Response.Write(space + dr[countcolumn].ToString());
                    space = "\t";
                }
                Response.Write("\n");
            }
            Response.End();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string appid = app_id.Value;
            string name = cust_name.Value;
            string id = id_number.Value;
            DataTable dt =  GetData( appid, name, id);
            CreateExcelFile(dt);
        }

        //protected void ExportExcel(object sender, EventArgs e)
        //{
        //    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
        //    using (SqlConnection con = new SqlConnection(constr))
        //    {
        //        using (SqlCommand cmd = new SqlCommand("SELECT * FROM Customers"))
        //        {
        //            using (SqlDataAdapter sda = new SqlDataAdapter())
        //            {
        //                cmd.Connection = con;
        //                sda.SelectCommand = cmd;
        //                using (DataTable dt = new DataTable())
        //                {
        //                    sda.Fill(dt);
        //                    using (XLWorkbook wb = new XLWorkbook())
        //                    {
        //                        wb.Worksheets.Add(dt, "Customers");

        //                        Response.Clear();
        //                        Response.Buffer = true;
        //                        Response.Charset = "";
        //                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //                        Response.AddHeader("content-disposition", "attachment;filename=SqlExport.xlsx");
        //                        using (MemoryStream MyMemoryStream = new MemoryStream())
        //                        {
        //                            wb.SaveAs(MyMemoryStream);
        //                            MyMemoryStream.WriteTo(Response.OutputStream);
        //                            Response.Flush();
        //                            Response.End();
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

    }
}