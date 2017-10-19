using DMS.Framework;
using DMS.Tools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SLIK.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.IO;

namespace CBS.ImportExportSLIK
{
    public class ImportSLIK
    {
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

        public Dictionary<string, object> ImportSLIKInputFile(string iwidFilePath, string connString, int dbTimeOut)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result["Message"] = "";
            result["LogSummary"] = null;
            result["LogDetail"] = null;
            try
            {
                FileInfo iwidFile = new FileInfo(iwidFilePath);
                string detailLogFileName = Path.GetFileNameWithoutExtension(iwidFilePath) + "_detail.log";
                string summLogFileName = Path.GetFileNameWithoutExtension(iwidFilePath) + "_summary.log";
                result["LogDetail"] = new FileInfo(Path.GetFullPath(iwidFilePath) + detailLogFileName);
                result["LogSummary"] = new FileInfo(Path.GetFullPath(iwidFilePath) + summLogFileName);
                string _nFile_ = Path.GetFileNameWithoutExtension(iwidFilePath);
                //TODO: validate format file
                int row_total_ = 0;
                using (DbConnection conn = new DbConnection(connString))
                {
                    using (StreamReader reader = new StreamReader(iwidFile.FullName))
                    {
                        int row = 0;
                        int startRow = 1;
                        string delimeter = "|";
                        while (reader.Peek() > 0)
                        {
                            string strLine = reader.ReadLine();
                            row++;
                            if (strLine == null || strLine.Trim() == "")
                                continue;
                            if (row < startRow)
                                continue;
                            string[] fieldValues = strLine.Split(new string[] { delimeter }, StringSplitOptions.None);
                            string appId = "";
                            try
                            {
                                DataTable dtAppId = conn.GetDataTable("exec dbo.usp_GetAppId @1,@2",
                                    new object[] { "SYSTEM", fieldValues[0].ToString() }, dbTimeOut);
                                if (dtAppId.Rows.Count == 0)
                                {
                                    //TODO: insert log ke table log

                                    continue;
                                }
                                appId = dtAppId.Rows[0]["AppId"].ToString();
                                dtAppId.Dispose();

                                //UNDONE:save ke table app
                                NameValueCollection Keys = new NameValueCollection();
                                NameValueCollection Fields = new NameValueCollection();
                                staticFramework.saveNVC(Keys, "AppId", appId);
                                staticFramework.saveNVC(Fields, "Type", fieldValues[1].ToString());
                                staticFramework.saveNVC(Fields, "OriginAppId", fieldValues[2].ToString());
                                staticFramework.save(Fields, Keys, "APP", conn);


                                //Save Doc
                                SaveSLIKDoc(appId, fieldValues[94].ToString(), conn);
                                SaveSLIKDoc(appId, fieldValues[95].ToString(), conn);
                                SaveSLIKDoc(appId, fieldValues[96].ToString(), conn);
                                SaveSLIKDoc(appId, fieldValues[97].ToString(), conn);
                                SaveSLIKDoc(appId, fieldValues[98].ToString(), conn);
                                SaveSLIKDoc(appId, fieldValues[99].ToString(), conn);
                                //End Save Doc

                                //TODO: Insert ke table CustPersonal, etc lihat data dictionary sheet IWID_INPUT_DICTIONARY
                            }
                            catch (Exception exp)
                            {
                                //TODO: insert log ke table detail log
                                NameValueCollection Keys = new NameValueCollection();
                                NameValueCollection Fields = new NameValueCollection();
                                staticFramework.saveNVC(Keys, "BatchFileName", _nFile_);
                                staticFramework.saveNVC(Keys, "RowNumber", row);
                                staticFramework.saveNVC(Fields, "LineNumber", row);
                                staticFramework.saveNVC(Fields, "KeyValue", fieldValues[2].ToString());
                                staticFramework.saveNVC(Fields, "LogType", "IWIDTYPE");
                                staticFramework.saveNVC(Fields, "LogMessage", exp);
                                staticFramework.save(Fields, Keys, "DataImportLogDetail", conn);
                            }
                        }
                        row_total_ = row;
                    }
                    //TODO:create log files
                    DateTime dt = DateTime.Now;
                    string _time_ = String.Format("{0:ddMMyyyyHHmm}", dt);
                    string sPath = "\\App_Data\\IWID_SPSFile_" + _time_ + ".resp";
                    StreamWriter SaveFile = new StreamWriter(sPath);
                    //SaveFile.WriteLine(row_total_ + "|" + a_success + "|" + a_failed + "|" + _time_start + "|" + _time_end);
                    SaveFile.Close();
                }
            }
            catch (Exception exp)
            {
                result["Message"] = exp.Message;
            }
            return result;
        }

