using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    [SerializeField] private string fileName;
    [SerializeField] private bool isEncrypted;
    [SerializeField] private string secret;
    private GameData gameData;
    private List<IGameData> gameDataList;
    private FileHandler fileHandler;

    // Singleton Pattern
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(instance.gameObject);
    }

    private void Start()
    {
        gameDataList = FindAllData();
        fileHandler = new(Application.persistentDataPath, fileName, isEncrypted, secret);
        LoadGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IGameData> FindAllData()
    {
        IEnumerable<IGameData> data = FindObjectsOfType<MonoBehaviour>().OfType<IGameData>();
        return new List<IGameData>(data);
    }

    public void Delete()
    {
        fileHandler.Delete();
    }

    public bool HasGameData()
    {
        return fileHandler.Load() != null;
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        Debug.Log("Load Game");
        gameData = fileHandler.Load();

        if (gameData == null)
            NewGame();

        foreach (IGameData data in gameDataList)
            data.LoadData(gameData);
    }

    public void SaveGame()
    {
        Debug.Log("Saved game data");
        if (gameData == null)
            NewGame();

        foreach (IGameData data in gameDataList)
            data.SaveData(ref gameData);

        fileHandler.Save(gameData);
    }
}
