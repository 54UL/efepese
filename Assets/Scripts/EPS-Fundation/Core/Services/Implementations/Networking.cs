using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPS.Api;
using Unity.Netcode;
using Unity.Netcode.Transports.UNET;

namespace EPS
{
    public enum NetworkSytemType { UNDEFINED, HOST, CLIENT, SERVER };

    public class Networking : EPS.INetworking, EPS.Foundation.IService
    {
        private NetworkSytemType currentSystemType;

        public Networking()
        {
        }
        
        //INetworking
        public void Start(NetworkSytemType type, string ip, int port)
        {
            var networkManager = NetworkManager.Singleton;

            //Configure netcode

            //Init
            switch (type)
            {
                case NetworkSytemType.HOST:
                     networkManager.StartHost();
                    break;
                case NetworkSytemType.SERVER:
                     networkManager.StartServer();
                    break;
                case NetworkSytemType.CLIENT:
                    networkManager.GetComponent<UNetTransport>().ConnectAddress = ip; //takes string
                    networkManager.GetComponent<UNetTransport>().ConnectPort = port;  //takes integer
                    networkManager.StartClient();
                break;    
            }
            currentSystemType = type;
        }

        public void Stop()
        {
            var networkManager = NetworkManager.Singleton;

            //switch (currentSystemType)
            //{
            //    case NetworkSytemType.HOST:
            //        //return networkManager.
            //    case NetworkSytemType.SERVER:
            //        return networkManager.();
            //    case NetworkSytemType.CLIENT:
            //        return networkManager.();
            //    break;    
            //}
        }

        //IService
        public System.Func<IEnumerator> LoopCourrutine()
        {
            return null;
        }

        public void Loop()
        {
        }


        public void OnDestroy()
        {
        }

        public void OnInit(DependencyManager manager)
        {
        }

        public void OnReset()
        {
        }

        public string ReferencedName()
        {
            return this.GetType().ToString();
        }
    }
}

