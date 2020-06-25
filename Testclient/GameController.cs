using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Testclient
{
    class GameController
    {

        private SocketClient client;

        public GameController(SocketClient client)
        {
            this.client = client;
        }

        public void AddForceData(int force)
        {
            client.Verstuur("f" + force.ToString());
        }
    }
}
