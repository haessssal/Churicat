using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.IO;
using System;
using System.Linq;

public class ButtonHandler : MonoBehaviour
{
    public int game1trycnt = 0;
    public int game2trycnt = 0;
    public int game3trycnt = 0;
    public int game4trycnt = 0;
    public int final1trycnt = 0;

    public PopupManager popupManager;
    public StarManager starManager;
    public UIManager uiManager;
    public GameManager gameManager;
    public DialogSystem dialogSystem;
    public SwipeText swipeText;
    public FinalClues finalClues;

    public bool isRight;

    public SwipeText scrollView1SwipeText;
    public SwipeText scrollView2SwipeText;


    public TMP_Text IsLockedText;
    public TMP_Text CantRetryText;
    public TMP_Text WhoText;

    public GameObject NextButton;
    private string animal;

    public void OnStartButtonClick()
    {
        SceneManager.LoadScene("CaseScene"); 
    }

    public void OnSkipButtonClick()
    {
        SceneManager.LoadScene("Map1Scene");
    }

    public void On201ButtonClick()
    {    
        if (game1trycnt < 4)
        {
            SceneManager.LoadScene("Game1Scene");
            game1trycnt++;
            gameManager.Save();
        }
        
        else
        {
            popupManager.OpenPopup("CantRetry");
        }
    }

    public void On202ButtonClick(){
        if (game2trycnt < 4)
        {
            SceneManager.LoadScene("Game2Scene");
            game2trycnt++;
            // Debug.Log($"game2trycnt: {game2trycnt}");
            gameManager.Save();
        }
        
        else
        {
            popupManager.OpenPopup("CantRetry");
        }
    }
    
    public void On301ButtonClick(){
        if (game3trycnt < 4)
        {
            SceneManager.LoadScene("Game3Scene");
            game3trycnt++;
            gameManager.Save();
        }
        
        else
        {
            popupManager.OpenPopup("CantRetry");
        }
    }
    
    public void On302ButtonClick(){
        if (game4trycnt < 4)
        {
            SceneManager.LoadScene("Game4Scene");
            game4trycnt++;
            gameManager.Save();
        }
        
        else
        {
            popupManager.OpenPopup("CantRetry");
        }
    }

    public void OnFinal1ButtonClick()
    {
        if (game1trycnt >= 1 && game2trycnt >= 1 && game3trycnt >= 1 && game4trycnt >= 1 && final1trycnt < 4)
        {
            SceneManager.LoadScene("Final1Scene");
        }

        else 
        {
            popupManager.OpenPopup("CantRetry");
        }  
    }

    private IEnumerator ShowLockedText()
    {
        IsLockedText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        IsLockedText.gameObject.SetActive(false);
    }

    public void OnCase1ButtonClick(){
        SceneManager.LoadScene("IntroScene");
        // 원래는 introscene
    }

    public void OnCase2Button()
    {
        popupManager.OpenPopup("CantRetry");
    }

    public void OnCase3Button()
    {
        popupManager.OpenPopup("CantRetry");
    }

    public void OnOptionButtonClick()
    {
        popupManager.OpenPopup("Option");
    }

    public void OnPauseButtonClick()
    {
        popupManager.OpenPopup("Pause");
    }

    public void OnHomeButtonClick()
    {
        // popupManager.OpenPopup("Home");
        SceneManager.LoadScene("CaseScene");
    }

    public void OnBackButtonClick()
    {
        SceneManager.LoadScene("Map1Scene");
    }

    public void OnGetoutButtonClick()
    {
        // 해당 게임에서 얻은 clue data 지우기
        string path = Path.Combine(Application.dataPath, "ClueData.json");

        if (File.Exists(path))
        {
            try
            {
                string existingJson = File.ReadAllText(path);
                ClueDataList existingClueDataList = JsonUtility.FromJson<ClueDataList>(existingJson);

                List<ClueData> cluesToRemove = uiManager.GetFoundClues();

                existingClueDataList.clues = existingClueDataList.clues
                    .Where(clue => !cluesToRemove.Any(c => c.clueID == clue.clueID))
                    .ToList();

                // JSON 파일에 업데이트된 데이터를 다시 저장
                string updatedJson = JsonUtility.ToJson(existingClueDataList, true);
                File.WriteAllText(path, updatedJson);
            }

            catch (Exception e)
            {
                Debug.LogError($"Failed to update ClueData.json: {e.Message}");
            }
        }

        // move scene
        SceneManager.LoadScene("Map1Scene");
    }

    public void OnHintButtonClick()
    {
        popupManager.OpenPopup("Hint");
    }

    public void OnCloseButtonClick(GameObject closeButton)
    {
        popupManager.ClosePopup(closeButton);
    }

