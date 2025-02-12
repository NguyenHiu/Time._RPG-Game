using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleSkill : Skill
{
    [Header("Blackhole info")]
    [SerializeField] private GameObject blackHolePrefab;
    [SerializeField] private List<KeyCode> validKeys;
    [SerializeField] private float maxRadius = 10;
    [SerializeField] private float growthSpeed = 1.5f;
    [SerializeField] private float shrinkSpeed = 3f;
    [SerializeField] private float pickTime = 3f;
    [SerializeField] private int attackTimes = 4;
    private GameObject blackHoleObj;

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    protected override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackhole = Instantiate(blackHolePrefab);
        newBlackhole.GetComponent<BlackHoleController>().SetupBlackHole(
            PlayerManager.instance.player.transform.position,
            maxRadius,
            growthSpeed,
            shrinkSpeed,
            pickTime,
            validKeys,
            attackTimes
        );
        blackHoleObj = newBlackhole;
    }

    public GameObject GetBlackHoleObj()
    {
        return blackHoleObj;
    }

}
