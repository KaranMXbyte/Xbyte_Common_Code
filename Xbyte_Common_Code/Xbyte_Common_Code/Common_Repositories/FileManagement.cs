using System.IO;
using System.Text;

namespace Xbyte_Common_Code.Common_Repositories
{
    public class FileManagement
    {

        /// <summary>
        /// This Method to create new file
        /// </summary>
        /// <param name="path">Whole file path with File Name</param>
        /// <param name="text">File Content</param>
        /// <param name="append">True/False</param>
        public void CreateFile(string path, string text, bool append)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                string directoryPath = Path.GetDirectoryName(path);
                Directory.CreateDirectory(directoryPath);
            }
            StreamWriter(path, text, append);
        }

        #region Private Method

        void CreateDirectory(string path, string folderName)
        {
            path = Path.Combine(path, folderName);
            Directory.CreateDirectory(path);
        }

        void StreamWriter(string path, string text, bool append)
        {
            using (StreamWriter file = new StreamWriter(path, append, Encoding.UTF8))
            {
                file.WriteLine(text);
            }
        }
        #endregion
    }
}
