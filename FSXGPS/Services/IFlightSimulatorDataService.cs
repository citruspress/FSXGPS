using System;
using FSXGPS.Data;

namespace FSXGPS.Services
{
    internal interface IFlightSimulatorDataService : IDisposable
    {
        bool Connected { get; }

        Position Position { get; }

        Attitude Attitude { get; }

        void Update();
    }
}
