using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbyte_Common_Code.Common_Repositories;

namespace Xbyte_Common_Code.QueryMakers
{
    class MysqlQueriesDynamic
    {
        #region Variables

        DbContext dbContext;
        StringConstraints stringConstraints;
        DataTable dt;
        string query;
        #endregion

        public MysqlQueriesDynamic(StringConstraints stringConstraints, FileManagement fileManagement)
        {
            dbContext = new DbContext(fileManagement, stringConstraints);
        }

        public string GetSearchQuery(string Project)
        {
            string NewQuery = "";
            query = "select * from header_list where project='"+ Project + "' and E_office_Header_value<>''";
            dt= dbContext.ExecuteSQLDataTable(query);
            NewQuery = "select ";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                NewQuery += dt.Rows[i][1] + " as " + dt.Rows[i][2] +" ";
            }
            NewQuery = " from productdata_" + stringConstraints.Date +" ";
            return NewQuery;
        }
    }
}
