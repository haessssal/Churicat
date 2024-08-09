using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

[Serializable]
public class ClueData
{
    public string clueName;
    public string imagePath;
}

[Serializable]
public class ClueDataList
{
    public List<ClueData> clues = new List<ClueData>();
}

public class InventoryManager : MonoBehaviour
{
    public List<Image> slotImages;  
    public List<ClueData> loadedClues = new List<ClueData>();  // data from json

    void Start()
    {
        Debug.Log("InventoryManager Start");
        LoadData();
        DisplayCluesInInventory();
    }

    public void LoadData()
    {
        // string path = Path.Combine(Application.persistentDataPath, "ClueData.json");
        string path = Path.Combine(Application.dataPath, "ClueData.json");  // in asset folder


        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            // loadedClues = JsonUtility.FromJson<List<ClueData>>(json); // json > List<ClueData>

            ClueDataList clueDataList = JsonUtility.FromJson<ClueDataList>(json);
            loadedClues = clueDataList.clues;
            
            foreach (var clue in loadedClues)
            {
                Debug.Log($"Loaded Clue: {clue.clueName}, Path: {clue.imagePath}");
            }
        }

        else
        {
            Debug.LogWarning("No ClueData.json file found.");
        }
    }

    void DisplayCluesInInventory()
    {
        // show clue data in each slot
        for (int i = 0; i < loadedClues.Count && i < slotImages.Count; i++)
        {
            // load image (from resources)
            Sprite clueSprite = Resources.Load<Sprite>(loadedClues[i].imagePath);

            if (clueSprite != null)
            {
                slotImages[i].sprite = clueSprite;
                slotImages[i].color = Color.white;
                Debug.Log($"Displayed clue in slot: {loadedClues[i].clueName}");
            }

            else
            {
                Debug.LogWarning($"Could not load sprite from path: {loadedClues[i].imagePath}");
            }
        }
    }

    /*
    Sprite LoadSpriteFromPath(string path)
    {
        // load image (from resources)
        return Resources.Load<Sprite>(path); 
    }
    */
}