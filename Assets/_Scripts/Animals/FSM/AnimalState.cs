using UnityEngine;

public class AnimalState
{
    protected Animal animal;
    protected AnimalFSMController stateMachineController;
    protected AnimalData animalData;

    protected float startTime;

    private string _animBoolName;

    public AnimalState(Animal animal, AnimalFSMController stateMachineController, AnimalData animalData, string animBoolName)
    {
        this.animal = animal;
        this.stateMachineController = stateMachineController;
        this.animalData = animalData;
        this._animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        animal.Anim.SetBool(_animBoolName, true);
        startTime = Time.time;
    }

    public virtual void Exit()
    {
        animal.Anim.SetBool(_animBoolName, false);
    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {

    }
}
