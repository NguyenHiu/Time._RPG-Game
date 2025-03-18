using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int currency;
    public SerializableDictionary<string, int> inventory;
    public List<string> equipments;
    public SerializableDictionary<string, bool> skills;

    public GameData()
    {
        currency = 0;
        inventory = new();
        equipments = new();
        skills = new();
    }
}
