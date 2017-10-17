using DMS.Framework;
using DMS.Tools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SLIK.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Web.Script.Serialization;

namespace CBS.ImportExportSLIK
{
    public class ImportSLIK
    {
        public class Person
        {
            public string Name { get; set; }
            public string  Address { get; set; }
        }

        private void SaveSLIKDoc(string appId, string docCode, DbConnection conn)
        {
            NameValueCollection Keys = new NameValueCollection();
            NameValueCollection Fields = new NameValueCollection();
            staticFramework.saveNVC(Keys, "AppId", appId);
            staticFramework.saveNVC(Fields, "DocCd", docCode);
            staticFramework.save(Fields, Keys, "AppDoc",
                "DECLARE @Seq INT \n" +
                "SELECT @Seq=ISNULL(MAX(Seq),0)+1 FROM AppDoc " +
                "WHERE AppId='" + appId + "' \n"
                , conn);
        }

        public Dictionary<string, object> ImportSLIKinputFile(string slikFilePath, string connString, int dbTimeOut)
        {
            connString = "Data Source=103.14.20.19\rentas;Initial Catalog=DBSCBS;User ID=DBSCBSUSER;Password=ASDqwe123!@#";
            Dictionary<string, object> result = new Dictionary<string, object>();
            result["Message"] = "";
            result["LogSummary"] = null;
            result["LogDetail"] = null;
            try
            {
                FileInfo slikFile = new FileInfo(slikFilePath);
                string detailLogFileName = Path.GetFileNameWithoutExtension(slikFilePath) + "_detail.log";
                string summLogFileName = Path.GetFileNameWithoutExtension(slikFilePath) + "_summary.log";
                result["LogDetail"] = new FileInfo(Path.GetFullPath(slikFilePath) + detailLogFileName);
                result["LogSummary"] = new FileInfo(Path.GetFullPath(slikFilePath) + summLogFileName);

                //TODO: validate format file

                string json = File.ReadAllText(slikFilePath);
                List<Person> list = null;
                //JavaScriptSerializer js = new JavaScriptSerializer();

                if(!string.IsNullOrEmpty(json))
                    list = JsonConvert.DeserializeObject<List<Person>>(json);

                foreach(var _obj in list)
                {

                }

            } //harus dihapus klo using db balik
            #region using db
            //    using (DbConnection conn = new DbConnection(connString))
            //    {
            //        using (StreamReader reader =  new StreamReader(slikFile.FullName))
            //        {
            //            using (JsonTextReader jReader = new JsonTextReader(reader))
            //            {
            //                while (jReader.Read())
            //                {
            //                    JavaScriptSerializer js = new JavaScriptSerializer();
            //                    object[] objArr = js.Deserialize<object[]>(reader.ToString());
            //                    //Object obj = (JObject)JToken.ReadFrom(jReader);
            //                    //JArray arrJson = JArray.Parse(obj.ToString());
            //                    //foreach (var test in obj)
            //                    //{
            //                    //    Console.WriteLine(test.ToString());
            //                    //}
            //                }
            //                #region old
            //                /*
            //                int row = 0;
            //                int startRow = 1;
            //                string delimeter = "|";
            //                while (reader.Peek() > 0)
            //                {
            //                    string strLine = reader.ReadLine();
            //                    row++;
            //                    if (strLine == null || strLine.Trim() == "")
            //                        continue;
            //                    if (row < startRow)
            //                        continue;
            //                    string[] fieldValues = strLine.Split(new string[] { delimeter }, StringSplitOptions.None);
            //                    string appId = "";
            //                    try
            //                    {
            //                        DataTable dtAppId = conn.GetDataTable("exec dbo.usp_GetAppId @1,@2",
            //                            new object[] { "SYSTEM", fieldValues[0].ToString() }, dbTimeOut);
            //                        if (dtAppId.Rows.Count == 0)
            //                        {
            //                            //TODO: insert log ke table log

            //                            continue;
            //                        }
            //                        appId = dtAppId.Rows[0]["AppId"].ToString();
            //                        dtAppId.Dispose();

            //                        //UNDONE:save ke table app
            //                        NameValueCollection Keys = new NameValueCollection();
            //                        NameValueCollection Fields = new NameValueCollection();
            //                        staticFramework.saveNVC(Keys, "AppId", appId);
            //                        staticFramework.saveNVC(Fields, "Type", fieldValues[1].ToString());
            //                        staticFramework.saveNVC(Fields, "OriginAppId", fieldValues[2].ToString());
            //                        staticFramework.save(Fields, Keys, "APP", conn);


            //                        //Save Doc
            //                        SaveSLIKDoc(appId, fieldValues[94].ToString(), conn);
            //                        SaveSLIKDoc(appId, fieldValues[95].ToString(), conn);
            //                        SaveSLIKDoc(appId, fieldValues[96].ToString(), conn);
            //                        SaveSLIKDoc(appId, fieldValues[97].ToString(), conn);
            //                        SaveSLIKDoc(appId, fieldValues[98].ToString(), conn);
            //                        SaveSLIKDoc(appId, fieldValues[99].ToString(), conn);
            //                        //End Save Doc

            //                        //TODO: Insert ke table CustPersonal, etc lihat data dictionary sheet IWID_INPUT_DICTIONARY
            //                    }
            //                    catch (Exception exp)
            //                    {
            //                        //TODO: insert log ke table detail log
            //                    }
            //                }
            //                */
            //                #endregion old
            //            } 
            //        }
            //        //TODO:create log files
            //    }
            //}
            #endregion using db
            catch (Exception exp)
            {
                result["Message"] = exp.Message;
            }
            return result;
        }
    }
}
