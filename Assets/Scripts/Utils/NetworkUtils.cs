using System.Net;
using System.Net.Sockets;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public static class NetworkUtils
{
    public static string GetLocalIPAddress()
    {
        string localIP = "Not found";

        foreach (var host in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
        {
            if (host.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = host.ToString();
                break;
            }
        }
        
        return localIP;
    }

    public static ushort GetTransportPort()
    {
        var transport = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
        return transport.ConnectionData.Port;
    }
}