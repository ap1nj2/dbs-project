﻿using DMS.Framework;
using DMS.Tools;
using DMSControls;
using IMKAHFA;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SLIK.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Web;

namespace CBS.ImportExportSLIK
{
    public class ImportSLIK
    {
        timeProcess _timer_ = new timeProcess();
        public Dictionary<string, object> ImportIdebText(string filePath)
        {
            string connString = ConfigurationManager.ConnectionStrings["DBSCBSConnectionString"].ConnectionString;
            Dictionary<string, object> result = new Dictionary<string, object>();
            int _row=0;
            result["Message"] = "Success";
            result["LogSummary"] = null;
            result["LogDetail"] = null;
            string _nFile_="";
            string _aFailed = "";
            string _aSuccess = "";

            try
            {
                _timer_.Start();
                FileInfo SLIKFilePath = new FileInfo(filePath);
                string detailLogFileName = Path.GetFileNameWithoutExtension(filePath) + "_detail.log";
                string summLogFileName = Path.GetFileNameWithoutExtension(filePath) + "_summary.log";
                result["LogDetail"] = new FileInfo(Path.GetFullPath(filePath) + detailLogFileName);
                result["LogSummary"] = new FileInfo(Path.GetFullPath(filePath) + summLogFileName);
                _nFile_ = Path.GetFileNameWithoutExtension(filePath);

                //MAPPING AND DESERIALIZE TABLE
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
                JToken fasilitas = type["fasilitas"];
                if (type != null)
                {
                    using (DbConnection conn = new DbConnection(connString))
                    {
                        try
                        {
                            if (type.Equals(perusahaan))
                            {
                                IdebCorpSearchVar icorpSearch = JsonConvert.DeserializeObject<IdebCorpSearchVar>(type["parameterPencarian"].ToString());
                                List<IdebCorpData> icorpDt = JsonConvert.DeserializeObject<List<IdebCorpData>>(type["dataPokokDebitur"].ToString());
                                List<IdebCorpGroup> icorpGrp = JsonConvert.DeserializeObject<List<IdebCorpGroup>>(type["kelompokPengurusPemilik"].ToString());
                                List<IdebSuratBerharga> iSrtBerharga = JsonConvert.DeserializeObject<List<IdebSuratBerharga>>(fasilitas["suratBerharga"].ToString());

                                InsertHeader(iHeader, conn);
                                InsertSaveParam(icorpSearch, iHeader.KodeReferensiPengguna, conn);
                                InsertCorpData(icorpDt, iHeader.KodeReferensiPengguna, conn);
                                InsertCorpGroup(icorpGrp, iHeader.KodeReferensiPengguna, conn);

                                //ini hanya sementara menunggu konfirmasi IfacID dapet dari mana
                                InsertFac(iHeader.KodeReferensiPengguna,"1",conn);

                                _row += 2 + icorpDt.Count + icorpGrp.Count;

                            }
                            else if (type.Equals(individual))
                            {
                                IdebIndivSearchVar iDebInvdSearch = JsonConvert.DeserializeObject<IdebIndivSearchVar>(type["parameterPencarian"].ToString());
                                List<IdebIndivData> iDataInd = JsonConvert.DeserializeObject<List<IdebIndivData>>(type["dataPokokDebitur"].ToString());
                                
                                InsertHeader(iHeader, conn);
                                InsertSrcParamIdv(iDebInvdSearch, iHeader.KodeReferensiPengguna, conn);
                                InsertDataIdv(iDataInd, iHeader.KodeReferensiPengguna, conn);

                                //ini hanya sementara menunggu konfirmasi IfacID dapet dari mana
                                InsertFac(iHeader.KodeReferensiPengguna,"1", conn);

                                _row += 2 + iDataInd.Count;
                            }

                            //same pola
                            IdebFacSummary ifacSumm = JsonConvert.DeserializeObject<IdebFacSummary>(type["ringkasanFasilitas"].ToString());
                            List<IdebKredit> iKredit = JsonConvert.DeserializeObject<List<IdebKredit>>(fasilitas["kredit"].ToString());
                            List<IdebLc> ilc = JsonConvert.DeserializeObject<List<IdebLc>>(fasilitas["lc"].ToString());
                            List<IdebBankGaransi> iBankGar = JsonConvert.DeserializeObject<List<IdebBankGaransi>>(fasilitas["bankGaransi"].ToString());
                            List<IdebFasilitasLainnya> iFasLain = JsonConvert.DeserializeObject<List<IdebFasilitasLainnya>>(fasilitas["fasilitasLainnya"].ToString());

                            InsertFacSummary(ifacSumm, iHeader.KodeReferensiPengguna, conn);
                            InsertFacKredit(iKredit,1, conn);
                            InsertFacLc(ilc,1, conn);
                            InsertBankGar(iBankGar,1, conn);
                            InsertFacOthers(iFasLain,1, conn);

                            _row += 1 + iKredit.Count + ilc.Count + iBankGar.Count + iFasLain.Count;
                            _aSuccess = "Success";
                        }
                        catch (Exception ex)
                        {
                            result["Message"] = ex.Message;
                            _aFailed = "Failed";
                            NameValueCollection Keys = new NameValueCollection();
                            NameValueCollection Fields = new NameValueCollection();
                            staticFramework.saveNVC(Keys, "BatchFileName", _nFile_);
                            staticFramework.saveNVC(Fields, "LogType", "SLIKTYPE");
                            staticFramework.saveNVC(Fields, "LogMessage", ex);
                            staticFramework.save(Fields, Keys, "DataImportLogDetail",conn);
                        }
                    }
                    _timer_.Stop();
                    string _time_start_ = String.Format("{0:ddMMyyyyHHmm}", _timer_.StartAt.Value);
                    string _time_end_ = String.Format("{0:ddMMyyyyHHmm}", _timer_.EndAt.Value);
                    DateTime dta = DateTime.Now;
                    string _time_ = String.Format("{0:ddMMyyyyHHmm}", dta);
                    string sPath = AppDomain.CurrentDomain.BaseDirectory + "SLIK_FILE_" + _time_ + ".resp";
                    //string sPath = "~/App_Data/" + "SLIK_FILE_" + _time_ + ".txt";
                    StreamWriter SaveFile = new StreamWriter(sPath);
                    SaveFile.WriteLine( _row+ "|" + _aSuccess + "|" + _aFailed + "|" + _time_start_ + "|" + _time_end_);
                    SaveFile.Close();
                }
            }
            catch (Exception ex)
            {
                result["Message"] = ex.Message;
                using (DbConnection conn = new DbConnection(connString))
                {
                    NameValueCollection Keys = new NameValueCollection();
                    NameValueCollection Fields = new NameValueCollection();
                    staticFramework.saveNVC(Keys, "BatchFileName", _nFile_);
                    staticFramework.saveNVC(Keys, "RowNumber", 5);
                    staticFramework.saveNVC(Fields, "LogType", "SLIKTYPE");
                    staticFramework.saveNVC(Fields, "LogMessage", result["Message"].ToString());
                    staticFramework.save(Fields, Keys, "DataImportLogDetail", conn);
                }
            }
            return result;
        }

