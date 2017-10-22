using System;
using System.Configuration;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

using CBS.Library.BOL;
using CBS.Library.EF;

namespace CBS.Library
{
    public class Class1
    {        
        public int BureauReqFileSlik()
        {
            List<AppBureauReq> list = MainBol.AppBureauReqBol.GetAllData();            
            int jmlList = list.Count;

            var jsonSerialiser = new JavaScriptSerializer();
            var jsonData = jsonSerialiser.Serialize(list);

            string filePath = @"C:\scbsliktest_bacth02.txt";
            string dummyLine = "";
            for (int i = 0; i < jmlList;i++ )
            {
                dummyLine = list[i].ReqId+"|03|I|"+list[i].IDNumber+Environment.NewLine+dummyLine;
            }            

            try 
            {
                if (File.Exists(filePath))
                {
                    //File.AppendAllText(filePath, dummyLine+Environment.NewLine);
                    return 0;
                }
                else
                {
                    File.WriteAllText(filePath, dummyLine+Environment.NewLine);
                    return 1;
                }
            }
            catch(Exception ex)
            {
                return 2;
            }                                   
        }
    }    
}
