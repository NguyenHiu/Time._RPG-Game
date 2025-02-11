using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlackHoleHotkey : MonoBehaviour
{
    public bool isPressed {  get; private set; }
    public Enemy belongTo {  get; private set; }
    private TextMeshProUGUI text;
    private KeyCode key;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetHotKey(KeyCode _key, Vector2 _pos, Enemy _belongTo)
    {
        key = _key;
        text.text = _key.ToString();
        transform.position = _pos;
        belongTo = _belongTo;
    }

    private void Update()
    {
        if (Input.GetKeyDown(key))
        {
            text.color = Color.clear;
            isPressed = true;
        }
    }
}
