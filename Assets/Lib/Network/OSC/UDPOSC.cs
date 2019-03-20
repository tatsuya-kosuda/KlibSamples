using UnityEngine;
using UnityOSC;

namespace Kosu.UnityLibrary
{
    public class UDPOSCSender : BaseUDPSender
    {

        public UDPOSCSender(string host, int port) : base(host, port) { }

        protected override void Constructed(string host, int port)
        {
            Debug.Log("[UDPOSCSender] Invoke Constructor Method");
            Connect(host, port);
        }

        public static OSCMessage CreateOSCMessage(string address, params object[] objects)
        {
            var msg = new OSCMessage(address);

            foreach (object msgvalue in objects)
            {
                msg.Append(msgvalue);
            }

            return msg;
        }

        public static OSCBundle CreateOSCBundle(string address, params object[] objects)
        {
            return null;
        }

        protected override byte[] Serialize<T>(T data)
        {
            if (!(data is OSCPacket))
            {
                Debug.LogError($"data format is invalid : datatype = {typeof(T)}");
                return null;
            }

            return (data as OSCPacket).BinaryData;
        }

    }

    public class UDPOSCReciever : BaseUDPReciever<OSCPacket>
    {

        public UDPOSCReciever(int port) : base(port) { }

        protected override void Constructed(int port)
        {
            Debug.Log("[UDPOSCReciever] Invoke Constructor Method");
            StartServer(port);
        }

        protected override OSCPacket DeSerialize(byte[] data)
        {
            return OSCPacket.Unpack(data);
        }

    }
}