        private string DatetimeToString(DateTime? date)
        {
            if (date == null)
                return string.Empty;
            else
                return string.Format("{0:yyyy-MM-dd HH':'mm':'ss}", date);
        }

        private string DateToString(DateTime? date)
        {
            if (date == null)
                return string.Empty;
            else
                return string.Format("{0:yyyy-MM-dd}", date);
        }

        private void InsertAgunan(IdebCollateral[] itemAgunan, string facID, DbConnection conn)
        {
            //insert to agunan
            int seq = 1;
            foreach (var item in itemAgunan)
            {
                NameValueCollection Keys = Keys = new NameValueCollection();
                NameValueCollection Field = Field = new NameValueCollection();
                staticFramework.saveNVC(Keys, "FacId", facID);
                staticFramework.saveNVC(Keys, "Seq", seq);
                staticFramework.saveNVC(Field, "DebtectorFacId", item.DebtectorFacId);
                staticFramework.saveNVC(Field, "DebtectorId", item.DebtectorId);
                staticFramework.saveNVC(Field, "JenisAgunanKet", item.JenisAgunanKet);
                staticFramework.saveNVC(Field, "NilaiAgunanMenurutLJK", item.NilaiAgunanMenurutLJK);
                staticFramework.saveNVC(Field, "ProsentaseParipasu", item.ProsentaseParipasu);
                staticFramework.saveNVC(Field, "TanggalUpdate", DatetimeToString(item.TanggalUpdate));
                staticFramework.saveNVC(Field, "NomorAgunan", item.NomorAgunan);
                staticFramework.saveNVC(Field, "JenisPengikatan", item.JenisPengikatan);
                staticFramework.saveNVC(Field, "TanggalPengikatan", DateToString(item.TanggalPengikatan));
                staticFramework.saveNVC(Field, "NamaPemilikAgunan", item.NamaPemilikAgunan);
                staticFramework.saveNVC(Field, "AlamatAgunan", item.AlamatAgunan);
                staticFramework.saveNVC(Field, "LokasiAgunan", item.LokasiAgunan);
                staticFramework.saveNVC(Field, "TglPenilaianLjk", DateToString(item.TglPenilaianLjk));
                staticFramework.saveNVC(Field, "PeringkatAgunan", item.PeringkatAgunan);
                staticFramework.saveNVC(Field, "LembagaPemeringkat", item.LembagaPemeringkat);
                staticFramework.saveNVC(Field, "BuktiKepemilikan", item.BuktiKepemilikan);
                staticFramework.saveNVC(Field, "NilaiAgunanNjop", item.NilaiAgunanNjop);
                staticFramework.saveNVC(Field, "NilaiAgunanIndep", item.NilaiAgunanIndep);
                staticFramework.saveNVC(Field, "NamaPenilaiIndep", item.NamaPenilaiIndep);
                staticFramework.saveNVC(Field, "Asuransi", item.Asuransi);
                staticFramework.saveNVC(Field, "TanggalPenilaianPenilaiIndependen", DateToString(item.TanggalPenilaianPenilaiIndependen));
                staticFramework.saveNVC(Field, "Keterangan", item.Keterangan);
                staticFramework.save(Field, Keys, "IdebFacCollateral", conn);
            }
        }

        private void InsertPenjamin(IdebPenjamin[] itemPenjamin, string FacID, DbConnection conn)
        {
            int seq = 1;
            foreach (var item in itemPenjamin)
            {
                NameValueCollection Keys = new NameValueCollection();
                NameValueCollection Field = new NameValueCollection();
                staticFramework.saveNVC(Keys, "FacId", FacID);
                staticFramework.saveNVC(Keys, "Seq", seq);
                staticFramework.saveNVC(Field, "NamaPenjamin", item.NamaPenjamin);
                staticFramework.saveNVC(Field, "NomorIdentitas", item.NomorIdentitas);
                staticFramework.saveNVC(Field, "TanggalUpdate", DatetimeToString(item.TanggalUpdate));
                staticFramework.saveNVC(Field, "TanggalBuat", DatetimeToString(item.TanggalBuat));
                staticFramework.saveNVC(Field, "JenisPenjamin", item.JenisPenjamin);
                staticFramework.saveNVC(Field, "AlamatPenjamin", item.AlamatPenjamin);
                staticFramework.saveNVC(Field, "Keterangan", item.Keterangan);
                staticFramework.save(Field, Keys, "IdebFacPenjamin", conn);
                seq++;
            }
        }

