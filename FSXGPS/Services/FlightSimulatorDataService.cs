using System;
using FsuipcSdk;
using FSXGPS.Data;

namespace FSXGPS.Services
{
    class FlightSimulatorDataService : IFlightSimulatorDataService
    {
        private readonly Fsuipc _fsuipc;
        private bool _fsuipcIsOpen;
        private int _dwResult = -1;
        private int _altitudeToken = -1;
        private int _longitudeToken = -1;
        private int _latitudeToken = -1;
        private int _headingToken = -1;
        private int _speedToken = -1;

        public FlightSimulatorDataService(Fsuipc fsuipc)
        {
            Aircraft = new Aircraft();
            _fsuipc = fsuipc;
        }

        public bool Connected => _fsuipcIsOpen;

        public Aircraft Aircraft { get; }

        public void Update()
        {
            const double feetToMetersMultiplier = 0.3048;

            if (!_fsuipcIsOpen)
            {
                _fsuipcIsOpen = InitializeFsuipc();
                return;
            }

            int dwResult = -1;
            _fsuipc.FSUIPC_Read(0x570, 8, ref _altitudeToken, ref dwResult);
            _fsuipc.FSUIPC_Read(0x568, 8, ref _longitudeToken, ref dwResult);
            _fsuipc.FSUIPC_Read(0x560, 8, ref _latitudeToken, ref dwResult);
            _fsuipc.FSUIPC_Read(0x580, 4, ref _headingToken, ref dwResult);
            _fsuipc.FSUIPC_Read(0x02B4, 4, ref _speedToken, ref dwResult);

            _fsuipc.FSUIPC_Process(ref _dwResult);
            
            if (dwResult != Fsuipc.FSUIPC_ERR_OK)
            {
                _fsuipcIsOpen = false;
                _fsuipc.FSUIPC_Close();
                return;
            }

            long altitude;
            ReadValue(_altitudeToken, out altitude);
            Aircraft.Altitude = altitude * 3.28084D / (65536D * 65536D) * feetToMetersMultiplier;

            long longitude;
            ReadValue(_longitudeToken, out longitude);
            Aircraft.Longitude = longitude * 360D / (65536D * 65536D * 65536D * 65536D);

            long latitude;
            ReadValue(_latitudeToken, out latitude);
            Aircraft.Latitude = latitude * 90D / (10001750D * 65536D * 65536D);

            int heading;
            ReadValue(_headingToken, out heading);
            Aircraft.Heading = heading * 360D / (65536D * 65536D);

            long speed;
            ReadValue(_speedToken, out speed);
            Aircraft.GroundSpeed = speed / 65536D;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _fsuipc.FSUIPC_Close();
            }
        }

        private bool InitializeFsuipc()
        {
            int dwResult = -1;
            _fsuipc.FSUIPC_Open(0, ref dwResult);

            return dwResult == Fsuipc.FSUIPC_ERR_OK;
        }

        private void ReadValue(int token, out long val)
        {
            byte[] bytes = new byte[sizeof(long)];
            _fsuipc.FSUIPC_Get(ref token, sizeof(long), ref bytes);
            val = BitConverter.ToInt64(bytes, 0);
        }

        private void ReadValue(int token, out int val)
        {
            byte[] bytes = new byte[sizeof(int)];
            _fsuipc.FSUIPC_Get(ref token, sizeof(int), ref bytes);
            val = BitConverter.ToInt32(bytes, 0);
        }
    }
}