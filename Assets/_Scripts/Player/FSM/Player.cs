using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    #region States
    //Controller
    public PlayerFSMController StateMachineController {  get; private set; }
    //Idle States
    public PlayerIdleState IdleState { get; private set; }
    public PlayerIdleMoveTailState IdleMoveTailState { get; private set; }
    public PlayerIdleMoveEarsState IdleMoveEarsState { get; private set; }
    public PlayerIdleTurnState IdleTurnState { get; private set; }
    public PlayerIdlePoopState IdlePoopState { get; private set; }
    public PlayerIdleEatState IdleEatState { get; private set; }
    //Move States
    public PlayerTrotState TrotState { get; private set; }
    public PlayerGallopState GallopState { get; private set; }
    //Lassoed States
    public PlayerSlidingState SlidingState { get; private set; }
    public PlayerAnimalLassoedState LassoedAnimalState { get; private set; }
    public PlayerAnimalCaughtState CaughtAnimalState { get; private set; }
    #endregion

    #region Components
    //Local
    public Rigidbody2D Rigidbody { get; private set; }
    public Animator Anim {  get; private set; }
    public InputHandler InputHandler { get; private set; }
    public PlayerInput InputComponent { get; private set; }
    public AudioManager audioManager;

    //external
    public Transform endGamePos;
    public GameObject poop;
    public Poop poopScript;
    #endregion

    #region Player Data
    [SerializeField]
    private PlayerData _playerData;
    #endregion

    #region Other
    private Vector2 _workSpace;
    private Vector2 _currentVelocity;

    public Vector3 playerPositionOnAnimalLassoed;

    [HideInInspector]
    public int facingDirection = 1;
    public int xInput {  get; private set; }
    #endregion

    #region Unity Callback Functions
    private void Awake()
    {
        Instance = this;

        StateMachineController = new PlayerFSMController();

        IdleState = new PlayerIdleState(this, StateMachineController, _playerData, "idle");
        IdleMoveTailState = new PlayerIdleMoveTailState(this, StateMachineController, _playerData, "idleTail");
        IdleMoveEarsState = new PlayerIdleMoveEarsState(this, StateMachineController, _playerData, "idleEars");
        IdleTurnState = new PlayerIdleTurnState(this, StateMachineController, _playerData, "idleTurn");
        IdlePoopState = new PlayerIdlePoopState(this, StateMachineController, _playerData, "idlePoop");
        IdleEatState = new PlayerIdleEatState(this, StateMachineController, _playerData, "idleEat");

        TrotState = new PlayerTrotState(this, StateMachineController, _playerData, "trot");
        GallopState = new PlayerGallopState(this, StateMachineController, _playerData, "gallop");

        SlidingState = new PlayerSlidingState(this, StateMachineController, _playerData, "sliding");
        LassoedAnimalState = new PlayerAnimalLassoedState(this, StateMachineController, _playerData, "lassoed");
        CaughtAnimalState = new PlayerAnimalCaughtState(this, StateMachineController, _playerData, "trot");

        InputHandler = GetComponent<InputHandler>();
        Rigidbody = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        InputComponent = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        StateMachineController.Initialize(IdleState);
    }

    private void Update()
    {
        StateMachineController.CurrentState.LogicUpdate();

        xInput = InputHandler.NormInputX;

        HandleSlidingStateChange();
        HandleEndGameSlidingStateChange();
    }

    private void FixedUpdate()
    {
        StateMachineController.CurrentState.PhysicsUpdate();
    }
    #endregion

    public void ChangeState(PlayerState state)
    {
        StateMachineController.ChangeState(state);
    }

    private void HandleSlidingStateChange()
    {
        bool slidingStateConditions = LassoController.Instance.AnimalLassoed
            && StateMachineController.CurrentState != SlidingState
            && StateMachineController.CurrentState != LassoedAnimalState
            && StateMachineController.CurrentState != CaughtAnimalState;

        if (slidingStateConditions)
        {
            ChangeState(SlidingState);
        }
    }

    public void SetVelocityX(float velocity)
    {
        _workSpace.Set(velocity, _currentVelocity.y);
        Rigidbody.linearVelocity = _workSpace;
        _currentVelocity = _workSpace;
    }

    public void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    public void SpawnPoop()
    {
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y, -1f);
        GameObject poopInstance = Instantiate(poop, spawnPosition, transform.rotation);
    }

    public void DisableInput()
    {
        InputComponent.enabled = false;
    }

    public void EnableInput()
    {
        InputComponent.enabled = true;
    }

    private void HandleEndGameSlidingStateChange()
    {
        bool conditionsMet = StateMachineController.CurrentState == GallopState && transform.position.x >= endGamePos.position.x && GameManager.Instance.animalsCaught == 5;
        if (conditionsMet)
        {
            ChangeState(SlidingState);
        }
    }
}
