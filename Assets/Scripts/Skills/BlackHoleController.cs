using System.Collections.Generic;
using UnityEngine;

public class BlackHoleController : MonoBehaviour
{
    [SerializeField] private GameObject textPrefab;

    private float maxRadius;
    private float growthSpeed;
    private float shrinkSpeed;
    private bool isGrowing;
    private bool isShrinking;
    private float pickTime;
    private List<KeyCode> validKeys;
    private int attackTimes;
    private float pickTimer;
    private List<BlackHoleHotkey> currHotkeys;


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
        // Increase the local scale when initializing Ultimate
        if (isGrowing)
        {
            PlayerManager.instance.player.anim.speed = 0.3f;

            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxRadius + 1, maxRadius + 1), growthSpeed * Time.deltaTime);

            if (transform.localScale.x >= maxRadius)
            {
                isGrowing = false;
                pickTimer = pickTime;
            }
        }
        // After the black hole achieves the maximum size, we start the timer to remain the black hole
        else
        {
            pickTimer -= Time.deltaTime;

            // If the timer is ring xD, we attack the marked enemies inside the hole
            // Then clean the scene
            if (pickTimer < 0)
            {
                TryAttackEnemies();
                DestroyAllHotkeys();
                isShrinking = true;
            }
        }

        // Decrease the size of the black hole until it disappears
        if (isShrinking)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-.1f, -.1f), shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x <= 1f)
            {
                PlayerManager.instance.player.anim.speed = 1f;
                isShrinking = false;
                Destroy(gameObject);
                PlayerManager.instance.player.SetTransparent(false);
            }
        }
    }

    // Destroy all Hotkey objects
    private void DestroyAllHotkeys()
    {
        for (int i = currHotkeys.Count - 1; i >= 0; i--)
        {
            Destroy(currHotkeys[i].gameObject);
            currHotkeys.RemoveAt(i);
        }

    }

    // TryAttackEnemies creates clones to attack the marked enemies
    private void TryAttackEnemies()
    {
        List<Enemy> targets = new();

        foreach (BlackHoleHotkey hk in currHotkeys)
        {
            if (hk.isPressed)
                targets.Add(hk.belongTo);
        }

        if (targets.Count > 0)
            PlayerManager.instance.player.SetTransparent(true);

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
            // An ememy acrosses the black hole collider will be attached to a hot key
            // The choosen hot key is randomize
            SetupHotkey(e);

            // Freeze the enemy
            e.SetFreeze(true);
        }
    }

    // SetupHotkey attaches a hotkey to the provided enemy (above)
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
