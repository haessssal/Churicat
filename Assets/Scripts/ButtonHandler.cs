using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    public PopupManager popupManager;
    public StarManager starManager;
    public UIManager uiManager;

    private int game1retrycnt = 0;
    private int game2retrycnt = 0;
    private int game3retrycnt = 0;
    private int game4retrycnt = 0;

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
    }

    public void On202ButtonClick(){
        SceneManager.LoadScene("Game2Scene");
    }
    
    public void On301ButtonClick(){
        SceneManager.LoadScene("Game3Scene");
    }
    
    public void On302ButtonClick(){
        SceneManager.LoadScene("Game4Scene");
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
            uiManager.ShowRandomHint();
        }

        starManager.LoseStar();
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
            game1retrycnt++;
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
