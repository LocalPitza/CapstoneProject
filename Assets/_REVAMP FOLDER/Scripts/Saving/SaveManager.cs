using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    static readonly string CompanyName = Application.companyName != null && Application.companyName != ""
                                         ? Application.companyName
                                         : "DefaultCompany";

    static readonly string SaveFolderPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), CompanyName);
    static readonly string FILEPATH = Path.Combine(SaveFolderPath, "Save.json");

    //static readonly string FILEPATH = Application.persistentDataPath + "/Save.json";

    public static void Save(GameSaveState save)
    {
        if (!Directory.Exists(SaveFolderPath))
        {
            Directory.CreateDirectory(SaveFolderPath);
        }

        string json = JsonUtility.ToJson(save);
        File.WriteAllText(FILEPATH, json);
    }

    public static GameSaveState Load()
    {
        GameSaveState loadedSave = null;

        if(File.Exists(FILEPATH))
        {
            string json = File.ReadAllText(FILEPATH);
            loadedSave = JsonUtility.FromJson<GameSaveState>(json);
        }

        return loadedSave;
    }

    public static bool HasSave()
    {
        return File.Exists(FILEPATH);
    }
}
