using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Tooltips_Stat : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI description;

    public void EnableTooltips(string _description)
    {
        description.text = _description;
        gameObject.SetActive(true);
    }

    public void DisableTooltips()
    {
        gameObject.SetActive(false);
    }
}
