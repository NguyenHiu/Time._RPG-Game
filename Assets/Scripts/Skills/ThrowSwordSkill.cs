using UnityEngine;

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
    public ThrowSwordType throwType;
    [SerializeField] private float returnSpeed;
    [SerializeField] private float freezeTime;
    [SerializeField] private float destroyTime;
    [SerializeField] private float damage;

    [Header("Bounce info")]
    [SerializeField] private int bounceTimes;
    [SerializeField] private float bounceSpeed;
    [SerializeField] private float bounceGravity;

    [Header("Pierce info")]
    [SerializeField] private int pierceTimes;
    [SerializeField] private float pierceGravity;

    [Header("Spin info")]
    [SerializeField] private float spinGravity = 2;
    [SerializeField] private float spinMaxDistance = 7;
    [SerializeField] private float spinTimer = 1f;
    [SerializeField] private float hitCooldown = .35f;

    [Header("Skill info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float defaultGravityScale;
    private float gravityScale;


    [Header("Aim info")]
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private GameObject aimCurve;
    [SerializeField] private int numberOfDots;
    [SerializeField] private float dotsDistance;
    private GameObject[] dots;
    private Vector2 finalForce;

    protected override void Start()
    {
        base.Start();

        GenerateDots();
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

        ctrl.SetupSword(_pos, finalForce, gravityScale, returnSpeed, freezeTime, destroyTime, damage);
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
}
