using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState 
{
    public PlayerStateMachine stateMachine { get; private set; }
    public Player player { get; private set; }
    public Rigidbody2D rb { get; private set; }

    protected string animName;
    protected float xInput;
    protected float yInput;
    protected float stateTimer;
    protected bool triggeredAnim;

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animName)
    {
        player = _player;
        stateMachine = _stateMachine;
        animName = _animName;
    }

    public virtual void Enter()
    {
        //Debug.Log("Enter state: " + animName);
        player.anim.SetBool(animName, true);
        rb = player.rb;
        triggeredAnim = false;
    }

    public virtual void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        player.anim.SetFloat("yVelocity", rb.velocity.y);
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        //Debug.Log("Exist state: " + animName);
        player.anim.SetBool(animName, false);
    }

    public void TriggerAnim()
    {
        triggeredAnim = true;
    }
}
