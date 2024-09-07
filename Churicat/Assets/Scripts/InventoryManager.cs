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
    public string clueID;
    public string imagePath;
    public string clueExplain;
    public string clueName;
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
    public TMP_Text ClueName;

    private List<int> slotToClueIndex;


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
            
            /*
            foreach (var clue in loadedClues)
            {
                Debug.Log($"Loaded Clue: {clue.clueID}, Path: {clue.imagePath}");
            }
            */
        }

        else
        {
            Debug.LogWarning("No ClueData.json file found.");
        }
    }

    /*
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
                Debug.Log($"Displayed clue in slot: {loadedClues[i].clueID}");
            }

            else
            {
                Debug.LogWarning($"Could not load sprite from path: {loadedClues[i].imagePath}");
            }
        }
    }
    */

    void DisplayCluesInInventory()
    {
        // 범위에 따라 슬롯 인덱스 초기화
        int nextSlot10_19 = 0;   // clue10 ~ clue19
        int nextSlot20_29 = 10;  // clue20 ~ clue29
        int nextSlot30_39 = 20;  // clue30 ~ clue39
        int nextSlot40_49 = 30;  // clue40 ~ clue49

        // 각 슬롯에 해당하는 단서의 인덱스 저장
        slotToClueIndex = new List<int>(new int[slotImages.Count]);

        for (int i = 0; i < slotToClueIndex.Count; i++)
        {
            slotToClueIndex[i] = -1; // 초깃값: 단서 없음
        }

        for (int i = 0; i < loadedClues.Count; i++)
        {
            var clue = loadedClues[i];
            int clueNumber;

            if (int.TryParse(clue.clueID.Replace("Clue", ""), out clueNumber))
            {
                int slotIndex = -1;

                if (clueNumber >= 10 && clueNumber < 20 && nextSlot10_19 < 10)
                {
                    slotIndex = nextSlot10_19;  // clue10 ~ clue19
                    nextSlot10_19++;
                }
                else if (clueNumber >= 20 && clueNumber < 30 && nextSlot20_29 < 20)
                {
                    slotIndex = nextSlot20_29;  // clue20 ~ clue29
                    nextSlot20_29++;
                }
                else if (clueNumber >= 30 && clueNumber < 40 && nextSlot30_39 < 30)
                {
                    slotIndex = nextSlot30_39;  // clue30 ~ clue39
                    nextSlot30_39++;
                }
                else if (clueNumber >= 40 && clueNumber < 50 && nextSlot40_49 < 40)
                {
                    slotIndex = nextSlot40_49;  // clue40 ~ clue49
                    nextSlot40_49++;
                }

                // 가능한 slotIndex 일 때만 삽입
                if (slotIndex >= 0 && slotIndex < slotImages.Count)
                {
                    
                    Sprite clueSprite = Resources.Load<Sprite>(clue.imagePath);
                    if (clueSprite != null)
                    {
                        slotImages[slotIndex].sprite = clueSprite;
                        slotImages[slotIndex].color = Color.white;
                        slotToClueIndex[slotIndex] = i;  // 해당 슬롯에 어떤 단서가 들어갔는지 인덱스 저장
                        // Debug.Log($"Displayed clue in slot {slotIndex}: {clue.clueID}");
                    }
                    else
                    {
                        Debug.LogWarning($"Could not load sprite from path: {clue.imagePath}");
                    }
                }
            }
        }   

        // 각 슬롯에 대한 클릭 이벤트 
        for (int i = 0; i < slotImages.Count; i++)
        {
            int index = i; // 로컬로 저장
            Button button = slotImages[i].GetComponent<Button>();

            if (button != null)
            {
                button.onClick.RemoveAllListeners(); // 기존 리스너 제거
                button.onClick.AddListener(() => OnClueClicked(slotToClueIndex[index]));
            }

            slotImages[i].preserveAspect = true;
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
                button.onClick.RemoveAllListeners();  // 기존 리스너 제거
                button.onClick.AddListener(() => OnClueClicked(slotToClueIndex[index])); 
                // button.onClick.AddListener(() => OnClueClicked(index));
            }

            slotImages[i].preserveAspect = true;
        }

        bigImage.preserveAspect = true;
    }

    public void OnClueClicked(int clueIndex)
    {
        /*
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
        */

        if (clueIndex >= 0 && clueIndex < loadedClues.Count)
        {
            ClueData clue = loadedClues[clueIndex];  // 올바른 단서 데이터 1!
            ClueExplanation.text = clue.clueExplain;  
            ClueName.text = clue.clueName;

            // 단서 이미지를 큰 이미지로 
            Sprite clueSprite = Resources.Load<Sprite>(clue.imagePath);
            if (clueSprite != null)
            {
                bigImage.sprite = clueSprite;
                bigImage.color = Color.white;
                bigImage.preserveAspect = true;
            }
        }
    }

    public void AddClue(ClueData newClue)
    {
        // check if the clue already exists
        if (!loadedClues.Any(c => c.clueID == newClue.clueID))
        {
            loadedClues.Add(newClue);
            SaveInvenData();
            DisplayCluesInInventory();  // Update inventory UI
        }

        else{
            Debug.Log($"Clue {newClue.clueID} already exists.");
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