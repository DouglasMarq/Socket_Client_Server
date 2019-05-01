using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;

namespace Socket_Client_Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly BackgroundWorker worker = new BackgroundWorker();
        private string saveTxt;
        private string saveTxt2;
        private string saveTxtClient;
        private string saveTxtClient2;

        public MainWindow()
        {
            InitializeComponent();
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;

            //Listeners dos Text Box do server
            txtBoxServerPort.GotFocus += TxtBoxServerPort_GotFocus;
            txtBoxServerPort.LostFocus += TxtBoxServerPort_LostFocus;
            txtBoxServerPort.TextChanged += TxtBoxServerPort_TextChanged;
            txtBoxServerIP.GotFocus += TxtBoxServerIP_GotFocus;
            txtBoxServerIP.LostFocus += TxtBoxServerIP_LostFocus;
            txtBoxServerIP.TextChanged += TxtBoxServerIP_TextChanged;

            //Listeners dos Text Box do Client
            txtBoxClientPort.GotFocus += TxtBoxClientPort_GotFocus;
            txtBoxClientPort.LostFocus += TxtBoxClientPort_LostFocus;
            txtBoxClientPort.TextChanged += TxtBoxClientPort_TextChanged;
            txtBoxClientIP.GotFocus += TxtBoxClientIP_GotFocus;
            txtBoxClientIP.LostFocus += TxtBoxClientIP_LostFocus;
            txtBoxClientIP.TextChanged += TxtBoxClientIP_TextChanged;
        }

        #region listeners
        private void TxtBoxServerPort_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtBoxServerPort.Text == "Insira a Porta")
            {
                saveTxt = txtBoxServerPort.Text;
                txtBoxServerPort.Text = "";
            }
        }

        private void TxtBoxServerPort_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (Regex.IsMatch(txtBoxServerPort.Text, "[^0-9]"))
            {
                txtBoxServerPort.Text = txtBoxServerPort.Text.Remove(txtBoxServerPort.Text.Length - 1);
            }
        }

        private void TxtBoxServerPort_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBoxServerPort.Text))
            {
                txtBoxServerPort.Text = saveTxt;
            }
        }

        private void TxtBoxServerIP_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtBoxServerIP.Text == "Insira o IP")
            {
                saveTxt2 = txtBoxServerIP.Text;
                txtBoxServerIP.Text = "";
            }
        }

        private void TxtBoxServerIP_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBoxServerIP.Text))
            {
                txtBoxServerIP.Text = saveTxt2;
            }
        }

        private void TxtBoxServerIP_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (Regex.IsMatch(txtBoxServerIP.Text, "[^0-9-.]"))
            {
                txtBoxServerIP.Text = txtBoxServerIP.Text.Remove(txtBoxServerIP.Text.Length - 1);
            }
        }

        private void TxtBoxClientPort_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtBoxClientPort.Text == "Insira a Porta")
            {
                saveTxtClient = txtBoxClientPort.Text;
                txtBoxClientPort.Text = "";
            }
        }

        private void TxtBoxClientPort_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBoxClientPort.Text))
            {
                txtBoxClientPort.Text = saveTxtClient;
            }
        }

        private void TxtBoxClientPort_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (Regex.IsMatch(txtBoxClientPort.Text, "[^0-9-.]"))
            {
                txtBoxClientPort.Text = txtBoxClientPort.Text.Remove(txtBoxClientPort.Text.Length - 1);
            }
        }

        private void TxtBoxClientIP_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtBoxClientIP.Text == "Insira o IP")
            {
                saveTxtClient2 = txtBoxClientIP.Text;
                txtBoxClientIP.Text = "";
            }
        }

        private void TxtBoxClientIP_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBoxClientIP.Text))
            {
                txtBoxClientIP.Text = saveTxtClient2;
            }
        }

        private void TxtBoxClientIP_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (Regex.IsMatch(txtBoxClientIP.Text, "[^0-9-.]"))
            {
                txtBoxClientIP.Text = txtBoxClientIP.Text.Remove(txtBoxClientIP.Text.Length - 1);
            }
        }
        #endregion

        private void BtnClient_Click(object sender, RoutedEventArgs e)
        {
            Client ClientWindow = new Client(txtBoxClientIP.Text, Convert.ToInt32(txtBoxClientPort.Text));
            ClientWindow.Activate();
        }

        private void BtnServer_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtBoxServerIP.Text))
            {
                Server ServerWindow = new Server(txtBoxServerIP.Text, Convert.ToInt32(txtBoxServerPort.Text));
                ServerWindow.Activate();
                //background check
                //worker.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("Porta e/ou IP vazio", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }
    }
}
