// using Unity.Netcode;
// using UnityEngine;
//
// public struct HoldableObjectState : INetworkSerializable
// {
//     public int parentPlayerId;
//     public int parentCounterId;
//     
//     public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
//     {
//         serializer.SerializeValue(ref parentPlayerId);
//         serializer.SerializeValue(ref parentCounterId);
//     }
// }
