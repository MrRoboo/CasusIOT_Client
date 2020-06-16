using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Windows.Devices.WiFi;
using Windows.Networking;
using Windows.Networking.Connectivity;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace Testclient
{
    public class SocketClient
    {
        //Aanpassen naar eigen netwerk:
        private const string IP = "RaspberryPI3_MP"; // Client (eigen adres) vast (adres dat niet mee doet!)        

        private readonly string _ip;
        private readonly int _port;
        private StreamSocket _socket;
        private DataWriter _writer;

        private string Eigen_IP;
        public delegate void Error(string message);
        public delegate void DataRecived(string data);

        public string Ip { get { return _ip; } }
        public int Port { get { return _port; } }

        /// <summary>
        /// Initialiseer client en maak verbinding met de server
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public SocketClient(string ip, int port)
        {
            _ip = ip;
            _port = port;
            Eigen_IP = GetIpAddress().ToString();
            Task.Run(() => Connect()).Wait();
        }

        /// <summary>
        /// Maak verbinding met de server en verstuur een eerste bericht met eigen ip-adres
        /// </summary>
        public async void Connect()
        {
            try
            {
                var hostName = new HostName(Ip);
                _socket = new StreamSocket();
                await _socket.ConnectAsync(hostName, Port.ToString());
                _writer = new DataWriter(_socket.OutputStream);
                Task.Delay(50).Wait();

                //Verstuur een bericht naar de server met eigen IP-Adres
                Verstuur(Eigen_IP);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Verstuur een bericht naar de server
        /// </summary>
        /// <param name="message">Bericht dat verstuurd moet worden</param>
        public async void Verstuur(string message)
        {

            if (_writer != null)
            {
                _writer.WriteUInt32(_writer.MeasureString(message));
                _writer.WriteString(message);

                try
                {
                    await _writer.StoreAsync();
                    await _writer.FlushAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            else
            {
                Debug.WriteLine("no connection");
            }
        }

        /// <summary>
        /// Zoek eigen IP adres 
        /// </summary>
        /// <returns></returns>
        public IPAddress GetIpAddress()
        {
            var hosts = NetworkInformation.GetHostNames();
            foreach (var host in hosts)
            {
                IPAddress addr;
                if (IPAddress.TryParse(host.DisplayName, out addr))
                    if ((host.DisplayName != IP) && (addr.AddressFamily == AddressFamily.InterNetwork))
                        return addr;
            }
            return null;
        }
    }
}
