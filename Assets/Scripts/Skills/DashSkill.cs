using UnityEngine;
using UnityEngine.UI;

public class DashSkill : Skill
{
    [Header("Dash")]
    [SerializeField] private bool canDash = false;
    [SerializeField] private UI_SkillSlot dashSkillSlot;

    [Header("Leave Mirage")]
    [SerializeField] private bool canLeaveMirage = false;
    [SerializeField] private UI_SkillSlot leaveMirageSkillSlot;

    protected override void Update()
    {
        base.Update();
    }

    protected override void Start()
    {
        base.Start();
        dashSkillSlot.GetComponent<Button>().onClick.AddListener(() => UnlockDash());
        leaveMirageSkillSlot.GetComponent<Button>().onClick.AddListener(() => UnlockLeaveMirage());
    }

    protected override void UseSkill()
    {
        base.UseSkill();
    }

    private void UnlockDash()
    {
        Debug.Log("Attemp to unlock dash");
        if (!dashSkillSlot.isLocked)
            canDash = true;
    }

    protected override void CheckLockStatus()
    {
        base.CheckLockStatus();

        UnlockDash();
        UnlockLeaveMirage();
    }

    private void UnlockLeaveMirage()
    {
        if (!leaveMirageSkillSlot.isLocked)
            canLeaveMirage = true;
    }

    public void LeaveMirage(Vector2 pos)
    {
        if (canLeaveMirage)
            SkillManager.instance.cloneSkill.CreateClone(pos);
    }

    public bool CanDash() => canDash;
}
