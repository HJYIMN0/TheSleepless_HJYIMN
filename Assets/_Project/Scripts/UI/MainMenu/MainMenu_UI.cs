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
    [SerializeField] private CanvasGroup _mainMenuCanvasGroup;
    [SerializeField] private CanvasGroup _optionsCanvasGroup;

    [Header("Popup References")]
    [SerializeField] private GameObject popupPrefab;
    [SerializeField] private PopupLogic _popupLogic;
    [SerializeField] private CanvasGroup _popupCanva;

    [Header("Popup Strings")]
    public string SaveExistsText = "Save File Detected!, Would you like to restart game?";
    public string ExitButtonText = "Are you sure you want to exit?";

    private void Awake()
    {
        DOTween.Init();
        optionsTitle.SetActive(false);
        popupPrefab.SetActive(false);
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
            _popupLogic.CallPopup(SaveExistsText);
        }
        else
        {
            GameManager.Instance.StartNewGame();
        }
    }

    public void LoadGame()
    {
        if (SaveSystem.DoesSaveExist()) 
        {
            GameManager.Instance.LoadMainScene();
        }
    }

    public void QuitGame() => _popupLogic.CallPopup(ExitButtonText);

    public void ExitApplication()
    {
#if UNITY_EDITOR
        Debug.Log("Quit Game called - Application.Quit() will not work in the editor.");
#else
        Application.Quit();
#endif
    }
}
