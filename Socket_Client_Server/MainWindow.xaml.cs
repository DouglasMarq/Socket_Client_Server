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

        public MainWindow()
        {
            InitializeComponent();
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            txtBoxServerPort.GotFocus += TxtBoxServerPort_GotFocus;
            txtBoxServerPort.LostFocus += TxtBoxServerPort_LostFocus;
            txtBoxServerPort.TextChanged += TxtBoxServerPort_TextChanged;
            txtBoxServerIP.GotFocus += TxtBoxServerIP_GotFocus;
            txtBoxServerIP.LostFocus += TxtBoxServerIP_LostFocus;
            txtBoxServerIP.TextChanged += TxtBoxServerIP_TextChanged;
        }

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

        private void BtnClient_Click(object sender, RoutedEventArgs e)
        {

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
