using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private bool canAttack;

    public void CreateClone(Vector2 clonePos)
    {
        GameObject newClone = Instantiate(clonePrefab);
        CloneSkillController controller = newClone.GetComponent<CloneSkillController>();
        controller.SetupClone(clonePos, cloneDuration, canAttack);
    }
}
