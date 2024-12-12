using UnityEngine;

public class PlayerState
{
    protected Player player;
    protected PlayerFSMController stateMachineController;
    protected PlayerData playerData;

    protected float startTime;

    public string animBoolName;

    public PlayerState(Player player, PlayerFSMController stateMachineController, PlayerData playerData, string animBoolName)
    {
        this.player = player;
        this.stateMachineController = stateMachineController;
        this.playerData = playerData;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        player.Anim.SetBool(animBoolName, true);
        startTime = Time.time;
    }

    public virtual void Exit()
    {
        player.Anim.SetBool(animBoolName, false);
    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {

    }
}
