using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyStateMachine stateMachine {  get; private set; }
    public Animator anim {  get; private set; }

    private void Awake()
    {
        stateMachine = new EnemyStateMachine();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerCurrentAnim() => stateMachine.currentState.TriggerAnim();
}
