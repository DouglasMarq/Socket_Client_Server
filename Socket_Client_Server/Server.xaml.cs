using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;

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

        public Server(string Ip, int Port)
        {
            InitializeComponent();
            this.Show();
            ip = Ip;
            port = Port;
            host = Dns.GetHostEntry(ip);
            ipAddress = host.AddressList[0];
            localEndPoint = new IPEndPoint(ipAddress, port);
            //arrumar
            lblIp.Content = "IP: " + ip;
            SetUpSocket();
        }

        private int port;
        private string ip;
        private Socket listener;
        private Socket handler;

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

        private void BackgroundMethod()
        {
            handler = listener.Accept();

             string data = null;
            byte[] bytes = null;
            while (true)
            {
                bytes = new byte[1024];
                int bytesRec = handler.Receive(bytes);
                data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                if (data.IndexOf("<EOF>") > -1)
                {
                    break;
                }
            }

            byte[] msg = Encoding.ASCII.GetBytes(data);
            handler.Send(msg);
            handler.Shutdown(SocketShutdown.Both);
            //handler.Close();
            Dispatcher.Invoke(() =>
            {
                txtBoxLog.AppendText("\n Recebido: " + data);
            });
            BackgroundMethod();
        }

        private void BtnShutdown_Click(object sender, RoutedEventArgs e)
        {
            if (listener != null)
            {
                listener.Close();
            }
            this.Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            if (listener != null)
            {
                listener.Close();
            }
            base.OnClosed(e);
        }
    }
}
