using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace net.ELF.DBTool
{
    internal class SqlServer
    {
        SqlConnection sqlConn;
        SqlCommand sqlCmd;
        int dbTimeout;
        string connstr;

        /// <summary>
        /// 构造Sql Server数据库操作类
        /// </summary>
        /// <param name="db_host">Sql Server服务器地址</param>
        /// <param name="db_user">Sql Server用户名</param>
        /// <param name="db_user_pwd">Sql Server密码</param>
        /// <param name="db_name">Sql Server中的数据库名字</param>
        public SqlServer(string connectStr, int dbTimeout)
        {
            this.dbTimeout = dbTimeout;
            connstr = connectStr;
            sqlConn = new SqlConnection(connstr);
            sqlConn.Open();
            sqlCmd = new SqlCommand();
            sqlCmd.Connection = sqlConn;
            sqlCmd.CommandTimeout = dbTimeout;
        }

        /// <summary>
        /// 数据库连接，如果无法连上，重试5次
        /// </summary>
        /// <param name="retryTimes">当前重试次数</param>
        private void ConnectDB(int retryTimes)
        {
            if (retryTimes > 5)
                throw new Exception("数据库无法连接!");
            try
            {
                sqlConn = new SqlConnection(connstr);
                sqlConn.Open();
                sqlCmd = new SqlCommand();
                sqlCmd.Connection = sqlConn;
                sqlCmd.CommandTimeout = this.dbTimeout;
            }
            catch (System.Data.SqlClient.SqlException)
            {
                ConnectDB(++retryTimes);
            }
        }

        /// <summary>
        /// 通过sql语句获取查询结果集DbDataReader
        /// </summary>
        /// <param name="sqlString">sql查询语句</param>
        /// <returns>包含查询结果的DbDataReader对象</returns>
        public DbDataReader ExecuteReader(string sqlString)
        {
            sqlCmd.CommandText = sqlString;
            SqlTransaction trans = sqlConn.BeginTransaction(IsolationLevel.ReadUncommitted);
            sqlCmd.Transaction = trans;
            SqlDataReader reader = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);
            return reader;
        }

        private static Regex goSplitReg = new Regex("\r\ngo\r\n", RegexOptions.IgnoreCase);


        /// <summary>
        /// 通过sql语句获取查询结果集DataTable，一般用作查询数据行数或统计
        /// </summary>
        /// <param name="sqlString">sql语句</param>
        /// <param name="affectNum">0--不判断</param>
        /// <returns>包含查询结果的DataTable对象</returns>
        public DataTable FillDataTable(string sqlString, int affectNum)
        {
            SqlCommand comm = new SqlCommand(sqlString, sqlConn);
            SqlDataAdapter sqlDataAdpter = new SqlDataAdapter(comm);
            DataTable dt = new DataTable();
            try
            {
                sqlDataAdpter.Fill(dt);
                if (dt.Rows.Count != affectNum && affectNum != 0)
                    throw new Exception("the affect num from the database is wrong");
                return dt;
            }
            finally
            {
                sqlDataAdpter.Dispose();
                comm.Dispose();
            }
        }
        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sqlString">sql语句</param>
        /// <param name="affect_num">限制影响行数，0--不判断</param>
        public void ExecuteNonQuery(string sqlString, int affect_num)
        {
            SqlTransaction trans = sqlConn.BeginTransaction();
            try
            {
                int all_result_num = 0;
                string[] sqlItems = goSplitReg.Split(sqlString);
                foreach (string sqlStr in sqlItems)
                {
                    int num;
                    try
                    {
                        SqlCommand comm = new SqlCommand(sqlStr, sqlConn, trans);
                        num = comm.ExecuteNonQuery();
                    }
                    catch (SqlException)
                    {
                        trans.Rollback();
                        trans.Dispose();
                        trans = sqlConn.BeginTransaction();
                        SqlCommand comm = new SqlCommand(sqlStr, sqlConn, trans);
                        num = comm.ExecuteNonQuery();
                    }
                    //执行 UPDATE、INSERT 和 DELETE 语句，返回值为该命令所影响的行数。
                    //对于所有其他类型的语句，返回值为 -1。如果发生回滚，返回值也为 -1
                    if (num != -1)
                        all_result_num += num;
                }
                if (all_result_num != -1 && all_result_num != affect_num && affect_num != 0)
                {
                    throw new Exception(string.Format("返回数据与预期不一致, 预期: {0}, 实际: {1}", affect_num, all_result_num));
                }
                else
                {
                    trans.Commit();
                }
            }
            catch (Exception e)
            {
                trans.Rollback();
                throw (e);
            }
            finally
            {
                trans.Dispose();
            }
        }

        /// <summary>
        /// 保持与Sql Server的连接
        /// </summary>
        public void KeepOpen()
        {
            if (sqlConn.State == ConnectionState.Broken || sqlConn.State == ConnectionState.Closed)
                sqlConn.Open();
        }

        /// <summary>
        /// 关闭与Sql Server的连接
        /// </summary>
        public void Close()
        {
            sqlCmd.Dispose();
            sqlConn.Close();
            sqlConn.Dispose();
        }

        /// <summary>
        /// 获得Sql Server参数对象， 用作Sql Server的存储过程使用
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns>Sql Server的参数对象</returns>
        public DbParameter GetParameter(string parameterName, object value)
        {
            SqlParameter sp = new SqlParameter(parameterName, value);
            return sp;
        }

        /// <summary>
        /// 获取Sql Server的Command对象
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <returns>Command对象</returns>
        public DbCommand GetDbCmd(string sqlStr)
        {
            return new SqlCommand(sqlStr, sqlConn);
        }

        /// <summary>
        /// 重置所持资源
        /// </summary>
        public void ResetDbOperate()
        {
            this.sqlCmd = new SqlCommand();
            sqlCmd.Connection = this.sqlConn;
            sqlCmd.CommandTimeout = this.dbTimeout;
        }
    }
}
