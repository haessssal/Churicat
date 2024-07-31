using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text Clicknum;
    public TMP_Text KeyCluenum;
    private int KeyCluecnt = 0;
    private int Touchcnt = 15;
    private int Found = 0;

    public List<Image> emptyBoxes;

    public List<GameObject> cluePrefabs;
    public List<GameObject> keyCluePrefabs;

    public PopupManager popupManager;
    
    void Start()
    {
        Clicknum.text = Touchcnt.ToString();
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

    // TODO: when retry
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

        ResetGameObjects();
    }

    void ResetGameObjects()
    {
        // TODO
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

                    break; 
                }
            }
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
}
