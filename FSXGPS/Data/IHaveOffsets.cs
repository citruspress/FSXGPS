using System.Collections.Generic;
using FSXGPS.FSUIPC;

namespace FSXGPS.Data
{
    internal interface IHaveOffsets
    {
        IEnumerable<IOffsetData> Offsets { get; }
    }
}