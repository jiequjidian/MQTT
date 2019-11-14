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
            //try
            {
                if (tb == "YW")
                {
                    //string[] serieNameP = { "datetimee", "出水流量", "进水流量", "进水液位" };
                    dt = SQLserver.GetDataDischarge(startTime, endTime);
                    jsonStr = Dtb2Json(dt, seriesNames);//将datatable转换为json字符串
                }
                else if (tb == "LLday")
                {
                    dt = SQLserver.GetDataDischarge(tb,startTime, endTime);
                    jsonStr = Dtb2Json(dt, seriesNames);//将datatable转换为json字符串
                }
               else if (tb == "all")//主页数据获取
                {
                    string[] tbS = {"YW","JS","CS","LLday","YQ","EQ"};
                    jsonStr= getList2Json(tbS);
                }
                else
                {
                    tb = tb.Substring(0,2);
                    dt = SQLserver.GetData(tb, startTime, endTime);
                    jsonStr = Dtb2Json(dt, seriesNames);//将datatable转换为json字符串
                }
                
                 
                string ccc = "";
            }
           // catch (Exception ex)
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


            for (int i = 0; i < dtb.Rows.Count; i++)
            {//去除相同分钟数的数据,warning=1时表示为特殊数据，不可删除
                if (i>0)
                {
                    if (((DateTime)dtb.Rows[i]["datetimee"]).ToString("yyyy/MM/dd HH:mm")== ((DateTime)dtb.Rows[i-1]["datetimee"]).ToString("yyyy/MM/dd HH:mm"))
                    {
                        bool ccc = dtb.Columns.Contains("warning");
                        if (dtb.Columns.Contains("warning"))
                        {
                            if (dtb.Rows[i]["warning"] != null)//warning字段为空时证明无异常值
                            {
                                //即使连续两条报警数据，同一分钟也只保留1条
                                dtb.Rows.RemoveAt(i - 1);
                            }
                        }
                        else
                        {
                            dtb.Rows.RemoveAt(i);
                        }
                    }
                }
            }
            while (dtb.Rows.Count > 400)
            {//防止数据量过大，隔1取1
                for (int i = 1; i < dtb.Rows.Count; i++)
                {
                    if (i % 2 == 0 )
                    {
                        if (dtb.Columns.Contains("warning") && dtb.Rows[i]["warning"] != null)
                        {
                            dtb.Rows.RemoveAt(i);//只移除非异常数据
                        }
                        else
                        {
                            dtb.Rows.RemoveAt(i-1);//只移除非异常数据
                        }
                    }
                }
                   
            }

            foreach (DataRow row in dtb.Rows)
            {
                Dictionary<string, object> drow = new Dictionary<string, object>();
                foreach (DataColumn col in dtb.Columns)
                {
                    if (seriesNames.Contains(col.ColumnName))
                    {
                        if (col.DataType == (new DateTime()).GetType())//如果是时间数据
                        {
                            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 01, 01));
                            string timeStr = ((DateTime)row[col.ColumnName]).ToString("yyyy/MM/dd HH:mm");
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

        public static string getList2Json(string[] tbNameS)
        {
            DataTable dt = SQLserver.getLast(tbNameS[0]);//YW
            DataTable dt1 = SQLserver.getLast(tbNameS[1]);//JS
            DataTable dt2 = SQLserver.getLast(tbNameS[2]);//CS
            DataTable dt3 = SQLserver.getLast(tbNameS[3]);//LLday
            DataTable dt4 = SQLserver.getLast(tbNameS[4]);//YQ
            DataTable dt5 = SQLserver.getLast(tbNameS[5]);//EQ

            JavaScriptSerializer jss = new JavaScriptSerializer();
            ArrayList dic = new ArrayList();
            Dictionary<string, object> drow = new Dictionary<string, object>();

            dt=deleMore(dt);
            dt1 = deleMore(dt1);
            dt2 = deleMore(dt2);
            dt3 = deleMore(dt3);
            dt4 = deleMore(dt4);
            dt5 = deleMore(dt5);

            //================================液位表==========================================================================
            foreach (DataRow row in dt.Rows)//YW
            {
                foreach (DataColumn col in dt.Columns)
                {
                    //if (seriesNames.Contains(col.ColumnName))
                    {
                        if (col.DataType == (new DateTime()).GetType())//如果是时间数据
                        {
                            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
                            string timeStr = ((DateTime)row[col.ColumnName]).ToString("yyyy/MM/dd HH:mm");
                            drow.Add(col.ColumnName, timeStr);
                        }
                        else
                        {
                            drow.Add(col.ColumnName , row[col.ColumnName]);
                        }
                    }
                }
            }
            //=========================================进水表==========================================================================
            foreach (DataRow row in dt1.Rows)//JS
            {
                foreach (DataColumn col in dt1.Columns)
                {
                    //if (seriesNames.Contains(col.ColumnName))
                    {
                        if (col.DataType == (new DateTime()).GetType())//如果是时间数据
                        {
                            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
                            string timeStr = ((DateTime)row[col.ColumnName]).ToString("yyyy/MM/dd HH:mm");
                            drow.Add(col.ColumnName+"1", timeStr);
                        }
                        else
                        {
                            drow.Add(col.ColumnName+"1", row[col.ColumnName]);
                        }
                    }
                }
               
            }
            //=================================================出水表=========================================================================
            foreach (DataRow row in dt2.Rows)//CS
            {
                foreach (DataColumn col in dt2.Columns)
                {
                   // if (seriesNames.Contains(col.ColumnName))
                    {
                        if (col.DataType == (new DateTime()).GetType())//如果是时间数据
                        {
                            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
                            string timeStr = ((DateTime)row[col.ColumnName]).ToString("yyyy/MM/dd HH:mm");
                            drow.Add(col.ColumnName+"2", timeStr);
                        }
                        else
                        {
                            drow.Add(col.ColumnName+"2", row[col.ColumnName]);
                        }
                    }


                }               
            }
            //===================================================日累计流量=============================================================
            foreach (DataRow row in dt3.Rows)//LLday
            {
                foreach (DataColumn col in dt3.Columns)
                {
                    // if (seriesNames.Contains(col.ColumnName))
                    {
                        if (col.DataType == (new DateTime()).GetType())//如果是时间数据
                        {
                            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
                            string timeStr = ((DateTime)row[col.ColumnName]).ToString("yyyy/MM/dd HH:mm");
                            drow.Add(col.ColumnName + "_LLday", timeStr);
                        }
                        else
                        {
                            drow.Add(col.ColumnName + "_LLday", row[col.ColumnName]);
                        }
                    }


                }

            }
            //=========================================================一期MLSS/DO================================================================
            foreach (DataRow row in dt4.Rows)//YQ
            {
                foreach (DataColumn col in dt4.Columns)
                {
                    //if (seriesNames.Contains(col.ColumnName))
                    {
                        if (col.DataType == (new DateTime()).GetType())//如果是时间数据
                        {
                            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
                            string timeStr = ((DateTime)row[col.ColumnName]).ToString("yyyy/MM/dd HH:mm");
                            drow.Add(col.ColumnName+"3", timeStr);
                        }
                        else
                        {
                            drow.Add(col.ColumnName+"3" , row[col.ColumnName]);
                        }
                    }
                }
            }

            //==================================================二期MLSS/DO===============================================================================
            foreach (DataRow row in dt5.Rows)//EQ
            {
                foreach (DataColumn col in dt5.Columns)
                {
                    //if (seriesNames.Contains(col.ColumnName))
                    {
                        if (col.DataType == (new DateTime()).GetType())//如果是时间数据
                        {
                            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
                            string timeStr = ((DateTime)row[col.ColumnName]).ToString("yyyy/MM/dd HH:mm");
                            drow.Add(col.ColumnName+"4", timeStr);
                        }
                        else
                        {
                            drow.Add(col.ColumnName+"4" , row[col.ColumnName]);
                        }
                    }
                }
            }

            dic.Add(drow);
            string ccc= jss.Serialize(dic);
            return ccc;
        }

        /// <summary>
        /// 根据warning值和分钟数及奇偶来删除部分数据，防止数据量过大
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DataTable deleMore(DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {//去除相同分钟数的数据,warning=1时表示为特殊数据，不可删除
                if (i > 0)
                {
                    if (((DateTime)dt.Rows[i]["datetimee"]).ToString("yyyy/MM/dd HH:mm") == ((DateTime)dt.Rows[i - 1]["datetimee"]).ToString("yyyy/MM/dd HH:mm"))
                    {
                        if (dt.Rows[i]["warning"] != null) //warning字段为空时证明无异常值
                        {//即使连续两条报警数据，同一分钟也只保留1条
                            dt.Rows.RemoveAt(i - 1);
                        }
                        else
                        {
                            dt.Rows.RemoveAt(i);
                        }

                    }
                }
            }
            while (dt.Rows.Count > 400)
            {//防止数据量过大，隔1取1
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i % 2 == 0 && dt.Rows[i]["warning"] == null)
                    {
                        dt.Rows.RemoveAt(i);//只移除非异常数据
                    }
                }

            }

            return dt;
        }

    }
}