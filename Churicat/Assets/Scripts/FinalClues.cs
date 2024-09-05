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
    public List<Image> finalImages;

    void Start()
    {
        LoadData();
        DisplayCluesInFinal();
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
                Debug.Log($"Loaded Clue: {clue.clueName}, Path: {clue.imagePath}");
            }
            */
        }

        else
        {
            Debug.LogWarning("No ClueData.json file found.");
        }
    }

    void DisplayCluesInFinal()
    { 
        // 슬롯 초기화
        foreach (var image in finalImages)
        {
            image.sprite = null;
        }

        // 해당 단서들만 슬롯에 추가
        List<string> targetClues = new List<string> { "Clue21", "Clue22", "Clue23", "Clue24" };
        int index = 0;

        foreach (var clue in inventoryManager.loadedClues)
        {
            if (targetClues.Contains(clue.clueName) && index < finalImages.Count)
            {
                // 이미지 로드 from Resources
                Sprite clueSprite = Resources.Load<Sprite>(clue.imagePath);

                if (clueSprite != null)
                {
                    finalImages[index].sprite = clueSprite;
                    finalImages[index].color = Color.white; // 불투명하게 설정
                    // Debug.Log($"Displayed clue in final slot: {clue.clueName}");
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
