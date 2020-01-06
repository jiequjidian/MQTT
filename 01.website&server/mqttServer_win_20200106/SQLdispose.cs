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

        SqlConnection connection = null;     //数据库连接


       public void openConnect()
        {
            if (connection == null)
            {
                //实例化sql连接
                connection = new SqlConnection(connectString);
            }
            else
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                else
                {
                    connection.Close();
                    connection.Open();
                }
            }
        }

        /// <summary>
        /// 从数据库读取数据，并返回一张表
        /// </summary>
        /// <param name="myCommandStr">数据库操作命令</param>
        /// <returns></returns>
        public DataTable ExecuteWithReturn(string myCommandStr)
        {            
            DataTable dataTable = new DataTable();

            openConnect();//连接数据库
            SqlCommand myCommand = new SqlCommand(myCommandStr, connection);
            SqlDataAdapter myAdapter = new SqlDataAdapter(myCommand);
            myAdapter.Fill(dataTable);
            myAdapter.Dispose();
            connection.Close();
            
            return dataTable;
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
