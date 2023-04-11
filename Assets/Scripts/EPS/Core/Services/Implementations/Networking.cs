﻿//GET PLAYER FROM SERVER
//NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPS.Api;
using Unity.Netcode;
using Unity.Netcode.Transports.UNET;

namespace EPS.Core
{
    public enum NetworkSytemType { UNDEFINED, HOST, CLIENT, SERVER };

    public class Networking : INetworking, EPS.Foundation.IService
    {
        private NetworkSytemType currentSystemType;
     
        public Networking()
        {
            
        }

        //TODO: IMPLENT CALLBACK REGISTER HERE
        //void ConfigureNetworking()
        //{
        //    networkManager.OnClientConnectedCallback += (ulong id) =>
        //    {
        //        if (networkManager.IsClient)
        //        {
        //            string welcomeMessage = string.Format("Hello : {0} :3 (player connected)", id.ToString());
        //            LogInfo(welcomeMessage);
        //            //RenderShell(false);
        //            EnableLobbyCamera(false);
        //            ConectionStatus = true;
        //        }
        //        else
        //        {
        //            LogInfo("Not a client");
        //            ConectionStatus = false;
        //        }
        //    };

        //    var transport = networkManager.transform.GetComponent<UNetTransport>();
        //    transport.OnTransportEvent += OnTransportEvent;
        //}

        //INetworking
        private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
        {
            // The client identifier to be authenticated
            var clientId = request.ClientNetworkId;

            // Additional connection data defined by user code
            var connectionData = request.Payload;

            // Your approval logic determines the following values
            response.Approved = true;
            response.CreatePlayerObject = true;

            // The prefab hash value of the NetworkPrefab, if null the default NetworkManager player prefab is used
            response.PlayerPrefabHash = null;

            // Position to spawn the player object (if null it uses default of Vector3.zero)
            response.Position = Vector3.zero;

            // Rotation to spawn the player object (if null it uses the default of Quaternion.identity)
            response.Rotation = Quaternion.identity;

            // If additional approval steps are needed, set this to true until the additional steps are complete
            // once it transitions from true to false the connection approval response will be processed.
            response.Pending = false;
        }

        public bool Start(NetworkSytemType type, string ip, int port)
        {
            var networkManager = NetworkManager.Singleton;
            currentSystemType = type;
            // networkManager.ConnectionApprovalCallback = ApprovalCheck;

            switch (type)
            {
                case NetworkSytemType.HOST:
                     return networkManager.StartHost();
                case NetworkSytemType.SERVER:
                     return networkManager.StartServer();
                case NetworkSytemType.CLIENT:
                    networkManager.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes("EPS CUSTOM DATA(INSERT JSON STRING HERE)...");// EXAMPLE...
                    networkManager.GetComponent<UNetTransport>().ConnectAddress = ip; //takes string
                    networkManager.GetComponent<UNetTransport>().ConnectPort = port;  //takes integer
                    return networkManager.StartClient();
                default:
                    return false;
            }
        }

        public void Stop()
        {
            var networkManager = NetworkManager.Singleton;
            networkManager.Shutdown(false);
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

        public void SetNetworkingConfig(NetworkSytemType type, string ip, int port)
        {
            throw new System.NotImplementedException();
        }

        public bool IsConnected()
        {
            throw new System.NotImplementedException();
        }

        public void OnClientConnected(System.Action<ulong> onClientConnectedCallback)
        {
            throw new System.NotImplementedException();
        }

        public void OnTransportEvent(System.Action<NetworkEvent, ulong, System.ArraySegment<byte>, float> transportEventCallback)
        {
            throw new System.NotImplementedException();
        }

        public NetworkType GetNetworkingType()
        {
            throw new System.NotImplementedException();
        }

        public NetworkManager GetNetworkManager()
        {
            return NetworkManager.Singleton;
        }
    }
}

