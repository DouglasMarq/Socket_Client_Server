using Socket_Client_Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socket_Client_Server
{
    public class Core
    {
        public static Task StartAsync(string ip, int port)
            => new Core().StartServerAsync(ip, port);

        private async Task StartServerAsync(string ip, int port)
        {
            StartServerService.Init(ip, port);
        }
    }
}
