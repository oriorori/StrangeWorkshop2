using System.Collections.Generic;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class InteractInfo
{
    public Transform parentTransform;
    public HoldableObject holdableObject;
}

[RequireComponent(typeof(CapsuleCollider), typeof(PlayerInput))]
public class PlayerController : NetworkBehaviour, IObservable<InteractInfo>
{
    #region Components
    [SerializeField] private PlayerInput _playerInput;
    #endregion
    
    private InputHandler _inputHandler;
    private StateMachine _playerStateMachine;

    private PlayerIdleState _idleState;

    private bool _isHolding;
    
    private List<IObserver<InteractInfo>> _holdableObjectObservers = new List<IObserver<InteractInfo>>();

    public override void OnStartClient()
    {
        Initialize();
        if (IsOwner)
        {
            InitializeOnOwner();
        }
    }
    
    void Update()
    {
        if (!IsOwner) return;
        transform.position += _inputHandler.Movement;
        _playerStateMachine.Update();
    }

    // public override void OnNetworkSpawn()
    // {
    //     base.OnNetworkSpawn();
    //     
    //     var transport = NetworkManager.Singleton.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>();
    //     
    //     if (!IsOwner)
    //     {
    //         Debug.Log($"[Client] Connecting to: {transport.ConnectionData.Address}:{transport.ConnectionData.Port}");
    //     }
    //     else
    //     {
    //
    //         Debug.Log($"[Host] Listening on: {transport.ConnectionData.Address}:{transport.ConnectionData.Port}");
    //     }
    // }

    #region init
    private void Initialize()
    {
        // Owner 관계없이 필요한 초기화들
        _playerStateMachine = new StateMachine();
        _idleState = new PlayerIdleState();
        _playerStateMachine.ChangeState(_idleState);
    }

    private void InitializeOnOwner()
    {
        // owner일 때만 필요한 초기화
        _inputHandler = new InputHandler(_playerInput);

        _inputHandler.eventPressedInteractButton += OnPressedInteract;
    }
    #endregion

    private void OnPressedInteract()
    {
        if (!Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 10f))
        {
            Debug.Log("충돌 없음");
            return;
        }

        // 이미 뭔가 들고 있을때
        if (_isHolding)
        {
            Debug.Log("손에 이미 있습니다");
        }

        // 손에 뭐가 없을 때
        else
        {
            Debug.Log("손에 없음");
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("HoldableObject"))
            {
                Debug.Log("holdableobject 확인");
                Subscribe(hit.collider.gameObject.GetComponent<HoldableObject>());
                Notify(new InteractInfo()
                {
                    parentTransform = transform,
                    holdableObject = hit.collider.gameObject.GetComponent<HoldableObject>()
                });
            }
        }
    }
    
    
    public void Subscribe(IObserver<InteractInfo> observer)
    {
        if (_holdableObjectObservers.Contains(observer)) return;
        _holdableObjectObservers.Add(observer);
    }

    public void Unsubscribe(IObserver<InteractInfo> observer)
    {
        if (_holdableObjectObservers.Contains(observer)) return;
        _holdableObjectObservers.Remove(observer);
    }

    public void Notify(InteractInfo value)
    {
        Debug.Log("Notified");
        _holdableObjectObservers.ForEach(o => o.OnNext(value));
    }
    
    #region debug

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 10f);
    }
    #endregion
}
