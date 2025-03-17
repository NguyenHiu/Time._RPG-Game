using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Crystal Infor")]
    private GameObject currentCrystal;
    private bool canSummonCrystal = false;
    [SerializeField] private float damage = 5;
    [SerializeField] private bool canMove = true;
    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private UI_SkillSlot crystalSkillSlot;

    [Header("Explosive Crystal")]
    [SerializeField] private bool canExplode = true;
    [SerializeField] private float growSpeed = 2.5f;
    [SerializeField] private float duration = 2f;

    [Header("Shocking")]
    [SerializeField] private bool canShock = false;
    [SerializeField] private UI_SkillSlot shockingSkillSlot;

    [Header("Swap Position Crystal")]
    [SerializeField] private bool canSwap = false;
    private bool swapped = false;
    [SerializeField] private UI_SkillSlot swapSkillSlot;

    [Header("Multi Crystals")]
    [SerializeField] private bool canUseMultiCrystals = false;
    [SerializeField] private float multiCrystalCooldown = 3f;
    [SerializeField] private int noCrystals = 3;
    List<GameObject> crystalHolders;
    [SerializeField] private UI_SkillSlot multiCrystalSkillSlot;

    protected override void Start()
    {
        base.Start();
        crystalHolders = new();
        RefillCrystal();

        crystalSkillSlot.GetComponent<Button>().onClick.AddListener(() => UnlockCrystal());
        shockingSkillSlot.GetComponent<Button>().onClick.AddListener(() => UnlockShockingCrystal());
        swapSkillSlot.GetComponent<Button>().onClick.AddListener(() => UnlockSwappingCrystal());
        multiCrystalSkillSlot.GetComponent<Button>().onClick.AddListener(() => UnlockMultiCrystal());
    }

    protected override void UseSkill()
    {
        base.UseSkill();

        TryUseSingleCrystal();
        TryUseMultiCrystals();
    }

    public override bool TryUseSkill()
    {
        bool res = base.TryUseSkill();

        // Enable Swap
        if (!canUseMultiCrystals && currentCrystal != null && canSwap && !swapped)
            cooldownTimer = 0;

        return res;
    }

    private void TryUseSingleCrystal()
    {
        if (!canUseMultiCrystals)
        {
            if (currentCrystal == null)
            {
                currentCrystal = Instantiate(crystalPrefab);
                Vector2 pos = PlayerManager.instance.player.transform.position;
                pos.y -= 0.5f;
                closestTarget = GetTheClosestEnemy(pos);
                currentCrystal.GetComponent<CrystalController>().
                    SetupCrystal(
                        pos, canExplode,
                        growSpeed, canMove,
                        moveSpeed, duration,
                        closestTarget,
                        damage, canShock
                    );

                if (canSwap) 
                    swapped = false;
            }
            else if (canSwap && !swapped)
            {
                // Mark that this crystal has already swapped
                swapped = true;
                
                // Calculate crystal position
                Vector2 crystalPos = currentCrystal.transform.position;
                crystalPos.y += 0.5f;
                Vector2 playerPos = PlayerManager.instance.player.transform.position;
                playerPos.y -= 0.5f;
                
                // Try to swap
                currentCrystal.GetComponent<CrystalController>().TrySwapCrystal(playerPos);
            }
        }
    }

    private void TryUseMultiCrystals()
    {
        if (canUseMultiCrystals)
        {
            if (crystalHolders.Count > 0)
            {
                // Create crystal continuously
                cooldown = 0;

                GameObject crystalHolder = crystalHolders[^1];
                GameObject newCrystal = Instantiate(crystalHolder, PlayerManager.instance.player.transform.position, Quaternion.identity);
                crystalHolders.Remove(crystalHolder);

                Vector2 pos = PlayerManager.instance.player.transform.position;
                pos.y -= 0.5f;
                closestTarget = GetTheClosestEnemy(pos);
                newCrystal.GetComponent<CrystalController>().
                    SetupCrystal(
                        pos, canExplode,
                        growSpeed, canMove,
                        moveSpeed, duration,
                        closestTarget,
                        damage, canShock
                    );

                // Set cooldown & Refill crystal holders 
                if (crystalHolders.Count <= 0)
                {
                    cooldown = multiCrystalCooldown;
                    RefillCrystal();
                }
            }
        }
    }

    public void RefillCrystal()
    {
        while (crystalHolders.Count < noCrystals)
            crystalHolders.Add(crystalPrefab);
    }

    private void UnlockCrystal()
    {
        if (!crystalSkillSlot.isLocked)
            canSummonCrystal = true;
    }

    private void UnlockShockingCrystal()
    {
        if (!shockingSkillSlot.isLocked)
            canShock = true;
    }

    private void UnlockSwappingCrystal()
    {
        if (!swapSkillSlot.isLocked)
            canSwap = true;
    }

    private void UnlockMultiCrystal()
    {
        if (!multiCrystalSkillSlot.isLocked)
            canUseMultiCrystals = true;
    }

    public bool CanSummonCrystal() => canSummonCrystal;
}
