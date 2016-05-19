using System;
using System.Collections.Generic;
using FSXGPS.FSUIPC;

namespace FSXGPS.Data
{
    internal class Position
    {
        private const double FeetToMetersMultiplier = 0.3048;

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double GroundSpeed { get; set; }
        public double Altitude { get; set; }

        public IEnumerable<IOffsetData> Offsets
        {
            get
            {
                return new IOffsetData[]
                {
                    new OffsetData<double>(data => BitConverter.ToInt64(data, 0) * 3.28084D / (65536D * 65536D) * FeetToMetersMultiplier, data => Altitude = data, 0x570, 8),
                    new OffsetData<double>(data => BitConverter.ToInt64(data, 0) * 360D / (65536D * 65536D * 65536D * 65536D), data => Longitude = data, 0x568, 8),
                    new OffsetData<double>(data => BitConverter.ToInt64(data, 0) * 90D / (10001750D * 65536D * 65536D), data => Latitude = data, 0x560, 8),
                    new OffsetData<double>(data => BitConverter.ToInt32(data, 0) / 65536D, data => GroundSpeed = data, 0x02B4, 4)
                };
            }
        }
    }
}
