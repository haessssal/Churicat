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
    public PopupManager popupManager;
    public StarManager starManager;
    public UIManager uiManager;
    public GameManager gameManager;

    public TMP_Text IsLockedText;
    public TMP_Text CantRetryText;
    public TMP_Text WhoText;

    public GameObject NextButton;
    private string animal;

    public int game1trycnt = 0;
    public int game2trycnt = 0;
    public int game3trycnt = 0;
    public int game4trycnt = 0;

    public void OnStartButtonClick()
    {
        SceneManager.LoadScene("IntroScene"); 
    }

    public void OnSkipButtonClick()
    {
        SceneManager.LoadScene("CaseScene");
    }

    public void On201ButtonClick()
    {    
        if (game1trycnt < 4)
        {
            SceneManager.LoadScene("Game1Scene");
            game1trycnt++;
        }
        
        else
        {
            StartCoroutine(ShowCantRetryText());
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
            StartCoroutine(ShowCantRetryText());
        }
    }
    
    public void On301ButtonClick(){
        if (game3trycnt < 4)
        {
            SceneManager.LoadScene("Game3Scene");
            game3trycnt++;
        }
        
        else
        {
            StartCoroutine(ShowCantRetryText());
        }
    }
    
    public void On302ButtonClick(){
        if (game4trycnt < 4)
        {
            SceneManager.LoadScene("Game4Scene");
            game4trycnt++;
        }
        
        else
        {
            StartCoroutine(ShowCantRetryText());
        }
    }

    public void OnFinal1ButtonClick()
    {
        if (game1trycnt >= 1 && game2trycnt >= 1 && game3trycnt >= 1 && game4trycnt >= 1)
        {
            SceneManager.LoadScene("Final1Scene");
        }

        else 
        {
            StartCoroutine(ShowLockedText());
        }  
    }

    private IEnumerator ShowLockedText()
    {
        IsLockedText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        IsLockedText.gameObject.SetActive(false);
    }

    public void OnCase1ButtonClick(){
        SceneManager.LoadScene("Map1Scene");
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
                    .Where(clue => !cluesToRemove.Any(c => c.clueName == clue.clueName))
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
        if (starManager.StarInt >= 2)
        {
            starManager.LoseStar();
            uiManager.ShowRandomHint();
        }
    }

    public void OnAdHintButtonClick()
    {
        // TODO
    }

    public void OnRetryButtonClick(GameObject retryButton)
    {
        popupManager.OpenPopup("Retry");
        popupManager.ClosePopup(retryButton);
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
        if (starManager.StarInt >= 2)
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
                StartCoroutine(ShowCantRetryText());
            }
            
        }

        else
        {
            popupManager.OpenPopup("Cantbuy");
        }   
    }

    private IEnumerator ShowCantRetryText()
    {
        CantRetryText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        CantRetryText.gameObject.SetActive(false);
    }

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
                return game1trycnt < 4;
            case "Game2Scene":
                return game2trycnt < 4;
            case "Game3Scene":
                return game3trycnt < 4;
            case "Game4Scene":
                return game4trycnt < 4;
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
        WhoText.text = "DOCHI";
        WhoText.gameObject.SetActive(true);
        animal = "Dochi";
        NextButton.SetActive(true);
    }

    public void OnDogClick()
    {
        WhoText.text = "DOG";
        WhoText.gameObject.SetActive(true);
        animal = "Dog";
        NextButton.SetActive(true);
    }

    public void OnHamClick()
    {
        WhoText.text = "HAM";
        WhoText.gameObject.SetActive(true);
        animal = "Ham";
        NextButton.SetActive(true);
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
}
