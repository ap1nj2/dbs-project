using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using System.Collections.Specialized;
using DMS.Tools;

namespace CBS.Web.Common
{
    public class ModuleSupport
    {
        #region private vars
        private static string _moduleids;
        private static string Q_AUTHUSERMENU = "EXEC ES_AUTHUSERMENU @1, @2, @3 ";
        private static string SP_USP_ERRORLOG_SAVE = "EXEC USP_ERRORLOG_SAVE @1, @2, @3, @4, @5, @6, @7 ";
        #endregion

        #region authenticate
        public static string decryptConnStr(string encryptedConnStr)
        {
            string connStr, encpwd, decpwd = "";
            int pos1, pos2;
            pos1 = encryptedConnStr.IndexOf("pwd=");
            pos2 = encryptedConnStr.IndexOf(";", pos1 + 4);
            encpwd = encryptedConnStr.Substring(pos1 + 4, pos2 - pos1 - 4);
            for (int i = 2; i < encpwd.Length; i++)
            {
                char chr = (char)(encpwd[i] - 2);
                decpwd += new string(chr, 1);
            }
            connStr = encryptedConnStr.Replace(encpwd, decpwd);
            return connStr;
        }

        public static void AuthUserMenuAccess(Page page, string userid, string menucode, string modid)
        {
            if (!AuthUserMenuAccess(userid, menucode, modid))
            {
                page.Response.Write("You are not authorized to access this page!!");
                page.Response.End();
                return;
            }
        }

        public static bool AuthUserMenuAccess(string userid, string menucode, string modid)
        {
            bool retval = false;
            int timeout = int.Parse(ConfigurationSettings.AppSettings["dbTimeOut"]);
            using (DbConnection conn = new DbConnection(decryptConnStr(ConfigurationSettings.AppSettings["ConnString"])))
            {
                object[] par = new object[3] { userid, modid, menucode };
                conn.ExecReader(Q_AUTHUSERMENU, par, timeout);
                if (conn.hasRow())
                    if (conn.GetFieldValue(0) == "1")
                        retval = true;
            }
            return retval;
        }

        #region digital key
        public static string CalcSHA(string txt)
        {
            System.Security.Cryptography.HashAlgorithm crypt = new System.Security.Cryptography.SHA1Managed();
            byte[] byte_enc = crypt.ComputeHash(toByteArray(txt));
            crypt.Clear();
            return toHexStr(byte_enc);
        }

        public static string CalcSHA256(string txt)
        {
            System.Security.Cryptography.HashAlgorithm crypt = new System.Security.Cryptography.SHA256Managed();
            byte[] byte_enc = crypt.ComputeHash(toByteArray(txt));
            crypt.Clear();
            return toHexStr(byte_enc);
        }

        public static string CalcSHA512(string txt)
        {
            System.Security.Cryptography.HashAlgorithm crypt = new System.Security.Cryptography.SHA512Managed();
            byte[] byte_enc = crypt.ComputeHash(toByteArray(txt));
            crypt.Clear();
            return toHexStr(byte_enc);
        }

        public static string CalcMD5(string txt)
        {
            System.Security.Cryptography.HashAlgorithm crypt = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] byte_enc = crypt.ComputeHash(toByteArray(txt));
            crypt.Clear();
            return toHexStr(byte_enc);
        }

        public static string CalcMD160(string txt)
        {
            System.Security.Cryptography.HashAlgorithm crypt = new System.Security.Cryptography.RIPEMD160Managed();
            byte[] byte_enc = crypt.ComputeHash(toByteArray(txt));
            crypt.Clear();
            return toHexStr(byte_enc);
        }

        public static string CalcCRC16(string txt)
        {
            int container = 0, result = 0;
            foreach (char chr in txt)
            {
                result = updateCRC16(chr, container);
                container = result;
            }

            return result.ToString();
        }


