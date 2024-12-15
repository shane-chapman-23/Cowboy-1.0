using UnityEngine;

public class AnimalLassoedState : AnimalState
{
    private float _playerStrength = 20f;

    private float _pullingForce;
    private float _resistingForce;

    public AnimalLassoedState(Animal animal, AnimalFSMController stateMachineController, AnimalData animalData, string animBoolName) : base(animal, stateMachineController, animalData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        animal.Flip();

        MinigameManager.Instance.SetAnimalPositions(animal.transform.position);
        MinigameManager.Instance.SetMinigameStartedTrue();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        HandleGameWin();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        MoveAnimalWithRope();
        HandleGameLoss();
    }



    private void MoveAnimalWithRope()
    {
        CalculateMoveSpeed();
        int input = GetPlayerInput();
        int allowedMovementDirection = GetAllowedMovementDirection();

        if (animal.PlayerIsRight)
        {
            HandleMovementWhenPlayerIsOnRight(input, allowedMovementDirection);
        }
        else
        {
            HandleMovementWhenPlayerIsOnLeft(input, allowedMovementDirection);
        }
    }

    private void CalculateMoveSpeed()
    {
        float strengthRatio = Mathf.Clamp(animalData.AnimalStrength / _playerStrength, 0.0f, 1.0f);
        _pullingForce = 1f - strengthRatio;
        _resistingForce = animalData.AnimalStrength / 10f;
    }

    private int GetPlayerInput()
    {
        return Player.Instance.InputHandler.NormInputX;
    }

    private int GetAllowedMovementDirection()
    {
        return animal.PlayerIsRight ? -1 : 1;
    }

    private void HandleMovementWhenPlayerIsOnLeft(int input, int allowedMovementDirection)
    {
        if (input < 0)
        {
            animal.SetVelocity(Vector2.right * input, _pullingForce);
        }
        else
        {
            animal.SetVelocity(Vector2.right * allowedMovementDirection, _resistingForce);
        }
    }

    private void HandleMovementWhenPlayerIsOnRight(int input, int allowedMovementDirection)
    {
        if (input > 0)
        {
            animal.SetVelocity(Vector2.right * input, _pullingForce);
        }
        else
        {
            animal.SetVelocity(Vector2.right * allowedMovementDirection, _resistingForce);
        } 
    }

    private void HandleGameLoss()
    {
        if (MinigameManager.Instance.GameLost)
        {
            animal.ChangeState(animal.IdleState);
        }
    }

    private void HandleGameWin()
    {
        if (MinigameManager.Instance.GameWon)
        {
            animal.ChangeState(animal.CaughtState);
        }
    }
}
