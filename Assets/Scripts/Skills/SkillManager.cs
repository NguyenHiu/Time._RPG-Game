using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    public DashSkill dashSkill { get; private set; }
    public CloneSkill cloneSkill { get; private set; }
    public ThrowSwordSkill throwSwordSkill { get; private set; }
    public BlackHoleSkill blackholeSkill { get; private set; }
    public CrystalSkill crystalSkill { get; private set; }

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(instance.gameObject);

        dashSkill = GetComponent<DashSkill>();
        cloneSkill = GetComponent<CloneSkill>();
        throwSwordSkill = GetComponent<ThrowSwordSkill>();
        blackholeSkill = GetComponent<BlackHoleSkill>();
        crystalSkill = GetComponent<CrystalSkill>();
    }
}