        private void InsertHeader(IdebHeader item, DbConnection conn)
        {
            NameValueCollection Keys = new NameValueCollection();
            NameValueCollection Field = new NameValueCollection();
            staticFramework.saveNVC(Keys, "Idebid", item.KodeReferensiPengguna);
            staticFramework.saveNVC(Field, "ReqId", item.IdPermintaan);
            staticFramework.saveNVC(Field, "Seq", 1);
            staticFramework.saveNVC(Field, "KodeReferensiPengguna", item.KodeReferensiPengguna);
            staticFramework.saveNVC(Field, "TanggalHasil", DatetimeToString(item.TanggalHasil));
            staticFramework.saveNVC(Field, "IdPenggunaPermintaan", item.IdPermintaan);
            staticFramework.saveNVC(Field, "IdPenggunaPermintaan", item.IdPenggunaPermintaan);
            staticFramework.saveNVC(Field, "DibuatOleh", item.DibuatOleh);
            staticFramework.saveNVC(Field, "KodeLJKPermintaan", item.KodeLJKPermintaan);
            staticFramework.saveNVC(Field, "KodeCabangPermintaan", item.KodeCabangPermintaan);
            staticFramework.saveNVC(Field, "KodeTujuanPermintaan", item.KodeTujuanPermintaan);
            staticFramework.saveNVC(Field, "TanggalPermintaan", DatetimeToString(item.TanggalPermintaan));
            staticFramework.saveNVC(Field, "TotalBagian", item.TotalBagian);
            staticFramework.saveNVC(Field, "NomorBagian", item.NomorBagian);
            staticFramework.save(Field, Keys, "Ideb", conn);
        }

        private void InsertFac(string IdebId,string FacId, DbConnection conn)
        {
            NameValueCollection Keys = new NameValueCollection();
            NameValueCollection Field = new NameValueCollection();
            staticFramework.saveNVC(Keys, "FacId", FacId);
            staticFramework.saveNVC(Keys, "IdebId", IdebId);
            staticFramework.saveNVC(Keys, "Seq", FacId+1);
            staticFramework.save(Field, Keys, "IdebFac", conn);
        }

        #region Insert Perusahaan
        private void InsertSaveParam(IdebCorpSearchVar item, string Idebid, DbConnection conn)
        {
            NameValueCollection Keys = new NameValueCollection();
            NameValueCollection Field = new NameValueCollection();
            staticFramework.saveNVC(Keys, "Idebid", Idebid);
            staticFramework.saveNVC(Field, "NamaBadanUsaha", item.NamaBadanUsaha);
            staticFramework.saveNVC(Field, "Npwp", item.Npwp);
            staticFramework.saveNVC(Field, "TempatPendirian", item.TempatPendirian);
            staticFramework.saveNVC(Field, "TanggalAktaPendirian", DatetimeToString(item.TanggalAktaPendirian));
            staticFramework.saveNVC(Field, "NomorAktaPendirian", item.NomorAktaPendirian);
            staticFramework.save(Field, Keys, "IdebCorpSearchVar", conn);
        }

        private void InsertCorpData(List<IdebCorpData> item, string IdebId, DbConnection conn)
        {
            int seq = 1;
            foreach (var Item in item)
            {
                NameValueCollection Keys = new NameValueCollection();
                NameValueCollection Field = new NameValueCollection();
                staticFramework.saveNVC(Keys, "Idebid", IdebId);
                staticFramework.saveNVC(Keys, "Seq", seq.ToString());
                staticFramework.saveNVC(Field, "DebtectorId", Item.DebtectorId);
                staticFramework.saveNVC(Field, "NamaDebitur", Item.NamaDebitur);
                staticFramework.saveNVC(Field, "NamaLengkap", Item.NamaLengkap);
                staticFramework.saveNVC(Field, "Npwp", Item.Npwp);
                staticFramework.saveNVC(Field, "BentukBu", Item.BentukBu);
                staticFramework.saveNVC(Field, "GoPublic", Item.GoPublic);
                staticFramework.saveNVC(Field, "TempatPendirian", Item.TempatPendirian);
                staticFramework.saveNVC(Field, "NoAktaPendirian", Item.NoAktaPendirian);
                staticFramework.saveNVC(Field, "TglAktaPendirian", DateToString(Item.TglAktaPendirian));
                staticFramework.saveNVC(Field, "Pelapor", Item.Pelapor);
                staticFramework.saveNVC(Field, "TanggalDibentuk", DatetimeToString(Item.TanggalDibentuk));
                staticFramework.saveNVC(Field, "TanggalUpdate", DatetimeToString(Item.TanggalUpdate));
                staticFramework.saveNVC(Field, "Alamat", Item.Alamat);
                staticFramework.saveNVC(Field, "Kelurahan", Item.Kelurahan);
                staticFramework.saveNVC(Field, "Kecamatan", Item.Kecamatan);
                staticFramework.saveNVC(Field, "KabKota", Item.KabKota);
                staticFramework.saveNVC(Field, "KodePos", Item.KodePos);
                staticFramework.saveNVC(Field, "Negara", Item.Negara);
                staticFramework.saveNVC(Field, "NoAktaTerakhir", Item.NoAktaTerakhir);
                staticFramework.saveNVC(Field, "TglAktaTerakhir", DateToString(Item.TglAktaTerakhir));
                staticFramework.saveNVC(Field, "SektorEkonomi", Item.SektorEkonomi);
                staticFramework.saveNVC(Field, "Pemeringkat", Item.Pemeringkat);
                staticFramework.saveNVC(Field, "Peringkat", Item.Peringkat);
                staticFramework.saveNVC(Field, "TanggalPemeringkatan", DateToString(Item.TanggalPemeringkatan));
                staticFramework.save(Field, Keys, "IdebCorpData", conn);
                seq++;
            }
        }

