using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PopupLogic : MonoBehaviour
{
    [Header("Popup attributes")]
    [SerializeField] private GameObject popupPrefab;
    [SerializeField] private Image popupImage;
    [SerializeField] private CanvasGroup _popupCanvasGroup;
    [SerializeField] private TextMeshProUGUI popupText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private ButtonHoverSO popupSO;

    [Header("Menu Attributes")]
    [SerializeField] MainMenu_UI mainMenu;

    public bool isPopupVisible => popupPrefab.activeSelf;

    public void CallPopup(string message)
    {
        // if is null
        if (string.IsNullOrEmpty(message))
        {
            if (!popupPrefab.activeSelf) //if already off, we return
                return;

            AnimatePopup(); // if on, we close it
            return;
        }

        // Setup listeners
        confirmButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(() => CallPopup(null));
        popupText.text = message;

        if (!popupPrefab.activeSelf) //If we are opening it
        {

            if (message.Equals(mainMenu.SaveExistsText))
            {
                confirmButton.onClick.AddListener(() =>
                {
                    GameManager.Instance.StartNewGame();
                });
            }
            else if (message.Equals(mainMenu.ExitButtonText))
            {
                confirmButton.onClick.AddListener(() =>
                {
                    mainMenu.ExitApplication();
                });
            }
            else
            {
                confirmButton.onClick.AddListener(() =>
                {
                    Debug.Log("Popup confirmed.");
                    CallPopup(null);
                });
            }

            AnimatePopup();
        }
    }

    private void AnimatePopup()
    {
        //open Popup
        if (!popupPrefab.activeSelf)
        {
            popupPrefab.SetActive(true);
            popupImage.transform.localScale = Vector3.zero;
            popupImage.transform.DOScale(popupSO.scaleUpFactor, popupSO.scaleSpeed);
        }
        else
        {
            // Close popup
            popupImage.transform.DOScale(0f, popupSO.scaleSpeed).OnComplete(() =>
            {
                popupPrefab.SetActive(false);
            });
        }
    }


}
