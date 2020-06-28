using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Testclient
{
    class Distance
    {
        public double CalculateDistance(List<int> rssiList)
        {
            int rssi = this.CalculateAverage(rssiList);
            //double t = ((-48 - (rssi)) / (10 * 2));
            double y = (10 * 3);
            double x = (-48 - (rssi));

            double halfResult = x / y;
            double result = Math.Pow(10, halfResult);

            return result;
        }

        private int CalculateAverage(List<int> rssiList)
        {
            int rssi = 0;
            foreach (int i in rssiList)
            {
                rssi += rssi;
            }
            rssi = rssi / rssiList.Count();
            return rssi;
        }
    }
}
