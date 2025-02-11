using UnityEngine;

public enum ThrowSwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
};

public class ThrowSwordSkill : Skill
{
    [Header("Throw Type")]
    public ThrowSwordType throwType;

    [Header("Bounce info")]
    [SerializeField] private int bounceTimes;
    [SerializeField] private float bounceGravity;

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

    // CreateSword creates a new instance of Sword at the provided position and custom launch direction
    public void CreateSword(Vector2 _pos)
    {
        // Set the intial gravity scale to the default value
        gravityScale = defaultGravityScale;

        GameObject newSword = Instantiate(swordPrefab);
        ThrowSwordController ctrl = newSword.GetComponent<ThrowSwordController>();

        // If the throw type is bounce, setup the bounce info
        if (throwType == ThrowSwordType.Bounce)
        {
            gravityScale = bounceGravity;
            ctrl.SetupBounce(true, bounceTimes);
        }

        ctrl.SetupSword(_pos, finalForce, gravityScale);
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
