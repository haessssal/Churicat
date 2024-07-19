using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    public void OnStartButtonClick()
    {
        SceneManager.LoadScene("CaseScene"); 
    }

    public void On201ButtonClick(){
        SceneManager.LoadScene("GameScene");
    }

    public void OnCase1ButtonClick(){
        SceneManager.LoadScene("Map1Scene");
    }

    public void OnOptionButtonClick(){

    }

    public void OnPauseButtonClick(){

    }

    public void OnHomeButtonClick(){

    }
}
