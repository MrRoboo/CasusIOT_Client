using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace Testclient
{
    class GameController
    {
        public string gameState = "pending";
        private SocketClient client;

        private Led ledRood;
        private Led ledGroen;


        public GameController(SocketClient client, Led ledGroen, Led ledRood)
        {
            this.ledGroen = ledGroen;
            this.ledRood = ledRood;
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
                ledGroen.led.Write(GpioPinValue.Low);
                ledRood.led.Write(GpioPinValue.High);
            } else if ( stateData == "off")
            {
                ledGroen.led.Write(GpioPinValue.High);
                ledRood.led.Write(GpioPinValue.High);
            } else if (stateData == "standby")
            {
                ledGroen.led.Write(GpioPinValue.High);
                ledRood.led.Write(GpioPinValue.Low);
            }
        }

        private async Task WaitUntilAsync(Func<bool> func)
        {
            while (!func())
                await Task.Delay(100);
        }

        public async void SendForceData(int force, DateTime pressed)
        {
            //string[] list = new string[] { };
            //await WaitUntilAsync(() => list.Count() == 5);
            await Task.Delay(1000);
            client.Verstuur("f" + force.ToString());
        }
    }
}
