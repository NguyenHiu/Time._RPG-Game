using UnityEngine;
using UnityEngine.UI;

/*
 * -- Throw Sword Skill --
 * 
 * This skill includes several types of throwing, such as:
 *      + A normal curve,
 *      + A bouncing sword,
 *      + A fast pierce, or
 *      + Spinning the sword to gain heavy damage on the enemy.
 *      
 */


public enum ThrowSwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
};

public class ThrowSwordSkill : Skill
{
    [Header("General")]
    [SerializeField] private bool canThrowSword = false;
    public ThrowSwordType throwType = ThrowSwordType.Regular;
    [SerializeField] private float returnSpeed = 17f;
    [SerializeField] private float destroyTime = 10f;
    [SerializeField] private float damage = 2f;
    [SerializeField] private UI_SkillSlot throwSkillSlot;

    [Header("Time Stop")]
    [SerializeField] private bool canStopTime = false;
    [SerializeField] private float freezeTime = 1.5f;
    [SerializeField] private UI_SkillSlot timeStopSkillSlot;

    [Header("Bounce info")]
    [SerializeField] private int bounceTimes = 3;
    [SerializeField] private float bounceSpeed = 20f;
    [SerializeField] private float bounceGravity = 4.5f;
    [SerializeField] private UI_SkillSlot bounceSkillSlot;

    [Header("Pierce info")]
    [SerializeField] private int pierceTimes = 10;
    [SerializeField] private float pierceGravity = 1f;
    [SerializeField] private UI_SkillSlot pierceSkillSlot;

    [Header("Spin info")]
    [SerializeField] private float spinGravity = 2f;
    [SerializeField] private float spinMaxDistance = 7f;
    [SerializeField] private float spinTimer = 1f;
    [SerializeField] private float hitCooldown = .35f;
    [SerializeField] private UI_SkillSlot spinSkillSlot;

    [Header("Skill info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce = new(35, 25);
    [SerializeField] private float defaultGravityScale = 4.5f;
    private float gravityScale;

    [Header("Aim info")]
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private GameObject aimCurve;
    [SerializeField] private int numberOfDots = 20;
    [SerializeField] private float dotsDistance = 0.07f;
    private GameObject[] dots;
    private Vector2 finalForce;

    protected override void Start()
    {
        base.Start();

        GenerateDots();
        throwSkillSlot.GetComponent<Button>().onClick.AddListener(() => UnlockThrowSkill());
        bounceSkillSlot.GetComponent<Button>().onClick.AddListener(() => UnlockBounceSkill());
        pierceSkillSlot.GetComponent<Button>().onClick.AddListener(() => UnlockPierceSkill());
        spinSkillSlot.GetComponent<Button>().onClick.AddListener(() => UnlockSpinSkill());
        timeStopSkillSlot.GetComponent<Button>().onClick.AddListener(() => UnlockTimeStop());
    }

    protected override void Update()
    {
        base.Update();

        SetupGravity();

        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < numberOfDots; i++)
                dots[i].transform.position = DotPosition(i * dotsDistance);
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            Vector2 aimDir = AimDirection().normalized;
            finalForce = aimDir * launchForce;
        }
    }

    // SetupGravity is used to setup the gravity based on the throw type
    private void SetupGravity()
    {
        switch (throwType)
        {
            case ThrowSwordType.Bounce:
                gravityScale = bounceGravity;
                break;
            case ThrowSwordType.Pierce:
                gravityScale = pierceGravity;
                break;
            case ThrowSwordType.Spin:
                gravityScale = spinGravity;
                break;
            default:
                gravityScale = defaultGravityScale;
                break;
        }
    }

    // CreateSword creates a new instance of Sword at the provided position and custom launch direction
    public void CreateSword(Vector2 _pos)
    {
        GameObject newSword = Instantiate(swordPrefab);
        ThrowSwordController ctrl = newSword.GetComponent<ThrowSwordController>();

        // If the throw type is bounce, setup the bounce info
        if (throwType == ThrowSwordType.Bounce)
            ctrl.SetupBounce(true, bounceTimes, bounceSpeed);
        else if (throwType == ThrowSwordType.Pierce)
            ctrl.SetupPierce(pierceTimes);
        else if (throwType == ThrowSwordType.Spin)
            ctrl.SetupSpin(spinMaxDistance, spinTimer, hitCooldown);

        ctrl.SetupSword(_pos, finalForce, gravityScale, returnSpeed, GetFreezeTime(), destroyTime, damage);
        PlayerManager.instance.player.AssignNewSword(newSword);
    }

    // DotsActive sets active status of the aim curve
    public void DotsActive(bool _status)
    {
        aimCurve.SetActive(_status);
    }

    // GenerateDots creates `numberOfDots` dots as childrens of the aim curve and stores them in the dots var
    private void GenerateDots()
    {
        Vector2 playerPos = PlayerManager.instance.player.transform.position;
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, playerPos, Quaternion.identity, aimCurve.transform);
        }
    }

    // AimDirection returns a vector pointing from the player to the mouse's pointer
    private Vector2 AimDirection()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = mousePos - PlayerManager.instance.player.transform.position;
        return dir;
    }

    // DotPosition returns a position of a dot based on the t param
    private Vector2 DotPosition(float t)
    {
        Vector2 playerPos = PlayerManager.instance.player.transform.position;
        Vector2 aimDirNor = AimDirection().normalized;
        Vector2 initVelocity = new Vector2(aimDirNor.x * launchForce.x, aimDirNor.y * launchForce.y);
        Vector2 res = playerPos + initVelocity * t + 0.5f * Physics2D.gravity * gravityScale * t * t;
        return res;
    }

    private float GetFreezeTime()
    {
        if (canStopTime)
            return freezeTime;
        return 0;
    }

    private void UnlockThrowSkill()
    {
        if (!throwSkillSlot.isLocked)
        {
            throwType = ThrowSwordType.Regular;
            canThrowSword = true;
        }
    }

    private void UnlockBounceSkill()
    {
        if (!bounceSkillSlot.isLocked)
            throwType = ThrowSwordType.Bounce;
    }

    private void UnlockPierceSkill()
    {
        if (!pierceSkillSlot.isLocked)
            throwType = ThrowSwordType.Pierce;
    }

    private void UnlockSpinSkill()
    {
        if (!spinSkillSlot.isLocked)
            throwType = ThrowSwordType.Spin;
    }

    private void UnlockTimeStop()
    {
        if (!timeStopSkillSlot.isLocked)
            canStopTime = true;
    }

    public bool CanThrowSword() => canThrowSword;

    // ReturnCurrentSword returns the sword back to the player
    public void ReturnCurrentSword()
    {
        // Call method to return the sword
        PlayerManager.instance.player.sword.GetComponent<ThrowSwordController>().
            ReturnToPlayer();
    }

    // ResetCooldownTimer helps to assign cooldown to the skill
    // This function is expected to be called right after the player script clears the sword
    public void ResetCooldownTimer()
    {
        cooldownTimer = cooldown;
        TriggerCooldownUpdate();
    }
}
