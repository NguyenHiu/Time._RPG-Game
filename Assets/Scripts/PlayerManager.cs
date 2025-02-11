using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public Player player;

    public void Awake()
    {
        if (instance == null) 
            instance = this;
        else Destroy(instance.gameObject);
    }
}
