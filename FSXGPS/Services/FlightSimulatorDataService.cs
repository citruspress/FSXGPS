using System;
using System.Collections.Generic;
using System.Linq;
using FsuipcSdk;
using FSXGPS.Data;
using FSXGPS.FSUIPC;

namespace FSXGPS.Services
{
    internal class FlightSimulatorDataService : IFlightSimulatorDataService
    {
        private readonly List<IOffsetData> _offsetData = new List<IOffsetData>();
        private readonly Fsuipc _fsuipc;
        private bool _fsuipcIsOpen;

        public FlightSimulatorDataService(Fsuipc fsuipc)
        {
            Position = new Position();
            Attitude = new Attitude();
            _fsuipc = fsuipc;

            _offsetData.AddRange(Position.Offsets);
            _offsetData.AddRange(Attitude.Offsets);
        }

        public bool Connected => _fsuipcIsOpen;

        public Position Position { get; }

        public Attitude Attitude { get; }

        public void Update()
        {
            if (!_fsuipcIsOpen)
            {
                _fsuipcIsOpen = InitializeFsuipc();
                return;
            }

            if (_offsetData.Any(x => !x.Read(_fsuipc)))
            {
                _fsuipcIsOpen = false;
                _fsuipc.FSUIPC_Close();
                return;
            }

            int dwResult = -1;
            _fsuipc.FSUIPC_Process(ref dwResult);
            
            if (dwResult != Fsuipc.FSUIPC_ERR_OK)
            {
                _fsuipcIsOpen = false;
                _fsuipc.FSUIPC_Close();
                return;
            }

            _offsetData.ForEach(x => x.Update(_fsuipc));
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
    }
}