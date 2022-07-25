using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPS.Api;

namespace EPS
{
    public interface INetworking
    {
       bool Start(NetworkSytemType type, string ip, int port);
       void Stop();
    }
}


