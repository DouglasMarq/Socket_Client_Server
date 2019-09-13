using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socket_Client_Server
{
    public class Core
    {
        public static Task StartAsync()
            => new Core().CreateAsync();

        private async Task CreateAsync()
        {
            MainWindow MW = new MainWindow();
            MW.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
