using UnityEngine;

public class PlayerBlackHoleUltimateState : PlayerState
{
    private float enterGravity;
    private bool isCreated;
    private GameObject blackhole;

    public PlayerBlackHoleUltimateState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        base.Enter();


        enterGravity = player.rb.gravityScale;
        player.rb.velocity = new Vector2(0, 15);

        player.rb.gravityScale = 0;
        isCreated = false;
        stateTimer = .2f;
        blackhole = null;
    }

    public override void Exit()
    {
        base.Exit();
        player.rb.gravityScale = enterGravity;
        player.SetZeroVelocity();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0 && !isCreated)
        {
            player.rb.velocity = new Vector2(0, -0.1f);
            isCreated = true;
            SkillManager.instance.blackholeSkill.TryUseSkill();
            blackhole = SkillManager.instance.blackholeSkill.GetBlackHoleObj();
        }

        if (isCreated && blackhole == null)
        {
            stateMachine.ChangeState(player.fallState);
        }
    }
}
