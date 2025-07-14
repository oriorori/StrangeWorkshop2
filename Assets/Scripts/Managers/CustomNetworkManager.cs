using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class CustomNetworkManager : Singleton<CustomNetworkManager>
{
    void Awake()
    {
        // 항상 콜백 먼저 등록
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        }
    }
    
    public void StartHost()
    {
        Debug.Log($"IP: {NetworkUtils.GetLocalIPAddress()}");
        Debug.Log($"PortNumber: {NetworkUtils.GetTransportPort()}");
        NetworkManager.Singleton.StartHost();
        Debug.Log("Host Started");
    }

    public void StartClient(string ip)
    {
        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.SetConnectionData("10.43.168.25", 7777); // 기본 포트
        NetworkManager.Singleton.StartClient();
        Debug.Log("Client Started");
    }
    
    
    private void OnClientConnected(ulong clientId)
    {
        if (NetworkManager.Singleton.IsHost && clientId != NetworkManager.Singleton.LocalClientId)
        {
            Debug.Log($"✅ [Host] Client connected: {clientId}");
        }
    }

    private void OnClientDisconnected(ulong clientId)
    {
        Debug.Log($"❌ [Host] Client disconnected: {clientId}");
    }
}