        #region Hash-Based Support Function
        private static byte[] toByteArray(string src)
        {
            char[] char_src = src.ToCharArray();
            byte[] byte_src = new byte[char_src.Length];
            for (int i = 0; i < char_src.Length; i++)
                byte_src[i] = (byte)char_src[i];
            return byte_src;
        }
        private static string toHexStr(byte[] val)
        {
            string result = "";
            for (int i = 0; i < val.Length; i++)
                result += toHex(val[i]);
            return result;
        }
        private static string toHex(byte val)
        {
            if (val < 16)
                return "0" + val.ToString("X");
            return val.ToString("X");
        }
        #endregion
        #region CRC16 Support Function
        private static int[] crc_table = new int[256] { 0, 4129, 8258, 12387, 16516, 20645, 24774, 28903, 33032, 37161, 41290, 45419, 49548, 53677, 57806, 61935, 4657, 528, 12915, 8786, 21173, 17044, 29431, 25302, 37689, 33560, 45947, 41818, 54205, 50076, 62463, 58334, 9314, 13379, 1056, 5121, 25830, 29895, 17572, 21637, 42346, 46411, 34088, 38153, 58862, 62927, 50604, 54669, 13907, 9842, 5649, 1584, 30423, 26358, 22165, 18100, 46939, 42874, 38681, 34616, 63455, 59390, 55197, 51132, 18628, 22757, 26758, 30887, 2112, 6241, 10242, 14371, 51660, 55789, 59790, 63919, 35144, 39273, 43274, 47403, 23285, 19156, 31415, 27286, 6769, 2640, 14899, 10770, 56317, 52188, 64447, 60318, 39801, 35672, 47931, 43802, 27814, 31879, 19684, 23749, 11298, 15363, 3168, 7233, 60846, 64911, 52716, 56781, 44330, 48395, 36200, 40265, 32407, 28342, 24277, 20212, 15891, 11826, 7761, 3696, 65439, 61374, 57309, 53244, 48923, 44858, 40793, 36728, 37256, 33193, 45514, 41451, 53516, 49453, 61774, 57711, 4224, 161, 12482, 8419, 20484, 16421, 28742, 24679, 33721, 37784, 41979, 46042, 49981, 54044, 58239, 62302, 689, 4752, 8947, 13010, 16949, 21012, 25207, 29270, 46570, 42443, 38312, 34185, 62830, 58703, 54572, 50445, 13538, 9411, 5280, 1153, 29798, 25671, 21540, 17413, 42971, 47098, 34713, 38840, 59231, 63358, 50973, 55100, 9939, 14066, 1681, 5808, 26199, 30326, 17941, 22068, 55628, 51565, 63758, 59695, 39368, 35305, 47498, 43435, 22596, 18533, 30726, 26663, 6336, 2273, 14466, 10403, 52093, 56156, 60223, 64286, 35833, 39896, 43963, 48026, 19061, 23124, 27191, 31254, 2801, 6864, 10931, 14994, 64814, 60687, 56684, 52557, 48554, 44427, 40424, 36297, 31782, 27655, 23652, 19525, 15522, 11395, 7392, 3265, 61215, 65342, 53085, 57212, 44955, 49082, 36825, 40952, 28183, 32310, 20053, 24180, 11923, 16050, 3793, 7920 };
        private static void xor(int[] binary1, int[] binary2, int[] result)
        {
            for (int i = 0; i < 16; i++)
            {
                if (binary1[i] == binary2[i])
                    result[i] = 0;
                else
                    result[i] = 1;
            }
        }
        private static int toDecimal(int[] binary)
        {
            int tempDec = 0;
            int temp = 0;
            int j = 0;
            int factor = 0;

            for (int i = 0; i < 16; i++)
            {
                j = 15 - i;
                factor = 2;

                for (int k = 0; k <= i; k++)
                {
                    if (k == 0)
                    {
                        factor = 1;
                    }
                    else
                    {
                        factor *= 2;
                    }
                }
                temp = binary[j] * factor;

                tempDec = tempDec + temp;
            }
            return tempDec;
        }
        private static void toBinary(int dec, int[] binary)
        {
            int[] tempBin = new int[16];
            int tempDec = dec;

            for (int i = 15; i >= 0; i--)
            {
                if (tempDec != 1)
                {
                    tempBin[i] = tempDec % 2;
                    tempDec = tempDec / 2;
                }
                else if (tempDec == 1)
                {
                    tempBin[i] = 1;
                    tempDec = 0;
                }
                else
                {
                    tempBin[i] = 0;
                    tempDec = 0;
                }
            }
            for (int j = 0; j < 16; j++)
            {
                binary[j] = tempBin[j];
            }
        }
        private static int updateCRC16(char inputNum, int container)
        {
            int[] tempCRC = new int[16];
            int[] tempBin = new int[16];
            int[] tempBinInput = new int[16];
            int[] tempBinCRC = new int[16];
            int[] tempBinResult = new int[16];
            int[] tempBinResult2 = new int[16];
            int result = 0;
            int index = 0;
            int indexNum = 0;

            toBinary(container, tempCRC);

            // Move CRC to the right

            for (int i = 0; i < 8; i++)
            {
                tempBin[i + 8] = tempCRC[i];
            }

            index = toDecimal(tempBin);

            // USE INDEX TO GET NUMBER

            indexNum = crc_table[index];

            // Move CRC to the left

            for (int i = 0; i < 16; i++)
            {
                tempBin[i] = 0;
            }

            for (int i = 0; i < 8; i++)
            {
                tempBin[i] = tempCRC[i + 8];
            }

            toBinary(indexNum, tempBinCRC);

            toBinary(inputNum, tempBinInput);

            // XOR INDEX With 8 low order bits

            xor(tempBinCRC, tempBin, tempBinResult);

            xor(tempBinResult, tempBinInput, tempBinResult2);

            result = toDecimal(tempBinResult2);

            return result;
        }
        #endregion
        #endregion
        #endregion

