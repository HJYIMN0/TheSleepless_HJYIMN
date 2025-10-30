using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Collections;

public class SaveSystem
{
    public static string GetPath() => Application.persistentDataPath + "/save.data";

    public static bool Save()
    {
        GameManager _gameManager = GameManager.Instance;
        if (_gameManager == null)
        {
            Debug.LogError("GameManager instance not found.");
            return false;
        }

        SaveData data = LoadOrInitialize();

        data.day = _gameManager.Day;
        data.energy = _gameManager.Energy;
        data.paranoia = _gameManager.Paranoia;
        data.hunger = _gameManager.Hunger;
        data.hygiene = _gameManager.Hygiene;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(GetPath(), json);
        Debug.Log($"Saved {json} to {GetPath()}");
        return true;
    }

    public static bool DoesSaveExist() => File.Exists(GetPath());

    public static SaveData Load()
    {
        if (!DoesSaveExist()) return null;
        string json = File.ReadAllText(GetPath());
        SaveData data = JsonUtility.FromJson<SaveData>(json);
        //Debug.Log($"Loaded {json} from {GetPath()}");
        return data;
    }

    public static SaveData LoadOrInitialize()
    {
        SaveData data = Load();
        if (data == null)
        {
            data = new SaveData
            {
                username = "DefaultUser",
                day = 1,
                energy = GameManager.Instance.MaxEnergy,
                paranoia = 0,
                hunger = GameManager.Instance.MaxHunger,
                hygiene = GameManager.Instance.MaxHygiene,
                integrity = GameManager.Instance.MaxIntegrity,
                direction = GameManager.Instance.DesiredDirection,
                hour = 12,
                minute = 30,
                levelsProgress = new List<SaveData.LevelData>()
            };

            string json = JsonUtility.ToJson(data);
            File.WriteAllText(GetPath(), json);
            Debug.Log($"Initialized and saved default data to {GetPath()}");
        }
        return data;
    }

    public static void SetGameObjectPosition(GameObject gameObject, float x, float y, float z)
    {
        gameObject.transform.position = new Vector3(x, y, z);
    }

    public static void SetGameObjectPosition(GameObject gameObject, Vector3 position)
    {
        gameObject.transform.position = position;
    }

    public static void DeleteSave()
    {
        if (DoesSaveExist())
        {
            File.Delete(GetPath());
            Debug.Log("Save file deleted.");
        }
        else
        {
            Debug.Log("No save file to delete.");
        }
    }


}