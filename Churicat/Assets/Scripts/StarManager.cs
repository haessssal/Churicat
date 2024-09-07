using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;    

public class StarManager : MonoBehaviour
{
    public int StarInt = 10;
    public TMP_Text StarText;
    // obtained star num variable

    public GameManager gameManager;
    public PopupManager popupManager;

    private void Awake()
    {
        if (StarText == null)
        {
            StarText = GameObject.Find("StarText").GetComponent<TMP_Text>();
        }
    }

    public void Get3Star()
    {
        StarInt += 3;
        UpdateStarText();
        gameManager.Save();
    }
     
    public void Get2Star()
    {
        StarInt += 2;
        UpdateStarText();
        gameManager.Save();
    }

    public void Get1Star()
    {
        StarInt += 1;
        UpdateStarText();
        gameManager.Save();
    }

    public void LoseStar()
    {
        if (StarInt >= 1)
        {
            StarInt -= 1;
            if (StarInt < 0) StarInt = 0; 
            UpdateStarText();
            gameManager.Save();
        }

        else
        {
            popupManager.OpenPopup("Cantbuy");
        }
    }

    public void UpdateStarText()
    {
        if (StarText != null)
        {
            StarText.text = $"x {StarInt}";
        }
    }

}
