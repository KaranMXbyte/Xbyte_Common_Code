using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbyte_Common_Code.QueryMakers;
using Xbyte_Common_Code.Common_Repositories;


namespace Xbyte_Common_Code.Clients_Main_files
{
    class P_And_G_Common
    {
        #region Variables

        MysqlQueriesDynamic queriesDynamic;
        StringConstraints stringConstraints;
        FileManagement fileManagement;
        #endregion

        public P_And_G_Common()
        {
            stringConstraints = new StringConstraints();
            fileManagement = new FileManagement();

            queriesDynamic = new MysqlQueriesDynamic(stringConstraints,fileManagement);
        }


        public void CheckData()
        {
            string q = queriesDynamic.GetSearchQuery("PG_Core");
        }
    }
}