        private void InsertCorpGroup(List<IdebCorpGroup> item, string IdebId, DbConnection conn)
        {
            int seq = 1;
            foreach (var Item in item)
            {
                NameValueCollection Keys = new NameValueCollection();
                NameValueCollection Field = new NameValueCollection();
                staticFramework.saveNVC(Keys, "Idebid", IdebId);
                staticFramework.saveNVC(Field, "KodeLJK", Item.KodeLJK);
                staticFramework.saveNVC(Field, "NamaLJK", Item.NamaLJK);

                foreach (var itemPemilik in Item.PengurusList)
                {
                    staticFramework.saveNVC(Keys, "Seq", seq.ToString());
                    staticFramework.saveNVC(Field, "NamaSesuaiIdentitas", itemPemilik.NamaSesuaiIdentitas);
                    staticFramework.saveNVC(Field, "NomorIdentitas", itemPemilik.NomorIdentitas);
                    staticFramework.saveNVC(Field, "JenisKelamin", itemPemilik.KodeJenisKelamin);
                    staticFramework.saveNVC(Field, "PosisiPekerjaan", itemPemilik.KodePosisiPekerjaan);
                    staticFramework.saveNVC(Field, "ProsentaseKepemilikan", itemPemilik.ProsentaseKepemilikan);
                    staticFramework.saveNVC(Field, "StatusPengurusPemilik", itemPemilik.KodeStatusPengurusPemilik);
                    staticFramework.saveNVC(Field, "Alamat", itemPemilik.Alamat);
                    staticFramework.saveNVC(Field, "Kelurahan", itemPemilik.Kelurahan);
                    staticFramework.saveNVC(Field, "Kecamatan", itemPemilik.Kecamatan);
                    staticFramework.saveNVC(Field, "Kota", itemPemilik.KodeKota);
                    staticFramework.save(Field, Keys, "IdebCorpGroup", conn);
                    seq++;
                }

            }
        }
        #endregion

        #region Insert Individu
        private void InsertSrcParamIdv(IdebIndivSearchVar item, string IdebId, DbConnection conn)
        {
            NameValueCollection Keys = new NameValueCollection();
            NameValueCollection Field = new NameValueCollection();
            staticFramework.saveNVC(Keys, "Idebid", IdebId);
            staticFramework.saveNVC(Field, "NamaDebitur", item.NamaDebitur);
            staticFramework.saveNVC(Field, "JenisKelamin", item.JenisKelamin);
            staticFramework.saveNVC(Field, "JenisKelaminKet", item.JenisKelaminKet);
            staticFramework.saveNVC(Field, "NoIdentitas", item.NoIdentitas);
            staticFramework.saveNVC(Field, "Npwp", item.Npwp);
            staticFramework.save(Field, Keys, "IdebIndivSearchVar", conn);
        }

        private void InsertDataIdv(List<IdebIndivData> item, string IdebId, DbConnection conn)
        {
            int seq = 1;
            foreach (var Item in item)
            {
                NameValueCollection Keys = new NameValueCollection();
                NameValueCollection Field = new NameValueCollection();
                staticFramework.saveNVC(Keys, "Idebid", IdebId);
                staticFramework.saveNVC(Keys, "Seq", seq);
                staticFramework.saveNVC(Field, "NamaDebitur", Item.Identitas);
                staticFramework.saveNVC(Field, "NoIdentitas", Item.NoIdentitas);
                staticFramework.saveNVC(Field, "Alamat", Item.Alamat);
                staticFramework.saveNVC(Field, "JenisKelamin", Item.JenisKelamin);
                staticFramework.saveNVC(Field, "Npwp", Item.Npwp);
                staticFramework.saveNVC(Field, "TempatLahir", Item.TempatLahir);
                staticFramework.saveNVC(Field, "TanggalLahir", DateToString(Item.TanggalLahir));
                staticFramework.saveNVC(Field, "Pelapor", Item.Pelapor);
                staticFramework.saveNVC(Field, "TanggalDibentuk", DateToString(Item.TanggalDibentuk));
                staticFramework.saveNVC(Field, "TanggalUpdate", DateToString(Item.TanggalUpdate));
                staticFramework.saveNVC(Field, "Kelurahan", Item.Kelurahan);
                staticFramework.saveNVC(Field, "Kecamatan", Item.Kecamatan);
                staticFramework.saveNVC(Field, "KabKota", Item.KabKota);
                staticFramework.saveNVC(Field, "KodePos", Item.KodePos);
                staticFramework.saveNVC(Field, "Negara", Item.Negara);
                staticFramework.saveNVC(Field, "Pekerjaan", Item.Pekerjaan);
                staticFramework.saveNVC(Field, "TempatBekerja", Item.TempatBekerja);
                staticFramework.saveNVC(Field, "BidangUsaha", Item.BidangUsaha);
                staticFramework.saveNVC(Field, "KodeGelarDebitur", Item.KodeGelarDebitur);
                staticFramework.save(Field, Keys, "IdebIndivData", conn);
            }
        }
        #endregion

