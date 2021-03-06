using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Threading;

namespace AppUI.Core
{
    public class AsyncTCPServer : DispatcherObject
    {
        public Dictionary<string, Socket> Clients = new Dictionary<string, Socket>();
        public ObservableCollection<String> ClientIPList = new ObservableCollection<String>();

        public string data_Output;
        public byte[] data = new byte[1024];
        public void Start(string IP, int Port)
        {
            //创建套接字
            //获得本地的IP地址族
            //创建套接字
            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(IP), Port);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //绑定端口和IP
            socket.Bind(ipe);
            //设置监听

            socket.Listen(10);
            //连接客户端
            AsyncAccept(socket);
        }

        /// <summary>
        /// 连接到客户端
        /// </summary>
        /// <param name="socket"></param>
        private void AsyncAccept(Socket socket)
        {
            socket.BeginAccept(AsyncAccept, socket);

        }
        public void AsyncAccept(IAsyncResult result)
        {
            try
            {
                data_Output = "";
                var socket = result.AsyncState as Socket;
                Socket client = socket.EndAccept(result);
                Console.WriteLine(string.Format("客户端{0}连接成功...", client.RemoteEndPoint));
                data_Output = string.Format("~客户端{0}请求连接成功...", client.RemoteEndPoint);
                Dispatcher.BeginInvoke(new MReceiveDataShow(ReceiveDataShow), new ReceiveData
                {
                    ReceiveData_s = data_Output,
                });
                Clients.Add(client.RemoteEndPoint.ToString(), client);
                ClientIPList.Add(client.RemoteEndPoint.ToString());
                AsyncReveive(client);
                //ConsoleText += $"{CmdTag + string.Format("客户端{0}请求连接...", client.RemoteEndPoint) + "   state : " + result.AsyncState + Environment.NewLine}";
                socket.BeginAccept(AsyncAccept, result.AsyncState);
            }
            catch (Exception ex)
            {
                Console.WriteLine("客户端连接失败");
                data_Output = "`客户端连接失败";
                Dispatcher.BeginInvoke(new MReceiveDataShow(ReceiveDataShow), new ReceiveData
                {
                    ReceiveData_s = data_Output,
                });
            }

        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="client"></param>
        private void AsyncReveive(Socket client)
        {
            client.BeginReceive(data, 0, data.Length, SocketFlags.None, AsyncReveive, client);
        }

        public void AsyncReveive(IAsyncResult result)
        {
            int length = 0;
            data_Output = "";
            var client = result.AsyncState as Socket;

            try
            {
                bool part1 = client.Poll(1000, SelectMode.SelectRead);
                bool part2 = (client.Available == 0);
                if (part1 && part2)
                {
                    data_Output = string.Format("客户端" + client.RemoteEndPoint.ToString() + "发送信息：{0}", "断开连接");
                    Dispatcher.BeginInvoke(new MReceiveDataShow(ReceiveDataShow), new ReceiveData
                    {
                        ReceiveData_s = data_Output,
                    });
                    client.Close();
                    client.Dispose();
                }
                else
                {
                    length = client.EndReceive(result);
                    Console.WriteLine(string.Format("客户端" + client.RemoteEndPoint.ToString() + "发送信息：{0}", Encoding.UTF8.GetString(data, 0, length)));
                    //data_Output = Encoding.UTF8.GetString(data, 0, length);
                    data_Output = string.Format(Encoding.UTF8.GetString(data, 0, length));
                    Dispatcher.BeginInvoke(new MReceiveDataShow(ReceiveDataShow), new ReceiveData
                    {
                        ReceiveData_s = data_Output,
                    });

                    Array.Clear(data, 0, data.Length);
                    //AsyncSend(client, "%01$RD11\r");
                    client.BeginReceive(data, 0, data.Length, SocketFlags.None, AsyncReveive, client);
                }
            }
            catch (Exception ex)
            {
                //断开连接
                data_Output = string.Format("客户端发送信息：{0}", "断开连接");
                Dispatcher.BeginInvoke(new MReceiveDataShow(ReceiveDataShow), new ReceiveData
                {
                    ReceiveData_s = data_Output,
                });
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="client"></param>
        /// <param name="p"></param>
        public void AsyncSend(Socket client, string p)
        {
            data_Output = "";
            if (client == null || p == string.Empty) return;
            //数据转码
            byte[] data = new byte[1024];
            data = Encoding.UTF8.GetBytes(p);
            try
            {
                //开始发送消息
                client.BeginSend(data, 0, data.Length, SocketFlags.None, asyncResult =>
                {
                    //完成消息发送
                    int length = client.EndSend(asyncResult);
                    //输出消息
                    Console.WriteLine(string.Format("服务器向" + client.RemoteEndPoint.ToString() + "发出消息:{0}", p));
                    //data_Output = string.Format(string.Format("服务器向" + client.RemoteEndPoint.ToString() + "发出消息:{0}", p));
                    //Dispatcher.BeginInvoke(new MReceiveDataShow(ReceiveDataShow), new ReceiveData
                    //{
                    //    ReceiveData_s = data_Output,
                    //});
                }, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        //接收数据路由回调
        public struct ReceiveData
        {
            public string ReceiveData_s;
        }
        public event EventHandler<ReceiveData> ReceiveDataInvoked;
        private void ReceiveDataShow(ReceiveData ReceiveData_S)
        {
            ReceiveDataInvoked?.Invoke(this, ReceiveData_S);
        }
        private delegate void MReceiveDataShow(ReceiveData ReceiveData_S);
    }
}
                                                                                                                                                    