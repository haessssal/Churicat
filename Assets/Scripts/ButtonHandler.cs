using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    public PopupManager popupManager;
    public StarManager starManager;
    public UIManager uiManager;

    public void OnStartButtonClick()
    {
        SceneManager.LoadScene("IntroScene"); 
    }

    public void OnSkipButtonClick()
    {
        SceneManager.LoadScene("CaseScene");
    }

    public void On201ButtonClick(){
        SceneManager.LoadScene("GameScene");
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

    public void OnStarRetryButtonClick(GameObject starretryButton)
    {
        starManager.LoseStar();
        popupManager.ClosePopup(starretryButton);
        uiManager.init();
    }

    public void OnAdRetryButtonClick()
    {
        // TODO
    }
}
