using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System;

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

    private Dictionary<GameObject, Vector3> originalPositions = new Dictionary<GameObject, Vector3>();
    private List<ClueData> foundClues = new List<ClueData>();

    private List<string> hints = new List<string>
    {
        "Hint 1",
        "Hint 2",
        "Hint 3",
        "Hint 4",
        "Hint 5",
        "Hint 6",
        "Hint 7",
        "Hint 8",
        "Hint 9",
        "Hint 10"
    };

    private List<string> usedHints = new List<string>();
    
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
    }

    void Update()
    {
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
        foreach (var kvp in originalPositions)
        {
            kvp.Key.transform.position = kvp.Value;
            kvp.Key.SetActive(true); 
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

                    // save clue data
                    ClueData newClue = new ClueData
                    {
                        clueName = clueObject.name,
                        imagePath = "Clues/" + clueObject.name
                    };

                    foundClues.Add(newClue);

                    Debug.Log($"Added clue: {newClue.clueName}, Path: {newClue.imagePath}");

                    // save as json file
                    SaveClueData();

                    break; 
                }
            }
        }
    }

    void SaveClueData()
    {
        ClueDataList clueDataList = new ClueDataList { clues = foundClues };
        string json = JsonUtility.ToJson(clueDataList);
        string path = Path.Combine(Application.dataPath, "ClueData.json");

        Debug.Log($"JSON data: {json}");

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

    void UpdateKeyClueCount()
    {
        KeyCluecnt++;

        KeyCluenum.text = $"{KeyCluecnt} / 3";

        if (KeyCluecnt >= 3)
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

    public void ShowRandomHint()
    {
        if (hints.Count == 0)
        {
            HintText.text = "No more hints available";
            return;
        }

        int randomIndex = UnityEngine.Random.Range(0, hints.Count);
        string randomHint = hints[randomIndex];

        HintText.text = randomHint;

        usedHints.Add(randomHint);
        hints.RemoveAt(randomIndex);
    }
}

