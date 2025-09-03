using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatDataSO", menuName = "Scriptable Objects/PlayerStatDataSO")]
public class PlayerStatDataSO : ScriptableObject
{
    public float speed;
    public float dashSpeed;
    public float desiredRotationSpeed = 0.1f;
}
