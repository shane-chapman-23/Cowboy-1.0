using UnityEngine;

public class AnimalCaughtState : AnimalState
{
    public AnimalCaughtState(Animal animal, AnimalFSMController stateMachineController, AnimalData animalData, string animBoolName) : base(animal, stateMachineController, animalData, animBoolName)
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
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        FollowPlayer();
    }

    private void FollowPlayer()
    {
        Vector3 directionToPlayer = Player.Instance.transform.position - animal.transform.position;
        directionToPlayer.y = 0;
        float distanceToPlayer = directionToPlayer.magnitude;

        bool playerIsInFront = directionToPlayer.x > 0;
        bool playerIsWithin2f = distanceToPlayer < 2f;

        if (!playerIsInFront && playerIsWithin2f)
        {
            StopFollowing();
        }
        else
        {
            FollowPlayerInDirection(directionToPlayer);
            HandleAnimalFlip();
        }
    }

    private void StopFollowing()
    {
        animal.SetVelocity(Vector2.zero, 0f);
        animal.Anim.SetBool("idle", true);
        animal.Anim.SetBool("flee", false);
    }

    private void FollowPlayerInDirection(Vector3 directionToPlayer)
    {
        Vector2 direction = directionToPlayer.normalized;
        animal.SetVelocity(direction, 1f);
        animal.Anim.SetBool("idle", false);
        animal.Anim.SetBool("flee", true);
    }

    private void HandleAnimalFlip()
    {
        if (animal.transform.localScale.x > 0f && animal.PlayerIsRight)
        {
            animal.Flip();
        }
    }
}
