using System;
using System.Runtime.InteropServices;
using FsuipcSdk;

namespace FSXGPS.FSUIPC
{
    internal class OffsetData<T> : IOffsetData
    {
        private readonly Func<byte[], T> _convertData;
        private readonly Action<T> _updateData;
        private readonly int _offset;
        private readonly int _size;
        private int _token = -1;

        public OffsetData(Func<byte[], T> convertData, Action<T> updateData, int offset, int size)
        {
            _convertData = convertData;
            _updateData = updateData;
            _offset = offset;
            _size = size;
        }

        public bool Read(Fsuipc fsuipc)
        {
            int result = 0;
            fsuipc.FSUIPC_Read(_offset, _size, ref _token, ref result);
            return result == Fsuipc.FSUIPC_ERR_OK;
        }

        public void Update(Fsuipc fsuipc)
        {
            _updateData(_convertData(ReadValue(fsuipc)));
        }

        private byte[] ReadValue(Fsuipc fsuipc)
        {
            byte[] bytes = new byte[_size];
            fsuipc.FSUIPC_Get(ref _token, _size, ref bytes);
            return bytes;
        }
    }
}
