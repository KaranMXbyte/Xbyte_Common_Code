using MySql.Data.MySqlClient;
using System;
using System.Data;
using Xbyte_Common_Code.Common_Repositories;

namespace Xbyte_Common_Code
{
    public class DbContext
    {

        #region Private Field

        MySqlConnection con;
        FileManagement fileManagement;
        StringConstraints stringConstraints;

        int Retry = 0;
        public bool IsError = false;
        Boolean TableExists = false;
        #endregion

        #region Controller

        public DbContext(FileManagement fileManagement, StringConstraints stringConstraints)
        {
            this.fileManagement = fileManagement;
            this.stringConstraints = stringConstraints;

            con = new MySqlConnection(stringConstraints.ConnectionString);
        }
        #endregion

        #region Public Method

        /// <summary>
        /// This Method use for get data in Dataset formate
        /// </summary>
        /// <param name="query">MySQL Query</param>        
        /// <returns>Dataset Object</returns>
        public DataSet ExecuteSQLDataSet(string query)
        {
            using (MySqlDataAdapter da = new MySqlDataAdapter(query, con))
            {
                using (DataSet ds = new DataSet())
                {
                    while (Retry < 3)
                    {
                    RunAgain_DataSet:
                        try
                        {
                            if (ConnectionState.Open != con.State)
                                con.Open();
                            da.Fill(ds);
                        }
                        catch (MySqlException ex)
                        {
                            if (Retry > 3)
                            {
                                fileManagement.CreateFile(stringConstraints.LogFilePath, ex.Message + "\r\n query: " + query + "\r\n -------------------  \r\n", true);
                                IsError = true;
                                break;
                            }
                            Retry++;
                            goto RunAgain_DataSet;
                        }
                        finally
                        {
                            con.Close();
                        }
                        if (ConnectionState.Closed == con.State)
                            break;
                    }
                    Retry = 0;
                    return ds;
                }
            }
        }

        /// <summary>
        /// This Method use for get data in DataTable formate
        /// </summary>
        /// <param name="query">MySQL Query</param>       
        /// <returns>DataTable Object</returns>
        public DataTable ExecuteSQLDataTable(string query)
        {

            using (MySqlDataAdapter da = new MySqlDataAdapter(query, con))
            {
                using (DataTable dt = new DataTable())
                {
                    while (Retry < 3)
                    {
                    RunAgain_DataSet:
                        try
                        {
                            if (ConnectionState.Open != con.State)
                                con.Open();
                            da.SelectCommand.CommandTimeout = 0;
                            da.Fill(dt);
                        }
                        catch (MySqlException ex)
                        {
                            if (Retry > 3)
                            {
                                fileManagement.CreateFile(stringConstraints.LogFilePath, ex.Message + "\r\n query: " + query + "\r\n -------------------  \r\n", true);
                                IsError = true;
                                break;
                            }
                            Retry++;
                            goto RunAgain_DataSet;
                        }
                        finally
                        {
                            con.Close();
                        }
                        if (ConnectionState.Closed == con.State)
                            break;
                    }
                    Retry = 0;
                    return dt;
                }
            }
        }

        /// <summary>
        /// This method use for CRUD operation
        /// </summary>
        /// <param name="query">MySql Query</param>       
        public void SqlDMLQuery(string query)
        {
            using (MySqlCommand cmd = new MySqlCommand(query, con))
            {
                while (Retry <= 3)
                {
                RunAgain_Dml:
                    try
                    {
                        if (ConnectionState.Open != con.State)
                            con.Open();
                        cmd.CommandTimeout = 10000;
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException ex)
                    {
                        if (ex.Message.Contains("Duplicate entry"))
                        {
                          fileManagement.CreateFile(stringConstraints.LogFilePath, ex.Message + "\r\n query: " + query + "\r\n -------------------  \r\n", true);
                            break;
                        }
                        if (Retry > 3)
                        {
                            fileManagement.CreateFile(stringConstraints.LogFilePath, ex.Message + " ( Query: " + query + " ) \r\n -------------------  \r\n", true);
                            IsError = true;
                            break;
                        }
                        Retry++;
                        goto RunAgain_Dml;
                    }
                    finally
                    {
                        con.Close();
                    }
                    if (ConnectionState.Closed == con.State)
                        break;
                }
                Retry = 0;
            }
        }

        /// <summary>
        /// Executes the query, and returns the first column of the first row in the result
        ///    set returned by the query. Extra columns or rows are ignored.
        /// </summary>
        /// <param name="query">SQL Query</param>       
        /// <returns>The first column of the first row in the result set, or a null reference if the result set is empty</returns>
        public string SQLExecuteScalar(string query)
        {
            string Result = string.Empty;
            using (MySqlCommand cmd = new MySqlCommand(query, con))
            {
                while (Retry < 3)
                {
                RunAgain_DataSet:
                    try
                    {
                        if (ConnectionState.Open != con.State)
                            con.Open();
                        Result = cmd.ExecuteScalar().ToString();
                    }
                    catch (MySqlException ex)
                    {
                        if (Retry > 3)
                        {
                            fileManagement.CreateFile(stringConstraints.LogFilePath, ex.Message + "\r\n query: " + query + "\r\n -------------------  \r\n", true);
                            IsError = true;
                            break;
                        }
                        Retry++;
                        goto RunAgain_DataSet;
                    }
                    finally
                    {
                        con.Close();
                    }
                    if (ConnectionState.Closed == con.State)
                        break;
                }
                Retry = 0;
                return Result;
            }
        }

        /// <summary>
        /// This Method only use for is table exist or not
        /// </summary>
        /// <param name="query">MYSQL Query</param>        
        /// <returns>true / false </returns>
        public bool IsTableExists(string query)
        {           
            using (MySqlCommand cmd = new MySqlCommand(query, con))
            {
                while (Retry <= 3)
                {
                RunAgain_TableExists:
                    try
                    {
                        if (ConnectionState.Open != con.State)
                            con.Open();
                        TableExists = Convert.ToBoolean(cmd.ExecuteScalar());
                    }
                    catch (MySqlException ex)
                    {
                        TableExists = false;
                        if (Retry > 3)
                        {
                            IsError = true;
                            fileManagement.CreateFile(stringConstraints.LogFilePath, ex.Message + "\r\n query: " + query + "\r\n -------------------  \r\n", true);
                            break;
                        }
                        Retry++;
                        goto RunAgain_TableExists;
                    }
                    finally
                    {
                        con.Close();
                    }
                    if (ConnectionState.Closed == con.State)
                        break;
                }
            }
            Retry = 0;
            return TableExists;
        }
        #endregion
    }
}
