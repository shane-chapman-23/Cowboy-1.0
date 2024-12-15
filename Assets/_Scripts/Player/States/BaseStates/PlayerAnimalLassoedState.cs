using UnityEngine;

public class PlayerAnimalLassoedState : PlayerState
{
    public PlayerAnimalLassoedState(Player player, PlayerFSMController stateMachineController, PlayerData playerData, string animBoolName) : base(player, stateMachineController, playerData, animBoolName)
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

        HandleMinigameLoss();
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
}
