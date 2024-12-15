using System.Runtime.CompilerServices;
using UnityEngine;

public class AnimalSlidingState : AnimalMoveState
{
    private float _decelerationRate = 5f;
    public AnimalSlidingState(Animal animal, AnimalFSMController stateMachineController, AnimalData animalData, string animBoolName) : base(animal, stateMachineController, animalData, animBoolName)
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
        if (animal.Rigidbody.linearVelocity.magnitude > 0)
        {
            animal.Rigidbody.linearVelocity *= (1 - _decelerationRate * Time.deltaTime);
        }
    }

    private void CheckIfShouldChangeToLassoedState()
    {
        if (animal.Rigidbody.linearVelocity.magnitude < 0.1f)
        {
            animal.Rigidbody.linearVelocity = Vector2.zero;
            animal.ChangeState(animal.LassoedState);
        }
    }
}
