using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using TMPro;
using System.Linq;

[Serializable]
public class ClueData
{
    public string clueName;
    public string imagePath;
    public string clueExplain;
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
    public TMP_Text ClueExplanation;
    public Image bigImage;

    void Start()
    {
        Debug.Log("InventoryManager Start");
        LoadData();
        DisplayCluesInInventory();
        Setups();
    }

    // get clue data from json
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

    public void ClearInventory()
    {
        loadedClues.Clear();

        foreach (var slotImage in slotImages)
        {
            slotImage.sprite = null;
            slotImage.color = Color.clear;
        }

        Debug.Log("All clues removed from inventory.");
    }

    void Setups()
    {
        for (int i = 0; i < slotImages.Count; i++)
        {
            int index = i;
            Button button = slotImages[i].GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => OnClueClicked(index));
            }

            slotImages[i].preserveAspect = true;
        }

        bigImage.preserveAspect = true;
    }

    public void OnClueClicked(int index)
    {
        if (index >= 0 && index < loadedClues.Count)
        {
            ClueData clue = loadedClues[index];
            ClueExplanation.text = clue.clueExplain;  // Display clue description

            // Display clue image in the bigimage
            Sprite clueSprite = Resources.Load<Sprite>(clue.imagePath);
            if (clueSprite != null)
            {
                bigImage.sprite = clueSprite;
                bigImage.color = Color.white;  // Ensure image is visible

                bigImage.preserveAspect = true;
            }
        }
    }

    public void AddClue(ClueData newClue)
    {
        // check if the clue already exists
        if (!loadedClues.Any(c => c.clueName == newClue.clueName))
        {
            loadedClues.Add(newClue);
            SaveInvenData();
            DisplayCluesInInventory();  // Update inventory UI
        }

        else{
            Debug.Log($"Clue {newClue.clueName} already exists.");
        }
    }

    public void SaveInvenData()
    {
        ClueDataList clueDataList = new ClueDataList { clues = loadedClues };
        string json = JsonUtility.ToJson(clueDataList, true);  // true: indent format
        string path = Path.Combine(Application.dataPath, "ClueData.json");

        Debug.Log($"JSON data: {json}");

        try
        {
            File.WriteAllText(path, json);
            // Debug.Log($"ClueData.json file saved at: {path}");
        }

        catch (Exception e)
        {
            Debug.LogError($"Failed to save ClueData.json: {e.Message}");
        }
    }
}