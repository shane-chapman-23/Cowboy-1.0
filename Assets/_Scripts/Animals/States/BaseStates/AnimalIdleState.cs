using UnityEngine;

public class AnimalIdleState : AnimalState
{

    public AnimalIdleState(Animal animal, AnimalFSMController stateMachineController, AnimalData animalData, string animBoolName) : base(animal, stateMachineController, animalData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        animal.SetVelocity(Vector2.zero, 0f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        ChangeToFleeStateOnPlayerDetection();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void ChangeToFleeStateOnPlayerDetection()
    {
        if (animal.PlayerDetected)
        {
            animal.ChangeState(animal.FleeState);
        }
    }
}
