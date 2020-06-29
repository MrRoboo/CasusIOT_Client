using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Storage.Streams;

namespace Testclient
{
    public class Watcher
    {
        private BluetoothLEAdvertisementWatcher _watcher;
        private int _advertisementCount;
        private List<int> _rssiList;
        private Distance _distance;
        private double _dist;

        public double Dist
        {
            get { return _dist; }
        }

        public Watcher()
        {
            _watcher = new BluetoothLEAdvertisementWatcher();
            _advertisementCount = 0;
            _dist = 0;
            _rssiList = new List<int>();
            _distance = new Distance();
            this.Watch();
        }

        public void Watch()
        {
            //watcher = new BluetoothLEAdvertisementWatcher();
            _watcher.ScanningMode = BluetoothLEScanningMode.Active;

            _watcher.Received += OnAdvertisementReceived;

            var manufacturerData = new BluetoothLEManufacturerData();
            manufacturerData.CompanyId = 0xFFFE;

            // Make sure that the buffer length can fit within an advertisement payload (~20 bytes). 
            // Otherwise you will get an exception.
            var writer = new DataWriter();
            writer.WriteString("Hello World");
            manufacturerData.Data = writer.DetachBuffer();

            _watcher.AdvertisementFilter.Advertisement.ManufacturerData.Add(manufacturerData);

            _watcher.Start();
        }

        public void startWatcher()
        {
            _watcher.Start();
            Debug.WriteLine("start watcher");
        }

        private void OnAdvertisementReceived(BluetoothLEAdvertisementWatcher watcher, BluetoothLEAdvertisementReceivedEventArgs eventArgs)
        {
            // The received signal strength indicator (RSSI)
            Int16 rssi = eventArgs.RawSignalStrengthInDBm;
            _rssiList.Add(rssi);

            _advertisementCount += 1;


            if (_advertisementCount == 5)
            {
                _dist = _distance.CalculateDistance(_rssiList);
                Debug.WriteLine("Distance uit event: " + _dist);

                
                if (_checkIfWatcherIsActive())
                {
                    watcher.Stop();
                }
            }
        }

        private bool _checkIfWatcherIsActive()
        {
            if (_watcher.Status == BluetoothLEAdvertisementWatcherStatus.Started)
            {
                return true;
            }
            return false;
        }

        public void stopMeting()
        {
            if (_watcher.Status != BluetoothLEAdvertisementWatcherStatus.Stopped)
            {
                _watcher.Stop();
                Debug.WriteLine("Watcher status: " + _watcher.Status);
                _dist = 0;
            }
        }

        public double GetDistance()
        {
            Task.Delay(5000).Wait();
            Debug.WriteLine("Get distance: " + this._dist);
            return this._dist;
        }

        public void clearDistance()
        {
            _advertisementCount = 0;
            _rssiList.Clear();
        }
    }
}
