using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

public class NetworkGameManager : Singleton<NetworkGameManager>
{
    [HideInInspector] public Dictionary<int, PlayerController> players = new Dictionary<int, PlayerController>();

    public NetworkObject playerPrefab;

    public ulong ownerPlayerId;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    [Server]
    public void SpawnPlayer(NetworkConnection conn)
    {
        var nob = Instantiate(playerPrefab);
        Spawn(nob, conn);
    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestSpawnPlayerServerRpc()
    {
        Debug.Log("request");
        SpawnPlayer(InstanceFinder.ClientManager.Connection);
    }
}
