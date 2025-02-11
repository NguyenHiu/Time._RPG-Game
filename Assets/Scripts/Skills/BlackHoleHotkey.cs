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

    // SetHotKey prepares the hotkey object
    public void SetHotKey(KeyCode _key, Vector2 _pos, Enemy _belongTo)
    {
        key = _key;
        text.text = _key.ToString();
        transform.position = _pos;
        belongTo = _belongTo;
    }

    private void Update()
    {
        // If player presses the correct key, we will se the `isPressed` to true
        // This variable will be used to chose enemies to be attacked after the pick time of the black hole
        if (Input.GetKeyDown(key))
        {
            text.color = Color.clear;
            isPressed = true;
        }
    }
}
