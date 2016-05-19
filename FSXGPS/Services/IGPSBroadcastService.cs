namespace FSXGPS.Services
{
    internal interface IGpsBroadcastService
    {
        void BroadcastGpsData(IFlightSimulatorDataService flightSimulatorDataService);
        void BroadcastAttitudeData(IFlightSimulatorDataService flightSimulatorDataService);
    }
}
