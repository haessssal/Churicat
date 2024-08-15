using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    public PopupManager popupManager;
    public StarManager starManager;
    public UIManager uiManager;

    private int game1trycnt = 0;
    private int game2trycnt = 0;
    private int game3trycnt = 0;
    private int game4trycnt = 0;

    public void OnStartButtonClick()
    {
        SceneManager.LoadScene("IntroScene"); 
    }

    public void OnSkipButtonClick()
    {
        SceneManager.LoadScene("CaseScene");
    }

    public void On201ButtonClick(){
        SceneManager.LoadScene("Game1Scene");
        game1trycnt++;
    }

    public void On202ButtonClick(){
        SceneManager.LoadScene("Game2Scene");
        game2trycnt++;
    }
    
    public void On301ButtonClick(){
        SceneManager.LoadScene("Game3Scene");
        game3trycnt++;
    }
    
    public void On302ButtonClick(){
        SceneManager.LoadScene("Game4Scene");
        game4trycnt++;
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
        // TODO: clear current game data

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
            starManager.LoseStar();
            game1trycnt++;
            popupManager.ClosePopup(starretryButton);
            uiManager.init();
        }

        else
        {
            popupManager.OpenPopup("Cantbuy");
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

}
