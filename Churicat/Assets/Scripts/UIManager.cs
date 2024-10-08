using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System;
using System.Linq;

public class UIManager : MonoBehaviour
{
    public TMP_Text Clicknum;
    public TMP_Text KeyCluenum;
    public TMP_Text HintText;
    public PopupManager popupManager;

    private int KeyCluecnt = 0;
    private int Touchcnt = 15;
    private int Found = 0;

    public List<Image> emptyBoxes;

    private List<string> usedHints = new List<string>();

    private Dictionary<GameObject, Vector3> originalPositions = new Dictionary<GameObject, Vector3>();
    private List<ClueData> foundClues = new List<ClueData>();

    private bool isPopupActive = false;  
    private bool isDialogActive = false;
    
    void Start()
    {
        Clicknum.text = Touchcnt.ToString();

        // save the original location of the game object
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Object"))
        {
            originalPositions[obj] = obj.transform.position;
        }

        foreach (GameObject clue in GameObject.FindGameObjectsWithTag("Clue"))
        {
            originalPositions[clue] = clue.transform.position;
        }

        foreach (GameObject keyClue in GameObject.FindGameObjectsWithTag("KeyClue"))
        {
            originalPositions[keyClue] = keyClue.transform.position;
        }

        // LoadClueData();
    }

    public void SetPopupState(bool state)
    {
        isPopupActive = state;
    }

    public void SetDialogState(bool state)
    {
        isDialogActive = state;
    }

    void Update()
    {
        if (isDialogActive || isPopupActive) 
        {
            return;
        }

        // pc mouse click
        if (Input.GetMouseButtonDown(0))
        {
            CheckTouch(Input.mousePosition);
        }

        // mobile touch
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            CheckTouch(Input.GetTouch(0).position);
        }

