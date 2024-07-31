using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    public StarManager starManager;

    private void Start()
    {
        Load();
    }

    private void OnApplicationQuit()
    {
        Save(); 
    }

    public void Save()
    {
        PlayerPrefs.SetInt("Star", starManager.StarInt);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey("Star"))
        {
            starManager.StarInt = PlayerPrefs.GetInt("Star");
        }

        starManager.UpdateStarText();
    }

    [MenuItem("GameManager/Clear All SaveData")]
    public static void ClearAllSaveData()
    {
        if (EditorUtility.DisplayDialog("Delete Saved Data",
            "Are you sure you want to delete all the data in the game?", "Yes", "No"))
        {
            Debug.Log("All PlayerPrefs data deleted.");
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }

}
