using System.Collections.Generic;
using System.Linq;
using Xbyte_Common_Code.Common_Repositories;
using Xbyte_Common_Code.DataCollect;
using Xbyte_Common_Code.Models;
using Xbyte_Common_Code.QAs;
using Xbyte_Common_Code.QueryMakers;

namespace Xbyte_Common_Code.Clients_Main_files
{
    public class P_And_G_Common
    {
        #region Variables

        MysqlQueriesDynamic queriesDynamic;
        StringConstraints stringConstraints;
        FileManagement fileManagement;
        DataCollectFromDB dataCollectFromDB;
        QA_Checker qa_Checker;

        string Project_core = "PG_Core";
        string Project_Search = "PG_Search";
        #endregion

        public P_And_G_Common()
        {
            stringConstraints = new StringConstraints();
            fileManagement = new FileManagement();
            qa_Checker = new QA_Checker();
            queriesDynamic = new MysqlQueriesDynamic(stringConstraints, fileManagement);
            dataCollectFromDB = new DataCollectFromDB(fileManagement, stringConstraints);
        }

        /// <summary>
        /// This method use for P&G core feed Project
        /// </summary>
        /// <param name="feedId">E_office feed id</param>
        /// <returns></returns>
        public bool CheckData_Core(string feedId)
        {
            List<IssueDetectModel> issueDetectList = new List<IssueDetectModel>();
            List<P_And_G_Model_Core> PastFileData = dataCollectFromDB.GetP_And_G_DataFromDb(queriesDynamic.GetSearchQuery(Project_core, stringConstraints.PastDate, feedId));
            List<P_And_G_Model_Core> CurrentFileData = dataCollectFromDB.GetP_And_G_DataFromDb(queriesDynamic.GetSearchQuery(Project_core, stringConstraints.CurrentDate, feedId));

            for (int i = 0; i < CurrentFileData.Count; i++)
            {
                bool IsCheckStatus = false;
                P_And_G_Model_Core SingleRowDataFromPastTable = PastFileData.Where(x => x.HashId == CurrentFileData[i].HashId).FirstOrDefault();

                //This block will use for new row which is not found in past table
                {
                    IsCheckStatus = qa_Checker.IsContentNumeric(CurrentFileData[i].FinalPrice);
                    if (!IsCheckStatus)
                        issueDetectList.Add(GetIssueDetails(CurrentFileData[i].Url, "final price is not integer", "FinalPrice"));

                    IsCheckStatus = qa_Checker.IsContentNumeric(CurrentFileData[i].Stock);
                    if (CurrentFileData[i].Stock == "")
                        IsCheckStatus = false;

                    if (!IsCheckStatus)
                        issueDetectList.Add(GetIssueDetails(CurrentFileData[i].Url, "Stock not integer", "Stock"));

                    if (SingleRowDataFromPastTable == null) // if SingleRowData has a new row then qa will finish from here                    
                        continue;
                }

                IsCheckStatus = qa_Checker.IsContentMatch(SingleRowDataFromPastTable.Description, CurrentFileData[i].Description);
                if (!IsCheckStatus)
                    issueDetectList.Add(GetIssueDetails(CurrentFileData[i].Url, "Description is not match with past file", "Description"));
            }

            string Error = "";
            for (int i = 0; i < issueDetectList.Count; i++)
            {
                Error += "\r\n--------\r\n";
                Error += "Header name: " + issueDetectList[i].HeaderName + " \r\n Issue: " + issueDetectList[i].Issue + " , \r\n url: " + issueDetectList[i].Url;
            }

            fileManagement.CreateFile(stringConstraints.AutoJiraFilePath + Project_core + "_feed_id_" + feedId + ".txt", Error, false);

            if (issueDetectList.Count != 0)
                return true;
            else
                return false;

        }

        IssueDetectModel GetIssueDetails(string url, string issue, string headerName)
        {
            IssueDetectModel singleIssueDetect = new IssueDetectModel();
            singleIssueDetect.Url = url;
            singleIssueDetect.Issue = issue;
            singleIssueDetect.HeaderName = headerName;
            return singleIssueDetect;
        }
    }
}
