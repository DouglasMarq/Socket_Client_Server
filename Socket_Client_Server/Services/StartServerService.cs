using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Threading;

namespace Socket_Client_Server.Services
{
    public static class StartServerService
    {

        private static IPHostEntry host;
        private static IPAddress ipAddress;
        private static IPEndPoint localEndPoint;
        public static Hashtable ListaClientes = new Hashtable();
        private static int clients = 0;
        private static int _port;
        private static string _ip;
        private static Socket listener;
        private static TcpListener server;
        private static TcpClient clientSocket;
        public static event EventHandler Terminated;
        private static int i = 0;
        private static DispatcherTimer _contaTempo;
        private static Server ServerWindow;

        public static void Init(string ip, int port)
        {
            ServerWindow = new Server();
            ServerWindow.Show();
            _port = port;
            _ip = ip;
            host = Dns.GetHostEntry(ip);
            ipAddress = host.AddressList[0];
            localEndPoint = new IPEndPoint(ipAddress, port);

            try
            {
                server = new TcpListener(IPAddress.Parse(ip), port);
            }
            catch
            {
                server = new TcpListener(port);
            }
            clientSocket = default(TcpClient);
            server.Start();
            //ServerWindow.Activate();
            var ts = new ThreadStart(BackgroundMethod);
            var backgroundThread = new Thread(ts);
            backgroundThread.Start();
        }

        private static void BackgroundMethod()
        {
            while (true)
            {
                clients += 1;
                try
                {
                    clientSocket = server.AcceptTcpClient();
                    byte[] bytesFrom = new byte[1024];
                    string dataFromClient = null;
                    clientSocket.ReceiveBufferSize = 1024;
                    clientSocket.SendBufferSize = 1024;
                    NetworkStream networkStream = clientSocket.GetStream();
                    networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
                    dataFromClient = Encoding.UTF8.GetString(bytesFrom);
                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));
                    if (ListaClientes.Contains(dataFromClient))
                    {
                        ListaClientes.Add(dataFromClient + i, clientSocket);
                        i++;
                    }
                    else
                    {
                        ListaClientes.Add(dataFromClient, clientSocket);
                    }
                    broadcast(dataFromClient + " entrou no chat.", dataFromClient, false);
                    handleClient client = new handleClient();
                    client.startClient(clientSocket, dataFromClient, ListaClientes);
                    BackgroundMethod();
                }
                catch
                {
                    //
                }
            }
        }

        private static void broadcast(string msg, string uName, bool flag)
        {
            foreach (DictionaryEntry Item in ListaClientes)
            {
                TcpClient broadcastSocket;
                broadcastSocket = (TcpClient)Item.Value;
                NetworkStream broadcastStream = broadcastSocket.GetStream();
                Byte[] broadcastBytes = null;

                if (flag == true)
                {
                    broadcastBytes = Encoding.UTF8.GetBytes(uName + ": " + msg);
                }
                else
                {
                    broadcastBytes = Encoding.UTF8.GetBytes(msg);
                }

                broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);
                broadcastStream.Flush();
            }
        }

        public class handleClient
        {
            TcpClient clientSocket;
            string clNo;
            Hashtable clientsList;

            public void startClient(TcpClient inClientSocket, string clineNo, Hashtable cList)
            {
                this.clientSocket = inClientSocket;
                this.clNo = clineNo;
                this.clientsList = cList;
                Thread ctThread = new Thread(doChat);
                ctThread.Start();
            }

            private void doChat()
            {
                byte[] bytesFrom = new byte[clientSocket.ReceiveBufferSize];
                string dataFromClient = null;

                while (true)
                {
                    try
                    {
                        NetworkStream networkStream = clientSocket.GetStream();
                        networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
                        dataFromClient = Encoding.UTF8.GetString(bytesFrom);
                        dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));
                        broadcast(dataFromClient, clNo, true);
                    }
                    catch (Exception ex)
                    {
                        break;
                    }
                }
            }
        }
    }
}
