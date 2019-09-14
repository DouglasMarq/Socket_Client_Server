using MahApps.Metro.Controls;
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
    public partial class Server : MetroWindow
    {
        public static Hashtable ListaClientes = new Hashtable();
        private Socket listener;
        private TcpListener server;
        private TcpClient clientSocket;
        public EventHandler Terminated;
        private int i = 0;
        private DispatcherTimer _contaTempo;

        public Server()
        {
            var ts = new ThreadStart(contaTempo);
            var contaTempoThread = new Thread(ts);
            contaTempoThread.Start();
        }

        private void contaTempo()
        {
            //var Inicio = DateTime.Now.TimeOfDay;
            //_contaTempo = new DispatcherTimer(new TimeSpan(0, 0, 0, 1), DispatcherPriority.Render, delegate
            //{
            //    var tempo = DateTime.Now.TimeOfDay - Inicio;
            //    lblUptime.Content = "Uptime: " + tempo.Hours + ":" + tempo.Minutes + ":" + tempo.Seconds;
            //}, this.Dispatcher);
        }

        private void BtnShutdown_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
