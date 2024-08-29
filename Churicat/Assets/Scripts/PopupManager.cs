using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    public Button Background;

    public GameObject OptionPopup;
    public GameObject HintPopup;
    public GameObject PausePopup;
    public GameObject HomePopup;
    public GameObject Over1Popup;
    public GameObject Over2Popup;
    public GameObject Over3Popup;
    public GameObject RetryPopup;
    public GameObject CantbutPopup;
    public GameObject InventoryPopup;

    public GameObject DochiPopup;

    public void Start()
    {
        Background.gameObject.SetActive(false);

        OptionPopup.SetActive(false);
        HintPopup.SetActive(false);
        PausePopup.SetActive(false);
        HomePopup.SetActive(false);
        Over1Popup.SetActive(false);
        RetryPopup.SetActive(false);
        CantbutPopup.SetActive(false);
        InventoryPopup.SetActive(false);
        DochiPopup.SetActive(false);
    }

    public void OpenPopup(string popupName)
    {
        Background.gameObject.SetActive(true);

        switch (popupName)
        {
            case "Option":
                OptionPopup.SetActive(true);
                break;
            case "Hint":
                HintPopup.SetActive(true);
                break;
            case "Pause":
                PausePopup.SetActive(true);
                break;
            case "Home":
                HomePopup.SetActive(true);
                break;
            case "Over1":
                Over1Popup.SetActive(true);
                break;
            case "Over2":
                Over2Popup.SetActive(true);
                break;
            case "Over3":
                Over3Popup.SetActive(true);
                break;
            case "Retry":
                RetryPopup.SetActive(true);
                break;
            case "Cantbuy":
                CantbutPopup.SetActive(true);
                break;
            case "Inventory":
                InventoryPopup.SetActive(true);
                break;
            case "Dochi":
                DochiPopup.SetActive(true);
                break;
        }
    }

    public void ClosePopup(GameObject closeButton)
    {
        closeButton.transform.parent.gameObject.SetActive(false);
        Background.gameObject.SetActive(false);
    }
}
