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
        public bool sensorActive;
        private SocketClient client;

        private Led ledRood;
        private Led ledGroen;

        private Watcher watcher;
        private Publisher publisher;


        public GameController(SocketClient client, Led ledGroen, Led ledRood)
        {
            this.ledGroen = ledGroen;
            this.ledRood = ledRood;
            this.client = client;
            this.watcher = new Watcher();
            this.publisher = new Publisher();
            sensorActive = false;
        }



        public void SetGameState(string stateData)
        {
            gameState = stateData;
        }



        public void SetTouchState(string stateData)
        {
            if (stateData == "touch")
            {
                sensorActive = true;
                ledGroen.led.Write(GpioPinValue.Low);
                ledRood.led.Write(GpioPinValue.High);
                watcher.startWatcher();
                
            } else if ( stateData == "off")
            {
                ledGroen.led.Write(GpioPinValue.High);
                ledRood.led.Write(GpioPinValue.High);
            } else if (stateData == "standby")
            {
                ledGroen.led.Write(GpioPinValue.High);
                ledRood.led.Write(GpioPinValue.Low);
            } else if (stateData == "publisher")
            {
                publisher.publish();
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
            if (sensorActive)
            {
                double dist = watcher.GetDistance();
                await Task.Delay(1000);
                client.Verstuur("f" + force.ToString() + "Afstand: " + dist + "DateTime Pressed: " + pressed);
                publisher.stopMeting();
                watcher.stopMeting();
                watcher.clearDistance();

                sensorActive = false;
            }
        }
    }
}
