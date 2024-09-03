using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public struct Speaker
{
    public Image AnimalImage; 
    public Image dialogBoxImage;
    public TMP_Text textName;
    public TMP_Text textContent;
    public Button readButton;  // 넘어가는 버튼

}

[System.Serializable]
public struct DialogData
{
    public int speakerIndex;
    public string name;
    // [TextArea(3, 5)];
    public string content;
}

public class DialogSystem : MonoBehaviour
{
    [SerializeField]
    private Speaker[] speakers;
    [SerializeField]
    private DialogData[] contents;

    [SerializeField]
    private Image dialogBackground;

    [SerializeField]
    private bool isAutoStart = true;  // 자동시작 여부
    private bool isFirst = true;  // 최초 1회만 호출하기 위함
    private int nowContentIndex = -1;
    private int nowSpeakerIndex = 0;
    private float typingSpeed = 0.1f;
    private bool isTypingEffect = false;
    private UIManager uiManager;

    /*
    [SerializeField]
    private static int dialogcnt = 0;
    */

    private void Start()  // Awake() ??
    {
        uiManager = FindObjectOfType<UIManager>();

        Setup();

        if (isAutoStart)  // && dialogcnt < 1
        {
            UpdateDialog();
        }
    }

    private void Setup()
    {
        for (int i = 0; i <speakers.Length; ++i)
        {
            SetActiveObjects(speakers[i], false);
            speakers[i].AnimalImage.gameObject.SetActive(true);
        }
    }

    public bool UpdateDialog()
    {
        if (isFirst == true)
        {
            Setup();

            if (isAutoStart) SetNextDialog();

            isFirst = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            // 텍스트 타이핑 재생 중일 때 좌클릭 > 효과 정지
            if (isTypingEffect == true)
            {
                isTypingEffect = false;

                // 전체 대화 출력
                StopCoroutine("OnTypingText");
                speakers[nowSpeakerIndex].textContent.text = contents[nowContentIndex].content;
                speakers[nowSpeakerIndex].readButton.gameObject.SetActive(true);

                return false;
            }

            // 대화가 남아있을 경우 다음 대화 진행
            if (contents.Length > nowContentIndex + 1)
            {
                SetNextDialog();
            }

            // 남아있지 않을 경우 모든 오브젝트 비활성화
            else
            {
                // dialogcnt ++;

                for (int i = 0; i < speakers.Length; ++i)
                {
                    SetActiveObjects(speakers[i], false);
                    speakers[i].AnimalImage.gameObject.SetActive(false);
                }

                // uiManager.SetDialogState(false);
                return true;
            }
        }

        return false;
    }

    private void SetNextDialog()
    {
        // 이전 대화 비활성화
        SetActiveObjects(speakers[nowSpeakerIndex], false);

        // 다음 대화 진행
        nowContentIndex ++;

        // 현재 대화 인덱스 설정 / 활성화
        nowSpeakerIndex = contents[nowContentIndex].speakerIndex;
        SetActiveObjects(speakers[nowSpeakerIndex], true);
        speakers[nowSpeakerIndex].textName.text = contents[nowContentIndex].name;
        // speakers[nowSpeakerIndex].textContent.text = contents[nowContentIndex].content;
        StartCoroutine("OnTypingText");

        // uiManager.SetDialogState(true);
    }

    private IEnumerator OnTypingText()
    {
        int index = 0;
        isTypingEffect = true;

        // 텍스트를 한 글자씩 타이핑치듯 재생
        while (index < contents[nowContentIndex].content.Length)
        {
            speakers[nowSpeakerIndex].textContent.text = contents[nowContentIndex].content.Substring(0, index);
            index ++;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTypingEffect = false;
        speakers[nowSpeakerIndex].readButton.gameObject.SetActive(true);
    }

    private void SetActiveObjects(Speaker speaker, bool visible)
    {
        speaker.dialogBoxImage.gameObject.SetActive(visible);
        speaker.textName.gameObject.SetActive(visible);
        speaker.textContent.gameObject.SetActive(visible);
        
        speaker.readButton.gameObject.SetActive(false);
        
        // 화자 알파 값 변경
        Color color = speaker.AnimalImage.color;
        color.a = visible == true ? 1 : 0.2f;
        speaker.AnimalImage.color = color;

        dialogBackground.gameObject.SetActive(visible);
    }
}