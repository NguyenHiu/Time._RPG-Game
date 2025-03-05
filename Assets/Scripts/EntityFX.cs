using System.Collections;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;
    private Material originMat;

    [Header("Flash FX")]
    [SerializeField] private Material flashMat;

    [Header("Burn FX")]
    [SerializeField] private Material burningMat;
    private bool isBurning = false;

    [Header("Freezing FX")]
    [SerializeField] private Material freezingMat;
    private bool isFreezing = false;

    [Header("Shocking FX")]
    [SerializeField] private Material shockingMat;
    private bool isShocking = false;


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

    public IEnumerator Burning(float time)
    {
        if (isBurning) yield break;
        isBurning = true;

        float elapsed = 0f;
        bool flag = false;

        while (elapsed < time)
        {
            sr.material = flag ? originMat : burningMat;
            flag = !flag;
            elapsed += 0.15f; // Blinking every 0.15 seconds
            yield return new WaitForSeconds(0.15f);
        }

        sr.material = originMat; // Reset material at the end
        isBurning = false;
    }


    public IEnumerator Freezing(float time)
    {
        if (isFreezing) yield break;
        isFreezing = true;

        float elapsed = 0f;
        bool flag = false;

        while (elapsed < time)
        {
            sr.material = flag ? originMat : freezingMat;
            flag = !flag;
            elapsed += 0.15f; // Blinking every 0.15 seconds
            yield return new WaitForSeconds(0.15f);
        }

        sr.material = originMat; // Reset material at the end
        isFreezing = false;
    }

    public IEnumerator Shocking(float time)
    {
        if (isShocking) yield break;
        isShocking = true;

        float elapsed = 0f;
        bool flag = false;

        while (elapsed < time)
        {
            sr.material = flag ? originMat : shockingMat;
            flag = !flag;
            elapsed += 0.15f; // Blinking every 0.15 seconds
            yield return new WaitForSeconds(0.15f);
        }

        sr.material = originMat; // Reset material at the end
        isShocking = false;
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
