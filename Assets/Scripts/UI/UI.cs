using System.Collections;
using UnityEngine;

public class UI : MonoBehaviour
{
    public GameObject characterUI;
    public GameObject skillTreeUI;
    public GameObject craftUI;
    public GameObject optionsUI;
    public GameObject inGameUI;
    public UI_Tooltips_Equipment equipmentTooltips;
    public UI_Tooltips_Stat statTooltips;
    public UI_Tooltips_Skill skillTooltips;
    public UI_CraftWindow craftWindow;
    public UI_Fade fadeScreen;
    public UI_DieScreen dieScreen;

    private void Awake()
    {
        // Awake skill tree functions to setup skills' events
        SwitchTo(skillTreeUI);
    }
    private void Start()
    {
        SwitchTo(inGameUI);
        fadeScreen.gameObject.SetActive(true);
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
        {
            transform.GetChild(i).gameObject.SetActive(false);
            if (PlayerManager.instance)
                PlayerManager.instance.isInMenu = false;
        }

        if (_menu != null)
        {
            _menu.SetActive(true);

            if (PlayerManager.instance != null)
            {
                PlayerManager.instance.isInMenu = true;
                Debug.Log("Is In Menu: " + PlayerManager.instance.isInMenu);
            }

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
            inGameUI.SetActive(true);
            if (PlayerManager.instance != null)
            {
                PlayerManager.instance.isInMenu = false;
                Debug.Log("Is In Menu: " + PlayerManager.instance.isInMenu);

            }
            return;
        }


        SwitchTo(_menu);
    }

    public void SwitchToDieUI()
    {
        fadeScreen.FadeOut();
        StartCoroutine(ShowDieMsgDelay(1f));
    }

    private IEnumerator ShowDieMsgDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        dieScreen.ShowDieMessage();
    }

    public void RestartThisScene() => GameManager.instance.RestartScene();
}
