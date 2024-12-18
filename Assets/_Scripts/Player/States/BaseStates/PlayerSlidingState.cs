using UnityEngine;

public class PlayerSlidingState : PlayerState
{
    private float _decelerationRate = 5f;
    public PlayerSlidingState(Player player, PlayerFSMController stateMachineController, PlayerData playerData, string animBoolName) : base(player, stateMachineController, playerData, animBoolName)
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

        CheckIfShouldChangeToLassoedState();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        Decelerate();
    }

    private void Decelerate()
    {
        if (player.Rigidbody.linearVelocity.magnitude > 0)
        {
            player.Rigidbody.linearVelocity *= (1 - _decelerationRate * Time.deltaTime);
        }
    }

    private void CheckIfShouldChangeToLassoedState()
    {
        if (player.Rigidbody.linearVelocity.magnitude < 0.1f && GameManager.Instance.endGame)
        {
            player.ChangeState(player.IdleState);
            return;
        }

        if (player.Rigidbody.linearVelocity.magnitude < 0.1f && stateMachineController.CurrentState != player.LassoedAnimalState)
        {
            player.Rigidbody.linearVelocity = Vector2.zero;
            player.ChangeState(player.LassoedAnimalState);
        }
    }
}
