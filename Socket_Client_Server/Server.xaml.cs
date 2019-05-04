using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace Socket_Client_Server
{
    /// <summary>
    /// Interaction logic for Server.xaml
    /// </summary>
    public partial class Server : Window
    {
        private IPHostEntry host;
        private IPAddress ipAddress;
        private IPEndPoint localEndPoint;
        public static Hashtable ListaClientes = new Hashtable();
        private int clients = 0;
        private int port;
        private string ip;
        private Socket listener;
        private TcpListener server;
        private TcpClient clientSocket;

        private DispatcherTimer _contaTempo;

        public Server(string Ip, int Port)
        {
            InitializeComponent();
            this.Show();
            ip = Ip;
            port = Port;
            try
            {
                host = Dns.GetHostEntry(ip);
                ipAddress = host.AddressList[0];
                localEndPoint = new IPEndPoint(ipAddress, port);
            }
            catch
            {

            }
            lblIp.Content = "IP: " + ip;
            lblPort.Content = "Porta: " + port;

            var ts = new ThreadStart(contaTempo);
            var contaTempoThread = new Thread(ts);
            contaTempoThread.Start();
            SetUpServer();
        }

        private void contaTempo()
        {
            var Inicio = DateTime.Now.TimeOfDay;
            _contaTempo = new DispatcherTimer(new TimeSpan(0, 0, 0, 1), DispatcherPriority.Render, delegate
            {
                var tempo = DateTime.Now.TimeOfDay - Inicio;
                lblUptime.Content = "Uptime: " + tempo.Hours + ":" + tempo.Minutes + ":" + tempo.Seconds;
            }, this.Dispatcher);
        }

        private void SetUpServer()
        {
            try
            {
                txtBoxLog.AppendText("Abrindo Conexão em " + ip + " na porta " + port + ".");
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
                txtBoxLog.AppendText("\nConexão Aberta.");

                var ts = new ThreadStart(BackgroundMethod);
                var backgroundThread = new Thread(ts);
                backgroundThread.Start();
            }
            catch (ArgumentNullException)
            {
                txtBoxLog.AppendText("\nErro.");
                MessageBox.Show("Porta e/ou IP vazio.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
            catch (SocketException)
            {
                txtBoxLog.AppendText("\nErro.");
                MessageBox.Show("Porta em uso ou IP Inválido.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
            catch (ArgumentOutOfRangeException)
            {
                txtBoxLog.AppendText("\nErro.");
                MessageBox.Show("Selecione uma porta válida.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
            catch (FormatException)
            {
                txtBoxLog.AppendText("\nErro.");
                MessageBox.Show("Selecione um IP válido.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private async void SetUpSocket()
        {
            try
            {
                txtBoxLog.AppendText("Iniciando Conexão em " + ip + " na porta " + port + ".");
                listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                listener.Bind(localEndPoint);
                listener.Listen(5);
                txtBoxLog.AppendText("\nConexão Aberta.");

                var ts = new ThreadStart(BackgroundMethod);
                var backgroundThread = new Thread(ts);
                backgroundThread.Start();
            }
            catch (ArgumentNullException)
            {

                MessageBox.Show("Porta e/ou IP vazio.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
            catch (SocketException)
            {
                txtBoxLog.AppendText("\nErro.");
                MessageBox.Show("Porta em uso ou IP Inválido.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
            catch (ArgumentOutOfRangeException)
            {
                txtBoxLog.AppendText("\nErro.");
                MessageBox.Show("Selecione uma porta válida.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
            catch (FormatException)
            {
                txtBoxLog.AppendText("\nErro.");
                MessageBox.Show("Selecione um IP válido.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private int i = 0;

        private void BackgroundMethod()
        {
            while ((true))
            {
                clients += 1;

                Dispatcher.Invoke(() =>
                {
                    lblConnections.Content = "Conexões: " + ListaClientes.Count;
                });
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
                    Dispatcher.Invoke(() =>
                    {
                        txtBoxLog.AppendText("\n" + dataFromClient + " Entrou no chat. ");
                    });

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

        private static void log()
        {
            //Console.WriteLine("From client - " + clNo + " : " + dataFromClient);
        }

        private void BtnShutdown_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            try
            {
                broadcast("<SHUTDOWNTOKEN:77514>", null, false);
                server.Stop();
                listener.Close();
                clientSocket.Close();
            }
            catch
            {
            }
            base.OnClosed(e);
        }
    }
}
