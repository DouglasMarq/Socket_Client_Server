using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace Socket_Client_Server
{
    /// <summary>
    /// Interaction logic for Client.xaml
    /// </summary>
    public partial class Client : Window
    {
        private IPHostEntry host;
        private IPAddress ipAddress;
        private IPEndPoint IPEndPoint;

        public Client(string Ip, int Port)
        {
            InitializeComponent();
            this.Show();
            ip = Ip;
            port = Port;
            host = Dns.GetHostEntry(ip);
            ipAddress = host.AddressList[0];
            IPEndPoint = new IPEndPoint(ipAddress, port);
            txtBoxLog.AppendText("Conectado em: " + ip + " na porta: " + port + ".");
            ConnectSocket();
        }

        private int port;
        private string ip;
        private Socket Clients;
        private byte[] bytes;

        private void ConnectSocket()
        {
            Dispatcher.Invoke(() =>
            {
                    //txtBoxLog.AppendText("Conectado em: " + ip + " na porta: " + port + ".");
                    txtBoxLog.AppendText("Conectado em: " + ip + " na porta: " + port + ".");
            });

        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bytes = new byte[1024];
                Clients = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                Clients.Connect(IPEndPoint);

                //byte[] msg = Encoding.ASCII.GetBytes("This is a test<EOF>");
                byte[] msg = Encoding.ASCII.GetBytes(txtMsg.Text + "<EOF>");

                // Send the data through the socket.    
                int bytesSent = Clients.Send(msg);

                int bytesRec = Clients.Receive(bytes);
                Console.WriteLine("Echoed test = {0}",
                    Encoding.ASCII.GetString(bytes, 0, bytesRec));

                Clients.Shutdown(SocketShutdown.Both);
                //Clients.Close();
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
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            if (Clients != null)
            {
                Clients.Close();
            }
            this.Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            if (Clients != null)
            {
                Clients.Close();
            }
            base.OnClosed(e);
        }
    }
}
