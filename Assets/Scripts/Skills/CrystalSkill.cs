using System.Collections.Generic;
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

    [Header("Crystal Infor")]
    [SerializeField] private float damage;
    [SerializeField] private bool canShock;
    [SerializeField] private GameObject currentCrystal;

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
    [SerializeField] private float multiCrystalCooldown;
    [SerializeField] private int noCrystals;
    [SerializeField] List<GameObject> crystalHolders;

    protected override void Start()
    {
        base.Start();
        crystalHolders = new();
        RefillCrystal();
    }

    protected override void UseSkill()
    {
        base.UseSkill();

        TryUseSingleCrystal();
        TryUseMultiCrystals();
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
            }
            else if (canSwap)
            {
                Vector2 crystalPos = currentCrystal.transform.position;
                crystalPos.y += 0.5f;
                Vector2 playerPos = PlayerManager.instance.player.transform.position;
                playerPos.y -= 0.5f;
                currentCrystal.transform.position = playerPos;
                PlayerManager.instance.player.transform.position = crystalPos;
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
}
