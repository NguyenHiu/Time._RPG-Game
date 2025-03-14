using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrikeController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    { 
        if (collision.TryGetComponent<Enemy>(out var enemy))
            PlayerManager.instance.player.statCtrl.DoLightningDamage(enemy.statCtrl);
    }
}
