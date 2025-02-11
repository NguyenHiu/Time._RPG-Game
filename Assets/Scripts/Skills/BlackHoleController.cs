using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Unity.VisualScripting;
using UnityEngine;

public class BlackHoleController : MonoBehaviour
{
    [SerializeField] private GameObject textPrefab;

    private CircleCollider2D cCollider;
    public float maxRadius;
    public float growthSpeed;
    public float shrinkSpeed;
    public bool isGrowing;
    public bool isShrinking;
    public float pickTime;
    public List<KeyCode> validKeys;
    public int attackTimes;

    public float pickTimer;

    [SerializeField] private List<BlackHoleHotkey> currHotkeys;

    public void Awake()
    {
        cCollider = GetComponent<CircleCollider2D>();
    }

    public void SetupBlackHole(
        Vector2 _pos, 
        float _maxRadius,
        float _growthSpeed,
        float _shrinkSpeed,
        float _pickTime,
        List<KeyCode> _validKeys,
        int _attackTimes
    )
    {
        transform.position = _pos;
        maxRadius = _maxRadius;
        growthSpeed = _growthSpeed;
        shrinkSpeed = _shrinkSpeed;
        pickTime = _pickTime;
        validKeys = new(_validKeys);
        attackTimes = _attackTimes;

        isGrowing = true;
        currHotkeys = new();
    }

    private void Update()
    {

        if (isGrowing)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxRadius+1, maxRadius+1), growthSpeed * Time.deltaTime);

            if (transform.localScale.x >= maxRadius)
            {
                isGrowing = false;
                pickTimer = pickTime;
            }
        } else
        {
            pickTimer -= Time.deltaTime;
            if (pickTimer < 0)
            {
                PlayerManager.instance.player.SetTransparent(true);
                TryAttackEnemies();
                DestroyAllHotkeys();
                isShrinking = true;
            }
        }

        if (isShrinking)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-.1f, -.1f), shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x <= .1f)
            {
                PlayerManager.instance.player.SetTransparent(false);
            }

            if (transform.localScale.x <= 0)
            {
                isShrinking = false;
                Destroy(gameObject);
            }
        }
    }

    private void DestroyAllHotkeys()
    {
        for (int i = currHotkeys.Count - 1; i >= 0; i--)
        {
            Destroy(currHotkeys[i].gameObject);
            currHotkeys.RemoveAt(i);
        }
        
    }

    private void TryAttackEnemies()
    {
        List<Enemy> targets = new();

        foreach (BlackHoleHotkey hk in currHotkeys)
        {
            if (hk.isPressed)
                targets.Add(hk.belongTo);
        }

        while (targets.Count > 0 && attackTimes > 0)
        {
            Enemy randTarget = targets[Random.Range(0, targets.Count)];
            int randValue = Random.Range(0, 1);
            Vector3 randomSide = new(-2 * randValue + 2 * (1 - randValue), 0);
            SkillManager.instance.cloneSkill.CreateClone(randTarget.transform.position + randomSide);

            attackTimes--;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out var e))
        {
            SetupHotkey(e);
            e.SetFreeze(true);
        }
    }

    private void SetupHotkey(Enemy e)
    {
        KeyCode key = validKeys[Random.Range(0, validKeys.Count)];
        validKeys.Remove(key);

        Vector3 textPos = e.transform.position + (new Vector3(0, 1.5f));
        GameObject newHotkeyText = Instantiate(textPrefab);
        BlackHoleHotkey hotkeyObject = newHotkeyText.GetComponent<BlackHoleHotkey>();
        hotkeyObject.SetHotKey(key, textPos, e);
        currHotkeys.Add(hotkeyObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out var e))
        {
            e.SetFreeze(false);
        }
    }
}
