using TMPro;
using UnityEngine;
using Ink.Runtime;

public class InkManager : GenericSingleton<InkManager>
{
    [SerializeField] private GameObject canvaPrefab;
    [SerializeField] private TextMeshProUGUI canvaPrefabText;

    [SerializeField] private string dayVariableNameInInk = "DAY";
    [SerializeField] private string paranoiaVariableNameInInk = "PARANOIA";

    private TextAsset _currentTextAsset;
    private Story _currentStory;
    private GameObject _player;

    public bool IsDialogueActive => canvaPrefab.activeSelf;

    public override bool IsDestroyedOnLoad() => false;
    public override bool ShouldDetatchFromParent() => true;

    // --- Metodi Pubblici di Avvio ---

    public void StartDialogue(TextAsset inkJson)
        => PrepareStory(inkJson, 0, 0, false);

    public void StartDialogue(TextAsset inkJson, int day)
        => PrepareStory(inkJson, day, 0, true);

    public void StartDialogue(TextAsset inkJson, int day, int paranoia)
        => PrepareStory(inkJson, day, paranoia, true);

    // --- Logica Interna ---

    private void PrepareStory(TextAsset textAsset, int day, int paranoia, bool usesVariables)
    {
        if (textAsset == null) return;

        Debug.Log("Story prepared!");

        _currentTextAsset = textAsset;
        _currentStory = new Story(_currentTextAsset.text);

        // Impostazione variabili prima di iniziare la lettura
        if (usesVariables)
        {
            _currentStory.variablesState[dayVariableNameInInk] = day;
            _currentStory.variablesState[paranoiaVariableNameInInk] = paranoia;
        }

        // Gestione Stato Gioco
        ToggleSystems(true);

        // Avvio effettivo del testo
        ContinueDialogue();
    }

    public void ContinueDialogue()
    {
        if (CanStoryContinue())
        {
            // Fondamentale: Assegnare il risultato di Continue() alla UI
            canvaPrefabText.text = _currentStory.Continue();
        }
        else
        {
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        ToggleSystems(false);
        _currentStory = null;
    }

    private void ToggleSystems(bool isActive)
    {
        canvaPrefab.SetActive(isActive);

        if (_player == null) _player = GameObject.FindGameObjectWithTag("Player");

        if (_player != null)
        {
            Debug.Log("Player found! gameObject name is " + _player.gameObject.name);
            PlayerDialogueManager dialogueManager = _player.GetComponentInParent<PlayerDialogueManager>();
            if (dialogueManager == null)
            {
                Debug.Log("Player found but couldnt find PlayerDialogueManager component, make sure it is attached to the player.");
            }
            if (dialogueManager != null) dialogueManager.enabled = isActive;
        }
        else Debug.Log("Couldnt find player in scene, make sure it has the tag 'Player' assigned.");
    }

    public bool CanStoryContinue() => _currentStory != null && _currentStory.canContinue;
}