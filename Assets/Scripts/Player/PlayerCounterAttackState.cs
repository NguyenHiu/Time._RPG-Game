using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = player.counterAttackDuration;
        player.anim.SetBool("SuccessfulCounterAttack", false);
        player.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackRadius);
        foreach (var obj in colliders)
        {
            Enemy e = obj.GetComponent<Enemy>();
            if (e != null && e.CanBeStunned())
            {
                e.BeCounter();
                stateTimer = 10;
                player.anim.SetBool("SuccessfulCounterAttack", true);
                Vector2 clonePos = player.transform.position + new Vector3(player.facingDir * 2.5f, 0f);
                SkillManager.instance.parrySkill.CreateMirageAttack(clonePos);
            }
        }

        if (stateTimer < 0 || triggeredAnim)
        {
            stateMachine.ChangeState(player.idleState);
        }

    }
}
