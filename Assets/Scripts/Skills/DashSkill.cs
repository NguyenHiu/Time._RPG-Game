using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashSkill : Skill
{
    protected override void Update()
    {
        base.Update();

    }

    protected override void UseSkill()
    {
        base.UseSkill();

        Debug.Log("Created cloned behind");
    }
}
