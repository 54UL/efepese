using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPS.Api;
using Unity.Netcode;
using System;

namespace EPS
{
    public enum NetworkType {CLIENT, HOST, SERVER };

    public interface INetworking
    {
       void SetNetworkingConfig(NetworkSytemType type, string ip, int port);
       bool Start(NetworkSytemType type, string ip, int port);
       void Stop();
       bool IsConnected();
       void OnClientConnected(System.Action<ulong> onClientConnectedCallback);
       void OnTransportEvent(System.Action<NetworkEvent, ulong, ArraySegment<byte>, float> transportEventCallback);
       NetworkType GetNetworkingType();
    }
}


