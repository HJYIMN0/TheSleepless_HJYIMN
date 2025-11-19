using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;

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

    [Header("PauseMenu Attributes")]
    [SerializeField] private PauseMenu pauseMenu;

    public bool isPopupVisible => popupPrefab.activeSelf;

    private void Start()
    {
        if (pauseMenu == null)
        {
            Debug.Log("Pause menu missing. Looking...");
            pauseMenu = FindFirstObjectByType<PauseMenu>();
            if (pauseMenu == null)
            {
                Debug.LogWarning("Pause menu still missing after search.");
            }
        }

        if (mainMenu == null)
        {
            Debug.Log("Main menu missing. Looking...");
            mainMenu = FindFirstObjectByType<MainMenu_UI>();
            if (mainMenu == null)
            {
                Debug.LogWarning("Main menu still missing after search.");
            }
        }
    }

    public void CallPopup(string message)
    {
        if (pauseMenu == null)
        {
            Debug.LogWarning("MainMenu or PauseMenu reference is missing in PopupLogic.");
        }
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
        Debug.Log("Popup Message: " + message);
        if (!popupPrefab.activeSelf) //If we are opening it
        {
            if (mainMenu != null)
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
            }
            Debug.Log("PauseMenu reference: " + pauseMenu.gameObject.name);
            if (pauseMenu != null)
            {
                if (message.Equals(pauseMenu.quitToMainMenuText))
                {
                    confirmButton.onClick.AddListener(() =>
                    {
                        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
                    });
                }
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
