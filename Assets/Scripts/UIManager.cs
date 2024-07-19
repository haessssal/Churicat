using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text Clicknum;
    private int cnt = 10;

    public List<Image> emptyBoxes;
    
    void Start()
    {
        Clicknum.text = cnt.ToString();
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
                }
            }
        }
    }

    void DecreaseCount()
    {
        if (cnt > 0)
        {
            cnt--;
            Clicknum.text = cnt.ToString();
        }

        else
        {
            // popup
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

}
