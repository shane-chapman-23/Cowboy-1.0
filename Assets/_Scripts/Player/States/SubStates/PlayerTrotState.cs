using UnityEngine;

public class PlayerTrotState : PlayerMoveState
{
    public PlayerTrotState(Player player, PlayerFSMController stateMachineController, PlayerData playerData, string animBoolName) : base(player, stateMachineController, playerData, animBoolName)
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
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        player.SetVelocityX(playerData.trotVelocity * player.xInput);
    }
}
