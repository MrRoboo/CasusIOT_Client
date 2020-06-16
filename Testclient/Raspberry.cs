using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace Testclient
{
    class Raspberry
    {
        public GpioController gpio;
        
        public Raspberry()
        {
            gpio = GpioController.GetDefault();
        }
    }
}
