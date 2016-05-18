using System;
using FSXGPS.Data;

namespace FSXGPS.Services
{
    internal interface IFlightSimulatorDataService : IDisposable
    {
        bool Connected { get; }

        Aircraft Aircraft { get; }

        void Update();
    }
}
