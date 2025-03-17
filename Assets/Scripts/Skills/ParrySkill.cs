using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParrySkill : Skill
{
    [Header("Parry")]
    [SerializeField] private bool canParry;
    [SerializeField] private UI_SkillSlot parrySkillSlot;

    [Header("Mirage Attack")]
    [SerializeField] private bool canCreateMirage;
    [SerializeField] private UI_SkillSlot createMirageSkillSlot;

    protected override void Start()
    {
        base.Start();

        parrySkillSlot.GetComponent<Button>().onClick.AddListener(() => UnlockParry());
        createMirageSkillSlot.GetComponent<Button>().onClick.AddListener(() => UnlockCreateMirage());
    }

    public bool CanParry() => canParry;
    public void CreateMirageAttack(Vector2 pos)
    {
        if (canCreateMirage)
            SkillManager.instance.cloneSkill.CreateMirageAttack(pos);
    }

    private void UnlockParry()
    {
        if (!parrySkillSlot.isLocked)
            canParry = true;
    }

    private void UnlockCreateMirage()
    {
        if (!createMirageSkillSlot.isLocked)
            canCreateMirage = true;
    }
}