        #region Insert Fasilitas
        private void InsertFacSummary(IdebFacSummary item, string IdebId, DbConnection conn)
        {
            NameValueCollection Keys = new NameValueCollection();
            NameValueCollection Field = new NameValueCollection();
            staticFramework.saveNVC(Keys, "Idebid", IdebId);
            staticFramework.saveNVC(Field, "PlafonEfektifKredit", item.PlafonEfektifKredit);
            staticFramework.saveNVC(Field, "PlafonEfektifLc", item.PlafonEfektifLc);
            staticFramework.saveNVC(Field, "PlafonEfektifBg", item.PlafonEfektifBg);
            staticFramework.saveNVC(Field, "plafonEfektifSec", item.plafonEfektifSec);
            staticFramework.saveNVC(Field, "PlafonEfektifLainnya", item.PlafonEfektifLainnya);
            staticFramework.saveNVC(Field, "PlafonEfektifTotal", item.PlafonEfektifTotal);
            staticFramework.saveNVC(Field, "BakiDebetKredit", item.BakiDebetKredit);
            staticFramework.saveNVC(Field, "BakiDebetSec", item.BakiDebetSec);
            staticFramework.saveNVC(Field, "BakiDebetLainnya", item.BakiDebetLainnya);
            staticFramework.saveNVC(Field, "BakiDebetTotal", item.BakiDebetTotal);
            staticFramework.saveNVC(Field, "KrediturBankUmum", item.KrediturBankUmum);
            staticFramework.saveNVC(Field, "KrediturBPR_S", item.KrediturBPR_S);
            staticFramework.saveNVC(Field, "KrediturLp", item.KrediturLp);
            staticFramework.saveNVC(Field, "KrediturLainnya", item.KrediturLainnya);
            staticFramework.saveNVC(Field, "KolekTerburuk", item.KolekTerburuk);
            staticFramework.saveNVC(Field, "kolekBulanDataTerburuk", item.kolekBulanDataTerburuk);
            staticFramework.save(Field, Keys, "IdebFacSummary", conn);
        }

        private void InsertFacSb(List<IdebSuratBerharga> Item, int FacId, DbConnection conn)
        {
            foreach (var item in Item)
            {
                NameValueCollection Keys = new NameValueCollection();
                NameValueCollection Field = new NameValueCollection();
                staticFramework.saveNVC(Keys, "FacId", FacId.ToString());
                staticFramework.saveNVC(Field, "Ljk", item.Ljk);
                staticFramework.saveNVC(Field, "LjkKet", item.LjkKet);
                staticFramework.saveNVC(Field, "Cabang", item.Cabang);
                staticFramework.saveNVC(Field, "CabangKet", item.CabangKet);
                staticFramework.saveNVC(Field, "NominalSb", item.NominalSb);
                staticFramework.saveNVC(Field, "TanggalDibentuk", DatetimeToString(item.TanggalDibentuk));
                staticFramework.saveNVC(Field, "TanggalUpdate", DatetimeToString(item.TanggalUpdate));
                staticFramework.saveNVC(Field, "Bulan", item.Bulan);
                staticFramework.saveNVC(Field, "Tahun", item.Tahun);
                staticFramework.saveNVC(Field, "NoSuratBeharga", item.NoSuratBeharga);
                staticFramework.saveNVC(Field, "JenisSuratBeharga", item.JenisSuratBeharga);
                staticFramework.saveNVC(Field, "SovereignRate", item.SovereignRate);
                staticFramework.saveNVC(Field, "Listing", item.Listing);
                staticFramework.saveNVC(Field, "PeringkatSuratBeharga", item.PeringkatSuratBeharga);
                staticFramework.saveNVC(Field, "TujuanKepemilikan", item.TujuanKepemilikan);
                staticFramework.saveNVC(Field, "TujuanKepemilikanKet", item.TujuanKepemilikanKet);
                staticFramework.saveNVC(Field, "TanggalDiterbitkan", DateToString(item.TanggalDiterbitkan));
                staticFramework.saveNVC(Field, "TanggalJatuhTempo", DateToString(item.TanggalJatuhTempo));
                staticFramework.saveNVC(Field, "SukuBunga", item.SukuBunga);
                staticFramework.saveNVC(Field, "Valuta", item.KodeValuta);
                staticFramework.saveNVC(Field, "Kolektibilitas", item.Kolektibilitas);
                staticFramework.saveNVC(Field, "JumlahHariTunggakan", item.JumlahHariTunggakan);
                staticFramework.saveNVC(Field, "NilaiDalamMataUangAsal", item.NilaiDalamMataUangAsal);
                staticFramework.saveNVC(Field, "NilaiPasar", item.NilaiPasar);
                staticFramework.saveNVC(Field, "NilaiPerolehan", item.NilaiPerolehan);
                staticFramework.saveNVC(Field, "Tunggakan", item.Tunggakan);
                staticFramework.saveNVC(Field, "SebabMacet", item.SebabMacet);
                staticFramework.saveNVC(Field, "SebabMacetKet", item.SebabMacetKet);
                staticFramework.saveNVC(Field, "TanggalMacet", DateToString(item.TanggalMacet));
                staticFramework.saveNVC(Field, "Kondisi", item.Kondisi);
                staticFramework.saveNVC(Field, "KondisiKet", item.KondisiKet);
                staticFramework.saveNVC(Field, "TanggalKondisi", DateToString(item.TanggalKondisi));
                staticFramework.saveNVC(Field, "Keterangan", item.Keterangan);
                staticFramework.save(Field, Keys, "IdebFacSb", conn);

                InsertAgunan(item.Agunan, FacId.ToString(), conn);
                InsertPenjamin(item.Penjamin, FacId.ToString(), conn);
            }
        }