        #region Static Property
        public static string SupportedModuleIDs
        {
            get
            {
                if ((_moduleids == null) || (_moduleids == ""))
                {
                    char[] sep = new char[2] { ',', ';' };
                    string[] modids = ConfigurationSettings.AppSettings["SupportedModuleIDs"].Split(sep);
                    _moduleids = "'" + modids[0] + "'";
                    for (int i = 1; i < modids.Length; i++)
                        _moduleids += ", '" + modids[i] + "'";
                }
                return _moduleids;
            }
        }
        #endregion

        #region Static Method
        #region menu methods
        #region menu query
        private static string Q_GRPMENUHEADING = "select menucode, menudisplay from vw_grpaccessmenuall " +
            "where moduleid = @1 and groupid = @2 and menulevel = '0' order by menucode ";
        private static string Q_GRPMENUDETAIL = "select menucode, menudisplay, tm_linkname, tm_parsingparam from vw_grpaccessmenuall " +
            "where moduleid = @1 and groupid = @2 and menuparentid = @3 order by menucode ";
        #endregion
        public static string GenMenuData(string moduleid, string groupid)
        {
            string connstr = decryptConnStr(ConfigurationSettings.AppSettings["ConnString"]);
            int timeout = int.Parse(ConfigurationSettings.AppSettings["dbTimeOut"]);
            string xmlresult;
            using (StringWriter result = new StringWriter())
            {
                XmlTextWriter writer = new XmlTextWriter(result);
                writer.Formatting = Formatting.Indented;

                writer.WriteStartElement("MenuData");
                writer.WriteAttributeString("ImagesBaseURL", "image/menu/");
                writer.WriteAttributeString("DefaultGroupCssClass", "MenuGroup");
                writer.WriteAttributeString("DefaultItemCssClass", "MenuItem");
                writer.WriteAttributeString("DefaultItemCssClassOver", "MenuItemOver");
                writer.WriteAttributeString("DefaultItemCssClassDown", "MenuItemDown");

                //submodule menu
                using (DbConnection connhead = new DbConnection(connstr))
                {
                    object[] parhead = new object[2] { moduleid, groupid };
                    connhead.ExecReader(Q_GRPMENUHEADING, parhead, timeout);
                    writer.WriteStartElement("MenuGroup");		//start MenuGroup head
                    while (connhead.hasRow())
                    {
                        string submodid = connhead.GetFieldValue(0);
                        writer.WriteStartElement("MenuItem");	//start MenuItem sub
                        writer.WriteAttributeString("Label", connhead.GetFieldValue(1));
                        writer.WriteAttributeString("LeftIcon", "bullet.gif");
                        writer.WriteAttributeString("LeftIconOver", "bullet_over.gif");
                        writer.WriteAttributeString("LeftIconWidth", "20");
                        writer.WriteAttributeString("LeftIconHeight", "12");

                        writer.WriteStartElement("MenuGroup");		//start MenuGroup sub
                        using (DbConnection connparent = new DbConnection(connstr))
                        {
                            object[] parparent = new object[3] { moduleid, groupid, submodid };
                            connparent.ExecReader(Q_GRPMENUDETAIL, parparent, timeout);
                            while (connparent.hasRow())
                            {
                                string mncode = connparent.GetFieldValue(0),
                                    mndisp = connparent.GetFieldValue(1),
                                    mnlink = connparent.GetFieldValue(2),
                                    mnlinkparam = connparent.GetFieldValue(3);
                                writer.WriteStartElement("MenuItem");
                                writer.WriteAttributeString("Label", mndisp);
                                writer.WriteAttributeString("LeftIcon", "bullet.gif");
                                writer.WriteAttributeString("LeftIconOver", "bullet_over.gif");
                                writer.WriteAttributeString("LeftIconWidth", "20");
                                writer.WriteAttributeString("LeftIconHeight", "12");
                                if (mnlink != "")
                                    writer.WriteAttributeString("URL", mnlink + mnlinkparam);

                                GenMenuChild(writer, moduleid, groupid, mncode, connstr, timeout);	//gen child if any

                                writer.WriteEndElement();
                            }
                        }
                        writer.WriteEndElement();		//end MenuGroup sub
                        writer.WriteEndElement();		//end MenuItem sub
                    }
                    // always add logout menu 
                    writer.WriteStartElement("MenuItem");
                    writer.WriteAttributeString("Label", "Logout");
                    writer.WriteAttributeString("LeftIcon", "bullet.gif");
                    writer.WriteAttributeString("LeftIconOver", "bullet_over.gif");
                    writer.WriteAttributeString("LeftIconWidth", "20");
                    writer.WriteAttributeString("LeftIconHeight", "12");
                    writer.WriteAttributeString("URL", "Logout.aspx");
                    writer.WriteAttributeString("URLTarget", "_top");
                    writer.WriteEndElement();
                    writer.WriteEndElement();		//end MenuGroup head
                }

                writer.WriteEndElement();		//end MenuData

                xmlresult = result.ToString();
            }

            return xmlresult;
        }

