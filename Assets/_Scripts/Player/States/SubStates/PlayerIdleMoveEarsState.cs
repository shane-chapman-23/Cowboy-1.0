using UnityEngine;

public class PlayerIdleMoveEarsState : PlayerIdleState
{
    public PlayerIdleMoveEarsState(Player player, PlayerFSMController stateMachineController, PlayerData playerData, string animBoolName) : base(player, stateMachineController, playerData, animBoolName)
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
