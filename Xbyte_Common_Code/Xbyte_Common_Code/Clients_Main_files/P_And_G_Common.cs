using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbyte_Common_Code.QueryMakers;
using Xbyte_Common_Code.Common_Repositories;
using Xbyte_Common_Code.Models;
using Xbyte_Common_Code.DataCollect;

namespace Xbyte_Common_Code.Clients_Main_files
{
   public class P_And_G_Common
    {
        #region Variables

        MysqlQueriesDynamic queriesDynamic;
        StringConstraints stringConstraints;
        FileManagement fileManagement;
        DataCollectFromDB dataCollectFromDB;

        string Project = "PG_Core";
        #endregion

        public P_And_G_Common()
        {
            stringConstraints = new StringConstraints();
            fileManagement = new FileManagement();

            queriesDynamic = new MysqlQueriesDynamic(stringConstraints,fileManagement);
            dataCollectFromDB = new DataCollectFromDB(fileManagement, stringConstraints);
        }


        public void CheckData(string fileName)
        {

            List<P_And_G_Model> PastFileData = dataCollectFromDB.GetP_And_G_DataFromDb(queriesDynamic.GetSearchQuery(Project,stringConstraints.PastDate, fileName));
            List<P_And_G_Model> CurrentFileData = dataCollectFromDB.GetP_And_G_DataFromDb(queriesDynamic.GetSearchQuery(Project, stringConstraints.CurrentDate, fileName));

            for (int i = 0; i < CurrentFileData.Count; i++)
            {
                bool isUrlMatch= CurrentFileData[i].Url == PastFileData[i].Url;
            }
        }
    }
}
