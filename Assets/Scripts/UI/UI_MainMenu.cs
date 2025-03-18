using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string mainScene = "MainScene";
    [SerializeField] private Button continueButton;

    private void Start()
    {
        continueButton.interactable = SaveManager.instance.HasGameData();
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(mainScene);
    }

    public void NewGame()
    {
        SaveManager.instance.Delete();
        SceneManager.LoadScene(mainScene);
    }

    public void Exit()
    {
        //Application.Quit();
        Debug.Log("Quit.");
    }
}
