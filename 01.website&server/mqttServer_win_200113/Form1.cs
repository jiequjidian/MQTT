using MQTTnet;
using MQTTnet.Core.Adapter;
using MQTTnet.Core.Diagnostics;
using MQTTnet.Core.Protocol;
using MQTTnet.Core.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mqttServer_win
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private static MqttServer obj_mqttServer = null;//创建mqttServer对象
        public delegate void reviseTxt(string txtStr);
        private static reviseTxt changeTxt;
        private void Form1_Load(object sender, EventArgs e)
        {            
            changeTxt = new reviseTxt(changeMyTxt);//创建更改窗体内容委托供其他函数使用
             void  changeMyTxt(string str_b)
            {
                if (!tb_message.InvokeRequired)
                {
                    tb_message.AppendText(str_b);
                }
                else
                {//不是主线程调用时，通过主线程访问
                    this.Invoke(changeTxt,str_b);
                }
                    
            }

            //=====================开启mqtt消息代理服务=============================================
            MqttNetTrace.TraceMessagePublished += MqttNetTrace_TraceMessagePublished;
            new Thread(StartMqttServer).Start();

            //while (true)
            //{
            //    string inputString = Console.ReadLine().ToLower().Trim();

            //    if (inputString == "exit")
            //    {
            //        obj_mqttServer?.StopAsync();   //判断mqtt服务是否已经停止
            //        Console.WriteLine("MQTT服务已停止！");
            //        tb_message.AppendText("MQTT服务已停止！");
            //        break;
            //    }
            //    else if (inputString == "clients")
            //    {
            //        foreach (var item in obj_mqttServer.GetConnectedClients())//遍历连接到mqttServer的客户端
            //        {
            //            Console.WriteLine($"客户端标识：{item.ClientId}，协议版本：{item.ProtocolVersion}");
            //            tb_message.AppendText($"客户端标识：{item.ClientId}，协议版本：{item.ProtocolVersion}");
            //        }
            //    }
            //    else
            //    {
            //        Console.WriteLine($"命令[{inputString}]无效！");
            //        tb_message.AppendText($"命令[{inputString}]无效！");
            //    }
            //}
            

        }

        public static void StartMqttServer()
        {
            if (obj_mqttServer == null)//如果尚未创建mqtt服务器
            {
                try
                {
                    MqttServerOptions options = new MqttServerOptions
                    {
                        ConnectionValidator = p =>
                        {
                            if (p.ClientId == "c001")
                            {
                                if (p.Username != "u001" || p.Password != "p001")
                                {
                                    return MqttConnectReturnCode.ConnectionRefusedBadUsernameOrPassword;
                                }
                            }

                            return MqttConnectReturnCode.ConnectionAccepted;
                        }
                    };

                    obj_mqttServer = new MqttServerFactory().CreateMqttServer(options) as MqttServer; //采用MqttServerFactory对象的CreateMqttServer方法创建一个mqttServer 服务端
                    obj_mqttServer.ApplicationMessageReceived += MqttServer_ApplicationMessageReceived;//消息接收处理事件
                    obj_mqttServer.ClientConnected += MqttServer_ClientConnected;   //客户端连接处理事件
                    obj_mqttServer.ClientDisconnected += MqttServer_ClientDisconnected;  //客户端断开连接处理事件
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }
            }

            obj_mqttServer.StartAsync();   //异步启动mqttServer
            Console.WriteLine("MQTT服务启动成功！");
            changeTxt("MQTT服务启动成功！");
        }

        private static void MqttServer_ClientConnected(object sender, MqttClientConnectedEventArgs e)
        {
            changeTxt("有客户端连接");
            Console.WriteLine($"客户端[{e.Client.ClientId}]已连接，协议版本：{e.Client.ProtocolVersion}");
            changeTxt($"客户端[{e.Client.ClientId}]已连接，协议版本：{e.Client.ProtocolVersion}");
        }

        private static void MqttServer_ClientDisconnected(object sender, MqttClientDisconnectedEventArgs e)
        {
            changeTxt("有客户端断开连接");
            Console.WriteLine($"客户端[{e.Client.ClientId}]已断开连接！");
            changeTxt($"客户端[{e.Client.ClientId}]已断开连接！");
        }
         static string tag1 = "0";
        static string tag2 = "0";
       static  TimeSpan span = new TimeSpan();
      static   DateTime timeOrigin;
        private static void MqttServer_ApplicationMessageReceived(object sender, MqttApplicationMessageReceivedEventArgs e)
        {
             tag1 = "1";
            Console.WriteLine($"客户端[{e.ClientId}]>> 主题：{e.ApplicationMessage.Topic} 负荷：{Encoding.UTF8.GetString(e.ApplicationMessage.Payload)} Qos：{e.ApplicationMessage.QualityOfServiceLevel} 保留：{e.ApplicationMessage.Retain}");

            changeTxt($"客户端[{e.ClientId}]>> 主题：{e.ApplicationMessage.Topic} 负荷：{Encoding.UTF8.GetString(e.ApplicationMessage.Payload)} Qos：{e.ApplicationMessage.QualityOfServiceLevel} 保留：{e.ApplicationMessage.Retain}");

            if (span.TotalSeconds >= (1 * 60))//2分钟向数据库写入一次
            {

                writeToSql(Encoding.UTF8.GetString(e.ApplicationMessage.Payload));
                tag2 = "0";
            }

            //string dateStr = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
           // writeToSql(Encoding.UTF8.GetString(e.ApplicationMessage.Payload));
        }

        private static void MqttNetTrace_TraceMessagePublished(object sender, MqttNetTraceMessagePublishedEventArgs e)
        {
            Console.WriteLine($">> 线程ID：{e.ThreadId} 来源：{e.Source} 跟踪级别：{e.Level} 消息: {e.Message}");
            /*if (e.Exception != null)
            {
                Console.WriteLine(e.Exception);
            }*/
        }

       


        private static void writeToSql(string dataStr)
        {
           
            string sqlStr = null;

            string[] tb_JS = { "PH", "COD","NH3N","TP没有报警", "TN没有报警", "流量没有报警", "液位没有报警" };  //进水表所包含的信息

            double[] warmH_JS = { 9, 500, 40, 4, 50, 11, 12 };
            double[] warmL_JS = { 6,   0,  0, 0,  0,  6,  7 };//进水表内信息的报警值

            string[] tb_CS = { "PH", "COD", "NH3N", "TP","TN", "流量没有报警" };  //出水表所包含的信息

            double[] warmH_CS = { 9, 50, 1.5, 0.3, 15, 11};
            double[] warmL_CS = { 6,  0,   0,   0,  0,  0 };//出水表内信息的报警值





            string fielCoYW = null;
            string valueColYW = null;

            string fielCoJS = null;
            string valueColJS = null;
            string fielCoCS = null;
            string valueColCS = null;
            string fielCoLLday = null;
            string valueColLLday = null;

            string fielCoYQ = null;
            string valueColYQ = null;
            string fielCoEQ = null;
            string valueColEQ = null;
            string fielW_JS = null;
            string fielW_CS = null;
            string valueW_JS = null;
            string valueW_CS = null;

            // List<updata> dataList= JsonConvert.DeserializeObject<List<updata>>(dataStr);
            updata dataString = JsonConvert.DeserializeObject<updata>(dataStr);
            //DateTime reciveTime = dataString.time.ToLocalTime();
            //string datetimee = "'"+ reciveTime.ToString()+"'";
            string datetimee = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            for (int i = 0; i < dataString.Data.Count; i++)
            {
                string tableName = null;
                string unitName = dataString.Data[i].name;
                string unitValue = dataString.Data[i].value;
                switch (unitName.Substring(0, 2))
                {
                    
                    case "JS":  //进水数据，进水流量和进水液位数据存入液位表，其他数据存入进水表
                        tableName = "JS";
                        if (unitName == "JS_LL")
                        {
                            fielCoYW += unitName+ ",";
                            valueColYW += ((Convert.ToDouble(changeForSql(tableName,unitName, unitValue) )*3.6).ToString()) + ",";
                        }

                        //if (unitName=="JS_LL")
                        //{
                        //    fielCoYW += unitName + ",";
                        //    valueColYW += decimalFormat(unitValue) + ",";
                        //}
                        if ( unitName == "JS_YW")
                        {
                            fielCoYW += unitName + ",";
                            valueColYW += changeForSql(tableName,unitName, unitValue) + ",";
                        }
                        if (unitName.Substring(0, 3)=="JSL")//日累计流量表中的数据
                        {
                            fielCoLLday += unitName + ",";
                            valueColLLday += changeForSql(tableName,unitName, unitValue) + ",";
                        }
                        else {
                            string nameStr= unitName.Substring(3, unitName.Length - 3);
                            fielCoJS += nameStr + ",";
                            valueColJS += changeForSql(tableName, nameStr, unitValue) + ",";
                        }

                        for (int k = 0; k < tb_JS.Length; k++)
                            {
                            string nameStr1 = unitName.Substring(3, unitName.Length - 3);
                            if (nameStr1 == tb_JS[k])
                                {
                                    string str = changeForSql(tableName, nameStr1, unitValue);
                                    double iii = Convert.ToDouble(str);
                                double iCompare_H =(warmH_JS[k]);
                                double iCompare_L =(warmL_JS[k]);
                                    if (iii > iCompare_H || iii < iCompare_L)
                                    {
                                        fielW_JS = ",warning";
                                        valueW_JS += (k).ToString() + ".";
                                    }
                                }
                            }
                        break;
                    case "CS"://出水数据，存入出水表 

                        tableName = "CS";

                        if (unitName == "CS_LL")//出水流量直接放入液位表中
                        {
                            fielCoYW += unitName + ",";
                            valueColYW += (Convert.ToDouble(changeForSql(tableName,unitName, unitValue)) *3.6).ToString()+ ",";
                        }
                        if (unitName.Substring(0, 3) == "CSL")//日累计流量表中的数据
                        {
                            fielCoLLday += unitName + ",";
                            valueColLLday += changeForSql(tableName,unitName, unitValue) + ",";
                        }
                        else
                        {
                            string nameStr2 = unitName.Substring(3, unitName.Length - 3);
                            fielCoCS += nameStr2 + ",";
                            valueColCS += changeForSql(tableName, nameStr2, unitValue) + ",";
                        }

                        for (int k = 0; k < tb_CS.Length; k++)
                        {
                            string nameStr3 = unitName.Substring(3, unitName.Length - 3);
                            if (nameStr3 == tb_CS[k])
                            {
                                string str = changeForSql(tableName, nameStr3, unitValue);
                                double iii = Convert.ToDouble(str);
                                double iCompare_H =warmH_CS[k];
                                double iCompare_L = warmL_CS[k];
                                //int ccc = k + tb_JS.Length; 
                                if (iii > iCompare_H ||iii < iCompare_L)
                                {
                                    fielW_CS = ",warning";
                                    valueW_CS += (k).ToString()+'.';
                                }
                            }
                        }
                        break;
                    case "YQ"://反应池数据
                        tableName = "YQ";
                        string nameStr4 = unitName.Substring(3, unitName.Length - 3);
                        fielCoYQ += nameStr4 + ",";
                        valueColYQ += changeForSql(tableName, nameStr4, unitValue) + ",";                        
                        
                        break;
                    case "EQ"://反应池数据
                        tableName = "EQ";
                        string nameStr5 = unitName.Substring(3, unitName.Length - 3);
                        fielCoEQ += nameStr5 + ",";
                        valueColEQ += changeForSql(tableName, nameStr5, unitValue) + ",";
                        break;
                    default://有误的数据，不存入数据库
                        continue;
                }           
            }
           
             if (valueW_JS != null)
            {
                valueW_JS = ",\'" + (valueW_JS.Substring(0, valueW_JS.Length - 1))+"\'";
            }
            if (valueW_CS != null)
            { 
                valueW_CS = ",\'" + (valueW_CS.Substring(0, valueW_CS.Length - 1))+"\'" ;
            }
           
            string sqlStr_sonYW = null;
            string sqlStr_sonJS = null;
            string sqlStr_sonCS = null;
            string sqlStr_sonLLday = null;
            string sqlStr_sonYQ = null;
            string sqlStr_sonEQ = null;

            if (valueColYW != null)
            {
                 sqlStr_sonYW = "insert into  YW" + "(datetimee," + fielCoYW.Substring(0, fielCoYW.Length - 1) + ") values(" +"'"+ datetimee +"'"+ "," + valueColYW.Substring(0, valueColYW.Length - 1) + ")\n";
            }
            if (valueColJS != null)
            {
                sqlStr_sonJS = "insert into  JS" + "(datetimee," + fielCoJS.Substring(0, fielCoJS.Length - 1) + fielW_JS + ") values(" + "'" + datetimee + "'" + "," + valueColJS.Substring(0, valueColJS.Length - 1) + valueW_JS + ")\n";
            }
            if (valueColCS != null)
            {
                sqlStr_sonCS = "insert into  CS" + "(datetimee," + fielCoCS.Substring(0, fielCoCS.Length - 1) + fielW_CS + ") values(" + "'" + datetimee + "'" + "," + valueColCS.Substring(0, valueColCS.Length - 1) + valueW_CS + ")\n";
            }
            if (valueColLLday != null)
            {
                sqlStr_sonLLday = "insert into  LLday" + "(datetimee," + fielCoLLday.Substring(0, fielCoLLday.Length - 1) + ") values(" + "'" + datetimee + "'" + "," + valueColLLday.Substring(0, valueColLLday.Length - 1)+ ")\n";
            }


            if (valueColYQ != null)
            {
                sqlStr_sonYQ = "insert into  YQ" + "(datetimee," + fielCoYQ.Substring(0, fielCoYQ.Length - 1) + ") values(" + "'" + datetimee + "'" + "," + valueColYQ.Substring(0, valueColYQ.Length - 1) + ")\n";
            }
            if (valueColEQ != null)
            {
                sqlStr_sonEQ = "insert into  EQ" + "(datetimee," + fielCoEQ.Substring(0, fielCoEQ.Length - 1) + ") values(" + "'" + datetimee + "'" + "," + valueColEQ.Substring(0, valueColEQ.Length - 1) + ")\n";
            }

           
             
            
             
            sqlStr = sqlStr_sonYW+ sqlStr_sonJS +sqlStr_sonCS+ sqlStr_sonLLday+ sqlStr_sonYQ + sqlStr_sonEQ;

            if (sqlStr!=null)
            {                
                SQLdispose.ExecuteSql(sqlStr);
            }
            


        }

        /// <summary>
        /// 为字符段加单引号，且当字符串不是数字时将值赋0
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fielName">字段名</param>
        /// <param name="strF"></param>
        /// <returns></returns>
        private static string changeForSql(string tableName,string fielName,string strF)
        {
            if (isNumberS(strF))
            {
                return decimalFormat(delT(strF));
            }
            else
            {
                string sqlStr = "select PH from CS where id in(select MAX(id)-1 from CS)";
                return SQLdispose.GetSingle(sqlStr).ToString();
            }
        }





        

        /// <summary>
        /// 用遍历的方法判断一个字符串是否为合法数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool isNumberS(string str)
        {

            str = delT(str);

            //然后判断是否有且仅有一个小数点


            int pointN = 0;
            //遍历字符串中的字符
            for (int i = 0; i < str.Length; i++)
            {

                //若小数点个数大于1，则不是合法数字
                if (str[i] == '.')
                {
                    if (pointN >= 1)
                    {
                        return false;
                    }
                    pointN++;
                }

                //若字符不是小数点，则判断是否为数字（十进制数字，不包含罗马数字和分数等）
                else
                {
                    if (Char.IsDigit(str[i]))
                    {

                    }
                    else
                    {
                        //若有非法数字，则返回false
                        return false;
                    }
                }
            }


            return true;
        }

        /// <summary>
        /// 
        /// 保留小数点后两位
        /// </summary>
        /// <param name="decimalStr"></param>
        /// <returns></returns>
        public static string decimalFormat(string decimalStr)
        {
            int sss = decimalStr.IndexOf('.');
            if (decimalStr.IndexOf('.') != -1)
            {
                decimalStr = decimalStr.Substring(0, decimalStr.IndexOf('.') + 3);
            }
            return decimalStr;
        }

        /// <summary>
        /// 将字符串两端的单引号和双引号去掉
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string delT(string str)
        {
            //首先将字符串两端的合法的‘"’和‘'’去掉
            if (str[0] == '\'' && str[str.Length - 1] == '\'')
            {
                str = str.Replace('\'', ' ').Trim();
                // str = str.Remove('\'');
            }
            if (str[0] == '\"' && str[str.Length - 1] == '\"')
            {
                str = str.Replace('\"', ' ').Trim();
            }
            return str;
        }



        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (tag1 == "1")
            {
                if (tag2 == "0")
                {
                    timeOrigin = DateTime.Now;
                    tag2 = "1";
                }
                span = DateTime.Now - timeOrigin;
            }            
            
        }

        //private static void writeToSql(string tableName,DataTable tableContent)
        //{
        //    tableContent.TableName=tableName;
        //    SQLdispose sQLdispose = new SQLdispose();
        //    sQLdispose.appendToSQL(tableContent);
        //}
    }


   


}
