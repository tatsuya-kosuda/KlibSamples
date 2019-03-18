using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityOSC;
using System.Linq;

namespace Kosu.UnityLibrary
{
    public class TCPSender
    {

        private TcpClient _tcpClient;

        public void Init(string ip, int port)
        {
            _tcpClient = new TcpClient(ip, port);
        }

        public void Send(string address, params object[] objects)
        {
            try
            {
                using (NetworkStream stream = _tcpClient.GetStream())
                {
                    if (stream.CanWrite)
                    {
                        OSCMessage message = CreateOSCMessage(address, objects.ToList());
                        stream.Write(message.BinaryData, 0, message.BinaryData.Length);
                        Debug.Log("send to tcp server : length = " + message.BinaryData.Length);
                    }
                }
            }
            catch (SocketException e)
            {
                Debug.LogError("SocketException : " + e.ToString());
            }
        }

        private OSCMessage CreateOSCMessage<T>(string address, List<T> objects)
        {
            OSCMessage message = new OSCMessage(address);

            foreach (T msgvalue in objects)
            {
                message.Append(msgvalue);
            }

            return message;
        }

    }

    public class TCPReciever
    {

        private TcpListener _tcpListener;

        private Thread _tcpListenerThread;

        private TcpClient _tcpClient;

        public System.Action<OSCMessage> OnListenOSCMessage;

        public System.Action<OSCBundle> OnListenOSCBundle;

        private Queue<OSCPacket> _packetQueue = new Queue<OSCPacket>();

        public void Init(string ip, int port)
        {
            try
            {
                _tcpListener = new TcpListener(IPAddress.Parse(ip), port);
                _tcpListener.Start();
                Debug.Log("TCP Server start");
            }
            catch (SocketException e)
            {
                Debug.LogError("SocketException : " + e.ToString());
            }

            _tcpListenerThread = new Thread(new ThreadStart(Recieve));
            _tcpListenerThread.IsBackground = true;
            _tcpListenerThread.Start();
        }

        private void Recieve()
        {
            byte[] bytes = new byte[1024];

            while (true)
            {
                using (_tcpClient = _tcpListener.AcceptTcpClient())
                {
                    using (NetworkStream stream = _tcpClient.GetStream())
                    {
                        using (System.IO.MemoryStream mem = new System.IO.MemoryStream())
                        {
                            int length = 0;

                            while ((length = stream.Read(bytes, 0, bytes.Length)) > 0)
                            {
                                mem.Write(bytes, 0, length);
                            }

                            OSCPacket packet = OSCPacket.Unpack(mem.ToArray());
                            _packetQueue.Enqueue(packet);
                        }
                    }
                }
            }
        }

        public void UpdateListen()
        {
            while (_packetQueue.Count > 0)
            {
                OSCPacket packet = _packetQueue.Dequeue();

                if (packet == null)
                {
                    continue;
                }

                if (packet.IsBundle())
                {
                    OSCBundle bundle = packet as OSCBundle;
                    OnListenOSCBundle.SafeInvoke(bundle);
                }
                else
                {
                    OSCMessage message = packet as OSCMessage;
                    OnListenOSCMessage.SafeInvoke(message);
                }
            }
        }

    }
}
