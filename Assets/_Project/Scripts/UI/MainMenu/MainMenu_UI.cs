using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainMenu_UI : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private GameObject mainMenuTitle;
    [SerializeField] private GameObject optionsTitle;

    [Header("Popup attributes")]
    [SerializeField] private GameObject popupPrefab;
    [SerializeField] private TextMeshProUGUI popupText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private ButtonHoverSO popupSO;

    [Header("Popup Strings")]
    [SerializeField] private string SaveExistsText = "Save File Detected!, Would you like to restart game?";
    [SerializeField] private string ExitButtonText = "Are you sure you want to exit?";

    private CanvasGroup _mainMenuCanvasGroup;
    private CanvasGroup _optionsCanvasGroup;
    private CanvasGroup _popupCanvasGroup;

    private void Awake()
    {
        _mainMenuCanvasGroup = mainMenuTitle.GetComponent<CanvasGroup>();
        _optionsCanvasGroup = optionsTitle.GetComponent<CanvasGroup>();
    }
    public void Start()
    {
        ShowMainMenu();
    }

    public void CallMenu()
    {
        if (mainMenuTitle.activeSelf)
        {
            ShowOptionsMenu();
        }
        else
        {
            ShowMainMenu();
        }
    }

    private void ShowOptionsMenu()
    {
        optionsTitle.SetActive(true); //

        _mainMenuCanvasGroup.alpha = 0f;
        _mainMenuCanvasGroup.interactable = false;
        _mainMenuCanvasGroup.blocksRaycasts = false;
        _optionsCanvasGroup.alpha = 1f;
        _optionsCanvasGroup.interactable = true;
        _optionsCanvasGroup.blocksRaycasts = true;

        mainMenuTitle.SetActive(false); //
    }
    private void ShowMainMenu()
    {
        mainMenuTitle.SetActive(true); //

        _mainMenuCanvasGroup.alpha = 1f;
        _mainMenuCanvasGroup.interactable = true;
        _mainMenuCanvasGroup.blocksRaycasts = true;
        _optionsCanvasGroup.alpha = 0f;
        _optionsCanvasGroup.interactable = false;
        _optionsCanvasGroup.blocksRaycasts = false;

        optionsTitle.SetActive(false); //
    }

    public void StartGame()
    {
        if (SaveSystem.DoesSaveExist())
        {
            CallPopup(SaveExistsText);
        }
        else
        {
            GameManager.Instance.StartNewGame();
        }
    }

    public void QuitGame() => CallPopup(ExitButtonText);

    private void CallPopup(string message)
    {
        if (message == null)
        {
            StartCoroutine(AnimatePopup());
            return;
        }

        popupPrefab.SetActive(!popupPrefab.activeSelf);

        if (popupPrefab.activeSelf)
        {
            popupText.text = message;
            if (message.Equals(SaveExistsText))
            {
                confirmButton.onClick.RemoveAllListeners();
                cancelButton.onClick.RemoveAllListeners();

                confirmButton.onClick.AddListener(() =>
                {
                    GameManager.Instance.StartNewGame();
                });

                cancelButton.onClick.AddListener(() => CallPopup(null));

            }
            else if (message.Equals(ExitButtonText))
            {

            }

        }
    }

    public IEnumerator AnimatePopup()
    {
        //open Popup
        if (!popupPrefab.activeSelf)
        {
            popupPrefab.transform.localScale = Vector3.zero;
            popupPrefab.SetActive(true);

            while (popupPrefab.transform.localScale.magnitude < popupSO.scaleUpFactor)
            {

                //popupPrefab.transform.localScale.DOScale(popupSO.scaleUpFactor, popupSO.scaleSpeed);
                yield return null;
            }

        }
        else
        {
            // Apri popup
            popupPrefab.SetActive(true);
        }
        yield return null;
        
    }
}
