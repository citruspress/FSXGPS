using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FSXGPS.Services
{
    internal class GpsBroadcastService : IGpsBroadcastService
    {
        private readonly UdpClient _udpClient = new UdpClient();
        private readonly IPEndPoint _broadcastAddress = new IPEndPoint(IPAddress.Broadcast, 49002);

        public void BroadcastData(IFlightSimulatorDataService flightSimulatorDataService)
        {
            var altitude = Math.Round(flightSimulatorDataService.Aircraft.Altitude, 3).ToString(CultureInfo.InvariantCulture);
            var longitude = Math.Round(flightSimulatorDataService.Aircraft.Longitude, 3).ToString(CultureInfo.InvariantCulture);
            var latitude = Math.Round(flightSimulatorDataService.Aircraft.Latitude, 3).ToString(CultureInfo.InvariantCulture);
            var heading = Math.Round(flightSimulatorDataService.Aircraft.Heading, 3).ToString(CultureInfo.InvariantCulture);
            var speed = Math.Round(flightSimulatorDataService.Aircraft.GroundSpeed, 3).ToString(CultureInfo.InvariantCulture);
            byte[] sendBytes = Encoding.ASCII.GetBytes(string.Format("XGPSMy sim,{0},{1},{2},{3},{4}", longitude, latitude, altitude, heading, speed));
            _udpClient.Send(sendBytes, sendBytes.Length, _broadcastAddress);
        }
    }
}