        private static void GenMenuChild(XmlTextWriter writer, string moduleid, string groupid, string menuparent, string connstr, int timeout)
        {
            bool haschild = false, firstloop = true;
            using (DbConnection connchild = new DbConnection(connstr))
            {
                object[] parchild = new object[3] { moduleid, groupid, menuparent };
                connchild.ExecReader(Q_GRPMENUDETAIL, parchild, timeout);
                while (connchild.hasRow())
                {
                    if (firstloop)
                    {
                        writer.WriteAttributeString("RightIcon", "arrow.gif");
                        writer.WriteAttributeString("RightconWidth", "17");
                        writer.WriteStartElement("MenuGroup");		//start MenuGroup child
                        firstloop = false;
                        haschild = true;
                    }
                    string mncode = connchild.GetFieldValue(0),
                        mndisp = connchild.GetFieldValue(1),
                        mnlink = connchild.GetFieldValue(2),
                        mnlinkparam = connchild.GetFieldValue(3);
                    writer.WriteStartElement("MenuItem");
                    writer.WriteAttributeString("Label", mndisp);
                    writer.WriteAttributeString("LeftIcon", "bullet.gif");
                    writer.WriteAttributeString("LeftIconOver", "bullet_over.gif");
                    writer.WriteAttributeString("LeftIconWidth", "20");
                    writer.WriteAttributeString("LeftIconHeight", "12");
                    if (mnlink != "")
                        writer.WriteAttributeString("URL", mnlink + mnlinkparam);

                    GenMenuChild(writer, moduleid, groupid, mncode, connstr, timeout);		//gen child if any

                    writer.WriteEndElement();
                }
                if (haschild)
                    writer.WriteEndElement();		//end MenuGroup child
            }
        }

