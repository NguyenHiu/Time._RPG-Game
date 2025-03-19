using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_DieScreen : MonoBehaviour
{
    [SerializeField] private GameObject textObj;
    [SerializeField] private GameObject restartButton;

    private void Start()
    {
        textObj.SetActive(false);
        restartButton.SetActive(false);
    }
    public void ShowDieMessage()
    {
        StartCoroutine(WaitForTextObj(1f));
    }

    private IEnumerator WaitForTextObj(float delay)
    {
        textObj.SetActive(true);
        yield return new WaitForSeconds(delay);
        restartButton.SetActive(true);
    }


}
