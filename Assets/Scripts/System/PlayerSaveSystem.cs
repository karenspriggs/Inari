using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Newtonsoft.Json;

public static class PlayerSaveSystem
{
    static PlayerSaveData sessionSaveData;
    static bool saveDataExists => File.Exists(Application.persistentDataPath + saveFileName);
    static string saveFileName = "/SaveData.json";

    public static PlayerSaveData SessionSaveData
    {
        get
        {
            if (sessionSaveData == null)
            {
                if (saveDataExists)
                {
                    LoadGame();
                } else
                {
                    MakeNewGame();
                }
            }

            return sessionSaveData;
        }
    }

    public static void LoadGame()
    {
        string fileLocation = Application.persistentDataPath + saveFileName;
        string jsonString = File.ReadAllText(fileLocation);
        sessionSaveData = JsonConvert.DeserializeObject<PlayerSaveData>(jsonString);
    }

    public static void MakeNewGame()
    {
        Debug.Log("Making new save data");
        sessionSaveData = new PlayerSaveData();
    }

    public static void SaveGame()
    {
        Debug.Log("Trying to save");
        string fileLocation = Application.persistentDataPath + saveFileName;
        string jsonFileData = JsonConvert.SerializeObject(SessionSaveData);
        File.WriteAllText(fileLocation, jsonFileData);
    }
}
