using UnityEngine;

public class PlayerIdleEatState : PlayerIdleState
{
    public PlayerIdleEatState(Player player, PlayerFSMController stateMachineController, PlayerData playerData, string animBoolName) : base(player, stateMachineController, playerData, animBoolName)
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
        CheckForLassoInput();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
