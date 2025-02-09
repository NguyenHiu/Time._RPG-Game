using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    public DashSkill dashSkill { get; private set; }
    public CloneSkill cloneSkill { get; private set; }

    public void Awake()
    {
        if (instance == null) 
            instance = this;
        else Destroy(instance.gameObject);

        dashSkill = GetComponent<DashSkill>();
        cloneSkill = GetComponent<CloneSkill>();
    }
}
