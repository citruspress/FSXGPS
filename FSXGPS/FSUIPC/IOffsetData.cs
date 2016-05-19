using FsuipcSdk;

namespace FSXGPS.FSUIPC
{
    internal interface IOffsetData
    {
        bool Read(Fsuipc fsuipc);
        void Update(Fsuipc fsuipc);
    }
}