using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace Xbyte_Common_Code.Common_Repositories
{
    class HttpOperation
    {
        StringConstraints stringConstraints;
        FileManagement file;

        public HttpOperation(StringConstraints stringConstraints, FileManagement file)
        {
            this.stringConstraints = stringConstraints;
            this.file = file;
        }

        public string UpdateAtEOffice(string url)
        {
            string html = string.Empty;
        again:
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                webRequest.Host = new Uri(url).Host;
                webRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/68.0.3440.106 Safari/537.36";
                using (HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse())
                {
                    using (Stream resStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(resStream, Encoding.GetEncoding("utf-8"));
                        html = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                file.CreateFile(stringConstraints.LogFilePath, "Error" + ex.Message + " url: " + url + "\r\n -------------------  \r\n", true);
                Thread.Sleep(60000);
                goto again;
            }
            return html;
        }

    }
}
