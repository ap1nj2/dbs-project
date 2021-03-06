﻿using DMS.Framework;
using DMS.Tools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SLIK.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using IMKAHFA;

namespace CBS.ExportImport
{
    public class Importer
    {
        timeProcess _timer_ = new timeProcess();
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
                string _nFile_ = Path.GetFileNameWithoutExtension(iwidFilePath);
                //TODO: validate format file
                int row_total_ = 0;
                int success_ = 0;
                int failed_ = 0;
                using (DbConnection conn = new DbConnection(connString))
                {
                    using (StreamReader reader = new StreamReader(iwidFile.FullName))
                    {
                        int row = 0;
                        int startRow = 1;
                        string delimeter = "|";
                        while (reader.Peek() > 0)
                        {
                            _timer_.Start();
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

                                // save ke table app
                                NameValueCollection Keys = new NameValueCollection();
                                NameValueCollection Fields = new NameValueCollection();
                                staticFramework.saveNVC(Keys, "AppId", appId);
                                staticFramework.saveNVC(Fields, "Org", fieldValues[1].ToString());
                                staticFramework.saveNVC(Fields, "Type", fieldValues[2].ToString());
                                staticFramework.saveNVC(Fields, "OriginAppId", fieldValues[3].ToString());
                                staticFramework.saveNVC(Fields, "ReceiptDate", fieldValues[4].ToString());
                                staticFramework.saveNVC(Fields, "SolicitationId", fieldValues[5].ToString());
                                staticFramework.saveNVC(Fields, "VIPIndicator", fieldValues[6].ToString());
                                staticFramework.saveNVC(Fields, "UseCreditDesired", fieldValues[7].ToString());
                                staticFramework.saveNVC(Fields, "ProcCradleToGrave", fieldValues[8].ToString());
                                staticFramework.saveNVC(Fields, "CreateCustRecord", fieldValues[9].ToString());
                                staticFramework.saveNVC(Fields, "CreateRelRecord", fieldValues[11].ToString());
                                staticFramework.saveNVC(Fields, "RelCd", fieldValues[12].ToString());
                                staticFramework.saveNVC(Fields, "CreateBaseSegRecord", fieldValues[13].ToString());
                                staticFramework.saveNVC(Fields, "SameCust", fieldValues[14].ToString());
                                staticFramework.saveNVC(Fields, "EmbMethodCd", fieldValues[15].ToString());
                                staticFramework.saveNVC(Fields, "BaseSegCd", fieldValues[16].ToString());
                                staticFramework.saveNVC(Fields, "CardSchemeCd", fieldValues[17].ToString());
                                staticFramework.saveNVC(Fields, "LoanPlanNo", fieldValues[18].ToString());
                                staticFramework.saveNVC(Fields, "PrimaryAcc", fieldValues[19].ToString());
                                staticFramework.saveNVC(Fields, "LoanRefNo", fieldValues[21].ToString());
                                staticFramework.saveNVC(Fields, "RelTypeIndCd", fieldValues[22].ToString());
                                staticFramework.saveNVC(Fields, "ProcessTypeCd", fieldValues[109].ToString());
                                staticFramework.saveNVC(Fields, "AlgoProxyIncTest", fieldValues[111].ToString());
                                staticFramework.saveNVC(Fields, "PLNameOfBank", fieldValues[112].ToString());
                                staticFramework.saveNVC(Fields, "Oth1", fieldValues[113].ToString());
                                staticFramework.saveNVC(Fields, "AlgoSegTest", fieldValues[114].ToString());
                                staticFramework.saveNVC(Fields, "CustConsent", fieldValues[115].ToString());
                                staticFramework.saveNVC(Fields, "MarcomConsent", fieldValues[129].ToString());
                                staticFramework.saveNVC(Fields, "Oth2", fieldValues[137].ToString());
                                staticFramework.saveNVC(Fields, "AppRefNo", fieldValues[138].ToString());
                                staticFramework.saveNVC(Fields, "SourceCd", fieldValues[139].ToString());
                                staticFramework.saveNVC(Fields, "CampaignCd", fieldValues[140].ToString());
                                staticFramework.saveNVC(Fields, "LoanCentreCd", fieldValues[141].ToString());
                                staticFramework.saveNVC(Fields, "AgentCd", fieldValues[142].ToString());
                                staticFramework.saveNVC(Fields, "SalesCd", fieldValues[143].ToString());
                                staticFramework.saveNVC(Fields, "MgmCd", fieldValues[144].ToString());
                                staticFramework.saveNVC(Fields, "UpfrontReject", fieldValues[145].ToString());
                                staticFramework.saveNVC(Fields, "Oth3", fieldValues[146].ToString());
                                staticFramework.saveNVC(Fields, "Oth4", fieldValues[147].ToString());
                                staticFramework.saveNVC(Fields, "AlgoScoreTest", fieldValues[148].ToString());
                                staticFramework.saveNVC(Fields, "AlgoTestTrack", fieldValues[149].ToString());
                                staticFramework.saveNVC(Fields, "ExcludeIndustry", fieldValues[150].ToString());
                                staticFramework.saveNVC(Fields, "PLOptDBSMUE", fieldValues[151].ToString());
                                staticFramework.saveNVC(Fields, "CCOptDBSMUE", fieldValues[151].ToString());
                                staticFramework.saveNVC(Fields, "MLOptDBSMUE", fieldValues[151].ToString());
                                staticFramework.saveNVC(Fields, "SignatureDate", fieldValues[152].ToString());
                                staticFramework.saveNVC(Fields, "AreaScore", fieldValues[156].ToString());
                                staticFramework.saveNVC(Fields, "PSCd", fieldValues[157].ToString());
                                staticFramework.saveNVC(Fields, "CustType", fieldValues[172].ToString());
                                staticFramework.saveNVC(Fields, "TopupType", fieldValues[173].ToString());
                                staticFramework.saveNVC(Fields, "OldestMOB", fieldValues[174].ToString());
                                staticFramework.saveNVC(Fields, "OriginAccNo", fieldValues[175].ToString());
                                staticFramework.saveNVC(Fields, "DiscFlag", fieldValues[176].ToString());
                                staticFramework.saveNVC(Fields, "PINPIDPI2Flag", fieldValues[177].ToString());
                                staticFramework.saveNVC(Fields, "BranchId", fieldValues[181].ToString());
                                staticFramework.saveNVC(Fields, "RelWithBanks", fieldValues[182].ToString());
                                staticFramework.saveNVC(Fields, "NeedProbing", fieldValues[186].ToString());
                                staticFramework.saveNVC(Fields, "ProbingDetail", fieldValues[187].ToString());
                                staticFramework.saveNVC(Fields, "ProbingDetail2", fieldValues[188].ToString());
                                staticFramework.saveNVC(Fields, "TotalCardIssuers", fieldValues[192].ToString());
                                staticFramework.saveNVC(Fields, "BillPmtReg", fieldValues[193].ToString());
                                staticFramework.saveNVC(Fields, "SavingAccNo", fieldValues[207].ToString());
                                staticFramework.saveNVC(Fields, "ReqPrintBilling", fieldValues[208].ToString());
                                staticFramework.saveNVC(Fields, "Oth5", fieldValues[209].ToString());
                                staticFramework.saveNVC(Fields, "DocType", fieldValues[246].ToString());
                                staticFramework.save(Fields, Keys, "APP", conn);
                                

                                //Save Doc
                                SaveIWIDDoc(appId, fieldValues[94].ToString(), conn);
                                SaveIWIDDoc(appId, fieldValues[95].ToString(), conn);
                                SaveIWIDDoc(appId, fieldValues[96].ToString(), conn);
                                SaveIWIDDoc(appId, fieldValues[97].ToString(), conn);
                                SaveIWIDDoc(appId, fieldValues[98].ToString(), conn);
                                SaveIWIDDoc(appId, fieldValues[99].ToString(), conn);
                                //End Save Doc
                                success_ = success_ + 1;
                                //Insert ke table CustPersonal, etc lihat data dictionary sheet IWID_INPUT_DICTIONARY
                                
                                staticFramework.saveNVC(Keys, "AppId", appId);
                                staticFramework.saveNVC(Fields, "CustNo", fieldValues[10].ToString());
                                staticFramework.saveNVC(Fields, "CustName", fieldValues[23].ToString());
                                staticFramework.saveNVC(Fields, "CustNameOnID", fieldValues[24].ToString());
                                staticFramework.saveNVC(Fields, "CustNickName", fieldValues[25].ToString());
                                staticFramework.saveNVC(Fields, "CustNameInCard", fieldValues[26].ToString());
                                staticFramework.saveNVC(Fields, "NationalityCd", fieldValues[27].ToString());
                                staticFramework.saveNVC(Fields, "MothersMaidenName", fieldValues[28].ToString());
                                staticFramework.saveNVC(Fields, "GenderCd", fieldValues[29].ToString());
                                staticFramework.saveNVC(Fields, "PlaceOfBirth", fieldValues[30].ToString());
                                staticFramework.saveNVC(Fields, "DateOfBirth", fieldValues[31].ToString());
                                staticFramework.saveNVC(Fields, "IDNumber", fieldValues[32].ToString());
                                staticFramework.saveNVC(Fields, "EduCd", fieldValues[33].ToString());
                                staticFramework.saveNVC(Fields, "MaritalCd", fieldValues[34].ToString());
                                staticFramework.saveNVC(Fields, "NoOfDependents", fieldValues[35].ToString());
                                staticFramework.saveNVC(Fields, "HomeAddr1", fieldValues[36].ToString());
                                staticFramework.saveNVC(Fields, "HomeAddr2", fieldValues[37].ToString());
                                staticFramework.saveNVC(Fields, "HomeAddr3", fieldValues[38].ToString());
                                staticFramework.saveNVC(Fields, "HomeDistrictCd", fieldValues[39].ToString());
                                staticFramework.saveNVC(Fields, "HomeSubDistrictCd", fieldValues[40].ToString());
                                staticFramework.saveNVC(Fields, "HomeSubDistrictName", fieldValues[41].ToString());
                                staticFramework.saveNVC(Fields, "HomeDistrictName", fieldValues[42].ToString());
                                staticFramework.saveNVC(Fields, "HomeCityName", fieldValues[43].ToString());
                                staticFramework.saveNVC(Fields, "HomePostalCode", fieldValues[44].ToString());
                                staticFramework.saveNVC(Fields, "HomeAddrEqIDAddr", fieldValues[45].ToString());
                                staticFramework.saveNVC(Fields, "IDAddr1", fieldValues[46].ToString());
                                staticFramework.saveNVC(Fields, "IDAddr2", fieldValues[47].ToString());
                                staticFramework.saveNVC(Fields, "IDAddr3", fieldValues[48].ToString());
                                staticFramework.saveNVC(Fields, "IDSubDistrictCd", fieldValues[49].ToString());
                                staticFramework.saveNVC(Fields, "IDDistrictCd", fieldValues[50].ToString());
                                staticFramework.saveNVC(Fields, "IDSubDistrictName", fieldValues[51].ToString());
                                staticFramework.saveNVC(Fields, "IDDistrictName", fieldValues[52].ToString());
                                staticFramework.saveNVC(Fields, "IDCityName", fieldValues[53].ToString());
                                staticFramework.saveNVC(Fields, "HomePhoneNo", fieldValues[54].ToString());
                                staticFramework.saveNVC(Fields, "HomePhoneStaCd", fieldValues[55].ToString());
                                staticFramework.saveNVC(Fields, "PixelCode", fieldValues[56].ToString());
                                staticFramework.saveNVC(Fields, "PixelAddr", fieldValues[57].ToString());
                                staticFramework.saveNVC(Fields, "PixelWKCCode", fieldValues[58].ToString());
                                staticFramework.saveNVC(Fields, "MobilePhoneNo", fieldValues[59].ToString());
                                staticFramework.saveNVC(Fields, "MobilePhoneNo2", fieldValues[60].ToString());
                                staticFramework.saveNVC(Fields, "ResStaCd", fieldValues[61].ToString());
                                staticFramework.saveNVC(Fields, "ResLOSYY", fieldValues[62].ToString());
                                staticFramework.saveNVC(Fields, "IDTypeCd", fieldValues[93].ToString());
                                staticFramework.saveNVC(Fields, "IdCardTrackCd", fieldValues[105].ToString());
                                staticFramework.saveNVC(Fields, "IDPostalCode", fieldValues[110].ToString());
                                staticFramework.saveNVC(Fields, "Npwp", fieldValues[130].ToString());
                                staticFramework.saveNVC(Fields, "MailingAddrCd", fieldValues[150].ToString());
                                staticFramework.saveNVC(Fields, "Email", fieldValues[155].ToString());
                                staticFramework.saveNVC(Fields, "ChangeToNewHomeAddr", fieldValues[190].ToString());
                                staticFramework.saveNVC(Fields, "ExMobilePhone", fieldValues[240].ToString());
                                staticFramework.saveNVC(Fields, "CountryCd", fieldValues[241].ToString());
                                staticFramework.save(Fields, Keys, "CustPersonal", conn);
                                                                
                                // Insert to AppBilPmt
                                int a_ = 194;
                                int b_ = 195;
                                int c_ = 196;
                                for (int i = 0; i < 3; i++)
                                {
                                    a_ = a_ + (i * 3);
                                    b_ = b_ + (i * 3);
                                    c_ = c_ + (i * 3);
                                    staticFramework.saveNVC(Keys, "AppId", appId);
                                    staticFramework.saveNVC(Keys, "Seq", i);
                                    staticFramework.saveNVC(Fields, "BillerName", fieldValues[a_].ToString());
                                    staticFramework.saveNVC(Fields, "BillAccNo", fieldValues[b_].ToString());
                                    staticFramework.saveNVC(Fields, "NameOnBill", fieldValues[c_].ToString());
                                    staticFramework.save(Fields, Keys, "AppBillPmt", conn);
                                }

                                // Insert to AppExLoan
                                staticFramework.saveNVC(Keys, "AppId", appId);
                                staticFramework.saveNVC(Keys, "Seq", row);
                                staticFramework.saveNVC(Fields, "LnAccNo", fieldValues[206].ToString());
                                staticFramework.save(Fields, Keys, "AppExLoan", conn);

                                // Insert to AppFac
                                staticFramework.saveNVC(Keys, "AppId", appId);
                                staticFramework.saveNVC(Keys, "Seq", row);
                                staticFramework.saveNVC(Fields, "LimitReqAmt", fieldValues[131].ToString());
                                staticFramework.saveNVC(Fields, "LoanCode", fieldValues[132].ToString());
                                staticFramework.saveNVC(Fields, "Tenure", fieldValues[133].ToString());
                                staticFramework.saveNVC(Fields, "AccBeneficiaryName", fieldValues[134].ToString());
                                staticFramework.saveNVC(Fields, "AccBankName", fieldValues[135].ToString());
                                staticFramework.saveNVC(Fields, "AccBankId", fieldValues[136].ToString());
                                staticFramework.saveNVC(Fields, "AccBranchId", fieldValues[136].ToString());
                                staticFramework.saveNVC(Fields, "AccNo", fieldValues[136].ToString());
                                staticFramework.saveNVC(Fields, "PayeeName2", fieldValues[158].ToString());
                                staticFramework.saveNVC(Fields, "PayeeAccNo2", fieldValues[159].ToString());
                                staticFramework.saveNVC(Fields, "BranchAreaExp2", fieldValues[160].ToString());
                                staticFramework.saveNVC(Fields, "PayeeName3", fieldValues[161].ToString());
                                staticFramework.saveNVC(Fields, "PayeeAccNo3", fieldValues[162].ToString());
                                staticFramework.saveNVC(Fields, "BranchAreaExp3", fieldValues[163].ToString());
                                staticFramework.saveNVC(Fields, "DisbFeeFlag", fieldValues[164].ToString());
                                staticFramework.saveNVC(Fields, "DisbPercentage1", fieldValues[165].ToString());
                                staticFramework.saveNVC(Fields, "DisbPercentage2", fieldValues[166].ToString());
                                staticFramework.saveNVC(Fields, "DisbPercentage3", fieldValues[167].ToString());
                                staticFramework.saveNVC(Fields, "DisbFixAmt1", fieldValues[168].ToString());
                                staticFramework.saveNVC(Fields, "DisbFixAmt2", fieldValues[169].ToString());
                                staticFramework.saveNVC(Fields, "DisbFixAmt3", fieldValues[170].ToString());
                                staticFramework.saveNVC(Fields, "BranchAreaExp1", fieldValues[171].ToString());
                                staticFramework.saveNVC(Fields, "LoanPurpCd", fieldValues[184].ToString());
                                staticFramework.saveNVC(Fields, "PrevTopupLimit", fieldValues[189].ToString());
                                staticFramework.saveNVC(Fields, "CardType", fieldValues[203].ToString());
                                staticFramework.saveNVC(Fields, "PCTID", fieldValues[204].ToString());
                                staticFramework.saveNVC(Fields, "ProgTypeCd", fieldValues[210].ToString());
                                staticFramework.saveNVC(Fields, "FundTrans", fieldValues[211].ToString());
                                staticFramework.saveNVC(Fields, "FundTransLnAmtReq", fieldValues[212].ToString());
                                staticFramework.saveNVC(Fields, "FundTransLnTenureReq", fieldValues[213].ToString());
                                staticFramework.saveNVC(Fields, "FundTransLnPmtType", fieldValues[214].ToString());
                                staticFramework.saveNVC(Fields, "LoanPurpOth", fieldValues[215].ToString());
                                staticFramework.saveNVC(Fields, "DDNomType", fieldValues[216].ToString());
                                staticFramework.saveNVC(Fields, "DDPmtType", fieldValues[217].ToString());
                                staticFramework.saveNVC(Fields, "DDReqDays", fieldValues[218].ToString());
                                staticFramework.saveNVC(Fields, "PrevLnSegBaseCd", fieldValues[219].ToString());
                                staticFramework.saveNVC(Fields, "TopUpSegCd", fieldValues[220].ToString());
                                staticFramework.saveNVC(Fields, "InsProdCd", fieldValues[225].ToString());
                                staticFramework.saveNVC(Fields, "ApprvDecCd", fieldValues[226].ToString());
                                staticFramework.saveNVC(Fields, "ProjectCd", fieldValues[243].ToString());
                                staticFramework.saveNVC(Fields, "ProdTypeCd", fieldValues[244].ToString());
                                staticFramework.saveNVC(Fields, "ProgramCd", fieldValues[245].ToString());
                                staticFramework.saveNVC(Fields, "SubDecCd", fieldValues[248].ToString());
                                staticFramework.save(Fields, Keys, "AppFac", conn);

                                // Insert to AppFlag
                                staticFramework.saveNVC(Keys, "AppId", appId);
                                staticFramework.saveNVC(Fields, "DBSLnLimit_OS", fieldValues[154].ToString());
                                staticFramework.saveNVC(Fields, "BankCd", fieldValues[183].ToString());
                                staticFramework.saveNVC(Fields, "VerBillAddrMatch", fieldValues[221].ToString());
                                staticFramework.saveNVC(Fields, "VerMobilePhnMatch", fieldValues[222].ToString());
                                staticFramework.saveNVC(Fields, "NpwpDocFlag", fieldValues[223].ToString());
                                staticFramework.saveNVC(Fields, "AMLRiskRating", fieldValues[224].ToString());
                                staticFramework.save(Fields, Keys, "AppFlag", conn);

                                // Insert to AppSupp
                                staticFramework.saveNVC(Keys, "AppId", appId);
                                staticFramework.saveNVC(Keys, "Seq", row);
                                staticFramework.saveNVC(Fields, "NameAsIDCard", fieldValues[227].ToString());
                                staticFramework.saveNVC(Fields, "IDNumber", fieldValues[228].ToString());
                                staticFramework.saveNVC(Fields, "NameOnCard", fieldValues[229].ToString());
                                staticFramework.saveNVC(Fields, "Relationship", fieldValues[230].ToString());
                                staticFramework.saveNVC(Fields, "Gender", fieldValues[231].ToString());
                                staticFramework.saveNVC(Fields, "DateOfBirth", fieldValues[232].ToString());
                                staticFramework.saveNVC(Fields, "PlaceOfBirth", fieldValues[233].ToString());
                                staticFramework.saveNVC(Fields, "MothersMaidenName", fieldValues[234].ToString());
                                staticFramework.saveNVC(Fields, "MobilePhoneNo", fieldValues[235].ToString());
                                staticFramework.saveNVC(Fields, "Email", fieldValues[236].ToString());
                                staticFramework.saveNVC(Fields, "NationalityCd", fieldValues[237].ToString());
                                staticFramework.saveNVC(Fields, "CountryCd", fieldValues[242].ToString());
                                staticFramework.save(Fields, Keys, "AppSupp", conn);

                                // Insert to Cusjob
                                staticFramework.saveNVC(Keys, "AppId", appId);
                                staticFramework.saveNVC(Keys, "Seq", row);
                                staticFramework.saveNVC(Fields, "CurrLojYY", fieldValues[238].ToString());
                                staticFramework.saveNVC(Fields, "CurrLojMM", fieldValues[239].ToString());
                                staticFramework.save(Fields, Keys, "Cusjob", conn);

                                // Insert to CustExCC
                                staticFramework.saveNVC(Keys, "AppId", appId);
                                staticFramework.saveNVC(Keys, "Seq", row);
                                staticFramework.saveNVC(Fields, "CardNo", fieldValues[106].ToString());
                                staticFramework.saveNVC(Fields, "IssuerName", fieldValues[107].ToString());
                                staticFramework.saveNVC(Fields, "ExpiryDate", fieldValues[108].ToString());
                                staticFramework.saveNVC(Fields, "Limit", fieldValues[153].ToString());
                                staticFramework.saveNVC(Fields, "CardNo", fieldValues[178].ToString());
                                staticFramework.saveNVC(Fields, "IssuerName", fieldValues[179].ToString());
                                staticFramework.saveNVC(Fields, "ExpiryDate", fieldValues[180].ToString());
                                staticFramework.saveNVC(Fields, "CardNo", fieldValues[205].ToString());
                                staticFramework.save(Fields, Keys, "CustExCC", conn);

                                // Insert to CustFin
                                staticFramework.saveNVC(Keys, "AppId", appId);
                                staticFramework.saveNVC(Fields, "IncDocType", fieldValues[94].ToString());
                                staticFramework.saveNVC(Fields, "IncDeclare12M", fieldValues[101].ToString());
                                staticFramework.saveNVC(Fields, "IncDeclareExtraYearly", fieldValues[102].ToString());
                                staticFramework.saveNVC(Fields, "IncMonthlyBasedOnDoc", fieldValues[103].ToString());
                                staticFramework.saveNVC(Fields, "IncMultiplier", fieldValues[104].ToString());
                                staticFramework.saveNVC(Fields, "IncTotalBasedOnDoc", fieldValues[247].ToString());
                                staticFramework.save(Fields, Keys, "CustFin", conn);

                                // Insert to CustRel
                                staticFramework.saveNVC(Keys, "RelId", appId);
                                staticFramework.saveNVC(Fields, "AppId", appId);
                                staticFramework.saveNVC(Fields, "Seq", row);
                                staticFramework.saveNVC(Fields, "FullName", fieldValues[116].ToString());
                                staticFramework.saveNVC(Fields, "RelTypeCd", fieldValues[117].ToString());
                                staticFramework.saveNVC(Fields, "Addr1", fieldValues[118].ToString());
                                staticFramework.saveNVC(Fields, "Addr2", fieldValues[119].ToString());
                                staticFramework.saveNVC(Fields, "Addr3", fieldValues[120].ToString());
                                staticFramework.saveNVC(Fields, "SubDistrictCd", fieldValues[121].ToString());
                                staticFramework.saveNVC(Fields, "DistrictCd", fieldValues[122].ToString());
                                staticFramework.saveNVC(Fields, "CityName", fieldValues[123].ToString());
                                staticFramework.saveNVC(Fields, "PostalCode", fieldValues[124].ToString());
                                staticFramework.saveNVC(Fields, "HomePhoneNo", fieldValues[125].ToString());
                                staticFramework.saveNVC(Fields, "HomePhoneStaCd", fieldValues[126].ToString());
                                staticFramework.saveNVC(Fields, "MobilePhoneNo", fieldValues[127].ToString());
                                staticFramework.saveNVC(Fields, "OtherPhoneNo", fieldValues[128].ToString());
                                staticFramework.save(Fields, Keys, "CustRel", conn);

                                                            }
                            catch (Exception exp)
                            {
                                //insert log ke table detail log
                                NameValueCollection Keys = new NameValueCollection();
                                NameValueCollection Fields = new NameValueCollection();
                                staticFramework.saveNVC(Keys, "BatchFileName", _nFile_);
                                staticFramework.saveNVC(Keys, "RowNumber", row);
                                staticFramework.saveNVC(Fields, "LineNumber", row);
                                staticFramework.saveNVC(Fields, "KeyValue", fieldValues[4].ToString());
                                staticFramework.saveNVC(Fields, "LogType", "IWIDTYPE");
                                staticFramework.saveNVC(Fields, "LogMessage", exp);
                                staticFramework.save(Fields, Keys, "DataImportLogDetail", conn);
                                failed_ = failed_ + 1;
                            }
                        }
                        _timer_.Stop();
                        row_total_ = row;
                    }
                    // create log files
                    DateTime dt = DateTime.Now;
                    string _time_start = String.Format("{0:yyyy-MM-ddTHH:mm:ss}", _timer_.StartAt.Value);
                    string _time_end = String.Format("{0:yyyy-MM-ddTHH:mm:ss}", _timer_.EndAt.Value);
                    string _time_ = String.Format("{0:ddMMyyyyHHmm}", dt);
                    string sPath = "\\App_Data\\IWID_SPSFile_" + _time_ + ".resp";
                    StreamWriter SaveFile = new StreamWriter(sPath);
                    SaveFile.WriteLine(row_total_ + "|" + success_ + "|" + failed_ + "|" + _time_start + "|" + _time_end);
                    SaveFile.Close();
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
								SaveSPSDoc(Org, fieldValues["ORG"].ToString(), conn);
								SaveSPSDoc(Type, fieldValues["TYPE"].ToString(), conn);
								SaveSPSDoc(OriginAppId, fieldValues["APPLICATION NO"].ToString(), conn);
								SaveSPSDoc(ReceiptDate, fieldValues["RECEIPT DATE"].ToString(), conn);
								SaveSPSDoc(SolicitationId, fieldValues["SOLICITATION ID"].ToString(), conn);
								SaveSPSDoc(VIPIndicator, fieldValues["VIP INDICATOR"].ToString(), conn);
								SaveSPSDoc(UseCreditDesired, fieldValues["USE CREDIT DESIRED"].ToString(), conn);
								SaveSPSDoc(ProcCradleToGrave, fieldValues["PROCESS CRADLE TO GRAVE"].ToString(), conn);
								SaveSPSDoc(CreateCustRecord, fieldValues["CREATE CUSTOMER RECORD"].ToString(), conn);
								SaveSPSDoc(CreateRelRecord, fieldValues["CREATE RELATIONSHIP RECORD"].ToString(), conn);
																			
