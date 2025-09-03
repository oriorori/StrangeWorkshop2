using UnityEngine;

public class PlayerIdleState : IState
{
    public void Enter()
    {
        Debug.Log("IdleState 진입");
    }

    public void UpdateState()
    {
    }

    public void Exit()
    {
        Debug.Log("IdleState 해제");
    }
}