        public Dictionary<string, object> ImportIdebText(string filePath)
        {
            string conn = ConfigurationManager.ConnectionStrings["DBSCBSConnectionString"].ConnectionString;
            Dictionary<string, object> result = new Dictionary<string, object>();
            result["Message"] = "";
            result["LogSummary"] = null;
            result["LogDetail"] = null;

            try
            {
                FileInfo SLIKFilePath = new FileInfo(filePath);
                string detailLogFileName = Path.GetFileNameWithoutExtension(filePath) + "_detail.log";
                string summLogFileName = Path.GetFileNameWithoutExtension(filePath) + "_summary.log";
                result["LogDetail"] = new FileInfo(Path.GetFullPath(filePath) + detailLogFileName);
                result["LogSummary"] = new FileInfo(Path.GetFullPath(filePath) + summLogFileName);
                string _nFile_ = Path.GetFileNameWithoutExtension(filePath);

                //mapping table
                JObject jObj = JObject.Parse(File.ReadAllText(filePath));
                JToken header = jObj["header"];
                JToken perusahaan = jObj["perusahaan"];
                JToken individual = jObj["individual"];
                JToken type = null;

                if (perusahaan != null)
                {
                    type = perusahaan;
                }
                else if (individual != null)
                {
                    type = individual;
                }

                IdebHeader iHeader = JsonConvert.DeserializeObject<IdebHeader>(jObj["header"].ToString());

                if (type != null)
                {

                    IdebCorpSearchVar icorpSearch = JsonConvert.DeserializeObject<IdebCorpSearchVar>(type["parameterPencarian"].ToString());
                    List<IdebCorpData> icorpDt = JsonConvert.DeserializeObject<List<IdebCorpData>>(type["dataPokokDebitur"].ToString());

                    List<IdebCorpGroup> icorpGrp = JsonConvert.DeserializeObject<List<IdebCorpGroup>>(type["kelompokPengurusPemilik"].ToString());
                    IdebFacSummary ifacSumm = JsonConvert.DeserializeObject<IdebFacSummary>(type["ringkasanFasilitas"].ToString());

                    JToken fasilitas = type["fasilitas"];
                    List<IdebSuratBerharga> ifacilities = JsonConvert.DeserializeObject<List<IdebSuratBerharga>>(fasilitas["suratBerharga"].ToString());
                    List<IdebKredit> iKredit = JsonConvert.DeserializeObject<List<IdebKredit>>(fasilitas["kredit"].ToString());
                    List<IdebCollateral> ilc = JsonConvert.DeserializeObject<List<IdebCollateral>>(fasilitas["lc"].ToString());
                    List<IdebBankGaransi> iBankGar = JsonConvert.DeserializeObject<List<IdebBankGaransi>>(fasilitas["bankGaransi"].ToString());
                    List<IdebFasilitasLainnya> iFasLain = JsonConvert.DeserializeObject<List<IdebFasilitasLainnya>>(fasilitas["fasilitasLainnya"].ToString());

                    using (DbConnection db = new DbConnection(conn))
                    {
                        //insert to header
                        NameValueCollection keysHeader = new NameValueCollection();
                        NameValueCollection fieldHeader = new NameValueCollection();
                        Random rnd = new Random();
                        int no = rnd.Next(10);
                        staticFramework.saveNVC(keysHeader, "Idebid",no.ToString());
                        staticFramework.saveNVC(fieldHeader, "ReqId", "123");
                        staticFramework.saveNVC(fieldHeader, "Seq", "1234");
                        staticFramework.saveNVC(fieldHeader, "KodeReferensiPengguna", iHeader.KodeReferensiPengguna);
                        staticFramework.saveNVC(fieldHeader, "TanggalHasil", iHeader.TanggalHasil);
                        staticFramework.saveNVC(fieldHeader, "IdPenggunaPermintaan", iHeader.IdPermintaan);
                        staticFramework.saveNVC(fieldHeader, "IdPenggunaPermintaan", iHeader.IdPenggunaPermintaan.ToString());
                        staticFramework.saveNVC(fieldHeader, "DibuatOleh", iHeader.DibuatOleh);
                        staticFramework.saveNVC(fieldHeader, "KodeLJKPermintaan", iHeader.KodeLJKPermintaan);
                        staticFramework.saveNVC(fieldHeader, "KodeCabangPermintaan", iHeader.KodeCabangPermintaan);
                        staticFramework.saveNVC(fieldHeader, "KodeTujuanPermintaan", iHeader.KodeTujuanPermintaan);
                        staticFramework.saveNVC(fieldHeader, "TanggalPermintaan", iHeader.TanggalPermintaan.ToString());
                        staticFramework.saveNVC(fieldHeader, "TotalBagian", iHeader.TotalBagian);
                        staticFramework.saveNVC(fieldHeader, "NomorBagian", iHeader.NomorBagian);
                        staticFramework.save(fieldHeader, keysHeader,"Ideb",db);

                        //insert perusahaaan

                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            
            return result;
        }
    }
}
