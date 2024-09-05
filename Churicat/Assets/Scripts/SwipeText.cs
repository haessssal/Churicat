using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SwipeText : MonoBehaviour
{
    public GameObject scrollbar;
    public Button upButton;
    public Button downButton;
    public string nowText;

    float scroll_pos = 1;
    float[] pos;
    float distance;  // 각 pos 사이 거리

    // Start is called before the first frame update
    void Start()
    {
        scrollbar.GetComponent<Scrollbar>().value = 1;  // 시작 시 스크롤 위치 맨 위로
        distance = 1f / (transform.childCount - 1f);

        upButton.onClick.AddListener(MoveUp);
        downButton.onClick.AddListener(MoveDown);
    }

    void Update()
    {
        pos = new float[transform.childCount];

        for (int i = 0; i < pos.Length; i++)    
        {
            pos[i] = distance * i;
        }

        // 자동으로 스크롤 위치 맞춰줌
        for (int i = 0; i < pos.Length; i++)
        {
            if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
            {
                scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.1f);

                // 현재 보여지는 텍스트 nowText
                int reverseIndex = transform.childCount - 1 - i;
                nowText = transform.GetChild(reverseIndex).GetComponent<TMP_Text>().gameObject.name;
                // Debug.Log(nowText);
            }
        }
    }

    void MoveUp()
    {
        for (int i = 0; i < pos.Length; i++)
        {
            if (scroll_pos < pos[i])
            {
                scroll_pos = pos[i];    
                break;
            }
        }
    } 

    void MoveDown()
    {
        for (int i = pos.Length; i > 0; i--)
        {
            if (scroll_pos > pos[i - 1])
            {
                scroll_pos = pos[i - 1];
                break;
            }
        }
    }
}
