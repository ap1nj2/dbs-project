using CBS.Library.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBS.Library.BOL
{
    public class AppBureauReqBOL
    {
        DBSCBSEntities we;        

        public AppBureauReqBOL()
        {
            we = new DBSCBSEntities();
        }

        public List<AppBureauReq> GetAllData()
        {
            try 
            {
                we = new DBSCBSEntities();               
                return we.AppBureauReqs.ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }       
    }
}
