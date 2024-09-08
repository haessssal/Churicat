using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneChanger : MonoBehaviour
{
    public ButtonHandler buttonHandler;

    public void ChangeGame1Scene(string sceneName)
    {
        if (buttonHandler.game1trycnt < 4)
        {
            LoadSceneManager.LoadScene("Game1Scene");
        }
    }

    public void ChangeGame2Scene(string sceneName)
    {
        if (buttonHandler.game2trycnt < 4)
        {
            LoadSceneManager.LoadScene("Game2Scene");
        }
    }

    public void ChangeGame3Scene(string sceneName)
    {
        if (buttonHandler.game3trycnt < 4)
        {
            LoadSceneManager.LoadScene("Game3Scene");
        }
    }

    public void ChangeGame4Scene(string sceneName)
    {
        if (buttonHandler.game4trycnt < 4)
        {
            LoadSceneManager.LoadScene("Game4Scene");
        }
    }

    public void ChangeFinal1Scene(string sceneName)
    {
        if (buttonHandler.final1trycnt < 4 && buttonHandler.game1trycnt >= 1 && buttonHandler.game2trycnt >= 1 && buttonHandler.game3trycnt >= 1 && buttonHandler.game4trycnt >= 1)
        {
            LoadSceneManager.LoadScene("Final1Scene");
        }
    }
}
