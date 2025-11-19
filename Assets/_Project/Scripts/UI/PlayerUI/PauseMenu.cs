using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup pauseMenuCanvasGroup;

    public string quitToMainMenuText = "Are you sure you want to quit to main menu? Unsaved progress will be lost.";
    public PopupLogic _popupLogic;

    private TimeManager _timeManager => TimeManager.Instance;

    // Proprietà helper per capire se il gioco è in pausa
    private bool isPaused => pauseMenuCanvasGroup.alpha > 0f;

    void Start()
    {
        if (_popupLogic == null)
        {
            // ... (tua logica di ricerca PopupLogic invariata) ...
            _popupLogic = FindFirstObjectByType<PopupLogic>(FindObjectsInactive.Include);
            if (_popupLogic == null)
            {
                _popupLogic = new GameObject("PopupLogic_Temp").AddComponent<PopupLogic>();
            }
        }

        // Assicuriamoci che il gioco parta "non in pausa"
        ResumeGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CallPauseMenu();
        }
    }

    public void CallPauseMenu()
    {
        // QUI C'ERA L'ERRORE: LOGICA INVERTITA
        if (isPaused)
        {
            // Se è in pausa (aperto), dobbiamo riprendere (chiudere)
            ResumeGame();
        }
        else
        {
            // Se non è in pausa (chiuso), dobbiamo mettere in pausa (aprire)
            PauseGame();
        }
    }

    private void PauseGame()
    {
        pauseMenuCanvasGroup.alpha = 1f;
        pauseMenuCanvasGroup.interactable = true;
        pauseMenuCanvasGroup.blocksRaycasts = true;

        _timeManager.SetCurrentTimeState(TimeState.Paused);
    }
    private void ResumeGame()
    {
        pauseMenuCanvasGroup.alpha = 0f;
        pauseMenuCanvasGroup.interactable = false;
        pauseMenuCanvasGroup.blocksRaycasts = false;

        _timeManager.SetCurrentTimeState(TimeState.Running);
    }

    public void QuitToMainMenu()
    {
        _popupLogic.CallPopup(quitToMainMenuText);
    }
}