using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public GameObject characterUI;
    public GameObject skillTreeUI;
    public GameObject craftUI;
    public GameObject optionsUI;
    public Tooltips_Equipment equipmentTooltips;
    public Tooltips_Stat statTooltips;

    private void Start()
    {
        SwitchTo(null);
        equipmentTooltips.gameObject.SetActive(false);
        statTooltips.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            SwitchToUsingKeyboard(characterUI);
        if (Input.GetKeyDown(KeyCode.B))
            SwitchToUsingKeyboard(craftUI);
        if (Input.GetKeyDown(KeyCode.K))
            SwitchToUsingKeyboard(skillTreeUI);
        if (Input.GetKeyDown(KeyCode.O))
            SwitchToUsingKeyboard(optionsUI);
    }

    public void SwitchTo(GameObject _menu)
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);

        if (_menu != null)
            _menu.SetActive(true);
    }

    private void SwitchToUsingKeyboard(GameObject _menu)
    {
        if (_menu != null && _menu.activeInHierarchy)
        {
            _menu.SetActive(false);
            return;
        }

        SwitchTo(_menu);
    }
}