        #endregion

        #region Connection Support
        public static void Save(NameValueCollection FieldNameNVC, NameValueCollection FieldKeyNVC,
            string TableName, DbConnection Conn, int dbTimeOut)
        {
            string SqlKey = " WHERE 1=1";
            int i;
            for (i = 0; i < FieldKeyNVC.Count; i++)
            {
                string FieldName = FieldKeyNVC.GetKey(i);
                string FieldValue = FieldKeyNVC[i];
                SqlKey += " AND " + FieldName + " = " + FieldValue;
            }

            Conn.ExecReader("SELECT top 1 1 FROM " + TableName + SqlKey, null, dbTimeOut);
            if (Conn.hasRow())
            {
                string UpdField = "";
                for (i = 0; i < FieldNameNVC.Count; i++)
                {
                    string FieldName = FieldNameNVC.GetKey(i);
                    string FieldValue = FieldNameNVC[i];
                    if (i == 0)
                        UpdField = FieldName + " = " + FieldValue;
                    else
                        UpdField += ", " + FieldName + " = " + FieldValue;
                }
                Conn.ExecuteNonQuery("UPDATE " + TableName + " SET " + UpdField + SqlKey, null, dbTimeOut);
            }
            else
            {
                NameValueCollection insFieldNameNVC = new NameValueCollection();
                for (i = 0; i < FieldKeyNVC.Count; i++)
                {
                    string FieldName = FieldKeyNVC.GetKey(i);
                    string FieldValue = FieldKeyNVC[i];
                    FieldNameNVC[FieldName] = FieldValue;
                }

                for (i = 0; i < FieldNameNVC.Count; i++)
                {
                    string FieldName = FieldNameNVC.GetKey(i);
                    string FieldValue = FieldNameNVC[i];
                    insFieldNameNVC[FieldName] = FieldValue;
                }

                string InsFieldName = "", InsFieldValue = "";
                for (i = 0; i < insFieldNameNVC.Count; i++)
                {
                    string FieldName = insFieldNameNVC.GetKey(i);
                    string FieldValue = insFieldNameNVC[i];
                    if (i == 0)
                    {
                        InsFieldName = FieldName;
                        InsFieldValue = FieldValue;
                    }
                    else
                    {
                        InsFieldName += "," + FieldName;
                        InsFieldValue += "," + FieldValue;
                    }
                }
                Conn.ExecuteNonQuery("INSERT INTO " + TableName + " (" + InsFieldName + " ) VALUES (" + InsFieldValue + ")", null, dbTimeOut);
            }
        }

        public static void Delete(NameValueCollection FieldKeyNVC, string TableName, DbConnection Conn, int dbTimeOut)
        {
            string SqlKey = " WHERE 1=1";
            for (int i = 0; i < FieldKeyNVC.Count; i++)
            {
                string FieldName = FieldKeyNVC.GetKey(i);
                string FieldValue = FieldKeyNVC[i];
                SqlKey += " AND " + FieldName + " = " + FieldValue;
            }
            Conn.ExecuteNonQuery("DELETE " + TableName + SqlKey, null, dbTimeOut);
        }

        public static string AutoGenerate_Key(NameValueCollection FieldKeyNVC,
            string TableName, string FieldKeyGenerated, string HeaderKey, int KeyDigit,
            DbConnection Conn, int DbTimeOut)
        {
            string SqlKey = " WHERE 1=1";
            int i;
            for (i = 0; i < FieldKeyNVC.Count; i++)
            {
                string FieldName = FieldKeyNVC.GetKey(i);
                string FieldValue = FieldKeyNVC[i];
                SqlKey += " AND " + FieldName + " = " + FieldValue;
            }

            Conn.ExecReader("SELECT ISNULL(MAX(" + FieldKeyGenerated + "),'1') FROM " + TableName + SqlKey, null, DbTimeOut);
            if (!Conn.hasRow())
                return null;

            string ResultKey = Conn.GetFieldValue(0);
            if (HeaderKey != "" && HeaderKey != null)
                ResultKey = ResultKey.Replace(HeaderKey, "");
            else
                HeaderKey = "";

            ResultKey = Convert.ToString((Convert.ToInt64(ResultKey) + 1));
            int reslen = ResultKey.Length;
            while (KeyDigit > reslen)
            {
                ResultKey = '0' + ResultKey;
                KeyDigit--;
            }
            return HeaderKey + ResultKey;
        }

