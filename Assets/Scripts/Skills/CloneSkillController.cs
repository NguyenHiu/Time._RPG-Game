using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkillController : MonoBehaviour
{
    [SerializeField] private float losingSpeed;
    [SerializeField] private float cloneTimer;
    [SerializeField] private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;
        if (cloneTimer < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - Time.deltaTime * losingSpeed);
        }
    }

    public void SetupClone(Vector2 _pos, float _cloneDuration)
    {
        transform.position = _pos;
        cloneTimer = _cloneDuration;
    }
}
