using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Data.OleDb;

namespace CommonUtils
{
    /// <summary>
    /// SQL帮助类
    /// </summary>
    public class SqlHelper
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString;

        public SqlHelper()
        {
            ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["sqlconn"].ToString();
        }

        public SqlHelper(string connStr)
        {
            ConnectionString = connStr;

        }


        #region ExecuteNonQuery

        /// <summary>
        /// 在事务中执行方法
        /// </summary>
        /// <param name="act"></param>
        /// <returns></returns>
        public string ExeInTran(Func<SqlTransaction, SqlCommand, bool> act)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                SqlTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    bool b = act.Invoke(tx, cmd);

                    if (!b)
                        tx.Rollback();

                    //cmd.e
                    //act.Invoke(tx);
                    //int count = 1;
                    //for (int n = 0; n < SQLStringList.Count; n++)
                    //{
                    //    string strsql = SQLStringList[n];
                    //    if (strsql.Trim().Length > 1)
                    //    {
                    //        cmd.CommandText = strsql;
                    //        count += cmd.ExecuteNonQuery();
                    //    }
                    //}
                    tx.Commit();
                    return "";
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    return ex.ToString();
                }
            }
        }

        public int ExecuteProcReturn(string cmdText, string returnValue, params SqlParameter[] commandParams)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    PrepareCommand(con, cmd, CommandType.Text, cmdText, commandParams);

                    SqlParameter i = new SqlParameter("@backrt", SqlDbType.Int);

                    i.Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.Add(i);
                    int val = cmd.ExecuteNonQuery();
                    returnValue = cmd.Parameters["@backrt"].Value.ToString();
                    cmd.Parameters.Clear();
                    return val;
                }
            }
        }

        public int ExecuteNonQuery(CommandType cmdType, string cmdText, SqlParameter[] commandParams)
        {

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    PrepareCommand(con, cmd, cmdType, cmdText, commandParams);
                    int val = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    return val;
                }
            }

        }

        public int ExecuteNonQuery(string cmdText, params SqlParameter[] commandParams)
        {

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    PrepareCommand(con, cmd, CommandType.Text, cmdText, commandParams);
                    int val = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    return val;
                }
            }

        }


        public int ExecuteNonQuery(string cmdText)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(cmdText, con))
                {
                    con.Open();
                    int val = cmd.ExecuteNonQuery();
                    return val;
                }
            }
        }

        public int ExecuteNonQuery(string cmdText,string connString)
        {
            using (SqlConnection con = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand(cmdText, con))
                {
                    con.Open();
                    int val = cmd.ExecuteNonQuery();
                    return val;
                }
            }
        }
        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql">要执行的sql语句（单条，不执行事务）</param>
        /// <returns>无错返回string.Empty，有错返回Exception.Message</returns>
        public string ExecSql(string sql)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        return string.Empty;
                    }
                    catch (Exception ex)
                    {
                        con.Close();
                        return ex.Message;
                    }
                }
            }
        }
        #endregion

        #region ExecuteReader

        public SqlDataReader ExecuteReader(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand();
            try
            {
                PrepareCommand(con, cmd, cmdType, cmdText, commandParameters);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                con.Close();
                throw;
            }
        }

        #endregion

        #region ExecuteDataSet


        public DataSet ExecuteDataSet(CommandType cmdType, string cmdText, params SqlParameter[] para)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter();

                using (SqlCommand cmd = new SqlCommand())
                {
                    DataSet ds = new DataSet();
                    PrepareCommand(con, cmd, cmdType, cmdText, para);
                    adapter.SelectCommand = cmd;
                    adapter.Fill(ds);

                    return ds;
                }
            }
        }



        public DataSet ExecuteDataSet(string sql)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter();
                using (SqlCommand cmd = new SqlCommand())
                {
                    DataSet ds = new DataSet();
                    PrepareCommand(con, cmd, CommandType.Text, sql, null);
                    adapter.SelectCommand = cmd;
                    adapter.Fill(ds);

                    return ds;
                }
            }
        }

        public DataSet ExecuteDataSet(string conn, string sql)
        {
            using (SqlConnection con = new SqlConnection(conn))
            {
                SqlDataAdapter adapter = new SqlDataAdapter();
                using (SqlCommand cmd = new SqlCommand())
                {
                    DataSet ds = new DataSet();
                    PrepareCommand(con, cmd, CommandType.Text, sql, null);
                    adapter.SelectCommand = cmd;
                    adapter.Fill(ds);

                    return ds;
                }
            }
        }

        /// <summary>
        /// 根据指定的SQL语句,返回DATASET
        /// </summary>
        /// <param name="cmdtext">要执行带参的SQL语句</param>
        /// <param name="para">参数</param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string cmdtext, params SqlParameter[] para)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter();
                using (SqlCommand cmd = new SqlCommand())
                {
                    DataSet ds = new DataSet();
                    PrepareCommand(con, cmd, CommandType.Text, cmdtext, para);
                    adapter.SelectCommand = cmd;
                    adapter.Fill(ds);

                    return ds;
                }
            }
        }


        #endregion

        #region ExecuteScalar

        public object ExecuteScalar(string cmdText)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(cmdText, con))
                {
                    con.Open();
                    object obj = cmd.ExecuteScalar();
                    return obj;
                }
            }
        }

        public object ExecuteScalar(string cmdText,string connString)
        {
            using (SqlConnection con = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand(cmdText, con))
                {
                    con.Open();
                    object obj = cmd.ExecuteScalar();
                    return obj;
                }
            }
        }

        public object ExecuteScalar(string cmdText, params SqlParameter[] commandParameters)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    PrepareCommand(con, cmd, CommandType.Text, cmdText, commandParameters);
                    object val = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    return val;
                }
            }
        }
        //新添加的
        public object ExecuteScalar(string cmdText, CommandType ctype, params SqlParameter[] commandParameters)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        PrepareCommand(con, cmd, ctype, cmdText, commandParameters);
                        object val = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        return val;
                    }
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        #endregion


        #region 建立SqlCommand
        /// <summary>
        /// 建立SqlCommand
        /// </summary>
        /// <param name="con">SqlConnection　对象</param>
        /// <param name="cmd">要建立的Command</param>
        /// <param name="cmdType">CommandType</param>
        /// <param name="cmdText">执行的SQL语句</param>
        /// <param name="cmdParms">参数</param>
        private void PrepareCommand(SqlConnection con, SqlCommand cmd, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {
            if (con.State != ConnectionState.Open)
                con.Open();

            cmd.Connection = con;
            cmd.CommandType = cmdType;
            cmd.CommandText = cmdText;

            if (cmdParms != null)
                foreach (SqlParameter para in cmdParms)
                    cmd.Parameters.Add(para);
        }

        #endregion

        #region 用事物执行多条语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public int ExecuteSql(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();

                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch
                    {
                        connection.Close();
                        return -2;
                    }
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。执行成功:返回大于0的数字。执行失败:返回0。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>		
        public int ExecuteSqlTran(List<String> SQLStringList)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                SqlTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    int count = 1;
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n];
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            count += cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                    return count;
                }
                catch
                {
                    tx.Rollback();
                    return 0;
                }
            }
        }

        #endregion

        public int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        /// Execute a SqlCommand (that returns no resultset) against an existing database connection 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="conn">an existing database connection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public int ExecuteNonQuery(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        public int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        public SqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connectionString);

            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }


        public object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }

        public object ExecuteScalar(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }




        private void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

        #region 获取数据GetTable
        public DataTable GetDataTable(string sql)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter();
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    DataTable ds = new DataTable();
                    adapter.SelectCommand = cmd;
                    adapter.Fill(ds);
                    return ds;
                }
            }
        }

        public DataTable GetDataTable(string connStr, string sql)
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter adapter = new SqlDataAdapter();
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    DataTable ds = new DataTable();
                    adapter.SelectCommand = cmd;
                    adapter.Fill(ds);
                    return ds;
                }
            }
        }

        #endregion

        #region ACCESS
        public DataTable GetDataTable_ACCESS(string connStr, string sql)
        {
            using (OleDbConnection con = new OleDbConnection(connStr))
            {
                OleDbDataAdapter adapter = new OleDbDataAdapter();
                using (OleDbCommand cmd = new OleDbCommand(sql, con))
                {
                    DataTable ds = new DataTable();
                    adapter.SelectCommand = cmd;
                    adapter.Fill(ds);
                    return ds;
                }
            }
        }

        public DataSet ExecuteDataSet_ACCESS(string conn, string sql)
        {
            using (OleDbConnection con = new OleDbConnection(conn))
            {
                OleDbDataAdapter adapter = new OleDbDataAdapter();
                using (OleDbCommand cmd = new OleDbCommand())
                {
                    DataSet ds = new DataSet();
                    PrepareCommand_ACCESS(con, cmd, CommandType.Text, sql, null);
                    adapter.SelectCommand = cmd;
                    adapter.Fill(ds);

                    return ds;
                }
            }
        }

        private void PrepareCommand_ACCESS(OleDbConnection con, OleDbCommand cmd, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {
            if (con.State != ConnectionState.Open)
                con.Open();

            cmd.Connection = con;
            cmd.CommandType = cmdType;
            cmd.CommandText = cmdText;

            if (cmdParms != null)
                foreach (SqlParameter para in cmdParms)
                    cmd.Parameters.Add(para);
        }
        #endregion

        public bool TryConnect(string connString)
        {
            bool isConnected = false;
            try
            {
                SqlConnection conn = new SqlConnection(connString);
                conn.Open();
                isConnected = true;
                conn.Close();
            }
            catch(Exception e)
            {
                isConnected = false;
            }
            return isConnected;
        }

        public int ExecuteNonQuery_SQL(string connectionString, string cmdText)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(cmdText, con))
                {
                    con.Open();
                    int val = cmd.ExecuteNonQuery();
                    return val;
                }
            }
        }
    }
}
