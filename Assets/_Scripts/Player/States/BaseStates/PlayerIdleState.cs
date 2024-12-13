using UnityEngine;
using System.Collections.Generic;

public class PlayerIdleState : PlayerState
{
    private float _timeToNextVariation;
    private float _variationChangeInterval = 5f;

    private Dictionary<IdleStateVariation, float> _stateWeights;

    public bool poopAnimationPlaying;

    public PlayerIdleState(Player player, PlayerFSMController stateMachineController, PlayerData playerData, string animBoolName) : base(player, stateMachineController, playerData, animBoolName)
    {
        InitializeStateWeights();
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocityX(0f);

        _timeToNextVariation = _variationChangeInterval;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CheckForMovementInput();
    
        HandleIdleVariationChanges();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    #region Check Functions
    private void CheckForMovementInput()
    {
        if (player.xInput != 0 && !poopAnimationPlaying)
        {
            player.ChangeState(player.TrotState);
        }
    }

    protected void CheckForAnimationFinish()
    {
        if (player.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !player.Anim.IsInTransition(0))
        {
            player.ChangeState(player.IdleState);
        }
    }

    protected void CheckForLassoInput()
    {
        if (player.InputHandler.LassoInputDown)
        {
            player.ChangeState(player.IdleState);
        }
    }
    #endregion

    #region Idle Variation
    private void HandleIdleVariationChanges()
    {
        _timeToNextVariation -= Time.deltaTime;
        if (_timeToNextVariation <= 0f && !player.InputHandler.LassoInputDown)
        {
            ChangeToRandomIdleVariation();
            _timeToNextVariation = _variationChangeInterval;
        }
    }

    private void InitializeStateWeights()
    {
        _stateWeights = new Dictionary<IdleStateVariation, float>
        {
            { IdleStateVariation.IdleMoveEars, 0.6f },
            { IdleStateVariation.IdleMoveTail, 0.6f },
            { IdleStateVariation.IdleTurn, 0.3f },
            { IdleStateVariation.IdlePoop, 0.2f },
            { IdleStateVariation.IdleEat, 0.2f }
        };
    }

    private void ChangeToRandomIdleVariation()
    {
        IdleStateVariation selectedState = GetRandomWeightedState();
        switch (selectedState)
        {
            case IdleStateVariation.IdleMoveEars:
                player.ChangeState(player.IdleMoveEarsState);
                break;
            case IdleStateVariation.IdleMoveTail:
                player.ChangeState(player.IdleMoveTailState);
                break;
            case IdleStateVariation.IdleTurn:
                player.ChangeState(player.IdleTurnState);
                break;
            case IdleStateVariation.IdlePoop:
                player.ChangeState(player.IdlePoopState);
                break;
            case IdleStateVariation.IdleEat:
                player.ChangeState(player.IdleEatState);
                break;
        }
    }

    private IdleStateVariation GetRandomWeightedState()
    {
        float totalWeight = 0f;
        foreach (float weight in _stateWeights.Values)
        {
            totalWeight += weight;
        }

        float randomValue = Random.Range(0f, totalWeight);
        float cumulativeWeight = 0f;

        foreach (var state in _stateWeights)
        {
            cumulativeWeight += state.Value;
            if (randomValue < cumulativeWeight)
            {
                return state.Key;
            }
        }

        return IdleStateVariation.IdleMoveTail;
    }

    #endregion

}

public enum IdleStateVariation
{
    IdleMoveEars,
    IdleMoveTail,
    IdleTurn,
    IdlePoop,
    IdleEat
}
