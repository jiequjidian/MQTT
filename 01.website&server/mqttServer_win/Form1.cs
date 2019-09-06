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

            if (span.TotalSeconds >= (5*60))//5分钟向数据库写入一次
            {
                writeToSql(Encoding.UTF8.GetString(e.ApplicationMessage.Payload));
                tag2 = "0";
            }
            
            
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
            string tableName = null;
            string sqlStr = null;
            //string sqlStr = "insert all"+"\n";

            string fielCoA = null;
            string valueColA = null;
            string fielCoB = null;
            string valueColB = null;
           
            // List<updata> dataList= JsonConvert.DeserializeObject<List<updata>>(dataStr);
            updata dataString = JsonConvert.DeserializeObject<updata>(dataStr);
            DateTime reciveTime = dataString.time.ToLocalTime();
            string datetimee = "'"+ reciveTime.ToString()+"'";
           
            for (int i = 0; i < dataString.Data.Count; i++)
            {
                string unitName = dataString.Data[i].name;
                string unitValue = dataString.Data[i].value;
                switch (unitName.Substring(0, 2))
                {
                    case "JS":  //进水数据，存入进水表                         
                        fielCoA += unitName.Substring(3, unitName.Length - 3)+",";
                        valueColA += decimalFormat(unitValue) + ",";
                        break;
                    case "CS"://出水数据，存入出水表                         
                        fielCoB += unitName.Substring(3, unitName.Length - 3) + ",";
                        valueColB += decimalFormat(unitValue) + ",";
                        break;
                    default://有误的数据，不存入数据库
                        continue;
                }           
            }
            string sqlStr_sonA = "insert into  test" + "(datetimee," + fielCoA .Substring(0,fielCoA.Length-1)+ ") values("+datetimee+"," + valueColA.Substring(0,valueColA.Length-1) + ")\n";
            string sqlStr_sonB = "insert into  test1" + "(datetimee," + fielCoA.Substring(0, fielCoA.Length - 1) + ") values(" + datetimee + "," + valueColA.Substring(0, valueColA.Length - 1) + ")\n";
            sqlStr = sqlStr_sonA+sqlStr_sonB;

            SQLdispose sQ = new SQLdispose();
            sQ.ExecuteWithReturn(sqlStr);


        }

        public static string decimalFormat(string decimalStr)
        {
            int sss = decimalStr.IndexOf('.');
            if (decimalStr.IndexOf('.')!=-1)
            {
                decimalStr = decimalStr.Substring(0,decimalStr.IndexOf('.')+3);
            }
            return decimalStr;
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
