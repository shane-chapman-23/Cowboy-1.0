using UnityEngine;

public class PlayerAnimalCaughtState : PlayerState
{
    public PlayerAnimalCaughtState(Player player, PlayerFSMController stateMachineController, PlayerData playerData, string animBoolName) : base(player, stateMachineController, playerData, animBoolName)
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
        HandlePlayerFlipOnCaught();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        player.SetVelocityX(playerData.trotVelocity * -1);
    }

    private void HandlePlayerFlipOnCaught()
    {
        if (player.facingDirection == 1)
        {
            player.Flip();
        }
    }
}
