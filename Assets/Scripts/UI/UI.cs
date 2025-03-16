using UnityEngine;

public class UI : MonoBehaviour
{
    public GameObject characterUI;
    public GameObject skillTreeUI;
    public GameObject craftUI;
    public GameObject optionsUI;
    public UI_Tooltips_Equipment equipmentTooltips;
    public UI_Tooltips_Stat statTooltips;
    public UI_Tooltips_Skill skillTooltips;
    public UI_CraftWindow craftWindow;

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
        {
            _menu.SetActive(true);

            // Set default value 
            UI_CraftList craftList = _menu.GetComponentInChildren<UI_CraftList>();
            if (craftList != null)
            {
                craftList.InitCraftList();
            }
        }
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
