using UnityEngine;

public class Animal : MonoBehaviour
{
    public AnimalFSMController StateMachineController {  get; private set; }
    public AnimalIdleState IdleState { get; private set; }
    public AnimalFleeState FleeState { get; private set; }
    public AnimalSlidingState SlidingState { get; private set; }
    public AnimalLassoedState LassoedState { get; private set; }

    public Animator Anim {  get; private set; }
    public Rigidbody2D Rigidbody { get; private set; }


    public AnimalData animalData;

    public Vector2 DirectionToPlayer { get; private set; }
    public bool PlayerIsRight { get; private set; }
    public bool PlayerDetected { get; private set; }

    public int FacingDirection { get; private set; } = 1;


    private void Awake()
    {
        StateMachineController = new AnimalFSMController();

        IdleState = new AnimalIdleState(this, StateMachineController, animalData, "idle");
        FleeState = new AnimalFleeState(this, StateMachineController, animalData, "flee");
        SlidingState = new AnimalSlidingState(this, StateMachineController, animalData, "sliding");
        LassoedState = new AnimalLassoedState(this, StateMachineController, animalData, "lassoed");

        Anim = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StateMachineController.Initialize(IdleState);

        FacingDirection = 1;
    }

    private void Update()
    {
        StateMachineController.CurrentState.LogicUpdate();

        CheckDirectionToPlayer();
        CheckPlayerDirection();
        CheckForPlayer();
    }

    private void FixedUpdate()
    {
        StateMachineController.CurrentState.PhysicsUpdate();
    }

    public void ChangeState(AnimalState state)
    {
        StateMachineController.ChangeState(state);
    }

    public void SetVelocity(Vector2 direction, float velocity)
    {
        Rigidbody.linearVelocity = direction * velocity;
    }

    public void SetFacingDirection()
    {
        if ((PlayerIsRight && FacingDirection > 0) || (!PlayerIsRight && FacingDirection < 0))
        {
            Flip();
        }
    }

    public void Flip()
    {
        FacingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }


    #region Player Check Functions
    private void CheckForPlayer()
    {
        float distanceToPlayer = DistanceToPlayer();

        if (IsPlayerWithinDetectionRadius(distanceToPlayer))
        {
            PlayerDetected = true;
        }
        else
        {
            PlayerDetected = false;
        }
    }

    private void CheckPlayerDirection()
    {
        if (DirectionToPlayer.x > 0)
        {
            PlayerIsRight = true;
        }
        else
        {
            PlayerIsRight = false;
        }
    }
    
    private void CheckDirectionToPlayer()
    {
        DirectionToPlayer = Player.Instance.transform.position - transform.position;
    }

    private float DistanceToPlayer()
    {
        return DirectionToPlayer.magnitude;
    }

    private bool IsPlayerWithinDetectionRadius(float distance)
    {
        if (distance <= animalData.playerDetectionRadius)
            return true;

        return false;
    }
    #endregion
}


