using System;
using FishNet.Object;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public abstract class HoldableObject : NetworkBehaviour, IObserver<InteractInfo>
{

    public void Awake()
    {
    }
    
    public void OnNext(InteractInfo interactInfo)
    {
        if (interactInfo.holdableObject == this)
        {
        }
    }

    public void OnError(Exception error)
    {
        Debug.LogError(error);
    }

    public void OnCompleted()
    {
    }

    [ServerRpc]
    private void HoldServerRpc()
    {
        // transform.SetParent(holdableObjectTransform.parentTransform);
        // transform.localPosition = Vector3.zero;
    }
    
    // [ServerRpc]
    // public void RequestPickUpServerRpc(ulong clientId)
    // {
    //     // 서버에서 확인
    //     if (heldByClientId.Value == 0)
    //     {
    //         heldByClientId.Value = clientId; // 들었다고만 상태 변경
    //     }
    // }
    //
    // public override void OnNetworkSpawn()
    // {
    //     heldByClientId.OnValueChanged += (oldValue, newValue) =>
    //     {
    //         if (newValue == NetworkManager.Singleton.LocalClientId)
    //         {
    //             // 내가 들었음 → 로컬에서 위치 처리
    //             transform.SetParent(PlayerLocal.Instance.handTransform);
    //             transform.localPosition = Vector3.zero;
    //         }
    //         else if (oldValue == NetworkManager.Singleton.LocalClientId)
    //         {
    //             // 내가 놓았음
    //             transform.SetParent(null);
    //         }
    //     };
    // }
}
