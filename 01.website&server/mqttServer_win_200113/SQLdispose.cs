using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mqttServer_win
{
    class SQLdispose
    {
        //数据库连接字符串
        private String connectString = System.Configuration.ConfigurationManager.ConnectionStrings["connectionString"].ToString();         

        private string tableName;              //表名

       

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
                    string strConn =System.Configuration.ConfigurationManager.ConnectionStrings["connectionString"].ToString(); 
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


        //public void openConnect()
        //{
        //    if (connection == null)
        //    {
        //        //实例化sql连接
        //        connection = new SqlConnection(connectString);
        //    }
        //    else
        //    {
        //        if (connection.State == ConnectionState.Closed)
        //        {
        //            connection.Open();
        //        }
        //        else
        //        {
        //            connection.Close();
        //            connection.Open();
        //        }
        //    }
        //}

        ///// <summary>
        ///// 从数据库读取数据，并返回一张表
        ///// </summary>
        ///// <param name="myCommandStr">数据库操作命令</param>
        ///// <returns></returns>
        //public DataTable ExecuteWithReturn(string myCommandStr)
        //{            
        //    DataTable dataTable = new DataTable();

        //    //openConnect();//连接数据库
        //    SqlCommand myCommand = new SqlCommand(myCommandStr, connection);
        //    SqlDataAdapter myAdapter = new SqlDataAdapter(myCommand);
        //    myAdapter.Fill(dataTable);
        //    myAdapter.Dispose();
        //    connection.Close();

        //    return dataTable;
        //}




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
            using (SqlCommand cmd = new SqlCommand(SQLString, Connection))
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


        //将本地datatable追加到数据库中相应的表中
        public bool appendToSQL(DataTable mytable)
        {
            //1.打开数据库连接
            //openConnect();//连接数据库
            //string strSQL = "select top 14 * from DataInfo order by id desc "; //只是必须参数，在本函数中此参数没有意义
            //SqlCommand myCommand = new SqlCommand(strSQL, connection);
            //SqlDataAdapter myAdapter = new SqlDataAdapter(myCommand);
            //SqlCommandBuilder Builder = new SqlCommandBuilder(myAdapter);
            //DataTable tableceshi = mytable;
            //myAdapter.Update(mytable);
            //myAdapter.Dispose();
            //Builder.Dispose();
           //connection.Close();

            string ie;

            SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(connectString, SqlBulkCopyOptions.UseInternalTransaction);
            sqlbulkcopy.DestinationTableName = mytable.TableName;//数据库中的表名
            for (int i = 0; i < mytable.Rows.Count; i++)
            {
                ie = mytable.Rows[i][2].ToString();
            }
            sqlbulkcopy.WriteToServer(mytable);
           
            return true;

           

        }
    }
}
