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
    
    public GameObject CreateBlackHole(Vector2 _pos)
    {
        GameObject newBlackhole = Instantiate(blackHolePrefab);
        newBlackhole.GetComponent<BlackHoleController>().SetupBlackHole(
            _pos, 
            maxRadius, 
            growthSpeed, 
            shrinkSpeed, 
            pickTime,
            validKeys,
            attackTimes
        );
        return newBlackhole;
    }

}
