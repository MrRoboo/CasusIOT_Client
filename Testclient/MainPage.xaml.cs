using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Testclient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        SocketClient socketClient = new SocketClient("RaspberryPi3_SD", 9000);
        SocketServer socketServer;

        Raspberry raspberry = new Raspberry();

        GameController gameController;

        ForceSensor forceSensor;
        const int sensorPin = 0; //Line 0 maps to physical pin number 24 on the RPi2 or RPi3
        const int gpioPin = 25;
        
        private Button _button;

        //moet nog geimplementeerd worden met forcesensor
        private bool _isObjectTouched = false;
        private string _objectTouchmessage;

        //############################################
        //********************MAIN********************
        //############################################

        public MainPage()
        {
            this.InitializeComponent();

            socketServer = new SocketServer(9000, socketClient);

            //Koppel OnDataOntvangen aan de methode die uitgevoerd worden:
            socketServer.OnDataOntvangen += socketServer.Server_OnDataOntvangen;

            //Init game controller
            gameController = new GameController(socketClient);

            //Init force sensor
            forceSensor = new ForceSensor(sensorPin, gpioPin);
            forceSensor.gameController = gameController;

            InitButtons();

            Aansturen();
        }


        //############################################
        //*******************METHODS******************
        //############################################

        //Bepaald flow van het programma
        private void Aansturen()
        {
            while (true)
            {
                if (socketServer.getReady())
                {
                    socketServer.setReady(false);
                    String dataString = socketServer.getData();
                    Debug.WriteLine(dataString);

                    //De delay voorkomt de spam van berichten in de console tijdens het testen. 
                    //omdat de data van client naar server en visa versa wordt verstuurd.
                    Task.Delay(500).Wait();
                    socketClient.Verstuur("bericht vanuit de client");

                }


            }
        }


        //*****************Initiolize*****************
        public void InitButtons()
        {
            _button = new Button(5);
            _button.buttonID.ValueChanged += ButtonID_ValueChanged;
        }


        //############################################
        //*******************EVENTS*******************
        //############################################

        //check what button pressed and return correct sensor data
        private void ButtonID_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            if (args.Edge == GpioPinEdge.FallingEdge)
            {
                Trigger sensor = new Trigger("force sensor");
                _isObjectTouched = sensor.GetSensorValue();

                Debug.WriteLine("{0} button pressed. {1}", sensor.GetSensorName(), sensor.SensorActivation());
            }
        }
    }
}
