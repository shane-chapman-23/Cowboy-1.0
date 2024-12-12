using UnityEngine;

public class PlayerIdlePoopState : PlayerIdleState
{

    public PlayerIdlePoopState(Player player, PlayerFSMController stateMachineController, PlayerData playerData, string animBoolName) : base(player, stateMachineController, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        poopAnimationPlaying = true;
    }

    public override void Exit()
    {
        base.Exit();

        poopAnimationPlaying = false;
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
