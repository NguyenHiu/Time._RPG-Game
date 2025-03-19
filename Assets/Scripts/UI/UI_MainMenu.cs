using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string mainScene = "MainScene";
    [SerializeField] private Button continueButton;
    [SerializeField] private UI_Fade fade;

    private void Start()
    {
        continueButton.interactable = SaveManager.instance.HasGameData();
    }

    public void ContinueGame()
    {
        StartCoroutine(LoadSceneDelay(1.5f));
    }

    public void NewGame()
    {
        SaveManager.instance.Delete();
        StartCoroutine(LoadSceneDelay(1.5f));
    }

    public void Exit()
    {
        Application.Quit();
        Debug.Log("Quit.");
    }

    private IEnumerator LoadSceneDelay(float delay)
    {
        fade.FadeOut();
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(mainScene);
    }
}
