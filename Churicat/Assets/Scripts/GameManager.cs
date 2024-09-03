using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

// Final1Scene에서 ending1 정답(클릭한 동물 + 문장 2개) 일치할 경우 OpenPopup
// 기타엔딩 해금: ending2-3 정답으로 변경하고 나머지 초기화
// 하나라도 틀릴 경우 finaltrycnt +1 하고 팝업 ...

public class GameManager : MonoBehaviour
{
    public StarManager starManager;
    public DialogSystem dialogSystem;
    public static InventoryManager inventoryManager;
    public static ButtonHandler buttonHandler;

    private void Start()
    {
        Load();
        buttonHandler = FindObjectOfType<ButtonHandler>();
    }

    private void OnApplicationQuit()
    {
        Save(); 
    }

    public void Save()
    {
        PlayerPrefs.SetInt("Star", starManager.StarInt);

        buttonHandler = FindObjectOfType<ButtonHandler>();
        PlayerPrefs.SetInt("Game1TryCnt", buttonHandler.game1trycnt);
        PlayerPrefs.SetInt("Game2TryCnt", buttonHandler.game2trycnt);
        PlayerPrefs.SetInt("Game3TryCnt", buttonHandler.game3trycnt);
        PlayerPrefs.SetInt("Game4TryCnt", buttonHandler.game4trycnt);
        PlayerPrefs.SetInt("Final1TryCnt", buttonHandler.final1trycnt);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey("Star"))
        {
            starManager.StarInt = PlayerPrefs.GetInt("Star");
        }

        buttonHandler = FindObjectOfType<ButtonHandler>();
        if (PlayerPrefs.HasKey("Game1TryCnt"))
        {
            buttonHandler.game1trycnt = PlayerPrefs.GetInt("Game1TryCnt");
        }
        if (PlayerPrefs.HasKey("Game2TryCnt"))
        {
            buttonHandler.game2trycnt = PlayerPrefs.GetInt("Game2TryCnt");
        }
        if (PlayerPrefs.HasKey("Game3TryCnt"))
        {
            buttonHandler.game3trycnt = PlayerPrefs.GetInt("Game3TryCnt");
        }
        if (PlayerPrefs.HasKey("Game4TryCnt"))
        {
            buttonHandler.game4trycnt = PlayerPrefs.GetInt("Game4TryCnt");
        }
        if (PlayerPrefs.HasKey("Final1TryCnt"))
        {
            buttonHandler.final1trycnt = PlayerPrefs.GetInt("Final1TryCnt");
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

            // Clear the JSON file
            string path = Path.Combine(Application.dataPath, "ClueData.json");

            if (File.Exists(path))
            {
                File.WriteAllText(path, "{}");  
                Debug.Log($"ClueData.json file cleared at: {path}");
            }

            // Clear the inventory 
            if (inventoryManager != null)
            {
                inventoryManager.ClearInventory();
                Debug.Log("Inventory cleared.");
            }

            // Clear the trycnt
            buttonHandler.game1trycnt = 0;
            buttonHandler.game2trycnt = 0;
            buttonHandler.game3trycnt = 0;
            buttonHandler.game4trycnt = 0;
            buttonHandler.final1trycnt = 0;

        }
    }
}
