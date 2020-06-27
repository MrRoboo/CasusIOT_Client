using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace Testclient
{
    class Led
    {
        private Raspberry _raspberry;

        public GpioPin led;

        /// <summary>
        /// Initialize the pins
        /// </summary>
        /// <param name="raspberry">Raspberry connection</param>
        /// <param name="ledId">Id from the Pin</param>
        public Led(Raspberry raspberry, int pinId)
        {
            _raspberry = raspberry;
            led = LightInit(pinId);

        }

        /// <summary>
        /// Initialize the pin from the Led
        /// </summary>
        /// <param name="pinId">pin the Led will use</param>
        /// <returns></returns>
        protected GpioPin LightInit(int pinId)
        {
            GpioPin pin = _raspberry.gpio.OpenPin(pinId);
            pin.Write(GpioPinValue.High);
            pin.SetDriveMode(GpioPinDriveMode.Output);
            return pin;
        }

        /// <summary>
        /// data naar het shiftregister sturen om te bepalen welke ledjes moeten gaan branden
        /// </summary>
        /// <param name="b">01010101</param>

    }
}
