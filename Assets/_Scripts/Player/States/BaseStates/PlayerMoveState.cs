using UnityEngine;
using UnityEngine.Windows;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(Player player, PlayerFSMController stateMachineController, PlayerData playerData, string animBoolName) : base(player, stateMachineController, playerData, animBoolName)
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

        CheckIfShouldFlip();
        CheckForMovementInputVariations();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void CheckForMovementInputVariations()
    {
        if (player.xInput == 0)
        {
            player.ChangeState(player.IdleState);
            return;
        }

        if (player.InputHandler.GallopInput)
        {
            player.ChangeState(player.GallopState);
        }
        else
        {
            player.ChangeState(player.TrotState);
        }
    }
    public void CheckIfShouldFlip()
    {
        if (player.xInput != 0 && player.xInput != player.facingDirection)
        {
            player.Flip();
        }
    }
}
