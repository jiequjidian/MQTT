using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace serverForReadSQL
{
    public class SQLserver
    {

        //连接数据库        

        public static SqlConnection connection;
        /// <summary>
        /// 连接数据库
        /// </summary>
        public static SqlConnection Connection
        {
            get
            {
                if (connection == null)
                {
                    //远程连接数据库命令（前提远程数据库服务器已经配置好允许远程连接）  
                    //string strConn = @"Data Source=172.18.72.158;Initial Catalog=WebKuangjia;User ID=sa;Password=LIwei123;Persist Security Info=True";                    
                    //连接本地数据库命令                
                    string strConn = @"Data Source=qds106623297.my3w.com;Initial Catalog=qds106623297_db;User ID=qds106623297;Password=ysq071381061175;Persist Security Info=True";
                    connection = new SqlConnection(strConn);
                    connection.Open();
                }
                else if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                else if (connection.State == ConnectionState.Broken)
                {
                    connection.Close();
                    connection.Open();
                }
                return connection;
            }
        }


        /// <summary>
        /// 执行sql语句,返回数据库表   
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public static DataTable GetDataSet(string commandText, CommandType commandType, SqlParameter[] para)
        {
            //1.创建指令
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = Connection;
            cmd.CommandText = commandText;
            cmd.CommandType = commandType;

            DataTable temp = new DataTable();
            try
            {
                if (para != null)
                {
                    cmd.Parameters.AddRange(para);
                }
                //using (SqlDataReader reader = cmd.ExecuteReader())
                //{
                //    temp.Load(reader);
                //}
                //创建数据适配器
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                //数据填充
                da.Fill(temp);
                cmd.Dispose();
            }
            //catch
            //{
            //    string aaa = "";
            //}
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            //finally
            //{

            //}
            return temp;
        }


        /// <summary>
        /// 获取数据库中的数据
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="start_time"></param>
        /// <param name="end_time"></param>
        /// <returns></returns>
        public static DataTable GetData(string tb, string start_time, string end_time)
        {
            DataTable tableReturn=null;//声明表变量 
                                      
            string sqlStr = "select  * from " + tb + " where datetimee between '"+ start_time+"' and '"+ end_time+"'";
            try
            {
                tableReturn = GetDataSet(sqlStr, CommandType.Text, null);
                try
                {
                    tableReturn.Columns.Remove("id");
                }
                catch { }
            }
            catch
            {

            }
            return tableReturn;
        }

        public static DataTable GetDataDischarge(string start_time, string end_time)
        {
            DataTable tableReturn = null;//声明表变量 
            try
            {

                SqlParameter[] para = { new SqlParameter("@Sdate", DateTime.Parse(start_time)), new SqlParameter("@Edate", DateTime.Parse(end_time)) };

                tableReturn = GetDataSet("dischargeTable", CommandType.StoredProcedure, para);
            }
            catch
            {
                string aaaa = "";
            }
            return tableReturn;
        }

        public static DataTable GetDataDischarge(string sqlDis, string start_time, string end_time)
        {
            DataTable tableReturn = null;//声明表变量 
            try
            {

                SqlParameter[] para = { new SqlParameter("@Sdate", DateTime.Parse(start_time)), new SqlParameter("@Edate", DateTime.Parse(end_time)) };

                tableReturn = GetDataSet(sqlDis + "_dis", CommandType.StoredProcedure, para);
            }
            catch
            {
                string aaaa = "";
            }
            return tableReturn;
        }

        public static DataTable getLast(string tb)
        {
            DataTable tableReturn = new DataTable();
            string sqlStr = "select top 1 * from "+tb+" order by datetimee desc";
            try
            {
                 tableReturn = GetDataSet(sqlStr, CommandType.Text, null);
                try
                {
                    tableReturn.Columns.Remove("id");
                }
                catch { 
                }
            }
            catch
            {

            }
            return tableReturn;
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string SQLString)
        {
            using (SqlCommand cmd = new SqlCommand(SQLString, Connection))
            {
                try
                {
                    connection.Open();
                    object obj = cmd.ExecuteScalar();

                    return obj;
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    connection.Close();
                    throw new Exception(e.Message);
                }
            }
        }

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString)
        {
            using (SqlCommand cmd = new SqlCommand(SQLString, connection))
            {
                try
                {
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (System.Data.SqlClient.SqlException E)
                {
                    connection.Close();
                    throw new Exception(E.Message);
                }
            }
        }
        /// <summary>
        /// 执行SQL语句，返回DataTable对象
        /// </summary>
        /// <param name="SQLString"></param>
        /// <returns></returns>
        public static DataTable QueryTable(string SQLString)
        {
            DataTable dt = new DataTable();
            try
            {
                //connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(SQLString, Connection);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
                adapter.Fill(dt);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            return dt;
        }

    }
}