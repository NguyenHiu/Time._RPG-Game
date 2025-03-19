using UnityEngine;

public class PlayerDeathState : PlayerState
{
    public PlayerDeathState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //player.capsuleCD.enabled = false;
        GameObject.Find("PanelCanvas").GetComponent<UI>().SwitchToDieUI();
        AudioManager.instance.PlaySFX(11, null);
    }

    public override void Exit()
    {
        AudioManager.instance.StopSFX(11);
        base.Exit();
    }

    public override void TriggerAnim()
    {
        base.TriggerAnim();
    }

    public override void Update()
    {
        base.Update();
        player.SetZeroVelocity();
    }
}
