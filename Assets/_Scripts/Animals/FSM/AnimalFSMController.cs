using UnityEngine;

public class AnimalFSMController
{
    public AnimalState CurrentState { get; private set; }

    public void Initialize(AnimalState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(AnimalState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
