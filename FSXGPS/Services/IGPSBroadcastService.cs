namespace FSXGPS.Services
{
    internal interface IGpsBroadcastService
    {
        void BroadcastData(IFlightSimulatorDataService flightSimulatorDataService);
    }
}
