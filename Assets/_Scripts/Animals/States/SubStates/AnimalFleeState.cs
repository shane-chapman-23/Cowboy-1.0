using UnityEngine;

public class AnimalFleeState : AnimalMoveState
{
    private float _stopFleeingTimer = 0f;
    private float _stopFleeingDelay = 2f;

    public AnimalFleeState(Animal animal, AnimalFSMController stateMachineController, AnimalData animalData, string animBoolName) : base(animal, stateMachineController, animalData, animBoolName)
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

        animal.SetFacingDirection();
        UpdateFleeStatus();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        animal.SetVelocity(FleeDirection(), animalData.Velocity);
    }

    private Vector2 FleeDirection()
    {
        return (animal.PlayerIsRight ? Vector2.left : Vector2.right);
    }

    private void UpdateFleeStatus()
    {
        StartFleeTimer();
        StopFleeing();
    }



    private void StartFleeTimer()
    {
        if (animal.PlayerDetected)
        {
            _stopFleeingTimer = 0f;
            return;
        }

        _stopFleeingTimer += Time.deltaTime;
    }

    private void StopFleeing()
    {
        if (_stopFleeingTimer >= _stopFleeingDelay)
        {
            animal.ChangeState(animal.IdleState);
        }
    }
}
