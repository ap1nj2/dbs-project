﻿using DMS.Framework;
using DMS.Tools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SLIK.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;

namespace CBS.ExportImport
{
    public class Importer
    {
        private void SaveIWIDDoc(string appId, string docCode, DbConnection conn)
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

        public Dictionary<string, object> ImportIWIDInputFile(string iwidFilePath, string connString, int dbTimeOut)
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

                //TODO: validate format file

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
                                SaveIWIDDoc(appId, fieldValues[94].ToString(), conn);
                                SaveIWIDDoc(appId, fieldValues[95].ToString(), conn);
                                SaveIWIDDoc(appId, fieldValues[96].ToString(), conn);
                                SaveIWIDDoc(appId, fieldValues[97].ToString(), conn);
                                SaveIWIDDoc(appId, fieldValues[98].ToString(), conn);
                                SaveIWIDDoc(appId, fieldValues[99].ToString(), conn);
                                //End Save Doc

                                //TODO: Insert ke table CustPersonal, etc lihat data dictionary sheet IWID_INPUT_DICTIONARY
                            }
                            catch (Exception exp)
                            {
                                //TODO: insert log ke table detail log
                            }
                        }
                    }

                    //TODO:create log files
                }
            }
            catch (Exception exp)
            {
                result["Message"] = exp.Message;
            }
            return result;
        }
         
        public void ImportIdebText(string filePath)
        {
            JObject jObj = JObject.Parse(File.ReadAllText(filePath));
            JToken header = jObj["header"];
            JToken perusahaan = jObj["perusahaan"];
            JToken individual = jObj["individual"];
            JToken facilitySummary = null;

            if (perusahaan != null)
            {
                facilitySummary = perusahaan["ringkasanFasilitas"];
            }
            else if (individual != null)
            {
                facilitySummary = individual["ringkasanFasilitas"];
            }
            IdebHeader iHeader = JsonConvert.DeserializeObject<IdebHeader>(jObj["header"].ToString());
            if (facilitySummary != null)
            {
                IdebFacSummary ifacSumm = JsonConvert.DeserializeObject<IdebFacSummary>(facilitySummary.ToString());
            }
        }

        #region punya ROBI 
        /*
        //SPS Services :  Moch Robi Setiawan 
        private void SaveSPSDoc(string appId, string docCode, DbConnection conn)
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

        
        public static void UploadSPSApp(string spsFilePath, string connString, int dbTimeOut)


        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result["Message"] = "";
            result["LogSummary"] = null;
            result["LogDetail"] = null;
            try
            {
                FileInfo spsFile = new FileInfo(spsFilePath);
                string detailLogFileName = Path.GetFileNameWithoutExtension(spsFilePath) + "_detail.log";
                string summLogFileName = Path.GetFileNameWithoutExtension(spsFilePath) + "_summary.log";
                result["LogDetail"] = new FileInfo(Path.GetFullPath(spsFilePath) + detailLogFileName);
                result["LogSummary"] = new FileInfo(Path.GetFullPath(spsFilePath) + summLogFileName);

                //TODO: validate format file

                using (DbConnection conn = new DbConnection(connString))
                {
                    using (StreamReader reader = new StreamReader(spsFile.FullName))
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
                                DataTable dtAppId = conn.GetDataTable("exec dbo.AppMemmo @1,@2",
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
                                SaveSPSDoc(appId, fieldValues[94].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[95].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[96].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[97].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[98].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[99].ToString(), conn);
                                //End Save Doc

                                //TODO: Insert ke table CustPersonal, etc lihat data dictionary sheet IWID_INPUT_DICTIONARY
                            }
                            catch (Exception exp)
                            {
                                //TODO: insert log ke table detail log
                            }
                        }
                    }

                    //TODO:create log files
                }
            }
            catch (Exception exp)
            {
                result["Message"] = exp.Message;
            }
            return result;
        }
        public static void UploadSPSAppBDoc(string spsFilePath, string connString, int dbTimeOut)


        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result["Message"] = "";
            result["LogSummary"] = null;
            result["LogDetail"] = null;
            try
            {
                FileInfo spsFile = new FileInfo(spsFilePath);
                string detailLogFileName = Path.GetFileNameWithoutExtension(spsFilePath) + "_detail.log";
                string summLogFileName = Path.GetFileNameWithoutExtension(spsFilePath) + "_summary.log";
                result["LogDetail"] = new FileInfo(Path.GetFullPath(spsFilePath) + detailLogFileName);
                result["LogSummary"] = new FileInfo(Path.GetFullPath(spsFilePath) + summLogFileName);

                //TODO: validate format file

                using (DbConnection conn = new DbConnection(connString))
                {
                    using (StreamReader reader = new StreamReader(spsFile.FullName))
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
                                DataTable dtAppId = conn.GetDataTable("exec dbo.AppBilPmt @1,@2",
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
                                staticFramework.saveNVC(Fields, "DocCd", OriginAppId);
                                                                      
                                staticFramework.save(Fields, Keys, "DocCd", conn);

                                //Save Doc
                                SaveSPSDoc(appId, fieldValues[94].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[95].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[96].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[97].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[98].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[99].ToString(), conn);
                                //End Save Doc

                                //TODO: Insert ke table CustPersonal, etc lihat data dictionary sheet IWID_INPUT_DICTIONARY
                            }
                            catch (Exception exp)
                            {
                                //TODO: insert log ke table detail log
                            }
                        }
                    }

                    //TODO:create log files
                }
            }
            catch (Exception exp)
            {
                result["Message"] = exp.Message;
            }
            return result;
        }
        public static void UploadSPSAppBillPmt(string spsFilePath, string connString, int dbTimeOut)


        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result["Message"] = "";
            result["LogSummary"] = null;
            result["LogDetail"] = null;
            try
            {
                FileInfo spsFile = new FileInfo(spsFilePath);
                string detailLogFileName = Path.GetFileNameWithoutExtension(spsFilePath) + "_detail.log";
                string summLogFileName = Path.GetFileNameWithoutExtension(spsFilePath) + "_summary.log";
                result["LogDetail"] = new FileInfo(Path.GetFullPath(spsFilePath) + detailLogFileName);
                result["LogSummary"] = new FileInfo(Path.GetFullPath(spsFilePath) + summLogFileName);

                //TODO: validate format file

                using (DbConnection conn = new DbConnection(connString))
                {
                    using (StreamReader reader = new StreamReader(spsFile.FullName))
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
                                DataTable dtAppId = conn.GetDataTable("exec dbo.AppBilPmt @1,@2",
                                    new object[] { "SYSTEM", fieldValues[0].ToString() }, dbTimeOut);
                                if (dtAppId.Rows.Count == 0)
                                {
                                    //TODO: insert log ke table log

                                    continue;
                                }
                                appId = dtAppId.Rows[0]["DocCd"].ToString();
                                dtAppId.Dispose();

                                //UNDONE:save ke table app
                                NameValueCollection Keys = new NameValueCollection();
                                NameValueCollection Fields = new NameValueCollection();
                                staticFramework.saveNVC(Fields, "DocCd", OriginAppId);

                                staticFramework.save(Fields, Keys, "DocCd", conn);

                                //Save Doc
                                SaveSPSDoc(appId, fieldValues[94].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[95].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[96].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[97].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[98].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[99].ToString(), conn);
                                //End Save Doc

                                //TODO: Insert ke table CustPersonal, etc lihat data dictionary sheet IWID_INPUT_DICTIONARY
                            }
                            catch (Exception exp)
                            {
                                //TODO: insert log ke table detail log
                            }
                        }
                    }

                    //TODO:create log files
                }
            }
            catch (Exception exp)
            {
                result["Message"] = exp.Message;
            }
            return result;
        }
        public static void UploadSPSAppExLoan(string spsFilePath, string connString, int dbTimeOut)


        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result["Message"] = "";
            result["LogSummary"] = null;
            result["LogDetail"] = null;
            try
            {
                FileInfo spsFile = new FileInfo(spsFilePath);
                string detailLogFileName = Path.GetFileNameWithoutExtension(spsFilePath) + "_detail.log";
                string summLogFileName = Path.GetFileNameWithoutExtension(spsFilePath) + "_summary.log";
                result["LogDetail"] = new FileInfo(Path.GetFullPath(spsFilePath) + detailLogFileName);
                result["LogSummary"] = new FileInfo(Path.GetFullPath(spsFilePath) + summLogFileName);

                //TODO: validate format file

                using (DbConnection conn = new DbConnection(connString))
                {
                    using (StreamReader reader = new StreamReader(spsFile.FullName))
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
                                DataTable dtAppId = conn.GetDataTable("exec dbo.AppExLoan @1,@2",
                                    new object[] { "SYSTEM", fieldValues[0].ToString() }, dbTimeOut);
                                if (dtAppId.Rows.Count == 0)
                                {
                                    //TODO: insert log ke table log

                                    continue;
                                }
                                appId = dtAppId.Rows[0]["LnAccNo"].ToString();
                                dtAppId.Dispose();

                                //UNDONE:save ke table app
                                NameValueCollection Keys = new NameValueCollection();
                                NameValueCollection Fields = new NameValueCollection();

                                staticFramework.saveNVC(Fields, "LnAccNo", OriginAppId);

                                staticFramework.save(Fields, Keys, "LnAccNo", conn);

                                //Save Doc
                                SaveSPSDoc(appId, fieldValues[94].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[95].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[96].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[97].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[98].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[99].ToString(), conn);
                                //End Save Doc

                                //TODO: Insert ke table CustPersonal, etc lihat data dictionary sheet IWID_INPUT_DICTIONARY
                            }
                            catch (Exception exp)
                            {
                                //TODO: insert log ke table detail log
                            }
                        }
                    }

                    //TODO:create log files
                }
            }
            catch (Exception exp)
            {
                result["Message"] = exp.Message;
            }
            return result;
        }
        public static void UploadAppFlag(string spsFilePath, string connString, int dbTimeOut)


        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result["Message"] = "";
            result["LogSummary"] = null;
            result["LogDetail"] = null;
            try
            {
                FileInfo spsFile = new FileInfo(spsFilePath);
                string detailLogFileName = Path.GetFileNameWithoutExtension(spsFilePath) + "_detail.log";
                string summLogFileName = Path.GetFileNameWithoutExtension(spsFilePath) + "_summary.log";
                result["LogDetail"] = new FileInfo(Path.GetFullPath(spsFilePath) + detailLogFileName);
                result["LogSummary"] = new FileInfo(Path.GetFullPath(spsFilePath) + summLogFileName);

                //TODO: validate format file

                using (DbConnection conn = new DbConnection(connString))
                {
                    using (StreamReader reader = new StreamReader(spsFile.FullName))
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
                                DataTable dtAppId = conn.GetDataTable("exec dbo.AppFlag @1,@2",
                                    new object[] { "SYSTEM", fieldValues[0].ToString() }, dbTimeOut);
                                if (dtAppId.Rows.Count == 0)
                                {
                                    //TODO: insert log ke table log

                                    continue;
                                }
                                appId = dtAppId.Rows[0]["DBSLnLimit_OS"].ToString();
                                dtAppId.Dispose();

                                //UNDONE:save ke table app
                                NameValueCollection Keys = new NameValueCollection();
                                NameValueCollection Fields = new NameValueCollection();

                                staticFramework.saveNVC(Fields, "DBSLnLimit_OS", OriginAppId);
                                staticFramework.saveNVC(Fields, "BankCd", OriginAppId);
                                staticFramework.saveNVC(Fields, "VerBillAddrMatch", OriginAppId);
                                staticFramework.saveNVC(Fields, "VerMobilePhnMatch", OriginAppId);
                                staticFramework.saveNVC(Fields, "NpwpDocFlag", OriginAppId);
                                staticFramework.saveNVC(Fields, "AMLRiskRating", OriginAppId);

                                staticFramework.save(Fields, Keys, "DBSLnLimit_OS", conn);

                                //Save Doc
                                SaveSPSDoc(appId, fieldValues[94].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[95].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[96].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[97].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[98].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[99].ToString(), conn);
                                //End Save Doc

                                //TODO: Insert ke table CustPersonal, etc lihat data dictionary sheet IWID_INPUT_DICTIONARY
                            }
                            catch (Exception exp)
                            {
                                //TODO: insert log ke table detail log
                            }
                        }
                    }

                    //TODO:create log files
                }
            }
            catch (Exception exp)
            {
                result["Message"] = exp.Message;
            }
            return result;
        }
        public static void UploadAppFac(string spsFilePath, string connString, int dbTimeOut)


        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result["Message"] = "";
            result["LogSummary"] = null;
            result["LogDetail"] = null;
            try
            {
                FileInfo spsFile = new FileInfo(spsFilePath);
                string detailLogFileName = Path.GetFileNameWithoutExtension(spsFilePath) + "_detail.log";
                string summLogFileName = Path.GetFileNameWithoutExtension(spsFilePath) + "_summary.log";
                result["LogDetail"] = new FileInfo(Path.GetFullPath(spsFilePath) + detailLogFileName);
                result["LogSummary"] = new FileInfo(Path.GetFullPath(spsFilePath) + summLogFileName);

                //TODO: validate format file

                using (DbConnection conn = new DbConnection(connString))
                {
                    using (StreamReader reader = new StreamReader(spsFile.FullName))
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
                                DataTable dtAppId = conn.GetDataTable("exec dbo.AppFlag @1,@2",
                                    new object[] { "SYSTEM", fieldValues[0].ToString() }, dbTimeOut);
                                if (dtAppId.Rows.Count == 0)
                                {
                                    //TODO: insert log ke table log

                                    continue;
                                }
                                appId = dtAppId.Rows[0]["DBSLnLimit_OS"].ToString();
                                dtAppId.Dispose();

                                //UNDONE:save ke table app
                                NameValueCollection Keys = new NameValueCollection();
                                NameValueCollection Fields = new NameValueCollection();

                                staticFramework.saveNVC(Fields, "DBSLnLimit_OS", OriginAppId);
                                staticFramework.saveNVC(Fields, "BankCd", OriginAppId);
                                staticFramework.saveNVC(Fields, "VerBillAddrMatch", OriginAppId);
                                staticFramework.saveNVC(Fields, "VerMobilePhnMatch", OriginAppId);
                                staticFramework.saveNVC(Fields, "NpwpDocFlag", OriginAppId);
                                staticFramework.saveNVC(Fields, "AMLRiskRating", OriginAppId);

                                staticFramework.save(Fields, Keys, "DBSLnLimit_OS", conn);

                                //Save Doc
                                SaveSPSDoc(appId, fieldValues[94].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[95].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[96].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[97].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[98].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[99].ToString(), conn);
                                //End Save Doc

                                //TODO: Insert ke table CustPersonal, etc lihat data dictionary sheet IWID_INPUT_DICTIONARY
                            }
                            catch (Exception exp)
                            {
                                //TODO: insert log ke table detail log
                            }
                        }
                    }

                    //TODO:create log files
                }
            }
            catch (Exception exp)
            {
                result["Message"] = exp.Message;
            }
            return result;
        }
        public static void UploadSPSAppMemo(string spsFilePath, string connString, int dbTimeOut)


        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result["Message"] = "";
            result["LogSummary"] = null;
            result["LogDetail"] = null;
            try
            {
                FileInfo spsFile = new FileInfo(spsFilePath);
                string detailLogFileName = Path.GetFileNameWithoutExtension(spsFilePath) + "_detail.log";
                string summLogFileName = Path.GetFileNameWithoutExtension(spsFilePath) + "_summary.log";
                result["LogDetail"] = new FileInfo(Path.GetFullPath(spsFilePath) + detailLogFileName);
                result["LogSummary"] = new FileInfo(Path.GetFullPath(spsFilePath) + summLogFileName);

                //TODO: validate format file

                using (DbConnection conn = new DbConnection(connString))
                {
                    using (StreamReader reader = new StreamReader(spsFile.FullName))
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
                                DataTable dtAppId = conn.GetDataTable("exec dbo.AppMemmo @1,@2",
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
                                SaveSPSDoc(appId, fieldValues[94].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[95].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[96].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[97].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[98].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[99].ToString(), conn);
                                //End Save Doc

                                //TODO: Insert ke table CustPersonal, etc lihat data dictionary sheet IWID_INPUT_DICTIONARY
                            }
                            catch (Exception exp)
                            {
                                //TODO: insert log ke table detail log
                            }
                        }
                    }

                    //TODO:create log files
                }
            }
            catch (Exception exp)
            {
                result["Message"] = exp.Message;
            }
            return result;
        }
        public static void UploadSPSSupp(string spsFilePath, string connString, int dbTimeOut)

        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result["Message"] = "";
            result["LogSummary"] = null;
            result["LogDetail"] = null;
            try
            {
                FileInfo spsFile = new FileInfo(spsFilePath);
                string detailLogFileName = Path.GetFileNameWithoutExtension(spsFilePath) + "_detail.log";
                string summLogFileName = Path.GetFileNameWithoutExtension(spsFilePath) + "_summary.log";
                result["LogDetail"] = new FileInfo(Path.GetFullPath(spsFilePath) + detailLogFileName);
                result["LogSummary"] = new FileInfo(Path.GetFullPath(spsFilePath) + summLogFileName);

                //TODO: validate format file

                using (DbConnection conn = new DbConnection(connString))
                {
                    using (StreamReader reader = new StreamReader(spsFile.FullName))
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
                                DataTable dtAppId = conn.GetDataTable("exec dbo.AppMemmo @1,@2",
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
                                SaveSPSDoc(appId, fieldValues[94].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[95].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[96].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[97].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[98].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[99].ToString(), conn);
                                //End Save Doc

                                //TODO: Insert ke table CustPersonal, etc lihat data dictionary sheet IWID_INPUT_DICTIONARY
                            }
                            catch (Exception exp)
                            {
                                //TODO: insert log ke table detail log
                            }
                        }
                    }

                    //TODO:create log files
                }
            }
            catch (Exception exp)
            {
                result["Message"] = exp.Message;
            }
            return result;
        }
        public static void UploadSPSCustExCc(string spsFilePath, string connString, int dbTimeOut)

        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result["Message"] = "";
            result["LogSummary"] = null;
            result["LogDetail"] = null;
            try
            {
                FileInfo spsFile = new FileInfo(spsFilePath);
                string detailLogFileName = Path.GetFileNameWithoutExtension(spsFilePath) + "_detail.log";
                string summLogFileName = Path.GetFileNameWithoutExtension(spsFilePath) + "_summary.log";
                result["LogDetail"] = new FileInfo(Path.GetFullPath(spsFilePath) + detailLogFileName);
                result["LogSummary"] = new FileInfo(Path.GetFullPath(spsFilePath) + summLogFileName);

                //TODO: validate format file

                using (DbConnection conn = new DbConnection(connString))
                {
                    using (StreamReader reader = new StreamReader(spsFile.FullName))
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
                                DataTable dtAppId = conn.GetDataTable("exec dbo.AppMemmo @1,@2",
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
                                SaveSPSDoc(appId, fieldValues[94].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[95].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[96].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[97].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[98].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[99].ToString(), conn);
                                //End Save Doc

                                //TODO: Insert ke table CustPersonal, etc lihat data dictionary sheet IWID_INPUT_DICTIONARY
                            }
                            catch (Exception exp)
                            {
                                //TODO: insert log ke table detail log
                            }
                        }
                    }

                    //TODO:create log files
                }
            }
            catch (Exception exp)
            {
                result["Message"] = exp.Message;
            }
            return result;
        }
        public static void UploadSPSCustFin(string spsFilePath, string connString, int dbTimeOut)

        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result["Message"] = "";
            result["LogSummary"] = null;
            result["LogDetail"] = null;
            try
            {
                FileInfo spsFile = new FileInfo(spsFilePath);
                string detailLogFileName = Path.GetFileNameWithoutExtension(spsFilePath) + "_detail.log";
                string summLogFileName = Path.GetFileNameWithoutExtension(spsFilePath) + "_summary.log";
                result["LogDetail"] = new FileInfo(Path.GetFullPath(spsFilePath) + detailLogFileName);
                result["LogSummary"] = new FileInfo(Path.GetFullPath(spsFilePath) + summLogFileName);

                //TODO: validate format file

                using (DbConnection conn = new DbConnection(connString))
                {
                    using (StreamReader reader = new StreamReader(spsFile.FullName))
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
                                DataTable dtAppId = conn.GetDataTable("exec dbo.AppMemmo @1,@2",
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
                                SaveSPSDoc(appId, fieldValues[94].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[95].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[96].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[97].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[98].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[99].ToString(), conn);
                                //End Save Doc

                                //TODO: Insert ke table CustPersonal, etc lihat data dictionary sheet IWID_INPUT_DICTIONARY
                            }
                            catch (Exception exp)
                            {
                                //TODO: insert log ke table detail log
                            }
                        }
                    }

                    //TODO:create log files
                }
            }
            catch (Exception exp)
            {
                result["Message"] = exp.Message;
            }
            return result;
        }
        public static void UploadSPSCustJob(string spsFilePath, string connString, int dbTimeOut)

        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result["Message"] = "";
            result["LogSummary"] = null;
            result["LogDetail"] = null;
            try
            {
                FileInfo spsFile = new FileInfo(spsFilePath);
                string detailLogFileName = Path.GetFileNameWithoutExtension(spsFilePath) + "_detail.log";
                string summLogFileName = Path.GetFileNameWithoutExtension(spsFilePath) + "_summary.log";
                result["LogDetail"] = new FileInfo(Path.GetFullPath(spsFilePath) + detailLogFileName);
                result["LogSummary"] = new FileInfo(Path.GetFullPath(spsFilePath) + summLogFileName);

                //TODO: validate format file

                using (DbConnection conn = new DbConnection(connString))
                {
                    using (StreamReader reader = new StreamReader(spsFile.FullName))
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
                                DataTable dtAppId = conn.GetDataTable("exec dbo.AppMemmo @1,@2",
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
                                SaveSPSDoc(appId, fieldValues[94].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[95].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[96].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[97].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[98].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[99].ToString(), conn);
                                //End Save Doc

                                //TODO: Insert ke table CustPersonal, etc lihat data dictionary sheet IWID_INPUT_DICTIONARY
                            }
                            catch (Exception exp)
                            {
                                //TODO: insert log ke table detail log
                            }
                        }
                    }

                    //TODO:create log files
                }
            }
            catch (Exception exp)
            {
                result["Message"] = exp.Message;
            }
            return result;
        }
        public static void UploadSPSCustJobInfo(string spsFilePath, string connString, int dbTimeOut)

        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result["Message"] = "";
            result["LogSummary"] = null;
            result["LogDetail"] = null;
            try
            {
                FileInfo spsFile = new FileInfo(spsFilePath);
                string detailLogFileName = Path.GetFileNameWithoutExtension(spsFilePath) + "_detail.log";
                string summLogFileName = Path.GetFileNameWithoutExtension(spsFilePath) + "_summary.log";
                result["LogDetail"] = new FileInfo(Path.GetFullPath(spsFilePath) + detailLogFileName);
                result["LogSummary"] = new FileInfo(Path.GetFullPath(spsFilePath) + summLogFileName);

                //TODO: validate format file

                using (DbConnection conn = new DbConnection(connString))
                {
                    using (StreamReader reader = new StreamReader(spsFile.FullName))
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
                                DataTable dtAppId = conn.GetDataTable("exec dbo.AppMemmo @1,@2",
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
                                SaveSPSDoc(appId, fieldValues[94].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[95].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[96].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[97].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[98].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[99].ToString(), conn);
                                //End Save Doc

                                //TODO: Insert ke table CustPersonal, etc lihat data dictionary sheet IWID_INPUT_DICTIONARY
                            }
                            catch (Exception exp)
                            {
                                //TODO: insert log ke table detail log
                            }
                        }
                    }

                    //TODO:create log files
                }
            }
            catch (Exception exp)
            {
                result["Message"] = exp.Message;
            }
            return result;
        }
        public static void UploadSPSCustPersonal(string spsFilePath, string connString, int dbTimeOut)

        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result["Message"] = "";
            result["LogSummary"] = null;
            result["LogDetail"] = null;
            try
            {
                FileInfo spsFile = new FileInfo(spsFilePath);
                string detailLogFileName = Path.GetFileNameWithoutExtension(spsFilePath) + "_detail.log";
                string summLogFileName = Path.GetFileNameWithoutExtension(spsFilePath) + "_summary.log";
                result["LogDetail"] = new FileInfo(Path.GetFullPath(spsFilePath) + detailLogFileName);
                result["LogSummary"] = new FileInfo(Path.GetFullPath(spsFilePath) + summLogFileName);

                //TODO: validate format file

                using (DbConnection conn = new DbConnection(connString))
                {
                    using (StreamReader reader = new StreamReader(spsFile.FullName))
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
                                DataTable dtAppId = conn.GetDataTable("exec dbo.AppMemmo @1,@2",
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
                                SaveSPSDoc(appId, fieldValues[94].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[95].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[96].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[97].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[98].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[99].ToString(), conn);
                                //End Save Doc

                                //TODO: Insert ke table CustPersonal, etc lihat data dictionary sheet IWID_INPUT_DICTIONARY
                            }
                            catch (Exception exp)
                            {
                                //TODO: insert log ke table detail log
                            }
                        }
                    }

                    //TODO:create log files
                }
            }
            catch (Exception exp)
            {
                result["Message"] = exp.Message;
            }
            return result;
        }
        public static void UploadSPSCustRel(string spsFilePath, string connString, int dbTimeOut)

        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result["Message"] = "";
            result["LogSummary"] = null;
            result["LogDetail"] = null;
            try
            {
                FileInfo spsFile = new FileInfo(spsFilePath);
                string detailLogFileName = Path.GetFileNameWithoutExtension(spsFilePath) + "_detail.log";
                string summLogFileName = Path.GetFileNameWithoutExtension(spsFilePath) + "_summary.log";
                result["LogDetail"] = new FileInfo(Path.GetFullPath(spsFilePath) + detailLogFileName);
                result["LogSummary"] = new FileInfo(Path.GetFullPath(spsFilePath) + summLogFileName);

                //TODO: validate format file

                using (DbConnection conn = new DbConnection(connString))
                {
                    using (StreamReader reader = new StreamReader(spsFile.FullName))
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
                                DataTable dtAppId = conn.GetDataTable("exec dbo.AppMemmo @1,@2",
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
                                SaveSPSDoc(appId, fieldValues[94].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[95].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[96].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[97].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[98].ToString(), conn);
                                SaveSPSDoc(appId, fieldValues[99].ToString(), conn);
                                //End Save Doc

                                //TODO: Insert ke table CustPersonal, etc lihat data dictionary sheet IWID_INPUT_DICTIONARY
                            }
                            catch (Exception exp)
                            {
                                //TODO: insert log ke table detail log
                            }
                        }
                    }

                    //TODO:create log files
                }
            }
            catch (Exception exp)
            {
                result["Message"] = exp.Message;
            }
            return result;
        }*/
        #endregion punya robi
    }


}