        private void InsertFacKredit(List<IdebKredit> iKredit,int facId, DbConnection conn)
        {
            int FacId = facId;
            foreach (var item in iKredit)
            {
                NameValueCollection Keys = new NameValueCollection();
                NameValueCollection Field = new NameValueCollection();
                staticFramework.saveNVC(Keys, "FacId", FacId.ToString());
                staticFramework.saveNVC(Field, "Ljk", item.Ljk);
                staticFramework.saveNVC(Field, "LjkKet", item.LjkKet);
                staticFramework.saveNVC(Field, "Cabang", item.Cabang);
                staticFramework.saveNVC(Field, "CabangKet", item.CabangKet);
                staticFramework.saveNVC(Field, "BakiDebet", item.BakiDebet);
                staticFramework.saveNVC(Field, "TanggalDibentuk", DatetimeToString(item.TanggalDibentuk));
                staticFramework.saveNVC(Field, "TanggalUpdate", DatetimeToString(item.TanggalUpdate));
                staticFramework.saveNVC(Field, "Bulan", item.Bulan);
                staticFramework.saveNVC(Field, "Tahun", item.Tahun);
                staticFramework.saveNVC(Field, "SifatKredit", item.SifatKredit);
                staticFramework.saveNVC(Field, "SifatKreditKet", item.SifatKreditKet);
                staticFramework.saveNVC(Field, "JenisKredit", item.JenisKredit);
                staticFramework.saveNVC(Field, "JenisKreditKet", item.JenisKreditKet);
                staticFramework.saveNVC(Field, "AkadPembiayaan", item.AkadPembiayaan);
                staticFramework.saveNVC(Field, "AkadPembiayaanKet", item.AkadPembiayaanKet);
                staticFramework.saveNVC(Field, "NoRekening", item.NoRekening);
                staticFramework.saveNVC(Field, "BaruPerpanjangan", item.BaruPerpanjangan);
                staticFramework.saveNVC(Field, "TanggalAkadAwal", DateToString(item.TanggalAkadAwal));
                staticFramework.saveNVC(Field, "TanggalAwalKredit", DateToString(item.TanggalAwalKredit));
                staticFramework.saveNVC(Field, "NoAkadAkhir", item.NoAkadAkhir);
                staticFramework.saveNVC(Field, "TanggalAkadAkhir", DateToString(item.TanggalAkadAkhir));
                staticFramework.saveNVC(Field, "TanggalAwalKredit", DateToString(item.TanggalAwalKredit));
                staticFramework.saveNVC(Field, "TanggalMulai", DateToString(item.TanggalMulai));
                staticFramework.saveNVC(Field, "TanggalJatuhTempo", DateToString(item.TanggalJatuhTempo));
                staticFramework.saveNVC(Field, "KategoriDebiturKode", item.KategoriDebiturKode);
                staticFramework.saveNVC(Field, "KategoriDebiturKet", item.KategoriDebiturKet);
                staticFramework.saveNVC(Field, "JenisPenggunaan", item.JenisPenggunaan);
                staticFramework.saveNVC(Field, "JenisPenggunaanKet", item.JenisPenggunaanKet);
                staticFramework.saveNVC(Field, "SektorEkonomi", item.SektorEkonomi);
                staticFramework.saveNVC(Field, "SektorEkonomiKet", item.sektorEkonomiKet);
                staticFramework.saveNVC(Field, "KreditProgramPemerintah", item.KreditProgramPemerintah);
                staticFramework.saveNVC(Field, "KreditProgramPemerintahKet", item.KreditProgramPemerintahKet);
                staticFramework.saveNVC(Field, "LokasiProyek", item.LokasiProyek);
                staticFramework.saveNVC(Field, "LokasiProyekKet", item.LokasiProyekKet);
                staticFramework.saveNVC(Field, "Valuta", item.KodeValuta);
                staticFramework.saveNVC(Field, "SukuBunga", item.SukuBunga);
                staticFramework.saveNVC(Field, "JenisSukuBunga", item.JenisSukuBunga);
                staticFramework.saveNVC(Field, "JenisSukuBungaKet", item.JenisSukuBungaKet);
                staticFramework.saveNVC(Field, "Kolektibilitas", item.Kolektibilitas);
                staticFramework.saveNVC(Field, "JumlahHariTunggakan", item.JumlahHariTunggakan);
                staticFramework.saveNVC(Field, "NilaiProyek", item.NilaiProyek);
                staticFramework.saveNVC(Field, "PlafonAwal", item.PlafonAwal);
                staticFramework.saveNVC(Field, "RealisasiBulanBerjalan", item.RealisasiBulanBerjalan);
                staticFramework.saveNVC(Field, "NilaiDalamMataUangAsal", item.NilaiDalamMataUangAsal);
                staticFramework.saveNVC(Field, "SebabMacet", item.SebabMacet);
                staticFramework.saveNVC(Field, "SebabMacetKet", item.SebabMacetKet);
                staticFramework.saveNVC(Field, "TanggalMacet", item.TanggalMacet);
                staticFramework.saveNVC(Field, "TunggakanPokok", item.TunggakanPokok);
                staticFramework.saveNVC(Field, "TunggakanBunga", item.TunggakanBunga);
                staticFramework.saveNVC(Field, "FrekuensiTunggakan", item.FrekuensiTunggakan);
                staticFramework.saveNVC(Field, "Denda", item.Denda);
                staticFramework.saveNVC(Field, "FrekuensiRestrukturisasi", item.FrekuensiRestrukturisasi);
                staticFramework.saveNVC(Field, "TanggalRestrukturisasiAkhir", DateToString(item.TanggalRestrukturisasiAkhir));
                staticFramework.saveNVC(Field, "KodeCaraRestrukturisasi", item.KodeCaraRestrukturisasi);
                staticFramework.saveNVC(Field, "RestrukturisasiKet", item.RestrukturisasiKet);
                staticFramework.saveNVC(Field, "Kondisi", item.Kondisi);
                staticFramework.saveNVC(Field, "KondisiKet", item.KondisiKet);
                staticFramework.saveNVC(Field, "TanggalKondisi", DateToString(item.TanggalKondisi));
                staticFramework.saveNVC(Field, "Keterangan", item.Keterangan);
                staticFramework.save(Field, Keys, "IdebFacCredit", conn);
                InsertAgunan(item.Agunan, FacId.ToString(), conn);
                InsertPenjamin(item.Penjamin, FacId.ToString(), conn);
            }
        }

