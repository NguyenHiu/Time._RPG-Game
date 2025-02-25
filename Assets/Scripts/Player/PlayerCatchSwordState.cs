using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    public PlayerCatchSwordState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Vector2 swordPos = player.sword.transform.position;
        Vector2 playerPos = player.transform.position;
        if (swordPos.x * player.facingDir < playerPos.x * player.facingDir)
            player.Flip();

        // I use `facingDir` here because we have already flipped the facing direction towards the return direction
        // of the sword
        player.rb.velocity = new Vector2(-1 * player.swordReturnedImpact * player.facingDir, player.rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", .1f);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void TriggerAnim()
    {
        base.TriggerAnim();

        stateMachine.ChangeState(player.idleState);
    }
}
