using System;
using System.Collections.Generic;
using FSXGPS.FSUIPC;

namespace FSXGPS.Data
{
    internal class Attitude : IHaveOffsets
    {
        public double Heading { get; set; }
        public double Pitch { get; set; }
        public double Roll { get; set; }

        public IEnumerable<IOffsetData> Offsets
        {
            get
            {
                return new[]
                {
                    new OffsetData<double>(data => BitConverter.ToInt32(data, 0) * 360D / (65536D * 65536D), data => Heading = data < 0 ? (180 + (180 - Math.Abs(data))) : data, 0x580, 4),
                    new OffsetData<double>(data => BitConverter.ToInt32(data, 0) * 360D / (65536D * 65536D), data => Pitch = data, 0x578, 4),
                    new OffsetData<double>(data => BitConverter.ToInt32(data, 0) * 360D / (65536D * 65536D), data => Roll = data, 0x57C, 4)
                };
            }
        }
    }
}