        #region Query support
        public static string toSql(DateTime Value)
        {
            if (Value == null)
                return "NULL";
            else
                return "'" + Value.ToString("yyyy/MM/dd hh:mm:ss") + "'";
        }

        public static string toSql(string Value)
        {
            if (Value == null)
                return "NULL";
            else
                return "'" + Value + "'";
        }

        public static string toSql(decimal Value)
        {
            return Value.ToString();
        }

        public static string toSql(double Value)
        {
            return Value.ToString();
        }

        public static string toSql(int Value)
        {
           return Value.ToString();
        }

        public static string toSqlNow()
        {
            return "GETDATE()";
        }
        #endregion
        #endregion

        #region Error Log
        public static void LogError(Page source, Exception ex)
        {
            LogError(source.Request.RawUrl, (string)source.Session["UserID"], ex, source.Request.UserHostAddress, (string)source.Session["ModuleID"], (string)source.Session["ConnString"], (int)source.Session["DbTimeOut"]);
        }
        public static void LogError(string source, Page page, Exception ex)
        {
            LogError(source, (string)page.Session["UserID"], ex, page.Request.UserHostAddress, (string)page.Session["ModuleID"], (string)page.Session["ConnString"], (int)page.Session["DbTimeOut"]);
        }
        public static void LogError(string source, string user, Exception ex, string host, string modid, string connstr, int dbtimeout)
        {
            string errmsg = "", errdata = "", inner = "";
            try
            {
                errmsg = "MESSAGE: " + ex.Message;
                errdata = "DATA: " + ex.ToString();
                if (ex.InnerException != null)
                    inner = "INNEREXCEPTION: " + ex.InnerException.ToString();
            }
            catch (Exception ex1)
            {
                inner = "OTHER: " + ex1.Message;
            }
            
            using (DbConnection conn = new DbConnection(connstr))
            {
                object[] pardata = new object[7]
						{
							source, errmsg, errdata, inner, user, host, modid
						};
                conn.ExecuteNonQuery(SP_USP_ERRORLOG_SAVE, pardata, dbtimeout);
            }
        }
        #endregion

        #region auto disable controls
        public static void DisableControls(Control ctrls)
        {
            DisableControls(ctrls, false);
        }

        public static void DisableControls(Control ctrls, bool allowViewState)
        {
            if (!DisableChildControl(ctrls, false) && !allowViewState)		// !DisableChildControl means no postback control found in child controls 
                DisableChildViewState(ctrls);								// thus, viewstate shouldnt be needed... 

            // ensure to disable all buttons and linkbuttons found in main documents and its child frames using javascript 
            /*
            ctrls.Page.Response.Write("<script for=window event=onload language=javascript>" +
                "disDocBtns(document);" + 
                "function disDocBtns(doc){" +
                "for (i=0; i<doc.frames.length; i++){" +
                "disDocBtns(frames[i].document);}" +
                "disBtns(doc.Form1);disLnBtns(doc);}" +
                "function disBtns(frm){" +
                "for (i=0; i<frm.elements.length; i++){" +
                "elm = frm.elements[i];" +
                "if (elm.type=='button'||elm.type=='submit') elm.disabled = true;}}" +
                "function disLnBtns(doc){" +
                "for (i=0; i<doc.anchors.length; i++){" +
                "a = doc.anchors[i];" +
                "if (a.href.substring(0, 11) == 'javascript:') a.disabled = true;}}" +
                "</script>");
            */
        }

