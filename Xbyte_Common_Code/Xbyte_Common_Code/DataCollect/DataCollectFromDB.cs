using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbyte_Common_Code.Models;

namespace Xbyte_Common_Code.DataCollect
{
    class DataCollectFromDB
    {
        #region Variables

        List<P_And_G_Model> p_And_G_Models;
        #endregion


        #region Constructore

        public DataCollectFromDB()
        {
            p_And_G_Models = new List<P_And_G_Model>();
        } 
        #endregion

        public List<P_And_G_Model> GetP_And_G_DataFromDb()
        {


            return p_And_G_Models;
        }
    }
}
