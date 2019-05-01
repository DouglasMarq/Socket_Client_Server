using System.ComponentModel;
using System.Windows;

namespace Socket_Client_Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly BackgroundWorker worker = new BackgroundWorker();
        private bool isClient;
        private bool isServer;
        private Server ServerWindow = new Server();

        public MainWindow()
        {
            InitializeComponent();
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            isClient = false;
            isServer = false;
        }

        private void BtnClient_Click(object sender, RoutedEventArgs e)
        {
            if (!isServer && !isClient)
            {
                //do something 
                isClient = true;
            }
        }

        private void BtnServer_Click(object sender, RoutedEventArgs e)
        {
            if (!isClient && !isServer)
            {
                ServerWindow.Activate();
                ServerWindow.Show();
                isServer = true;
                worker.RunWorkerAsync();
            }
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void worker_RunWorkerCompleted(object sender,
                                                   RunWorkerCompletedEventArgs e)
        {
            //update ui once worker complete his work
        }

    }
}
