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
    public GameObject Over1Popup;
    public GameObject Over2Popup;
    public GameObject Over3Popup;
    public GameObject RetryPopup;
    public GameObject CantbuyPopup;
    public GameObject InventoryPopup;
    public GameObject CantRetryPopup;

    public GameObject DochiPopup;
    public GameObject DogPopup;
    public GameObject HamPopup;
    public GameObject IsRightPopup;
    public GameObject IsNotRightPopup;

    private UIManager uiManager;

    public void Start()
    {
        Background.gameObject.SetActive(false);
        uiManager = FindObjectOfType<UIManager>();

        OptionPopup.SetActive(false);
        HintPopup.SetActive(false);
        PausePopup.SetActive(false);
        Over1Popup.SetActive(false);
        RetryPopup.SetActive(false);
        CantbuyPopup.SetActive(false);
        InventoryPopup.SetActive(false);
        DochiPopup.SetActive(false);
        CantRetryPopup.SetActive(false);
        DogPopup.SetActive(false);
        HamPopup.SetActive(false);
        IsRightPopup.SetActive(false);
        IsNotRightPopup.SetActive(false);
    }

    public void OpenPopup(string popupName)
    {
        Background.gameObject.SetActive(true);
        uiManager.SetPopupState(true);

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
                CantbuyPopup.SetActive(true);
                break;
            case "Inventory":
                InventoryPopup.SetActive(true);
                break;
            case "Dochi":
                DochiPopup.SetActive(true);
                break;
            case "CantRetry":
                CantRetryPopup.SetActive(true);
                break;
            case "Dog":
                DogPopup.SetActive(true);
                break;
            case "Ham":
                HamPopup.SetActive(true);
                break;
            case "IsRight":
                IsRightPopup.SetActive(true);
                break;
            case "IsNotRight":
                IsNotRightPopup.SetActive(true);
                break;
        }
    }

    public void ClosePopup(GameObject closeButton)
    {
        closeButton.transform.parent.gameObject.SetActive(false);
        Background.gameObject.SetActive(false);
        uiManager.SetPopupState(false);
    }
}