        private void InsertFacLc(List<IdebLc> Ilc,int facId,DbConnection conn)
        {
            int FacId= facId;
            foreach (var item in Ilc)
            {
                NameValueCollection Keys = new NameValueCollection();
                NameValueCollection Field = new NameValueCollection();
                staticFramework.saveNVC(Keys, "FacId", FacId.ToString());
                staticFramework.saveNVC(Field, "Ljk", item.Ljk);
                staticFramework.saveNVC(Field, "LjkKet", item.LjkKet);
                staticFramework.saveNVC(Field, "Cabang", item.Cabang);
                staticFramework.saveNVC(Field, "CabangKet", item.CabangKet);
                staticFramework.saveNVC(Field, "NominalLc", item.NominalLc);
                staticFramework.saveNVC(Field, "TanggalDibentuk", DatetimeToString(item.TanggalDibentuk));
                staticFramework.saveNVC(Field, "TanggalUpdate", DatetimeToString(item.TanggalUpdate));
                staticFramework.saveNVC(Field, "Bulan", item.Bulan);
                staticFramework.saveNVC(Field, "Tahun", item.Tahun);
                staticFramework.saveNVC(Field, "NoLc", item.NoLc);
                staticFramework.saveNVC(Field, "JenisLc", item.JenisLc);
                staticFramework.saveNVC(Field, "JenisLcKet", item.JenisLcKet);
                staticFramework.saveNVC(Field, "TanggalDiterbitkan", DateToString(item.TanggalDiterbitkan));
                staticFramework.saveNVC(Field, "TanggalJatuhTempo", DateToString(item.TanggalJatuhTempo));
                staticFramework.saveNVC(Field, "NoAkadAwal", item.NoAkadAwal);
                staticFramework.saveNVC(Field, "TanggalAkadAwal", DateToString(item.TanggalAkadAwal));
                staticFramework.saveNVC(Field, "NoAkadAkhir", item.NoAkadAkhir);
                staticFramework.saveNVC(Field, "TanggalAkadAkhir", DateToString(item.TanggalAkadAkhir));
                staticFramework.saveNVC(Field, "BankCounterparty", item.BankCounterparty);
                staticFramework.saveNVC(Field, "NoAkadAkhir", item.NoAkadAkhir);
                staticFramework.saveNVC(Field, "TanggalAkadAkhir", DateToString(item.TanggalAkadAkhir));
                staticFramework.saveNVC(Field, "Valuta", item.KodeValuta);
                staticFramework.saveNVC(Field, "Plafon", item.Plafon);
                staticFramework.saveNVC(Field, "TanggalJatuhTempo", DateToString(item.TanggalJatuhTempo));
                staticFramework.saveNVC(Field, "TujuanLc", item.TujuanLc);
                staticFramework.saveNVC(Field, "TujuanLcKet", item.TujuanLcKet);
                staticFramework.saveNVC(Field, "SetoraJaminan", item.SetoranJaminan);
                staticFramework.saveNVC(Field, "TanggalWanPrestasi", item.TanggalWanPrestasi);
                staticFramework.saveNVC(Field, "Kondisi", item.Kondisi);
                staticFramework.saveNVC(Field, "KondisiKet", item.KondisiKet);
                staticFramework.saveNVC(Field, "TanggalKondisi", DateToString(item.TanggalKondisi));
                staticFramework.saveNVC(Field, "Keterangan", item.Keterangan);
                staticFramework.save(Field, Keys, "IdebFacLc", conn);
                InsertAgunan(item.Agunan, FacId.ToString(), conn);
                InsertPenjamin(item.Penjamin, FacId.ToString(), conn);
            }
            //FacId++;
        }

