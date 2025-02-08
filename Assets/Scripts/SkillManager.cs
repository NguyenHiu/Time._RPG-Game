using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    public void Awake()
    {
        if (instance == null) 
            instance = this;
        else Destroy(instance.gameObject);
    }
}