    public void OnStarHintButtonClick()
    {
        if (starManager.StarInt >= 1)
        {
            if (uiManager.ShowRandomHint())
            {
                starManager.LoseStar();
            }
            
            else
            {
                popupManager.OpenPopup("Cantbuy");
            }
        }

        else
        {
            popupManager.OpenPopup("Cantbuy");
        }
    }

    public void OnAdHintButtonClick()
    {
        // TODO
    }

    public void OnRetryButtonClick(GameObject retryButton)
    {
        popupManager.OpenPopup("Retry");
        // popupManager.ClosePopup(retryButton);
        StartCoroutine(CloseRetryPopupWithDelay(retryButton));
    }

    private IEnumerator CloseRetryPopupWithDelay(GameObject retryButton)
    {
        yield return new WaitForSeconds(0.5f); // 딜레이
        popupManager.ClosePopup(retryButton); // 팝업을 닫는다
    }

    public void OnGet3starButtonClick()
    {
        starManager.Get3Star();
    }

    public void OnGet2starButtonClick()
    {
        starManager.Get2Star();
    }

    public void OnGet1starButtonClick()
    {
        starManager.Get1Star();
    }

    public void OnStarRetryButtonClick(GameObject starretryButton)
    {
        if (starManager.StarInt >= 1)
        {
            if (CanRetry())
            {
                starManager.LoseStar();
                IncreaseTryCnt();
                popupManager.ClosePopup(starretryButton);
                uiManager.init();
            }

            else
            {
                popupManager.OpenPopup("CantRetry");
            }
            
        }

        else
        {
            popupManager.OpenPopup("Cantbuy");
        }   
    }

    /*
    private IEnumerator ShowCantRetryText()
    {
        CantRetryText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        CantRetryText.gameObject.SetActive(false);
    }
    */

    private void IncreaseTryCnt()
    {
        string nowSceneName = SceneManager.GetActiveScene().name;

        switch (nowSceneName)
        {
            case "Game1Scene":
                game1trycnt++;
                break;
            case "Game2Scene":
                game2trycnt++;
                break;
            case "Game3Scene":
                game3trycnt++;
                break;
            case "Game4Scene":
                game4trycnt++;
                break;
            default:
                Debug.LogWarning("Unknown scene: " + nowSceneName);
                break;
        }        
    }

    public bool CanRetry()
    {
        string nowSceneName = SceneManager.GetActiveScene().name;

        switch (nowSceneName)
        {
            case "Game1Scene":
                return game1trycnt <= 4;
            case "Game2Scene":
                return game2trycnt <= 4;
            case "Game3Scene":
                return game3trycnt <= 4;
            case "Game4Scene":
                return game4trycnt <= 4;
            default:
                Debug.LogWarning("Unknown scene: " + nowSceneName);
                return false;
        }
    }

    public void OnAdRetryButtonClick()
    {
        // TODO
    }

    public void OnResetButtonClick()
    {
        GameManager.ClearAllSaveData();
    }

    public void OnInvenButtonClick()
    {
        popupManager.OpenPopup("Inventory");
    }

    public void OnDochiClick()
    {
        WhoText.text = "고슴도치";
        WhoText.gameObject.SetActive(true);
        animal = "Dochi";
        NextButton.SetActive(true);
        finalClues.DisplayCluesInFinal("Dochi");
    }

    public void OnDogClick()
    {
        WhoText.text = "강아지";
        WhoText.gameObject.SetActive(true);
        animal = "Dog";
        NextButton.SetActive(true);
        finalClues.DisplayCluesInFinal("Dog");
    }

    public void OnHamClick()
    {
        WhoText.text = "햄스터";
        WhoText.gameObject.SetActive(true);
        animal = "Ham";
        NextButton.SetActive(true);
        finalClues.DisplayCluesInFinal("Ham");
    }

    public void OnNextButtonClick()
    {
        switch (animal)
        {
            case "Dochi":
                popupManager.OpenPopup("Dochi");
                break;
            case "Dog":
                popupManager.OpenPopup("Dog");
                break;
            case "Ham":
                popupManager.OpenPopup("Ham");
                break;
            default:
                Debug.LogWarning("Unknown animal: " + animal);
                break;
        }
    }

    public void OnReadButtonClick()
    {
        dialogSystem.UpdateDialog();
    }

    public void OnDecisionButtonClick()
    {
        if (animal == "Ham" && scrollView1SwipeText.nowText == "WordCorrect1" && scrollView2SwipeText.nowText == "WordCorrect2  ")
        {
            isRight = true;
            popupManager.OpenPopup("IsRight");
        }

        else
        {
            isRight = false;
            popupManager.OpenPopup("IsNotRight");
        }

        Debug.Log("isRight: " + isRight);
    }
}