        private void InsertBankGar(List<IdebBankGaransi> iBankGar,int facId, DbConnection conn)
        {
            int FacId = facId;
            foreach (var item in iBankGar)
            {
                NameValueCollection Keys = new NameValueCollection();
                NameValueCollection Field = new NameValueCollection();
                staticFramework.saveNVC(Keys, "FacId", FacId.ToString());
                staticFramework.saveNVC(Field, "Ljk", item.Ljk);
                staticFramework.saveNVC(Field, "LjkKet", item.LjkKet);
                staticFramework.saveNVC(Field, "Cabang", item.Cabang);
                staticFramework.saveNVC(Field, "CabangKet", item.CabangKet);
                staticFramework.saveNVC(Field, "NominalBg", item.NominalBg);
                staticFramework.saveNVC(Field, "TanggalDibentuk", DatetimeToString(item.TanggalDibentuk));
                staticFramework.saveNVC(Field, "TanggalUpdate", DatetimeToString(item.TanggalUpdate));
                staticFramework.saveNVC(Field, "Bulan", item.Bulan);
                staticFramework.saveNVC(Field, "Tahun", item.Tahun);
                staticFramework.saveNVC(Field, "NoRekening", item.NoRekening);
                staticFramework.saveNVC(Field, "JenisGaransi", item.JenisGaransi);
                staticFramework.saveNVC(Field, "JenisGaransiKet", item.JenisGaransiKet);
                staticFramework.saveNVC(Field, "TanggalDiterbitkan", DateToString(item.TanggalDiterbitkan));
                staticFramework.saveNVC(Field, "TanggalJatuhTempo", DateToString(item.TanggalJatuhTempo));
                staticFramework.saveNVC(Field, "NoAkadAwal", item.NoAkadAwal);
                staticFramework.saveNVC(Field, "TanggalAkadAwal", DateToString(item.TanggalAkadAwal));
                staticFramework.saveNVC(Field, "NoAkadAkhir", item.NoAkadAkhir);
                staticFramework.saveNVC(Field, "TanggalAkadAkhir", DateToString(item.TanggalAkadAkhir));
                staticFramework.saveNVC(Field, "NamaYangDijamin", item.NamaYangDijamin);
                staticFramework.saveNVC(Field, "Kolektibilitas", item.Kolektibilitas);
                staticFramework.saveNVC(Field, "Valuta", item.KodeValuta);
                staticFramework.saveNVC(Field, "Plafon", item.Plafon);
                staticFramework.saveNVC(Field, "TujuanGaransi", item.KodeTujuanGaransi);
                staticFramework.saveNVC(Field, "TujuanGaransiKet", item.TujuanGaransiKet);
                staticFramework.saveNVC(Field, "TanggalJatuhTempo", DateToString(item.TanggalJatuhTempo));
                staticFramework.saveNVC(Field, "SetoranJaminan", item.SetoranJaminan);
                staticFramework.saveNVC(Field, "TanggalWanPrestasi", DateToString(item.TanggalWanPrestasi));
                staticFramework.saveNVC(Field, "Kondisi", item.Kondisi);
                staticFramework.saveNVC(Field, "KondisiKet", item.KondisiKet);
                staticFramework.saveNVC(Field, "TanggalKondisi", DateToString(item.TanggalKondisi));
                staticFramework.saveNVC(Field, "Keterangan", item.Keterangan);
                staticFramework.save(Field, Keys, "IdebFacBg", conn);
                InsertAgunan(item.Agunan, FacId.ToString(), conn);
                InsertPenjamin(item.Penjamin, FacId.ToString(), conn);
            }
        }

        private void InsertFacOthers(List<IdebFasilitasLainnya> iFasLain,int facId, DbConnection conn)
        {
            int FacId = facId;
            foreach (var item in iFasLain)
            {
                NameValueCollection Keys = new NameValueCollection();
                NameValueCollection Field = new NameValueCollection();
                staticFramework.saveNVC(Keys, "FacId", FacId.ToString());
                staticFramework.saveNVC(Field, "Ljk", item.Ljk);
                staticFramework.saveNVC(Field, "LjkKet", item.LjkKet);
                staticFramework.saveNVC(Field, "Cabang", item.Cabang);
                staticFramework.saveNVC(Field, "CabangKet", item.CabangKet);
                staticFramework.saveNVC(Field, "NominalJumlahKwajibanIDR", item.NominalJumlahKwajibanIDR);
                staticFramework.saveNVC(Field, "TanggalDibentuk", DatetimeToString(item.TanggalDibentuk));
                staticFramework.saveNVC(Field, "TanggalUpdate", DatetimeToString(item.TanggalUpdate));
                staticFramework.saveNVC(Field, "Bulan", item.Bulan);
                staticFramework.saveNVC(Field, "Tahun", item.Tahun);
                staticFramework.saveNVC(Field, "NoRekening", item.NoRekening);
                staticFramework.saveNVC(Field, "JenisFasilitas", item.JenisFasilitas);
                staticFramework.saveNVC(Field, "JenisFasilitasKet", item.JenisFasilitasKet);
                staticFramework.saveNVC(Field, "TanggalMulai", DateToString(item.TanggalMulai));
                staticFramework.saveNVC(Field, "TanggalJatuhTempo", DateToString(item.TanggalJatuhTempo));
                staticFramework.saveNVC(Field, "Valuta", item.KodeValuta);
                staticFramework.saveNVC(Field, "NilaiDalamMataUangAsal", item.NilaiDalamMataUangAsal);
                staticFramework.saveNVC(Field, "SukuBunga", item.SukuBunga);
                staticFramework.saveNVC(Field, "Kolektibilitas", item.Kolektibilitas);
                staticFramework.saveNVC(Field, "JumlahHariTunggakan", item.JumlahHariTunggakan);
                staticFramework.saveNVC(Field, "TanggalMacet", DateToString(item.TanggalMacet));
                staticFramework.saveNVC(Field, "SebabMacet", item.SebabMacet);
                staticFramework.saveNVC(Field, "Tunggakan", item.Tunggakan);
                staticFramework.saveNVC(Field, "Kondisi", item.Kondisi);
                staticFramework.saveNVC(Field, "KondisiKet", item.KondisiKet);
                staticFramework.saveNVC(Field, "TanggalKondisi", DateToString(item.TanggalKondisi));
                staticFramework.saveNVC(Field, "Keterangan", item.Keterangan);
                staticFramework.save(Field, Keys, "IdebFacOther", conn);
                InsertAgunan(item.Agunan, FacId.ToString(), conn);
                InsertPenjamin(item.Penjamin, FacId.ToString(), conn);
            }
        }
        #endregion
        
    }
}
