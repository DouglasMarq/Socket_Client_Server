using MahApps.Metro.Controls;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;

namespace Socket_Client_Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private string saveTxt;
        private string saveTxtClient;
        private string saveTxtName;
        private ThreadStart clientThread;
        private Thread backgroundClient;
        private ThreadStart serverThread;
        private Thread backgroundServer;

        public MainWindow()
        {
            InitializeComponent();

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

            //listeners do Text Box do Nome
            txtBoxName.GotFocus += TxtBoxName_GotFocus;
            txtBoxName.LostFocus += TxtBoxName_LostFocus;
        }

        public void MWServerTerminated(object sender, EventArgs e)
        {
            backgroundClient.Abort();
            backgroundServer.Abort();
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
                saveTxt = txtBoxServerIP.Text;
                txtBoxServerIP.Text = "";
            }
        }

        private void TxtBoxServerIP_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBoxServerIP.Text))
            {
                txtBoxServerIP.Text = saveTxt;
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
                saveTxtClient = txtBoxClientIP.Text;
                txtBoxClientIP.Text = "";
            }
        }

        private void TxtBoxClientIP_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBoxClientIP.Text))
            {
                txtBoxClientIP.Text = saveTxtClient;
            }
        }

        private void TxtBoxClientIP_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (Regex.IsMatch(txtBoxClientIP.Text, "[^0-9-.]"))
            {
                txtBoxClientIP.Text = txtBoxClientIP.Text.Remove(txtBoxClientIP.Text.Length - 1);
            }
        }

        private void TxtBoxName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBoxName.Text))
            {
                txtBoxName.Text = saveTxtName;
            }
        }

        private void TxtBoxName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtBoxName.Text == "Insira um Nome")
            {
                saveTxtName = txtBoxName.Text;
                txtBoxName.Text = "";
            }
        }
        #endregion

        private void BtnClient_Click(object sender, RoutedEventArgs e)
        {
            clientThread = new ThreadStart(BackgroundClient);
            backgroundClient = new Thread(clientThread);
            backgroundClient.Start();
        }

        private void BackgroundClient()
        {
            Dispatcher.Invoke(() =>
            {
                if (!string.IsNullOrWhiteSpace(txtBoxServerIP.Text))
                {
                    try
                    {
                        Client ClientWindow = new Client(txtBoxClientIP.Text, Convert.ToInt32(txtBoxClientPort.Text), txtBoxName.Text);
                        ClientWindow.Activate();
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("Insira um IP/Porta válido(a).", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Porta e/ou IP vazio", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }

        private void BtnServer_Click(object sender, RoutedEventArgs e)
        {
            serverThread = new ThreadStart(BackgroundServer);
            backgroundServer = new Thread(serverThread);
            backgroundServer.Start();
        }

        private void BackgroundServer()
        {
            Dispatcher.Invoke(() => {
                if (!string.IsNullOrWhiteSpace(txtBoxServerIP.Text))
                {
                    try
                    {
                        Server ServerWindow = new Server(txtBoxServerIP.Text, Convert.ToInt32(txtBoxServerPort.Text));
                        ServerWindow.Terminated += MWServerTerminated;
                        ServerWindow.Activate();
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("Insira um IP/Porta válido(a).", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Porta e/ou IP vazio", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }
    }
}
