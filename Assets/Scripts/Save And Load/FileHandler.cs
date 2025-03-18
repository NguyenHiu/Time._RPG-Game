using System;
using System.IO;
using UnityEngine;

public class FileHandler
{
    private readonly string path;
    private readonly string fileName;
    private readonly bool isEncrypted;
    private readonly string secret;

    public FileHandler(string _path, string _fileName, bool _isEncrypted, string _secret)
    {
        path = _path;
        fileName = _fileName;
        isEncrypted = _isEncrypted;
        secret = _secret;
    }

    public void Save(GameData gameData)
    {
        string fullPath = Path.Combine(path, fileName);
        try
        {
            string jsonData = JsonUtility.ToJson(gameData);
            if (isEncrypted) jsonData = EncryptDecrypt(jsonData);
            using FileStream stream = new(fullPath, FileMode.Create);
            using StreamWriter writer = new(stream);
            writer.Write(jsonData);
        }
        catch (Exception e)
        {
            Debug.LogError("Error while trying to save data to file: " + fullPath + "\n" + e);
        }
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(path, fileName);
        GameData gameData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                using FileStream stream = new(fullPath, FileMode.Open);
                using StreamReader reader = new(stream);
                string jsonData = reader.ReadToEnd();
                if (isEncrypted) jsonData = EncryptDecrypt(jsonData);
                gameData = JsonUtility.FromJson<GameData>(jsonData);
            }
            catch (Exception e)
            {
                Debug.LogError("Error while trying to load data from file: " + fullPath + "\n" + e);
            }
        }

        return gameData;
    }

    private string EncryptDecrypt(string data)
    {
        string res = "";
        for (int i = 0; i < data.Length; i++)
        {
            res += (char)(data[i] ^ secret[i % secret.Length]);
        }
        return res;
    }
}
