using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kosu.UnityLibrary
{
    public class SampleNetworkView : BaseDebugUIView
    {

        [SerializeField]
        private DebugUIButton _tcpOSCConnectButtton = null;

        [SerializeField]
        private DebugUIButton _tcpOSCSendButtton = null;

        [SerializeField]
        private DebugUIButton _tcpOSCRecieverButtton = null;

        [SerializeField]
        private DebugUIButton _tcpConnectButtton = null;

        [SerializeField]
        private DebugUIButton _tcpSendButtton = null;

        [SerializeField]
        private DebugUIButton _tcpRecieverButtton = null;

        [SerializeField]
        private DebugUIButton _udpConnectButtton = null;

        [SerializeField]
        private DebugUIButton _udpSendButtton = null;

        [SerializeField]
        private DebugUIButton _udpRecieverButtton = null;

        [SerializeField]
        private DebugUIButton _udpOSCConnectButtton = null;

        [SerializeField]
        private DebugUIButton _udpOSCSendButtton = null;

        [SerializeField]
        private DebugUIButton _udpOSCRecieverButtton = null;

        [SerializeField]
        private DebugUIButton _udpJsonConnectButtton = null;

        [SerializeField]
        private DebugUIButton _udpJsonSendButtton = null;

        [SerializeField]
        private DebugUIButton _udpJsonRecieverButtton = null;

        [SerializeField]
        private DebugUIButton _tcpJsonConnectButtton = null;

        [SerializeField]
        private DebugUIButton _tcpJsonSendButtton = null;

        [SerializeField]
        private DebugUIButton _tcpJsonRecieverButtton = null;

        private TCPOSCSender _tcpOSCSender;

        private TCPOSCReciever _tcpOSCReciever;

        private BaseTCPSender _tcpSender;

        private BaseTCPReciever<string> _tcpReciever;

        private BaseUDPSender _udpSender;

        private BaseUDPReciever<string> _udpReciever;

        private UDPOSCSender _udpOSCSender;

        private UDPOSCReciever _udpOSCReciever;

        private UDPJsonSender _udpJsonSender;

        private UDPJsonReciever _udpJsonReciever;

        private TCPJsonSender _tcpJsonSender;

        private TCPJsonReciever _tcpJsonReciever;

        protected override void Bind()
        {
            base.Bind();
            _tcpOSCConnectButtton.onClick = () =>
            {
                _tcpOSCSender = new TCPOSCSender(NetworkUtils.GetIP(NetworkUtils.ADDRESSFAM.IPv4), 20000);
            };
            _tcpOSCSendButtton.onClick = () =>
            {
                _tcpOSCSender?.Send(TCPOSCSender.CreateOSCMessage("/test", "test"));
            };
            _tcpOSCRecieverButtton.onClick = () =>
            {
                _tcpOSCReciever = new TCPOSCReciever(NetworkUtils.GetIP(NetworkUtils.ADDRESSFAM.IPv4), 20000)
                {
                    IsQueueing = true,
                    onDataRecieved = (msg) =>
                    {
                        if (msg.IsBundle())
                        {
                            Debug.LogError("Not implemented");
                        }
                        else
                        {
                            Debug.Log($"UDP OSCMessage : Address = {msg.Address} Data = {msg.Data[0].ToString()}");
                        }
                    }
                };
            };
            _tcpConnectButtton.onClick = () =>
            {
                _tcpSender = new BaseTCPSender(NetworkUtils.GetIP(NetworkUtils.ADDRESSFAM.IPv4), 20003);
            };
            _tcpSendButtton.onClick = () =>
            {
                _tcpSender?.Send("tcp test message");
            };
            _tcpRecieverButtton.onClick = () =>
            {
                _tcpReciever = new BaseTCPReciever<string>(NetworkUtils.GetIP(NetworkUtils.ADDRESSFAM.IPv4), 20003)
                {
                    IsQueueing = true,
                    onDataRecieved = (str) =>
                    {
                        Debug.Log(str);
                    }
                };
            };
            _udpConnectButtton.onClick = () =>
            {
                _udpSender = new BaseUDPSender(NetworkUtils.GetIP(NetworkUtils.ADDRESSFAM.IPv4), 20001);
            };
            _udpSendButtton.onClick = () =>
            {
                _udpSender?.Send("test udp message");
            };
            _udpRecieverButtton.onClick = () =>
            {
                _udpReciever = new BaseUDPReciever<string>(20001)
                {
                    IsQueueing = true,
                    onDataRecieved = (str) =>
                    {
                        Debug.Log(str);
                    }
                };
            };
            _udpOSCConnectButtton.onClick = () =>
            {
                _udpOSCSender = new UDPOSCSender(NetworkUtils.GetIP(NetworkUtils.ADDRESSFAM.IPv4), 20002);
            };
            _udpOSCSendButtton.onClick = () =>
            {
                _udpOSCSender?.Send(UDPOSCSender.CreateOSCMessage("/test", "test"));
            };
            _udpOSCRecieverButtton.onClick = () =>
            {
                _udpOSCReciever = new UDPOSCReciever(20002)
                {
                    IsQueueing = true,
                    onDataRecieved = (msg) =>
                    {
                        if (msg.IsBundle())
                        {
                            Debug.LogError("Not implemented");
                        }
                        else
                        {
                            Debug.Log($"UDP OSCMessage : Address = {msg.Address} Data = {msg.Data[0].ToString()}");
                        }
                    }
                };
            };
            _udpJsonConnectButtton.onClick = () =>
            {
                _udpJsonSender = new UDPJsonSender(NetworkUtils.GetIP(NetworkUtils.ADDRESSFAM.IPv4), 20004);
            };
            _udpJsonSendButtton.onClick = () =>
            {
                _udpJsonSender?.Send(new SampleJsonData()
                {
                    test1 = "test1",
                    test2 = "test2",
                    test3 = 10,
                    test4 = 1.111f
                });
                _udpJsonSender?.Send(new SampleJsonData2()
                {
                    test5 = "test1",
                    test6 = "test2",
                    test7 = 10,
                    test8 = 1.111f
                });
            };
            _udpJsonRecieverButtton.onClick = () =>
            {
                _udpJsonReciever = new UDPJsonReciever(20004)
                {
                    IsQueueing = true,
                    onDataRecieved = (msg) =>
                    {
                        var recievedData = JsonUtility.FromJson<BaseJsonData>(msg);

                        switch (recievedData.className)
                        {
                            case nameof(SampleJsonData):
                                var data = JsonUtility.FromJson<SampleJsonData>(msg);
                                Debug.Log($"recieved samplejsondata : test1 = {data.test1} test2 = {data.test2} test3 = {data.test3} test4 = {data.test4}");
                                break;

                            case nameof(SampleJsonData2):
                                var data2 = JsonUtility.FromJson<SampleJsonData2>(msg);
                                Debug.Log($"recieved samplejsondata2 : test5 = {data2.test5} test6 = {data2.test6} test7 = {data2.test7} test8 = {data2.test8}");
                                break;
                        }
                    }
                };
            };
            _tcpJsonConnectButtton.onClick = () =>
            {
                _tcpJsonSender = new TCPJsonSender(NetworkUtils.GetIP(NetworkUtils.ADDRESSFAM.IPv4), 20005);
            };
            _tcpJsonSendButtton.onClick = () =>
            {
                _tcpJsonSender?.Send(new SampleJsonData()
                {
                    test1 = "test1",
                    test2 = "test2",
                    test3 = 10,
                    test4 = 1.111f
                });
                _tcpJsonSender?.Send(new SampleJsonData2()
                {
                    test5 = "test1",
                    test6 = "test2",
                    test7 = 10,
                    test8 = 1.111f
                });
            };
            _tcpJsonRecieverButtton.onClick = () =>
            {
                _tcpJsonReciever = new TCPJsonReciever(NetworkUtils.GetIP(NetworkUtils.ADDRESSFAM.IPv4), 20005)
                {
                    IsQueueing = true,
                    onDataRecieved = (msg) =>
                    {
                        var recievedData = JsonUtility.FromJson<BaseJsonData>(msg);

                        switch (recievedData.className)
                        {
                            case nameof(SampleJsonData):
                                var data = JsonUtility.FromJson<SampleJsonData>(msg);
                                Debug.Log($"recieved samplejsondata : test1 = {data.test1} test2 = {data.test2} test3 = {data.test3} test4 = {data.test4}");
                                break;

                            case nameof(SampleJsonData2):
                                var data2 = JsonUtility.FromJson<SampleJsonData2>(msg);
                                Debug.Log($"recieved samplejsondata2 : test5 = {data2.test5} test6 = {data2.test6} test7 = {data2.test7} test8 = {data2.test8}");
                                break;
                        }
                    }
                };
            };
        }

        protected override void UnBind()
        {
            base.UnBind();
            _tcpOSCReciever?.Close();
            _tcpOSCReciever = null;
            _tcpOSCSender?.Close();
            _tcpOSCSender = null;
            _tcpReciever?.Close();
            _tcpReciever = null;
            _tcpSender?.Close();
            _tcpSender = null;
            _udpOSCReciever?.Close();
            _udpOSCReciever = null;
            _udpOSCSender?.Close();
            _udpOSCSender = null;
            _udpReciever?.Close();
            _udpReciever = null;
            _udpSender?.Close();
            _udpSender = null;
            _udpJsonReciever?.Close();
            _udpJsonReciever = null;
            _udpJsonSender?.Close();
            _udpJsonSender = null;
            _tcpJsonReciever?.Close();
            _tcpJsonReciever = null;
            _tcpJsonSender?.Close();
            _tcpJsonSender = null;
        }

    }
}
