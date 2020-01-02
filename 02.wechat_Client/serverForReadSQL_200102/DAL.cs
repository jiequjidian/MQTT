using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace serverForReadSQL
{
    public class DAL
    {
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
                    //string strConn = ConfigurationManager.AppSettings["DbConnString"].ToString();
                    string strConn= @"Data Source=qds106623297.my3w.com;Initial Catalog=qds106623297_db;User ID=qds106623297;Password=ysq071381061175;Persist Security Info=True";
                    connection = new SqlConnection(strConn);
                    connection.Open();
                }
                else if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                else if (connection.State == System.Data.ConnectionState.Broken)
                {
                    connection.Close();
                    connection.Open();
                }
                return connection;
            }
        }

        /// <summary>
        /// 执行sql语句,返回被修改行数
        /// </summary>
        /// <param name="commandText"> 要执行的sql命令 </param>
        /// <param name="commandType">枚举类型</param>
        /// <param name="para"></param>
        /// <returns></returns>
        public static int ExecuteCommand(string commandText, CommandType commandType, SqlParameter[] para)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = Connection;
            cmd.CommandText = commandText;

            try
            {
                if (para != null)
                {
                    cmd.Parameters.AddRange(para);
                }
                return cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                connection.Close();
                cmd.Dispose();
            }
        }

        //执行sql语句,返回数据库表
        public static DataTable GetDataTable(string commandText, CommandType commandType, SqlParameter[] para)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = Connection;
            cmd.CommandText = commandText;
            cmd.CommandType = commandType;
            try
            {
                if (para != null)
                {
                    cmd.Parameters.AddRange(para);
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable temp = new DataTable();
                da.Fill(temp);
                return temp;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                connection.Close();
                cmd.Dispose();
            }
        }

        /// <summary>
        /// 查询数据库表
        /// </summary>
        /// <param name="tableName">查询的表名</param>
        /// <param name="rowNum"> 查询多少行</param>
        ///  <param name="keyword"> 查询的关键字</param>
        /// <param name="sequence"> 查询顺序,1为降序，0为升序</param>
        /// <returns></returns>
        public static DataTable Selecttable(string tableName, string keyword, int rowNum, int sequence)
        {
            string sql = null;
            string sequenceStr = "desc";
            if (keyword == null)
            {
                keyword = "id";
            }
            if (sequence == 1)
            {
                sequenceStr = "asc";
            }
            if (rowNum == 0)
            {
                sql = "select * from " + tableName + " order by " + keyword + " " + sequenceStr;
            }
            else
            {
                sql = "select top " + rowNum.ToString() + " * from " + tableName + " order " + keyword + " " + sequenceStr;
            }
            return GetDataTable(sql, CommandType.Text, null);
        }



        /// <summary>
        /// 查询表格中A字段为某值的时候 B字段的值
        /// </summary>
        /// <param name="tableName">要查询的表名</param>
        /// <param name="fieldDependency">查询的依赖项字段名</param>
        /// <param name="dependencyStr">查询的依赖项字段值</param>
        /// <param name="fieldTarget">查询的目标字段名</param>
        /// <returns></returns>
        public static string fieldInquire(string tableName, string fieldDependency, string dependencyStr, string fieldTarget)
        {

            if (tableName == null)
            {
                tableName = "usertable";
            }
            if (fieldDependency == null)
            {
                fieldDependency = "username";
            }
            if (fieldTarget == null)
            {
                fieldTarget = "password";
            }
            string sqlStr = @"select " + fieldTarget + " from dbo." + tableName + " where " + fieldDependency + " = '" + dependencyStr + "' ";
            SqlCommand cmd = new SqlCommand();


            cmd.Connection = Connection;
            cmd.CommandText = sqlStr;

            string ccc = cmd.ExecuteScalar().ToString();
            connection.Close();
            cmd.Dispose();
            ccc = ccc.Trim();
            string aaa = "000";
            return ccc;

            //try
            //{


            //}
            //catch
            //{
            //    MessageBox.Show("请检查要检索的字段是否输入错误");
            //    return "error";
            //}
            //finally
            //{
            //    connection.Close();
            //    cmd.Dispose();
            //}


        }

        /// <summary>
        /// 添加新用户
        /// </summary>
        /// <param name="userStr"></param>
        /// <param name="pwdStr"></param>
        /// <returns></returns>
        public static bool addUser(string userStr, string pwdStr)
        {
            string sqlStr = @"insert into  dbo.userInfo(" + "account" + "," + "pwd" + ")values (" + userStr + "," + pwdStr + ")";

            int influnceNum = ExecuteCommand(sqlStr, CommandType.Text, null);
            if (influnceNum > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}