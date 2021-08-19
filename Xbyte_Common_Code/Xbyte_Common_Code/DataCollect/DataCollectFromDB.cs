﻿using System.Collections.Generic;
using System.Data;
using Xbyte_Common_Code.Common_Repositories;
using Xbyte_Common_Code.Models;

namespace Xbyte_Common_Code.DataCollect
{
    class DataCollectFromDB
    {
        #region Variables

        List<P_And_G_Model> p_And_G_Models;
        DbContext dbContext;
        FileManagement fileManagement;
        StringConstraints stringConstraints;
        DataTable dt;
        #endregion


        #region Constructore

        public DataCollectFromDB(FileManagement fileManagement, StringConstraints stringConstraints)
        {
            this.fileManagement = fileManagement;
            this.stringConstraints = stringConstraints;

            p_And_G_Models = new List<P_And_G_Model>();
            dbContext = new DbContext(fileManagement, stringConstraints);
        }
        #endregion

        public List<P_And_G_Model> GetP_And_G_DataFromDb(string query)
        {
            List<P_And_G_Model> Data = new List<P_And_G_Model>();
            dt = dbContext.ExecuteSQLDataTable(query);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                P_And_G_Model temp_P_And_G_Model = new P_And_G_Model();
                temp_P_And_G_Model.Url= dt.Rows[i]["url"].ToString();
                temp_P_And_G_Model.ItemId = dt.Rows[i]["itemid"].ToString();
                Data.Add(temp_P_And_G_Model);
            }

            return Data;
        }
    }
}
