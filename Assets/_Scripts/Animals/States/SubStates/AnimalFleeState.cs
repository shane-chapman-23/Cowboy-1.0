using UnityEngine;

public class AnimalFleeState : AnimalMoveState
{
    private float _stopFleeingTimer = 0f;
    private float _stopFleeingDelay = 2f;

    private float _ignorePlayerTimer = 0f;
    private float _ignorePlayerDuration = 5f;

    private Vector2 _fleeDirection;

    public AnimalFleeState(Animal animal, AnimalFSMController stateMachineController, AnimalData animalData, string animBoolName) : base(animal, stateMachineController, animalData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        animal.IgnorePlayer = false;
        _ignorePlayerTimer = 0f;
        SetInitialFleeDirection();
    }

    public override void Exit()
    {
        base.Exit();
        animal.IgnorePlayer = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!animal.IgnorePlayer)
            animal.SetFacingDirection();

        UpdateFleeStatus();
        SetFleeDirection();
        UpdateIgnorePlayerStatus();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        animal.SetVelocity(_fleeDirection, animalData.Velocity);
    }

    private void SetFleeDirection()
    {
        Vector2 previousDirection = _fleeDirection;

        if (animal.transform.position.x <= GameManager.Instance.mapEdgeLeft.position.x)
        {
            animal.IgnorePlayer = true;
            _fleeDirection = Vector2.right;

        }
        else if (animal.transform.position.x >= GameManager.Instance.mapEdgeRight.position.x)
        {
            animal.IgnorePlayer = true;
            _fleeDirection = Vector2.left;
        }
        else if (!animal.IgnorePlayer)
        {
            _fleeDirection = (animal.PlayerIsRight ? Vector2.left : Vector2.right);
        }

        if (_fleeDirection != previousDirection)
        {
            animal.Flip();
        }

    }

    private void SetInitialFleeDirection()
    {
        _fleeDirection = (animal.PlayerIsRight ? Vector2.left : Vector2.right);
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

    private void UpdateIgnorePlayerStatus()
    {
        if (animal.IgnorePlayer)
        {
            _ignorePlayerTimer += Time.deltaTime;
            if (_ignorePlayerTimer >= _ignorePlayerDuration)
            {
                animal.IgnorePlayer = false;
                _ignorePlayerTimer = 0f;
            }
        }
    }
}