                                SaveSPSDoc(RelCd, fieldValues["RELATIONSHIP"].ToString(), conn);
                                SaveSPSDoc(CreateBaseSegRecord, fieldValues["CREATE BASE SEGMENT RECORD"].ToString(), conn);
                                SaveSPSDoc(SameCust, fieldValues["SAME CUSTOMER/BASE NUMBER"].ToString(), conn);
                                SaveSPSDoc(EmbMethodCd, fieldValues["EMBOSSING METHOD"].ToString(), conn);
                                SaveSPSDoc(BaseSegCd, fieldValues["BASE SEGMENT"].ToString(), conn);
                                SaveSPSDoc(CardSchemeCd, fieldValues["CARD SCHEME"].ToString(), conn);
								SaveSPSDoc(LoanPlanNo, fieldValues["LOAN PLAN NUMBER"].ToString(), conn);
                                SaveSPSDoc(PrimaryAcc, fieldValues["PRIMARY ACCOUNT"].ToString(), conn);
                                SaveSPSDoc(LoanRefNo, fieldValues["LOAN REFERENCE NUMBER"].ToString(), conn);
                                SaveSPSDoc(RelTypeIndCd, fieldValues["RELATIONSHIP TYPE INDICATOR"].ToString(), conn);
								
								SaveSPSDoc(ProcessTypeCd, fieldValues["PROCESS TYPE"].ToString(), conn);
                                SaveSPSDoc(AlgoProxyIncTest, fieldValues["ALGO PROXY INC TEST"].ToString(), conn);
                                SaveSPSDoc(PLNameOfBank, fieldValues["NAME OF THE BANK"].ToString(), conn);
                                SaveSPSDoc(Oth1, fieldValues["OTH1"].ToString(), conn);
                                SaveSPSDoc(AlgoSegTest, fieldValues["ALGO SEGMENT TEST"].ToString(), conn);
                                SaveSPSDoc(CustConsent, fieldValues["CUSTOMER CONSENT"].ToString(), conn);
								SaveSPSDoc(MarcomConsent, fieldValues["MARCOM CONSENT"].ToString(), conn);
                                SaveSPSDoc(Oth2, fieldValues["OTH2"].ToString(), conn);
                                SaveSPSDoc(AppRefNo, fieldValues["APPLICATION REFERENCE NUMBER"].ToString(), conn);
                                SaveSPSDoc(SourceCd, fieldValues["SOURCE CODE"].ToString(), conn);
								
