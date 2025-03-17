using System.Collections;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;
    private Material originMat;
    private bool isFlashing = false;

    [Header("Flash FX")]
    [SerializeField] private Material flashMat;

    [Header("Burn FX")]
    [SerializeField] private Color[] burningColors;

    [Header("Freezing FX")]
    [SerializeField] private Color[] freezingColors;

    [Header("Shocking FX")]
    [SerializeField] private Color[] shockingColors;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originMat = sr.material;
    }

    public IEnumerator Flash()
    {
        sr.material = flashMat;
        Color currColor = sr.color;
        isFlashing = true;
        sr.color = Color.white;
        yield return new WaitForSeconds(.2f);
        sr.color = currColor;
        isFlashing = false;
        sr.material = originMat;
    }

    public virtual void RedBlink()
    {
        if (sr.color != Color.white)
            sr.color = Color.white;
        else sr.color = Color.red;
    }

    public void CancelColorChange()
    {
        CancelInvoke();
        sr.color = Color.white;
    }

    public void BurningFxFor(float _seconds)
    {
        InvokeRepeating(nameof(BurningColorFx), 0, .3f);
        Invoke(nameof(CancelColorChange), _seconds);
    }

    private void BurningColorFx()
    {
        if (isFlashing) return;
        if (sr.color != burningColors[0])
            sr.color = burningColors[0];
        else sr.color = burningColors[1];
    }

    public void ShockingFxFor(float _seconds)
    {
        InvokeRepeating(nameof(ShockingColorFx), 0, .3f);
        Invoke(nameof(CancelColorChange), _seconds);
    }

    private void ShockingColorFx()
    {
        if (isFlashing) return;
        if (sr.color != shockingColors[0])
            sr.color = shockingColors[0];
        else sr.color = shockingColors[1];
    }

    public void FreezingFxFor(float _seconds)
    {
        InvokeRepeating(nameof(FreezingColorFx), 0, .3f);
        Invoke(nameof(CancelColorChange), _seconds);
    }

    private void FreezingColorFx()
    {
        if (isFlashing) return;
        if (sr.color != freezingColors[0])
            sr.color = freezingColors[0];
        else sr.color = freezingColors[1];
    }
}
