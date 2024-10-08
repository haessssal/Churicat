using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SwipeMap : MonoBehaviour
{
    public GameObject scrollbar;
    float scroll_pos = 0;
    float[] pos;

    public Image AnimalImage;  
    public Sprite[] Animalimages; 
    public TMP_Text AnimalText;
    public string[] Animaltexts;
    public TMP_Text Homenum;
    public string[] Homenums;
    public TMP_Text TrycntText;
    public ButtonHandler buttonHandler;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pos = new float[transform.childCount];
        float distance = 1f / (pos.Length - 1f);

        for (int i = 0; i < pos.Length; i++)    
        {
            pos[i] = distance * i;
        }

        if (Input.GetMouseButton(0))  // when clicked
        {
            scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
        } 
        
        else {
            for (int i = 0; i < pos.Length; i++)
            {
                if (scroll_pos < pos[i] + (distance/2) && scroll_pos > pos[i] - (distance/2))
                {
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                }
            }
        }

        for (int i = 0; i < pos.Length; i++)
        {
            if (scroll_pos < pos[i] + (distance/2) && scroll_pos > pos[i] - (distance/2))
            {                    
                // expand selected object
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.1f);
                
                for (int a = 0; a < pos.Length; a++)
                {
                    if (a != i)
                    {
                        transform.GetChild(a).localScale = Vector2.Lerp(transform.GetChild(a).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                    }
                }

                // animal image, text change
                AnimalImage.sprite = Animalimages[i];
                AnimalText.text = Animaltexts[i];
                Homenum.text = Homenums[i];

                // Update TrycntText with the current game's try count
                UpdateTrycntText(i);
            }
        }
    }

    private void UpdateTrycntText(int index)
    {
        int trycnt = 0;

        switch (index)
        {
            case 0:
                trycnt = GameManager.buttonHandler.game1trycnt;
                break;
            case 1:
                trycnt = GameManager.buttonHandler.game2trycnt;
                break;
            case 2:
                trycnt = GameManager.buttonHandler.game3trycnt;
                break;
            case 3:
                trycnt = GameManager.buttonHandler.game4trycnt;
                break;
            case 4:
                trycnt = GameManager.buttonHandler.final1trycnt;
                break;
            default:
                Debug.LogWarning("Invalid index for try count.");
                break;
        }

        TrycntText.text = "Try : " + trycnt + " / 4";
    }
}
