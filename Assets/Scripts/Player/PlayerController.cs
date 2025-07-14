using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    #region Components
    [SerializeField] private PlayerInput _playerInput;
    #endregion
    
    private InputHandler _inputHandler;

    void Awake()
    {
    }
    
    void Update()
    {
        if (!IsOwner) return;
        transform.position += _inputHandler.Movement;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
        var transport = NetworkManager.Singleton.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>();

        if (!IsOwner)
        {
            Debug.Log($"[Client] Connecting to: {transport.ConnectionData.Address}:{transport.ConnectionData.Port}");
        }
        else
        {
            _inputHandler = new InputHandler(_playerInput);

            Debug.Log($"[Host] Listening on: {transport.ConnectionData.Address}:{transport.ConnectionData.Port}");
        }
    }
}
