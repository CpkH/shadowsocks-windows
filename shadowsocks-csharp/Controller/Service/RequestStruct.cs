using System;
using System.Text;

namespace Shadowsocks.Controller
{
    struct RequestStruct
    {
        /*public byte VER;
            public byte CMD;
            public byte RSV;*/
        public byte ATYP;
        public string DST_ADDR;
        public ushort DST_PORT;

        public static RequestStruct? ConvertToRequestStruct(byte[] bytes, int length)
        {
            RequestStruct request = new RequestStruct();
            /*request.VER = bytes[0];
        request.CMD = bytes[1];
        request.RSV = bytes[2];*/
            request.ATYP = bytes[0];
            switch (request.ATYP)
            {
                case 1: // IP
                    request.DST_ADDR = bytes[1] + "." + bytes[2] + "." + bytes[3] + "." + bytes[4];
                    request.DST_PORT = BitConverter.ToUInt16(new[] { bytes[6], bytes[5] }, 0);
                    break;
                case 3:
                    byte domainLength = bytes[1];
                    request.DST_ADDR = Encoding.ASCII.GetString(bytes, 2, domainLength);
                    request.DST_PORT = BitConverter.ToUInt16(new[] { bytes[domainLength + 3], bytes[domainLength + 2] }, 0);
                    break;
                case 4:
                    request.DST_ADDR =
                        BitConverter.ToUInt16(new[] { bytes[2], bytes[1] }, 0) + ":" +
                        BitConverter.ToUInt16(new[] { bytes[4], bytes[3] }, 0) + ":" +
                        BitConverter.ToUInt16(new[] { bytes[6], bytes[5] }, 0) + ":" +
                        BitConverter.ToUInt16(new[] { bytes[8], bytes[7] }, 0) + ":" +
                        BitConverter.ToUInt16(new[] { bytes[10], bytes[9] }, 0) + ":" +
                        BitConverter.ToUInt16(new[] { bytes[12], bytes[11] }, 0) + ":" +
                        BitConverter.ToUInt16(new[] { bytes[14], bytes[13] }, 0) + ":" +
                        BitConverter.ToUInt16(new[] { bytes[16], bytes[15] }, 0);
                    request.DST_PORT = BitConverter.ToUInt16(new[] { bytes[18], bytes[17] }, 0);
                    break;
                default:
                    Console.WriteLine("Forward Request.");
                    return null;
            }
            Console.WriteLine("Request: " + request.DST_ADDR + " " + request.DST_PORT);
            return request;
        }
    }
}