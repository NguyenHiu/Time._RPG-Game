using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
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

    public override void Update()
    {
        base.Update();

        if (!player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.fallState);
        }

        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.jumpState);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.primaryAttackState);
        }

        if (Input.GetKeyDown(KeyCode.Q) && SkillManager.instance.parrySkill.CanParry())
        {
            stateMachine.ChangeState(player.counterAttackState);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && CanThrowSword() && !player.isBusy)
        {
            stateMachine.ChangeState(player.aimSwordState);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            stateMachine.ChangeState(player.blackHoleUltimateState);
        }
    }

    // CanCreateSword returns true if player can create new sword
    //                returns false and retrieves the existing sword if it exists
    private bool CanThrowSword()
    {
        // Check if the skill is cooldown
        if (!SkillManager.instance.throwSwordSkill.CanUseSkill())
            return false;

        // Check if the throw skill has already unlocked
        if (!SkillManager.instance.throwSwordSkill.CanThrowSword())
            return false;

        // If the sword has already existed
        if (player.sword == null)
            return true;

        // Retrieve the existing sword
        SkillManager.instance.throwSwordSkill.ReturnCurrentSword();
        return false;
    }
}