        OpenOverPopup();
    }

    // when retry
    public void init()
    {
        Touchcnt = 15;
        Found = 0;
        KeyCluecnt = 0;

        Clicknum.text = Touchcnt.ToString();
        KeyCluenum.text = $"{KeyCluecnt} / 3";

        // Clear empty boxes
        foreach (Image box in emptyBoxes)
        {
            foreach (Transform child in box.transform)
            {
                Destroy(child.gameObject);
            }
        }

        // Reset game objects
        ResetGameObjects();
    }

    void ResetGameObjects()
    {
        foreach (var k in originalPositions)
        {
            k.Key.transform.position = k.Value;
            k.Key.SetActive(true); 
        }
    }

    void CheckTouch(Vector2 screenPosition)
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

        // when touch the gameobject whose tag is 'Object' or 'Clue'
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Object") || hit.collider.CompareTag("Clue") || hit.collider.CompareTag("KeyClue"))
            {
                DecreaseCount();

                // add the object's image to an emptybox in the scroll view
                if (hit.collider.CompareTag("Clue") || hit.collider.CompareTag("KeyClue"))
                {
                    AddImageToEmptyBox(hit.collider.gameObject);
                    Found++;
                }

                if (hit.collider.CompareTag("KeyClue"))
                {
                    UpdateKeyClueCount();
                }
            }
        }
    }

    void DecreaseCount()
    {
        if (Touchcnt > 0 && Found < 10)
        {
            Touchcnt--;
            Clicknum.text = Touchcnt.ToString();
        }
    }

    void AddImageToEmptyBox(GameObject clueObject)
    {

        // Find the first empty box that doesn't have an image set
        foreach (Image box in emptyBoxes)
        {
            if (box.transform.childCount == 0) 
            {
                SpriteRenderer clueSpriteRenderer = clueObject.GetComponent<SpriteRenderer>();

                // Set the sprite of the clue object to a new image component inside the empty box
                if (clueSpriteRenderer != null)
                {
                    GameObject newImageObject = new GameObject("ClueImage");
                    newImageObject.transform.SetParent(box.transform);
                    newImageObject.transform.localPosition = Vector3.zero;
                    newImageObject.transform.localScale = Vector3.one;

                    Image newImage = newImageObject.AddComponent<Image>();
                    newImage.sprite = clueSpriteRenderer.sprite;
                    newImage.color = clueSpriteRenderer.color;  // for test
                    newImage.preserveAspect = true;

                    // Set RectTransform properties to match the parent
                    RectTransform rectTransform = newImage.GetComponent<RectTransform>();
                    rectTransform.anchorMin = Vector2.zero;
                    rectTransform.anchorMax = Vector2.one;

                    float padding = 17f;  // Set padding value (reduce size by desired amount)
                    rectTransform.offsetMin = new Vector2(padding, padding);
                    rectTransform.offsetMax = new Vector2(-padding, -padding);

                    // Deactivate the clue object
                    clueObject.SetActive(false);

                    ClueDescription clueDescriptionComponent = clueObject.GetComponent<ClueDescription>();
                    string clueDescription1 = clueDescriptionComponent != null ? clueDescriptionComponent.clueTexts : "NO EXPLANATION";
                    string clueDescription2 = clueDescriptionComponent != null ? clueDescriptionComponent.clueNames : "NO NAME";

                    // save clue data
                    ClueData newClue = new ClueData
                    {
                        clueID = clueObject.name,
                        imagePath = "Clues/" + clueObject.name,
                        clueExplain = clueDescription1,
                        clueName = clueDescription2
                    };

                    foundClues.Add(newClue);

                    Debug.Log($"Added clue: {newClue.clueID}, Path: {newClue.imagePath}");

                    // save as json file
                    SaveClueData();

                    break;
                }
            }
        }
    }

    void SaveClueData()
    {
        string path = Path.Combine(Application.dataPath, "ClueData.json");
        ClueDataList existingClueDataList = new ClueDataList();

        // 기존에 저장된 데이터가 있으면 불러오기
        if (File.Exists(path))
        {
            try
            {
                string existingJson = File.ReadAllText(path);
                existingClueDataList = JsonUtility.FromJson<ClueDataList>(existingJson);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load existing ClueData.json: {e.Message}");
            }
        }

        // 새로운 단서를 기존 데이터에 추가
        existingClueDataList.clues.AddRange(foundClues);

        // 중복된 데이터 제거
        existingClueDataList.clues = existingClueDataList.clues
            .GroupBy(c => c.clueID)
            .Select(g => g.First())
            .ToList();

        string json = JsonUtility.ToJson(existingClueDataList, true);

        try
        {   
            File.WriteAllText(path, json);
            Debug.Log($"ClueData.json file saved at: {path}");
        }
    
        catch (Exception e)
        {
            Debug.LogError($"Failed to save ClueData.json: {e.Message}");
        }
    }

    public List<ClueData> GetFoundClues()
    {
        return foundClues;  // 현재 게임에서 찾은 클루 데이터 목록을 반환
    }

    // 게임 124 TODO
    void UpdateKeyClueCount()
    {
        KeyCluecnt++;

        KeyCluenum.text = $"{KeyCluecnt} / 4";

        if (KeyCluecnt >= 4)
        {
            // popup
        }
    }

    void OpenOverPopup()
    {
        // Over1Popup: get star x 3
        // Over2Popup: get star x 2
        // Over3Popup: get star x 1
        // Over4Popup: get star x 0
        if (Touchcnt > 0 && Found == 10)
        {
            popupManager.OpenPopup("Over1");
        }

        else if (Touchcnt == 0 && Found >= 7)
        {
            popupManager.OpenPopup("Over2");
        }

        else if (Touchcnt == 0 && Found >= 5)
        {
            popupManager.OpenPopup("Over3");
        }

        else if (Touchcnt == 0 && Found <= 4)
        {
            popupManager.OpenPopup("Retry");
        }
    }

    public bool ShowRandomHint()
    {
        List<string> allHints = new List<string>();
        var clues = GameObject.FindGameObjectsWithTag("Clue");
        var keyclues = GameObject.FindGameObjectsWithTag("KeyClue");

        foreach (var clue in clues)
        {
            ClueDescription clueDescriptionComponent = clue.GetComponent<ClueDescription>();
            allHints.Add(clueDescriptionComponent.clueHints);
        }

        foreach (var keyclue in keyclues)
        {
            ClueDescription clueDescriptionComponent = keyclue.GetComponent<ClueDescription>();
            allHints.Add(clueDescriptionComponent.clueHints);
        }

        // except clue hint that has already found or already shown
        List<string> availableHints = new List<string>(allHints);
        foreach (var foundClue in foundClues)
        {
            availableHints.Remove(foundClue.clueExplain);
        }

        availableHints = availableHints.Except(usedHints).ToList();

        if (availableHints.Count == 0)
        {
            HintText.text = "No more hints available";
            return false;
        }

        int randomIndex = UnityEngine.Random.Range(0, availableHints.Count);
        string randomHint = availableHints[randomIndex];

        HintText.text = randomHint;

        usedHints.Add(randomHint);

        return true;
    }
}
