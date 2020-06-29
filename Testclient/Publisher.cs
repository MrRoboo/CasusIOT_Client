using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Storage.Streams;


    class Publisher
    {
        BluetoothLEAdvertisementPublisher publisher;

        public Publisher()
        {
            publisher = new BluetoothLEAdvertisementPublisher();
        }

        public void publish()
        {
            // Add custom data to the advertisement
            var manufacturerData = new BluetoothLEManufacturerData();
            manufacturerData.CompanyId = 0xFFFE;

            var writer = new DataWriter();
            writer.WriteString("Hello World");

            // Make sure that the buffer length can fit within an advertisement payload (~20 bytes). 
            // Otherwise you will get an exception.
            manufacturerData.Data = writer.DetachBuffer();

            // Add the manufacturer data to the advertisement publisher:
            publisher.Advertisement.ManufacturerData.Add(manufacturerData);

            publisher.Start();
            Debug.WriteLine("Publisher started");
        }

        public void stopMeting()
        {
            if (publisher.Status != BluetoothLEAdvertisementPublisherStatus.Stopped)
            {
                publisher.Stop();
                Debug.WriteLine("Publisher status: " + publisher.Status);
            }
        }
    }
