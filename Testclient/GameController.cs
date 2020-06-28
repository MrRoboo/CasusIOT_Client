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
        public string gameState = "pending";
        public bool touchState = false;
        private SocketClient client;


        public GameController(SocketClient client)
        {
            this.client = client;
        }



        public void SetGameState(string stateData)
        {
            gameState = stateData;
        }



        public void SetTouchState(string stateData)
        {
            if (stateData == "touch")
            {
                touchState = true;
            }
            else
            {
                touchState = false;
            }
        }



        public void SendForceData(int force)
        {
            client.Verstuur("f" + force.ToString());
        }
    }
}
