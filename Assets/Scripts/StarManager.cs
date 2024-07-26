using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;    

public class StarManager : MonoBehaviour
{
    public static int StarInt;
    public static TMP_Text StarText;
    // obtained star num variable

    private void Awake()
    {
        if (StarText == null)
        {
            StarText = GameObject.Find("StarText").GetComponent<TMP_Text>();
        }
    }

    void Start()
    {
        PlayerPrefs.SetInt("Star", StarInt);
        UpdateStarText();
    }

    void Update()
    {
        PlayerPrefs.SetInt("Star", StarInt);
        UpdateStarText();
    }

    public void Get3Star()
    {
        StarInt += 3;
        UpdateStarText();
    }
     
    public void Get2Star()
    {
        StarInt += 2;
        UpdateStarText();
    }

    public void Get1Star()
    {
        StarInt += 1;
        UpdateStarText();
    }

    public void LoseStar()
    {
        StarInt -= 2;
        UpdateStarText();
    }

    private void UpdateStarText()
    {
        if (StarText != null)
        {
            StarText.text = $"STAR x {StarInt}";
        }
    }
}
