using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    [Header("Flash FX")]
    [SerializeField] private Material flashMat;
    private SpriteRenderer sr;
    private Material originMat;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originMat = sr.material;
    }

    public IEnumerator Flash()
    {
        sr.material = flashMat;
        yield return new WaitForSeconds(.2f);
        sr.material = originMat;
    }

    public virtual void RedBlink()
    {
        if (sr.color != Color.white)
            sr.color = Color.white;
        else sr.color = Color.red;
    }

    private void CancelRedBlink()
    {
        CancelInvoke();
        sr.color = Color.white;
    }
}
