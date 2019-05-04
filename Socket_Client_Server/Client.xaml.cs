using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;

namespace Socket_Client_Server
{
    /// <summary>
    /// Interaction logic for Client.xaml
    /// </summary>
    public partial class Client : Window
    {
        TcpClient clientSocket = new TcpClient();
        NetworkStream serverStream = default(NetworkStream);
        string readData = null;
        private string name;
        private int port;
        private string ip;
        private Socket Clients;
        private byte[] bytes;

        public Client(string Ip, int Port, string Name)
        {
            InitializeComponent();
            this.Show();
            ip = Ip;
            port = Port;
            name = Name;
            lblIp.Content = "IP: " + ip;
            lblPort.Content = "Porta: " + port;
            ConnectServer();
        }

        private void ConnectServer()
        {
            try
            {
                clientSocket.Connect(ip, port);

                serverStream = clientSocket.GetStream();

                byte[] outStream = Encoding.UTF8.GetBytes(name + "$");
                clientSocket.ReceiveBufferSize = 1024;
                clientSocket.SendBufferSize = 1024;
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();

                Thread ctThread = new Thread(getMessage);
                ctThread.Start();
                Dispatcher.Invoke(() =>
                {
                    txtBoxLog.AppendText("Conectado em: " + ip + " na porta: " + port + ".");
                });
            }
            catch (SocketException)
            {
                MessageBox.Show("O servidor não respondeu.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }
        private void getMessage()
        {
            while (true)
            {
                serverStream = clientSocket.GetStream();
                int buffSize = 0;
                byte[] inStream = new byte[clientSocket.ReceiveBufferSize];
                buffSize = clientSocket.ReceiveBufferSize;
                serverStream.Read(inStream, 0, buffSize);
                string returndata = Encoding.UTF8.GetString(inStream);

                StringBuilder sb = new StringBuilder(returndata);
                sb.Replace("\0", "");
                string ReadDataMod = sb.ToString();
                if (ReadDataMod.Contains("<SHUTDOWNTOKEN:77514>"))
                {
                    Dispatcher.Invoke(() =>
                    {
                        this.Close();
                    });
                    
                    break;
                }
                else
                {
                    readData = "" + sb;
                    msg(readData);
                }
                
            }
        }

        private void msg(string readData)
        {
            var dt = DateTime.Now;
            Dispatcher.Invoke(() =>
            {
                txtBoxLog.AppendText("\n" + "[" + dt + "]" + " >> " + readData);
            });
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                byte[] outStream = Encoding.UTF8.GetBytes(txtMsg.Text + "$");
                serverStream.Write(outStream, 0, (int)outStream.Length);
                serverStream.Flush();
                txtMsg.Text = "";
            }
            catch (SocketException)
            {
                MessageBox.Show("Porta em uso ou IP Inválido.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Selecione uma porta válida.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
            catch (FormatException)
            {
                MessageBox.Show("Selecione um IP válido.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
            catch (NullReferenceException)
            {
                //nothing
            }
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            //txtMsg.Text = "saiu do chat.";
            //BtnSend_Click(null, null);
            this.Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            try
            {
                Clients.Close();
                clientSocket.Close();
                serverStream.Close();
            }
            catch
            {

            }
            txtMsg.Text = "saiu do chat.";
            BtnSend_Click(null, null);
            base.OnClosed(e);
        }
    }
}
