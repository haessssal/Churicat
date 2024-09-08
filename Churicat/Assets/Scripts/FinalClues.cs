using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using TMPro;
using System.Linq;

public class FinalClues : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public List<Image> dochiFinalImages;
    public List<Image> dogFinalImages;
    public List<Image> hamFinalImages;

    void Start()
    {
        LoadData();
        // DisplayCluesInFinal();
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
            inventoryManager.loadedClues = clueDataList.clues;
            
            /*
            foreach (var clue in inventoryManager.loadedClues)
            {
                Debug.Log($"Loaded Clue: {clue.clueIS}, Path: {clue.imagePath}");
            }
            */
        }

        else
        {
            Debug.LogWarning("No ClueData.json file found.");
        }
    }

    public void DisplayCluesInFinal(string popup)
    { 
        List<Image> selectedFinalImages = null;
        List<string> targetClues = new List<string>();

        switch (popup)
        {
            case "Dochi":
                selectedFinalImages = dochiFinalImages;
                targetClues = new List<string> { "Clue10", "Clue11", "Clue12", "Clue14", "Clue18", "Clue19" };
                break;
            case "Dog":
                selectedFinalImages = dogFinalImages;
                targetClues = new List<string> { "Clue20", "Clue21", "Clue22", "Clue26", "Clue27", "Clue29" };
                break;
            case "Ham":
                selectedFinalImages = hamFinalImages;
                targetClues = new List<string> { "Clue40", "Clue41", "Clue42", "Clue43", "Clue47", "Clue48" };
                break;
        }
        
        int index = 0;

        foreach (var clue in inventoryManager.loadedClues)
        {
            if (targetClues.Contains(clue.clueID) && index < selectedFinalImages.Count)
            {
                // 이미지 로드 from Resources
                Sprite clueSprite = Resources.Load<Sprite>(clue.imagePath);

                if (clueSprite != null)
                {
                    selectedFinalImages[index].sprite = clueSprite;
                    selectedFinalImages[index].color = Color.white; // 불투명하게 설정
                    selectedFinalImages[index].preserveAspect = true;
                    // Debug.Log($"Displayed clue in final slot: {clue.clueID}");
                    index++;
                }

                else
                {
                    Debug.LogWarning($"Could not load sprite from path: {clue.imagePath}");
                }
            }
        }
    }
}
