using UnityEngine;

public class PlayerIdleTurnState : PlayerIdleState
{
    public PlayerIdleTurnState(Player player, PlayerFSMController stateMachineController, PlayerData playerData, string animBoolName) : base(player, stateMachineController, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CheckForAnimationFinish();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
