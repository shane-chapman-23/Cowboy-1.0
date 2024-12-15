using UnityEngine;

public class PlayerAnimalLassoedState : PlayerState
{
    public PlayerAnimalLassoedState(Player player, PlayerFSMController stateMachineController, PlayerData playerData, string animBoolName) : base(player, stateMachineController, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.playerPositionOnAnimalLassoed = player.transform.position;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        HandleMinigameLoss();
        HandleMinigameWin();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void HandleMinigameLoss()
    {
        if (MinigameManager.Instance.GameLost)
        {
            player.ChangeState(player.IdleState);
        }
    }

    private void HandleMinigameWin()
    {
        if (MinigameManager.Instance.GameWon)
        {
            player.ChangeState(player.CaughtAnimalState);
        }
    }
}
