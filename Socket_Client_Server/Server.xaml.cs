using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Socket_Client_Server
{
    /// <summary>
    /// Interaction logic for Server.xaml
    /// </summary>
    public partial class Server : Window
    {
        public Server(string Ip, int Port)
        {
            InitializeComponent();
            this.Show();
            ip = Ip;
            port = Port;
            SetUpSocket();
        }

        private int port;
        private string ip;
        private TcpListener server;
        private Socket connection;

        private async void SetUpSocket()
        {
            try
            {
                txtBoxLog.AppendText("Iniciando Conexão em " + ip + " na porta " + port + ".");
                server = new TcpListener(IPAddress.Parse(ip), port);
                txtBoxLog.AppendText("\nConexão Aberta.");
                server.Start();
                await Task.Run(() =>
                {
                    while (true)
                    {
                        connection = server.AcceptSocket();
                    }
                });
            }
            catch (ArgumentNullException)
            {
                
                MessageBox.Show("Porta e/ou IP vazio.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
            catch (SocketException)
            {
                txtBoxLog.AppendText("\nErro.");
                MessageBox.Show("Porta em uso.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void BtnShutdown_Click(object sender, RoutedEventArgs e)
        {
            connection.Close();
            server.Stop();
            this.Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            connection.Close();
            server.Stop();
            base.OnClosed(e);
        }
    }
}
