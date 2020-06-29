using System;

public class Distance
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
            rssi += i;
        }
        rssi = rssi / (rssiList.Count() + 1);
        return rssi;
    }
}
