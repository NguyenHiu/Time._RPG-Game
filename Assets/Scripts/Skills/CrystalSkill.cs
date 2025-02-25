using UnityEngine;

/*  --Crystal Skill--
 *  
 *  When triggered, this skill creates a crystal at the player's position.
 *  If enemies touch this crystal, it will explode and cause damage to enemies inside the capsule collider.
 *  If the player triggers this skill again, they will be teleported to the position of the crystal, and
 *      the crystal will be destroyed.
 *      
 *  Note that: Multi-crystals and Single crystal have a invisible conflict in skill logic
 *              If you set canUseMultiCrystals to True, the number of crystals will depend on noCrystals variable
 *              (By default, you can always create a crystal, but if noCrystals = 0, you can't create any crystal!)
 *              
 *             Multi-crystals doesn't support swap position
 */

public class CrystalSkill : Skill
{
    [SerializeField] private GameObject crystalPrefab;
    private Transform closestTarget;

    [Header("Explosive Crystal")]
    [SerializeField] private bool canExplode;
    [SerializeField] private float growSpeed;
    [SerializeField] private float duration;

    [Header("Move Crystal")]
    [SerializeField] private bool canMove;
    [SerializeField] private float moveSpeed;

    [Header("Swap Position Crystal")]
    [SerializeField] private bool canSwap;

    [Header("Multi Crystals")]
    [SerializeField] private bool canUseMultiCrystals;
    [SerializeField] private float multiCrystalsCooldown;
    [SerializeField] private int noCrystals;
    [SerializeField] private int noCreatedCrystals;
    [SerializeField] GameObject lastCrystal;
    private float copiedCooldown;

    protected override void Start()
    {
        base.Start();
        noCreatedCrystals = 0;
        copiedCooldown = cooldown;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void UseSkill()
    {
        base.UseSkill();

        if ((!canUseMultiCrystals && !lastCrystal) || (canUseMultiCrystals && noCreatedCrystals < noCrystals))
        {
            lastCrystal = Instantiate(crystalPrefab);
            noCreatedCrystals++;
            CrystalController ctrl = lastCrystal.GetComponent<CrystalController>();
            Vector2 pos = PlayerManager.instance.player.transform.position;
            pos.y -= 0.5f;

            closestTarget = GetTheClosestEnemy(pos);

            ctrl.SetupCrystal(pos, canExplode, growSpeed, canMove, moveSpeed, duration, closestTarget);
        }
        else if (canSwap && lastCrystal && !canUseMultiCrystals)
        {
            Vector2 crystalPos = lastCrystal.transform.position;
            crystalPos.y += 0.5f;
            Vector2 playerPos = PlayerManager.instance.player.transform.position;
            playerPos.y -= 0.5f;
            lastCrystal.transform.position = playerPos;
            PlayerManager.instance.player.transform.position = crystalPos;
        }

        // This if-else allows us to enable or disable the multi-crystal while playing
        // by setting the cooldown each time using skill
        if (canUseMultiCrystals)
        {
            if (noCreatedCrystals < noCrystals)
            {
                cooldown = -.1f;
            }
            else
            {
                cooldown = multiCrystalsCooldown;
                cooldownTimer = cooldown;
                noCreatedCrystals = 0;
                lastCrystal = null;
            }
        }
        else
        {
            cooldown = copiedCooldown;
        }
    }
}
