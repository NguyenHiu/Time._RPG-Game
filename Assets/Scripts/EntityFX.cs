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

    [Header("Freezing FX")]
    [SerializeField] private Material freezingMat;

    [Header("Shocking FX")]
    [SerializeField] private Material shockingMat;


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
        float elapsed = 0f;
        bool isBurning = false;

        while (elapsed < time)
        {
            sr.material = isBurning ? originMat : burningMat;
            isBurning = !isBurning;
            elapsed += 0.3f; // Blinking every 0.3 seconds
            yield return new WaitForSeconds(0.3f);
        }

        sr.material = originMat; // Reset material at the end
    }


    public IEnumerator Freezing(float time)
    {
        float elapsed = 0f;
        bool isBurning = false;

        while (elapsed < time)
        {
            sr.material = isBurning ? originMat : freezingMat;
            isBurning = !isBurning;
            elapsed += 0.3f; // Blinking every 0.3 seconds
            yield return new WaitForSeconds(0.3f);
        }

        sr.material = originMat; // Reset material at the end
    }

    public IEnumerator Shocking(float time)
    {
        float elapsed = 0f;
        bool isBurning = false;

        while (elapsed < time)
        {
            sr.material = isBurning ? originMat : shockingMat;
            isBurning = !isBurning;
            elapsed += 0.3f; // Blinking every 0.3 seconds
            yield return new WaitForSeconds(0.3f);
        }

        sr.material = originMat; // Reset material at the end
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
