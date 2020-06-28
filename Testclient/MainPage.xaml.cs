using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
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
        Led ledGroen;
        Led ledRood;
        GameController gameController;

        ForceSensor forceSensor;
        const int sensorPin = 0; //Line 0 maps to physical pin number 24 on the RPi2 or RPi3
        const int gpioPin = 25;
        
        private Button _button;

        //moet nog geimplementeerd worden met forcesensor
        private bool _isObjectTouched = true;
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

            //init leds
            ledGroen = new Led(raspberry, 21); //gpio 21 = groene led
            ledRood = new Led(raspberry, 20); //gpio 20 = rode led
            ledRood.led.Write(GpioPinValue.Low);

            //Init game controller
            gameController = new GameController(socketClient, ledGroen, ledRood);

            //Init force sensor
            forceSensor = new ForceSensor(sensorPin, gpioPin);
            forceSensor.gameController = gameController;

            InitButtons();
            Thread main = new Thread(new ThreadStart(Aansturen));
            main.Start();
        }



        //############################################
        //*******************METHODS******************
        //############################################

        //Bepaald flow van het programma
        private void Aansturen()
        {
            while (gameController.gameState != "end")
            {
                //wanneer hij data ontvangt wordt deze true
                if (socketServer.getReady())
                {
                    socketServer.setReady(false);
                    String dataString = socketServer.getData();
                    Debug.WriteLine(dataString);
                    if ( dataString == "start" || dataString == "end")
                    {
                        gameController.SetGameState(dataString);
                    } else if ( dataString == "touch")
                    {
                        gameController.SetTouchState(dataString);
                    } else if ( dataString == "off")
                    {
                        gameController.SetTouchState(dataString);
                    }

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
