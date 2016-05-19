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
            var altitude = Math.Round(flightSimulatorDataService.Position.Altitude, 3).ToString(CultureInfo.InvariantCulture);
            var longitude = Math.Round(flightSimulatorDataService.Position.Longitude, 3).ToString(CultureInfo.InvariantCulture);
            var latitude = Math.Round(flightSimulatorDataService.Position.Latitude, 3).ToString(CultureInfo.InvariantCulture);
            var heading = Math.Round(flightSimulatorDataService.Attitude.Heading, 3).ToString(CultureInfo.InvariantCulture);
            var speed = Math.Round(flightSimulatorDataService.Position.GroundSpeed, 3).ToString(CultureInfo.InvariantCulture);
            byte[] sendBytes = Encoding.ASCII.GetBytes(string.Format("XGPSFSXGPS,{0},{1},{2},{3},{4}", longitude, latitude, altitude, heading, speed));
            _udpClient.Send(sendBytes, sendBytes.Length, _broadcastAddress);

            var pitch = Math.Round(flightSimulatorDataService.Attitude.Pitch, 3).ToString(CultureInfo.InvariantCulture);
            var roll = Math.Round(flightSimulatorDataService.Attitude.Roll, 3).ToString(CultureInfo.InvariantCulture);
            sendBytes = Encoding.ASCII.GetBytes(string.Format("XATTFSXGPS,{0},{1},{2}", heading, pitch, roll));
            //_udpClient.Send(sendBytes, sendBytes.Length, _broadcastAddress);
        }
    }
}