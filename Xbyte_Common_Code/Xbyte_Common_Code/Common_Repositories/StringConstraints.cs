using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Xbyte_Common_Code.Common_Repositories
{
    public class StringConstraints
    {
        #region DB Details
        
        public string ConnectionString
        {
            get
            {
                return "server=192.168.1.38; user id=root;password=xbyte;database=xbyte_common_code";
            }
        } 
        #endregion

        #region File Path

        /// <summary>
        /// Log file path with date
        /// </summary>
        public string LogFilePath
        {
            get
            {
                return Application.StartupPath + "\\Logs\\log_" + CurrentDate + ".txt";
            }
        }
        #endregion

        /// <summary>
        /// getting Current Date formate (yyyy_MM_dd)
        /// </summary>
        public string CurrentDate
        {
            get
            {
                return DateTime.Now.ToString("yyyy_MM_dd");
            }
        }

        /// <summary>
        /// getting Past Date formate (yyyy_MM_dd)
        /// </summary>
        public string PastDate
        {
            get
            {
                return DateTime.Now.AddDays(-1).ToString("yyyy_MM_dd");
            }
        }
    }
}
