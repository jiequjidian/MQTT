using MQTTnet;
using MQTTnet.Core.Adapter;
using MQTTnet.Core.Diagnostics;
using MQTTnet.Core.Protocol;
using MQTTnet.Core.Server;
using System;
using System.Text;
using System.Threading;

namespace mqttWeb._01.server
{
    public class mqttServer
    {
        private static MqttServer obj_mqttServer = null;//创建mqttServer对象

        static void main(string[] args)
        {
            MqttNetTrace.TraceMessagePublished += MqttNetTrace_TraceMessagePublished;
            new Thread(StartMqttServer).Start();

            while (true)
            {
                var inputString = Console.ReadLine().ToLower().Trim();

                if (inputString == "exit")
                {
                    obj_mqttServer?.StopAsync();   //判断mqtt服务是否已经停止
                    Console.WriteLine("MQTT服务已停止！");
                    break;
                }
                else if (inputString == "clients")
                {
                    foreach (var item in obj_mqttServer.GetConnectedClients())//遍历连接到mqttServer的客户端
                    {
                        Console.WriteLine($"客户端标识：{item.ClientId}，协议版本：{item.ProtocolVersion}");
                    }
                }
                else
                {
                    Console.WriteLine($"命令[{inputString}]无效！");
                }
            }
        }

        public static void StartMqttServer()
        {
            if (obj_mqttServer == null)//如果尚未创建mqtt服务器
            {
                try
                {
                    var options = new MqttServerOptions
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
        }

        private static void MqttServer_ClientConnected(object sender, MqttClientConnectedEventArgs e)
        {
            Console.WriteLine($"客户端[{e.Client.ClientId}]已连接，协议版本：{e.Client.ProtocolVersion}");
        }

        private static void MqttServer_ClientDisconnected(object sender, MqttClientDisconnectedEventArgs e)
        {
            Console.WriteLine($"客户端[{e.Client.ClientId}]已断开连接！");
        }

        private static void MqttServer_ApplicationMessageReceived(object sender, MqttApplicationMessageReceivedEventArgs e)
        {
            Console.WriteLine($"客户端[{e.ClientId}]>> 主题：{e.ApplicationMessage.Topic} 负荷：{Encoding.UTF8.GetString(e.ApplicationMessage.Payload)} Qos：{e.ApplicationMessage.QualityOfServiceLevel} 保留：{e.ApplicationMessage.Retain}");
        }

        private static void MqttNetTrace_TraceMessagePublished(object sender, MqttNetTraceMessagePublishedEventArgs e)
        {
            /*Console.WriteLine($">> 线程ID：{e.ThreadId} 来源：{e.Source} 跟踪级别：{e.Level} 消息: {e.Message}");
            if (e.Exception != null)
            {
                Console.WriteLine(e.Exception);
            }*/
        }

    }
}