								SaveSPSDoc(CampaignCd, fieldValues["CAMPAIGN CODE"].ToString(), conn);
                                SaveSPSDoc(LoanCentreCd, fieldValues["LOAN CENTRE CODE"].ToString(), conn);
                                SaveSPSDoc(AgentCd, fieldValues["AGENT CODE"].ToString(), conn);
                                SaveSPSDoc(SalesCd, fieldValues["SALES CODE"].ToString(), conn);
                                SaveSPSDoc(MgmCd, fieldValues["MGM CODE"].ToString(), conn);
                                SaveSPSDoc(UpfrontReject, fieldValues["UPFRONT REJECT"].ToString(), conn);
								SaveSPSDoc(Oth3, fieldValues["OTH 3"].ToString(), conn);
                                SaveSPSDoc(Oth4, fieldValues["OTH4"].ToString(), conn);
                                SaveSPSDoc(AlgoScoreTest, fieldValues[ALGO SCORE TEST].ToString(), conn);
                                SaveSPSDoc(AlgoTestTrack, fieldValues["ALGO TEST TRACK"].ToString(), conn);			
								SaveSPSDoc(ExcludeIndustry, fieldValues["EXCLUDE INDUSTRY"].ToString(), conn);
									SaveSPSDoc(PLOptDBSMUE, fieldValues["EXCLUDE INDUSTRY"].ToString(), conn);
									SaveSPSDoc(CCOptDBSMUE, fieldValues["EXCLUDE INDUSTRY"].ToString(), conn);
									SaveSPSDoc(MLOptDBSMUE, fieldValues["EXCLUDE INDUSTRY"].ToString(), conn);
									
									
                                SaveSPSDoc(SignatureDate, fieldValues["PERSONAL LOAN OPTIMUM DBS MUE	CREDIT CARD OPTIMUM DBS MUE	MONEY LINE OPTIMUM DBS MUE""].ToString(), conn);
                                SaveSPSDoc(AreaScore, fieldValues["SIGNATURE DATE"].ToString(), conn);
                                SaveSPSDoc(PSCd, fieldValues["AREA SCORE"].ToString(), conn);
                                SaveSPSDoc(CustType, fieldValues["PS CODE"].ToString(), conn);
								SaveSPSDoc(TopupType, fieldValues["CUSTOMER TYPE"].ToString(), conn);
                                SaveSPSDoc(OldestMOB, fieldValues["OLDEST MOB"].ToString(), conn);
								
                                SaveSPSDoc(OriginAccNo, fieldValues["ORIGINAL ACCOUNT NO."].ToString(), conn);
                                SaveSPSDoc(DiscFlag, fieldValues[DISC-FLAG].ToString(), conn);
								//SaveSPSDoc(PIN/PID/PI2 FLAG, fieldValues["//PIN/PID/PI2 FLAG"].ToString(), conn);
								
								SaveSPSDoc(BranchId, fieldValues["BRANCH CODE"].ToString(), conn);
                                SaveSPSDoc(RelWithBanks, fieldValues["RELATIONSHIP WITH THE BANKS"].ToString(), conn);
                                SaveSPSDoc(NeedProbing, fieldValues["NEED PROBING?"].ToString(), conn);
                                SaveSPSDoc(ProbingDetail, fieldValues["PROBING DETAIL"].ToString(), conn);
                                SaveSPSDoc(ProbingDetail2, fieldValues["PROBING DETAIL 2"].ToString(), conn);
                                SaveSPSDoc(TotalCardIssuers, fieldValues["DECLARED TOTAL CARD ISSUERS"].ToString(), conn);
								SaveSPSDoc(BillPmtReg, fieldValues["BILL PAYMENT REGISTRATION"].ToString(), conn);
                                SaveSPSDoc(SavingAccNo, fieldValues["DBS SAVING ACCOUNT NUMBER"].ToString(), conn);
                                SaveSPSDoc(ReqPrintBilling, fieldValues["REQUEST PRINT BILLING "].ToString(), conn);
                                SaveSPSDoc(Oth5, fieldValues["OTH5"].ToString(), conn);
								
								SaveSPSDoc(DocType, fieldValues["DOCUMENT TYPE"].ToString(), conn);
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
	}
        public static void UploadSPSAppDoc(string spsFilePath, string connString, int dbTimeOut)
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
                            string DocCd = "";
                            try
                            {
                                DataTable dtAppId = conn.GetDataTable("exec dbo.AppDoc @1,@2",
                                    new object[] { "SYSTEM", fieldValues[0].ToString() }, dbTimeOut);
                                if (dtAppId.Rows.Count == 0)
                                {
                                    //TODO: insert log ke table log

                                    continue;
                                }
                                DocCd = dtAppId.Rows[0]["DocCd"].ToString();
                                dtAppId.Dispose();

                                //UNDONE:save ke table app
                                NameValueCollection Keys = new NameValueCollection();
                                NameValueCollection Fields = new NameValueCollection();
                                staticFramework.saveNVC(Fields, "DocCd", OriginAppId);
                                                                      
                                staticFramework.save(Fields, Keys, "DocCd", conn);

                                //Save Doc
                                SaveSPSDoc(DocCd, fieldValues["DOC CODE 1"].ToString(), conn);
                                SaveSPSDoc(DocCd, fieldValues["DOC CODE 2"].ToString(), conn);
                                SaveSPSDoc(DocCd, fieldValues["DOC CODE 3"].ToString(), conn);
                                SaveSPSDoc(DocCd, fieldValues["DOC CODE 4"].ToString(), conn);
                                SaveSPSDoc(DocCd, fieldValues["DOC CODE 5"].ToString(), conn);
                                SaveSPSDoc(DocCd, fieldValues["DOC CODE 6"].ToString(), conn);
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
                            string BillerName = "";
                            try
                            {
                                DataTable dtAppId = conn.GetDataTable("exec dbo.AppBilPmt @1,@2",
                                    new object[] { "SYSTEM", fieldValues[0].ToString() }, dbTimeOut);
                                if (dtAppId.Rows.Count == 0)
                                {
                                    //TODO: insert log ke table log

                                    continue;
                                }
                                BillerName = dtAppId.Rows[0]["BillerName"].ToString();
                                dtAppId.Dispose();

                                //UNDONE:save ke table app
                                NameValueCollection Keys = new NameValueCollection();
                                NameValueCollection Fields = new NameValueCollection();
                                staticFramework.saveNVC(Fields, "BillerName", OriginAppId);

                                staticFramework.save(Fields, Keys, "BillerName", conn);

                                //Save Doc
                                SaveSPSDoc(BillerName, fieldValues["BILLER NAME 1"].ToString(), conn);
                                SaveSPSDoc(BillAccNo, fieldValues["BILL ACCOUNT NUMBER 1"].ToString(), conn);
                                SaveSPSDoc(NameOnBill, fieldValues["NAME ON BILL 1"].ToString(), conn);
                                SaveSPSDoc(BillerName, fieldValues["BILLER NAME 2"].ToString(), conn);
                                SaveSPSDoc(BillAccNo, fieldValues["BILL ACCOUNT NUMBER 2"].ToString(), conn);
                                SaveSPSDoc(NameOnBill, fieldValues["NAME ON BILL 2"].ToString(), conn);
								SaveSPSDoc(BillerName, fieldValues["BILLER NAME 3"].ToString(), conn);
                                SaveSPSDoc(BillAccNo, fieldValues["BILL ACCOUNT NUMBER 3"].ToString(), conn);
                                SaveSPSDoc(NameOnBill, fieldValues["NAME ON BILL 3"].ToString(), conn);
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
                            string LnAccNo = "";
                            try
                            {
                                DataTable dtAppId = conn.GetDataTable("exec dbo.AppExLoan @1,@2",
                                    new object[] { "SYSTEM", fieldValues[0].ToString() }, dbTimeOut);
                                if (dtAppId.Rows.Count == 0)
                                {
                                    //TODO: insert log ke table log

                                    continue;
                                }
                                LnAccNo = dtAppId.Rows[0]["LnAccNo"].ToString();
                                dtAppId.Dispose();

                                //UNDONE:save ke table app
                                NameValueCollection Keys = new NameValueCollection();
                                NameValueCollection Fields = new NameValueCollection();

                                staticFramework.saveNVC(Fields, "LnAccNo", OriginAppId);

                                staticFramework.save(Fields, Keys, "LnAccNo", conn);

                                //Save Doc
                                SaveSPSDoc(LnAccNo, fieldValues["DBS ACCOUNT NUMBER"].ToString(), conn);
                                
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
                            string DBSLnLimit_OS = "";
                            try
                            {
                                DataTable dtAppId = conn.GetDataTable("exec dbo.AppFlag @1,@2",
                                    new object[] { "SYSTEM", fieldValues[0].ToString() }, dbTimeOut);
                                if (dtAppId.Rows.Count == 0)
                                {
                                    //TODO: insert log ke table log

                                    continue;
                                }
                                DBSLnLimit_OS = dtAppId.Rows[0]["DBSLnLimit_OS"].ToString();
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
                                SaveSPSDoc(DBSLnLimit_OS, fieldValues["DBS LOAN LIMIT /OS BALANCE"].ToString(), conn);
                                SaveSPSDoc(BankCd, fieldValues["BANK  CODE"].ToString(), conn);
                                SaveSPSDoc(VerBillAddrMatch, fieldValues["BILLING ADDRESS MATCHING - VERIFICATION"].ToString(), conn);
                                SaveSPSDoc(VerMobilePhnMatch, fieldValues["MOBILE PHN NUMBER MATCHING - VERIFICATION"].ToString(), conn);
                                SaveSPSDoc(NpwpDocFlag, fieldValues["NPWP DOC FLAG"].ToString(), conn);
                                SaveSPSDoc(AMLRiskRating, fieldValues["AML RISK RATING"].ToString(), conn);
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
                            string LoanCode = "";
                            try
                            {
                                DataTable dtAppId = conn.GetDataTable("exec dbo.AppFlag @1,@2",
                                    new object[] { "SYSTEM", fieldValues[0].ToString() }, dbTimeOut);
                                if (dtAppId.Rows.Count == 0)
                                {
                                    //TODO: insert log ke table log

                                    continue;
                                }
                                LimitReqAmt = dtAppId.Rows[0]["LimitReqAmt"].ToString();
                                dtAppId.Dispose();

                                //UNDONE:save ke table app
                                NameValueCollection Keys = new NameValueCollection();
                                NameValueCollection Fields = new NameValueCollection();

                                staticFramework.saveNVC(Fields, "LimitReqAmt", OriginAppId);
                                staticFramework.saveNVC(Fields, "BankCd", OriginAppId);
                                staticFramework.saveNVC(Fields, "VerBillAddrMatch", OriginAppId);
                                staticFramework.saveNVC(Fields, "VerMobilePhnMatch", OriginAppId);
                                staticFramework.saveNVC(Fields, "NpwpDocFlag", OriginAppId);
                                staticFramework.saveNVC(Fields, "AMLRiskRating", OriginAppId);

                                staticFramework.save(Fields, Keys, "LimitReqAmt", conn);						
                                //Save Doc
                                SaveSPSDoc(LimitReqAmt, fieldValues["LOAN AMOUNT REQUEST"].ToString(), conn);
                                SaveSPSDoc(LoanCode, fieldValues["LOAN CODE"].ToString(), conn);
                                SaveSPSDoc(Tenure, fieldValues["LOAN TENOR"].ToString(), conn);
                                SaveSPSDoc(AccBeneficiaryName, fieldValues["BENEFICIARY NAME"].ToString(), conn);
                                SaveSPSDoc(AccBankName, fieldValues["BENEFICIARY BANK NAME"].ToString(), conn);
									SaveSPSDoc(AccBankId, fieldValues["BANK CODE"].ToString(), conn);
									SaveSPSDoc(AccBranchId, fieldValues["BR"].ToString(), conn);
									SaveSPSDoc(AccNo, fieldValues["ACC NO"].ToString(), conn);
                               // SaveSPSDoc(DBSLnLimit_OS, fieldValues[""].ToString(), conn);
                                SaveSPSDoc(PayeeName2, fieldValues["PAYEE NAME 2"].ToString(), conn);
								SaveSPSDoc(PayeeAccNo2, fieldValues["DC ACCOUNT 2"].ToString(), conn);
                                SaveSPSDoc(PayeeName2BranchAreaExp2, fieldValues["BRANCH AREA EXP 2"].ToString(), conn);
								
								SaveSPSDoc(PayeeName3, fieldValues["PAYEE NAME 3"].ToString(), conn);
                                SaveSPSDoc(PayeeAccNo3, fieldValues["DC ACCOUNT 3"].ToString(), conn);
                                SaveSPSDoc(BranchAreaExp3, fieldValues["BRANCH AREA EXP 3"].ToString(), conn);
                                SaveSPSDoc(DisbFeeFlag, fieldValues["DISB FEE FLAG"].ToString(), conn);
                                SaveSPSDoc(DisbPercentage1, fieldValues["DISB PERCENTAGE 1"].ToString(), conn);
								SaveSPSDoc(DisbPercentage2, fieldValues["DISB PERCENTAGE 2"].ToString(), conn);
						        SaveSPSDoc(DisbPercentage3, fieldValues["DISB PERCENTAGE 3"].ToString(), conn);
                                SaveSPSDoc(DisbFixAmt1, fieldValues["DISB FIXED AMT 1"].ToString(), conn);
								SaveSPSDoc(DisbFixAmt2, fieldValues["DISB FIXED AMT 2"].ToString(), conn);
                                SaveSPSDoc(DisbFixAmt3, fieldValues["DISB FIXED AMT 3"].ToString(), conn);
								
								SaveSPSDoc(BranchAreaExp1, fieldValues["BRANCH AREA EXP"].ToString(), conn);
                                //SaveSPSDoc(BankCd, fieldValues[""].ToString(), conn);
                                SaveSPSDoc(LoanPurpCd, fieldValues["URPOSE OF LOAN"].ToString(), conn);
                                SaveSPSDoc(PrevTopupLimit, fieldValues["PREVIOUS TOP UP LIMIT"].ToString(), conn);
                                SaveSPSDoc(CardType, fieldValues["CARD TYPE"].ToString(), conn);
								SaveSPSDoc(PCTID, fieldValues["PCT ID"].ToString(), conn);
						        SaveSPSDoc(ProgTypeCd, fieldValues["PROGRAM TYPE"].ToString(), conn);
                                SaveSPSDoc(FundTrans, fieldValues["MONEY LINE FUND TRANSFER (Y/N)"].ToString(), conn);
								SaveSPSDoc(FundTransLnAmtReq, fieldValues["FUND TRANSFER LOAN AMOUNT REQ"].ToString(), conn);
                                SaveSPSDoc(FundTransLnTenureReq, fieldValues["FUND TRANSFER LOAN TENOR REQ"].ToString(), conn);
                                //End Save Doc
								
								SaveSPSDoc(FundTransLnPmtType, fieldValues["FUND TRANSFER LOAN TYPE"].ToString(), conn);
                                SaveSPSDoc(LoanPurpOth, fieldValues["OTH PURPOSE OF LOAN"].ToString(), conn);
                                SaveSPSDoc(DDNomType, fieldValues["DD NOMINATED TYPE"].ToString(), conn);
                                SaveSPSDoc(DDPmtType, fieldValues["DD PAYMENT TYPE"].ToString(), conn);
                                SaveSPSDoc(DDReqDays, fieldValues["DD REQUEST DAYS"].ToString(), conn);
								SaveSPSDoc(PrevLnSegBaseCd, fieldValues["PREVIOUS LOAN SEGMENT BASE"].ToString(), conn);
						        SaveSPSDoc(TopUpSegCd, fieldValues["TOP UP SEGMENT"].ToString(), conn);
                                //SaveSPSDoc(VerBillAddrMatch, fieldValues[""].ToString(), conn);
								//SaveSPSDoc(VerMobilePhnMatch, fieldValues[""].ToString(), conn);
                                //SaveSPSDoc(NpwpDocFlag, fieldValues[""].ToString(), conn);

								//SaveSPSDoc(AMLRiskRating, fieldValues[""].ToString(), conn);
                                SaveSPSDoc(InsProdCd, fieldValues["INSURANCE PRODUCT CODE "].ToString(), conn);
                                SaveSPSDoc(ApprvDecCd, fieldValues["APPROVE DECLINE CODE"].ToString(), conn);
                                SaveSPSDoc(ProjectCd, fieldValues["PROJECT"].ToString(), conn);
                                SaveSPSDoc(ProdTypeCd, fieldValues["PRODUCT TYPE"].ToString(), conn);
								SaveSPSDoc(ProgramCd, fieldValues["PROGRAM"].ToString(), conn);
						        SaveSPSDoc(SubDecCd, fieldValues["SUB DECISION CODE"].ToString(), conn);

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
                                SaveSPSDoc(NameAsIDCard, fieldValues["SUPPLEMENTARY CARD NAME"].ToString(), conn);
                                SaveSPSDoc(IDNumber, fieldValues["SUPPLEMENTARY NO KTP/PASPOR"].ToString(), conn);
                                SaveSPSDoc(NameOnCard, fieldValues["SUPPLEMENTARY NAME ON CARD"].ToString(), conn);
                                SaveSPSDoc(Relationship, fieldValues["SUPPLEMENTARY RELATIONSHIP"].ToString(), conn);
                                SaveSPSDoc(Gender, fieldValues["SUPPLEMENTARY GENDER"].ToString(), conn);
                                SaveSPSDoc(DateOfBirth, fieldValues["SUPPLEMENTARY DOB (DD/MM/YYYY)"].ToString(), conn);
								SaveSPSDoc(PlaceOfBirth, fieldValues["SUPPLEMENTARY PLACE OF BIRTH"].ToString(), conn);
                                SaveSPSDoc(MothersMaidenName, fieldValues["SUPPLEMENTARY MOTHER'S MAIDEN NAME"].ToString(), conn);
                                SaveSPSDoc(MobilePhoneNo, fieldValues["SUPPLEMENTARY MOBILEPHONE"].ToString(), conn);
                                SaveSPSDoc(Email, fieldValues["SUPPLEMENTARY EMAIL ADDRESS"].ToString(), conn);
								
								SaveSPSDoc(NationalityCd, fieldValues["SUPPLEMENTARY NATIONALITY"].ToString(), conn);
                                SaveSPSDoc(CountryCd, fieldValues["SUPPLEMENTARY COUNTRY CODE"].ToString(), conn);
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
                                SaveSPSDoc(CardNo, fieldValues["CREDIT CARD NUMBER"].ToString(), conn);
                                SaveSPSDoc(IssuerName, fieldValues["NAME OF CARD ISSUER"].ToString(), conn);
                                SaveSPSDoc(ExpiryDate, fieldValues["CARD EXPIRY"].ToString(), conn);
                                SaveSPSDoc(Limit, fieldValues["CREDIT LIMIT AMOUNT"].ToString(), conn);
                                SaveSPSDoc(CardNo, fieldValues["2ND OTHER CARD NUMBER"].ToString(), conn);
                                SaveSPSDoc(IssuerName, fieldValues["2ND OTHER CARD ISSUERS "].ToString(), conn);
								SaveSPSDoc(ExpiryDate, fieldValues["2ND OTHER CARD EXPIRY"].ToString(), conn);
                                SaveSPSDoc(CardNo, fieldValues["DBS CREDIT CARD NUMBER"].ToString(), conn);
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
                                SaveSPSDoc(IncDocType, fieldValues["INCOME DOC TYPE"].ToString(), conn);
                                SaveSPSDoc(IncDeclare12M, fieldValues["12 MONTHS DECLARED INCOME "].ToString(), conn);
                                SaveSPSDoc(IncDeclareExtraYearly, fieldValues["EXTRA YEARLY DECLARED INCOME"].ToString(), conn);
                                SaveSPSDoc(IncMonthlyBasedOnDoc, fieldValues["MONTHLY INCOME BASED ON DOC"].ToString(), conn);
                                SaveSPSDoc(IncMultiplier, fieldValues["INCOME MULTIPLIER"].ToString(), conn);
                                SaveSPSDoc(IncTotalBasedOnDoc, fieldValues["TOTAL INCOME BASED ON DOC"].ToString(), conn);
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
                                SaveSPSDoc(WellEstablishCompType, fieldValues["WELL ESTABLISH COMPANY TYPE"].ToString(), conn);
                                SaveSPSDoc(ChangeToNewOffAddr, fieldValues["CHANGE TO NEW OFF/ BUSS ADDRESS (Y/N)"].ToString(), conn);
                                SaveSPSDoc(CurrLojYY, fieldValues["CURRENT COMPANY LENGTH ON JOB (YY )"].ToString(), conn);
                                SaveSPSDoc(CurrLojMM, fieldValues["CURRENT COMPANY LENGTH ON JOB (mm )"].ToString(), conn);
                                
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
                                SaveSPSDoc(CompName, fieldValues["COMPANY NAME"].ToString(), conn);
                                SaveSPSDoc(Addr1, fieldValues["COMPANY ADDRESS-1"].ToString(), conn);
                                SaveSPSDoc(Addr2, fieldValues["COMPANY ADDRESS-2"].ToString(), conn);
                                SaveSPSDoc(Addr3, fieldValues["COMPANY ADDRESS-3"].ToString(), conn);
                                SaveSPSDoc(DistrictName, fieldValues["COMPANY DISTRICT NAME"].ToString(), conn);
                                SaveSPSDoc(SubDistrictName, fieldValues["COMPANY SUB DISTRICT NAME"].ToString(), conn);
								SaveSPSDoc(DistrictCd, fieldValues["COMPANY DISTRICT CODE"].ToString(), conn);
                                SaveSPSDoc(CityName, fieldValues["COMPANY CITY"].ToString(), conn);
                                SaveSPSDoc(PostalCode, fieldValues["COMPANY POSTAL CODE"].ToString(), conn);
                                SaveSPSDoc(HomeAddrEqOffAddr, fieldValues["HOME ADDRESS = OFFICE ADDRESS ( 1.Y/ 2. N)"].ToString(), conn);
								
								SaveSPSDoc(PhoneNo, fieldValues["CO TEL NUMBER"].ToString(), conn);
                                SaveSPSDoc(PhoneExt, fieldValues["CO TEL EXTENSION"].ToString(), conn);
                                SaveSPSDoc(PhoneStaCd, fieldValues["CO TEL NUMBER STATUS"].ToString(), conn);
                                SaveSPSDoc(Phone2No, fieldValues["CO TEL NUMBER 2"].ToString(), conn);
                                SaveSPSDoc(Phone2Ext, fieldValues["CO TEL EXTENSION 2"].ToString(), conn);
                                SaveSPSDoc(Phone2StaCd, fieldValues["CO TEL NUMBER 2 STATUS"].ToString(), conn);
								SaveSPSDoc(Position, fieldValues["POSITION"].ToString(), conn);
                                SaveSPSDoc(PositionCd, fieldValues["POSITION CODE"].ToString(), conn);
                                SaveSPSDoc(NOB, fieldValues["NATURE OF BUSINESS"].ToString(), conn);
                                SaveSPSDoc(Department, fieldValues["DEPARTMENT"].ToString(), conn);
								
								SaveSPSDoc(EmployeeIDNo, fieldValues["EMPLOYEE ID NUMBER"].ToString(), conn);
                                SaveSPSDoc(JobStatus, fieldValues["JOB STATUS"].ToString(), conn);
                                
									SaveSPSDoc(TotalLojYY, fieldValues["TOTAL LENGTH ON JOB (YYMM)"].ToString(), conn);
									SaveSPSDoc(TotalLojMM, fieldValues["TOTAL LENGTH ON JOB (YYMM)"].ToString(), conn);
                                
									SaveSPSDoc(PrevLojYY, fieldValues["PREVIOUS COMPANY LENGTH ON JOB (YYMM)"].ToString(), conn);
									SaveSPSDoc(PrevLojMM, fieldValues["PREVIOUS COMPANY LENGTH ON JOB (YYMM)"].ToString(), conn);
									
								SaveSPSDoc(PervCompName, fieldValues["PREVIOUS COMPANY NAME"].ToString(), conn);
                                SaveSPSDoc(PrevPhoneNo, fieldValues["PREVIOUS CO TEL NUMBER"].ToString(), conn);
								SaveSPSDoc(PrevPhoneExt, fieldValues["PREVIOUS CO EXTENSION NUMBER"].ToString(), conn);
                                SaveSPSDoc(NoOfEmployee, fieldValues["NUMBER OF EMPLOYEE"].ToString(), conn);
                                SaveSPSDoc(EwssResult, fieldValues["EWSS RESULT"].ToString(), conn);
                                SaveSPSDoc(RelPartyResult, fieldValues["RELATED PARTY RESULT"].ToString(), conn);
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
                                SaveSPSDoc(CustNo, fieldValues["CUSTOMER NUMBER"].ToString(), conn);
                                SaveSPSDoc(CustName, fieldValues["NAME"].ToString(), conn);
                                SaveSPSDoc(CustNameOnID, fieldValues["NAME BASED ON ID"].ToString(), conn);
                                SaveSPSDoc(CustNickName, fieldValues["NICK NAME"].ToString(), conn);
                                SaveSPSDoc(CustNameInCard, fieldValues["NAME IN CARD"].ToString(), conn);
                                SaveSPSDoc(NationalityCd, fieldValues["NATIONALITY"].ToString(), conn);
								SaveSPSDoc(MotherMaidenName, fieldValues["MOTHER MAIDEN NAME"].ToString(), conn);
                                SaveSPSDoc(GenderCd, fieldValues["GENDER"].ToString(), conn);
                                SaveSPSDoc(PlaceOfBirth, fieldValues["PLACE OF BIRTH"].ToString(), conn);
                                SaveSPSDoc(DateOfBirth, fieldValues["DATE OF BIRTH"].ToString(), conn);
                                
								SaveSPSDoc(IDNumber, fieldValues["KTP NUMBER"].ToString(), conn);
                                SaveSPSDoc(EduCd, fieldValues["EDUCATIONAL LEVEL"].ToString(), conn);
                                SaveSPSDoc(MaritalCd, fieldValues["MARITAL STATUS"].ToString(), conn);
                                SaveSPSDoc(NoOfDependents, fieldValues["NUMBER OF DEPENDENTS"].ToString(), conn);
                                SaveSPSDoc(HomeAddr1, fieldValues["HOME CURRENT ADDRESS 1"].ToString(), conn);
                                SaveSPSDoc(HomeAddr2, fieldValues["HOME CURRENT ADDRESS 2"].ToString(), conn);
								SaveSPSDoc(HomeAddr3, fieldValues["HOME CURRENT ADDRESS 3"].ToString(), conn);
                                SaveSPSDoc(HomeDistrictCd, fieldValues["HOME DISTRICT CODE"].ToString(), conn);
                                SaveSPSDoc(HomeSubDistrictCd, fieldValues["HOME SUB DISTRICT CODE"].ToString(), conn);
                                SaveSPSDoc(HomeSubDistrictName, fieldValues["HOME SUB DISTRICT NAME"].ToString(), conn);
                                
								SaveSPSDoc(HomeDistrictName, fieldValues["HOME DISTRICT NAME"].ToString(), conn);
                                SaveSPSDoc(HomeCityName, fieldValues["HOME CITY"].ToString(), conn);
                                SaveSPSDoc(HomePostalCode, fieldValues["HOME POSTAL CODE"].ToString(), conn);
                                SaveSPSDoc(HomeAddrEqIDAddr, fieldValues["CURRENT ADDRESS = ID CARD ADDRESS"].ToString(), conn);
                                SaveSPSDoc(IDAddr1, fieldValues["ID CARD ADDRESS 1"].ToString(), conn);
                                SaveSPSDoc(IDAddr2, fieldValues["ID CARD ADDRESS 2"].ToString(), conn);
								SaveSPSDoc(IDAddr3, fieldValues["ID CARD ADDRESS 3"].ToString(), conn);
                                SaveSPSDoc(IDSubDistrictCd, fieldValues["ID CARD SUB DISTRICT CODE"].ToString(), conn);
                                SaveSPSDoc(IDDistrictCd, fieldValues["ID CARD DISTRICT CODE"].ToString(), conn);
                                SaveSPSDoc(IDSubDistrictName, fieldValues["ID CARD SUB DISTRICT NAME"].ToString(), conn);
								
								SaveSPSDoc(IDDistrictName, fieldValues["ID CARD DISTRICT NAME"].ToString(), conn);
                                SaveSPSDoc(IDCityName, fieldValues["ID CARD CITY"].ToString(), conn);
                                SaveSPSDoc(HomePhoneNo, fieldValues["HOME TEL NUMBER"].ToString(), conn);
                                SaveSPSDoc(HomePhoneStaCd, fieldValues["HOME TEL STATUS"].ToString(), conn);
                                SaveSPSDoc(PixelCode, fieldValues["PIXEL CODE"].ToString(), conn);
                                SaveSPSDoc(PixelAddr, fieldValues["PIXEL ADDRESS"].ToString(), conn);
								SaveSPSDoc(PixelWKCCode, fieldValues["PIXEL WKC CODE"].ToString(), conn);
                                SaveSPSDoc(MobilePhoneNo, fieldValues["MOBILE TEL NUMBER"].ToString(), conn);
                                SaveSPSDoc(MobilePhoneNo, fieldValues["MOBILE TEL NUMBER 2"].ToString(), conn);
                                SaveSPSDoc(ResStaCd, fieldValues["RESIDENTIAL STATUS"].ToString(), conn);
								
								SaveSPSDoc(ResLOSYY, fieldValues["LENGTH OF STAY (YY)"].ToString(), conn);
                                SaveSPSDoc(IDTypeCd, fieldValues["IDENTITY (ID)TYPE"].ToString(), conn);
                                SaveSPSDoc(IdCardTrackCd, fieldValues["ID CARD TRACK"].ToString(), conn);
                                SaveSPSDoc(IDPostalCode, fieldValues["ID POSTAL CODE"].ToString(), conn);
                                SaveSPSDoc(Npwp, fieldValues["TAX NUMBER (NPWP)"].ToString(), conn);
                                SaveSPSDoc(MailingAddrCd, fieldValues["MAILING ADDRESS IND"].ToString(), conn);
								SaveSPSDoc(Email, fieldValues["EMAIL ADDRESS"].ToString(), conn);
                                SaveSPSDoc(ChangeToNewHomeAddr, fieldValues["CHANGE TO NEW HOME ADDRESS (Y/N)"].ToString(), conn);
                                SaveSPSDoc(ExMobilePhone, fieldValues["EXISTING MOBILE PHONE NUMBER"].ToString(), conn);
                                SaveSPSDoc(CountryCd, fieldValues["COUNTRY CODE"].ToString(), conn);
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
                                SaveSPSDoc(FullName, fieldValues["EMERGENCY CONT NAME"].ToString(), conn);
                                SaveSPSDoc(RelTypeCd, fieldValues["EMERG RELATIONSHIP"].ToString(), conn);
                                SaveSPSDoc(Addr1, fieldValues["EMERGENCY ADDRESS-1"].ToString(), conn);
                                SaveSPSDoc(Addr2, fieldValues["EMERGENCY ADDRESS-2"].ToString(), conn);
                                SaveSPSDoc(Addr3, fieldValues["EMERGENCY ADDRESS-3"].ToString(), conn);
								SaveSPSDoc(SubDistrictCd, fieldValues["EMERGENCY SUB DISTRICT"].ToString(), conn);
                                SaveSPSDoc(DistrictCd, fieldValues["EMERGENCY DISTRICT"].ToString(), conn);
                                SaveSPSDoc(CityName, fieldValues["EMERGENCY CITY"].ToString(), conn);
                                SaveSPSDoc(PostalCode, fieldValues["EMERGENCY POSTAL CD"].ToString(), conn);
                                SaveSPSDoc(HomePhoneNo, fieldValues["EMERGENCY HOME TEL"].ToString(), conn);
								
								SaveSPSDoc(EMERGENCY HOME TEL STATUS, fieldValues[""].ToString(), conn);
                                SaveSPSDoc(EMERGENCY MOBILE TEL, fieldValues[""].ToString(), conn);
                                SaveSPSDoc(EMERGENCY TEL OTHERS, fieldValues[""].ToString(), conn);
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
    }  
}