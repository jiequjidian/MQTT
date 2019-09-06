using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace serverForReadSQL.web
{
    public partial class forWeChat : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static  string getData(string tb,string startTime,string endTime,string[] seriesNames)
        {
            //string[] sname = new string[10];
            // sname = seriesNames.Split(',');
            string jsonStr = null;
            DataTable dt = new DataTable();
            try
            {
                if (tb=="two")
                {
                    dt = SQLserver.GetDataDischarge(startTime, endTime);
                    jsonStr = Dtb2Json(dt, seriesNames);//将datatable转换为json字符串
                }
                else if (tb == "all")
                {
                    string[] tbS = {"test","test1" };
                    jsonStr= getList2Json(tbS, seriesNames);
                }
                else
                {
                    
                    dt = SQLserver.GetData(tb, startTime, endTime);
                    jsonStr = Dtb2Json(dt, seriesNames);//将datatable转换为json字符串
                }
                
                 
                string ccc = "";
               // jsonStr= JsonConvert.SerializeObject(jsonStr);
            }
            catch (Exception ex)
            {
                
            }

            string aaa = "";
            return jsonStr;
        }

       


        /// <summary>
        /// 将datatable类型数据转换为标准json对象
        /// </summary>
        /// <param name="dtb"></param>
        /// <returns></returns>
        [WebMethod]
        public static string Dtb2Json(DataTable dtb,string[] seriesNames)
        {
            // string[] cav = { "1", "2" };
            JavaScriptSerializer jss = new JavaScriptSerializer();
            ArrayList dic = new ArrayList();
            foreach (DataRow row in dtb.Rows)
            {
                Dictionary<string, object> drow = new Dictionary<string, object>();
                foreach (DataColumn col in dtb.Columns)
                {
                    if (seriesNames.Contains(col.ColumnName))
                    {
                        if (col.DataType == (new DateTime()).GetType())//如果是时间数据
                        {
                            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
                            string timeStr = ((DateTime)row[col.ColumnName]).ToString();
                            drow.Add(col.ColumnName, timeStr);
                        }
                        else
                        {
                            drow.Add(col.ColumnName, row[col.ColumnName]);
                        }
                    }
                    
                   
                }
                dic.Add(drow);
            }
            return jss.Serialize(dic); //形式为"{"x":1,"y0":1,"y1":1,"y2":1},{...}"
        }

        public static string getList2Json(string[] tbNameS, string[] seriesNames)
        {
            DataTable dt = SQLserver.getLast(tbNameS[0],seriesNames);
            DataTable dt1 = SQLserver.getLast(tbNameS[1], seriesNames);

            JavaScriptSerializer jss = new JavaScriptSerializer();
            ArrayList dic = new ArrayList();
            Dictionary<string, object> drow = new Dictionary<string, object>();
            foreach (DataRow row in dt.Rows)
            {
                
                foreach (DataColumn col in dt.Columns)
                {
                    if (seriesNames.Contains(col.ColumnName))
                    {
                        if (col.DataType == (new DateTime()).GetType())//如果是时间数据
                        {
                            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
                            string timeStr = ((DateTime)row[col.ColumnName]).ToString();
                            drow.Add(col.ColumnName, timeStr);
                        }
                        else
                        {
                            drow.Add(col.ColumnName, row[col.ColumnName]);
                        }
                    }
                }
               
            }
            foreach (DataRow row in dt1.Rows)
            {
                foreach (DataColumn col in dt1.Columns)
                {
                    if (seriesNames.Contains(col.ColumnName))
                    {
                        if (col.DataType == (new DateTime()).GetType())//如果是时间数据
                        {
                            //System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
                            //string timeStr = ((DateTime)row[col.ColumnName]).ToString();
                            //drow.Add(col.ColumnName+"1", timeStr);
                        }
                        else
                        {
                            drow.Add(col.ColumnName+"1", row[col.ColumnName]);
                        }
                    }


                }
               
            }
            dic.Add(drow);
            string ccc= jss.Serialize(dic);
            return ccc;
        }

    }
}