        private static bool DisableChildControl(Control ctrl, bool postback)
        {
            foreach (Control child in ctrl.Controls)
                try
                {
                    postback = DisableChildControl(child, postback);
                }
                catch { }

            switch (ctrl.GetType().Name)
            {
                case "HtmlInputButton":
                    HtmlInputButton bt = (HtmlInputButton)ctrl;
                    if (bt.Value.ToLower() == "edit" ||
                        bt.Value.ToLower() == "view" ||
                        bt.Value.ToLower() == "detail" ||
                        bt.Value.ToLower() == "download"
                        )
                        break;
                    bt.Disabled = true;
                    break;
                case "HtmlAnchor":
                    HtmlAnchor a = (HtmlAnchor)ctrl;
                    if (a.InnerText.ToLower() == "edit" ||
                        a.InnerText.ToLower() == "view" ||
                        a.InnerText.ToLower() == "detail" ||
                        a.InnerText.ToLower() == "download"
                        )
                        break;
                    a.Disabled = true;
                    break;
                case "HtmlInputFile":
                    HtmlInputFile fl = (HtmlInputFile)ctrl;
                    fl.Disabled = true;
                    break;
                case "HyperLink":
                    HyperLink lnk = (HyperLink)ctrl;
                    if (lnk.Text.ToLower() == "edit" ||
                        lnk.Text.ToLower() == "view" ||
                        lnk.Text.ToLower() == "detail" ||
                        lnk.Text.ToLower() == "download"
                        )
                        break;
                    lnk.Enabled = false;
                    break;
                case "Button":
                case "LinkButton":
                case "DataGridLinkButton":
                case "CheckBox":
                case "RadioButton":
                case "BTN_UPDATE":
                    WebControl wc = (WebControl)ctrl;
                    wc.Enabled = false;
                    break;
                case "TextBox":
                case "TXT_CURRENCY":
                case "TXT_DECIMAL":
                case "TXT_NUMBER":
                    TextBox txt = (TextBox)ctrl;
                    txt.ReadOnly = true;
                    break;
                case "DropDownList":
                    ListControl lc = (ListControl)ctrl;
                    if (!lc.AutoPostBack)
                        lc.Enabled = false;
                    else
                        postback = true;
                    break;
                case "ListBox":
                case "CheckBoxList":
                case "RadioButtonList":
                    ListControl lc2 = (ListControl)ctrl;
                    if (!lc2.AutoPostBack)
                        lc2.Enabled = false;
                    else
                        postback = true;
                    break;
                case "UC_Date_ascx":
                    //UC_Date dt = (UC_Date)ctrl;
                    //dt.Enabled = false;
                    break;
                case "UC_RefList_ascx":
                    //UC_RefList rl = (UC_RefList)ctrl;
                    //rl.Enabled = false;
                    break;
                case "CC_Date":
                    DMSControls.CC_Date ccdt = (DMSControls.CC_Date)ctrl;
                    ccdt.Enabled = false;
                    break;
                default:
                    break;
            }
            return postback;
        }

        private static void DisableChildViewState(Control ctrl)
        {
            foreach (Control child in ctrl.Controls)
                try
                {
                    DisableChildViewState(child);
                }
                catch { }
            ctrl.EnableViewState = false;

            switch (ctrl.GetType().Name)
            {
                case "DropDownList":
                    ListControl ddl = (ListControl)ctrl;
                    for (int i = ddl.Items.Count - 1; i > 0; i--)
                        if (!ddl.Items[i].Selected)
                            ddl.Items.RemoveAt(i);
                    break;
                case "CheckBoxList":
                case "RadioButtonList":
                    ListControl lc = (ListControl)ctrl;
                    for (int i = lc.Items.Count - 1; i >= 0; i--)
                        if (!lc.Items[i].Selected)
                            lc.Items.RemoveAt(i);
                    break;
                default:
                    break;
            }
        }

        #endregion
        public static WebControl NextCtrl(Page page, WebControl currCtrl)
        {
            bool found = false;
            foreach (Control allctrl in page.Controls)
                try
                {
                    WebControl ctrl = (WebControl)allctrl;
                    if (ctrl.TabIndex < currCtrl.TabIndex)
                        continue;
                    if (!found)
                    {
                        if (ctrl.ClientID == currCtrl.ClientID)
                            found = true;
                        continue;
                    }
                    return ctrl;
                }
                catch { }
            return null;
        }

        #endregion
    }
}
