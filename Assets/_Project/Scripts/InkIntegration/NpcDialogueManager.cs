using UnityEngine;
using UnityEngine.UI;

public class NpcDialogueManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float buttonPositionOffset = 2.5f; // Abbassato un po', 5m è tanto
    [SerializeField] private float distanceToTrigger = 4f;
    [SerializeField] private LayerMask playerLayer;

    [Header("Data")]
    [SerializeField] private TextAsset dialogueAsset;
    [SerializeField] private GameObject talkButtonPrefab; // Assicurati che sia un PREFAB

    // Stato interno
    private GameObject _currentInstance; // Riferimento all'istanza clonata in scena
    private InkManager _inkManager;
    private Transform _mainCamTransform;

    private void Start()
    {
        _inkManager = InkManager.Instance;
        if (Camera.main != null) _mainCamTransform = Camera.main.transform;
    }

    private void Update()
    {
        // 1. Controllo Fisica
        bool isPlayerNear = Physics.CheckSphere(transform.position, distanceToTrigger, playerLayer);

        // 2. Macchina a Stati Semplice
        if (isPlayerNear && !_inkManager.IsDialogueActive)
        {
            Debug.Log("FOund player!");
            // Se il player è vicino MA il bottone non esiste ancora -> Crealo
            if (_currentInstance == null)
            {
                ShowButton();
            }
            // Se esiste, aggiorna la rotazione (Billboard)
            else
            {
                if (_mainCamTransform != null)
                    _currentInstance.transform.rotation = _mainCamTransform.rotation;
            }

            if (!_currentInstance.activeSelf)
            {
                _currentInstance.SetActive(true);
            }
        }
        else
        {
            // Se il player è lontano MA il bottone esiste ancora -> Distruggilo
            if (_currentInstance != null)
            {
                HideButton();
            }
        }
    }

    private void ShowButton()
    {
        // Calcolo posizione
        Vector3 spawnPos = transform.position + (Vector3.up * buttonPositionOffset);

        // Creazione Istanza e salvataggio nel riferimento locale
        _currentInstance = Instantiate(talkButtonPrefab, spawnPos, _mainCamTransform.rotation, transform);

        // Recupero del componente DALL'ISTANZA, non dal prefab
        Button btn = _currentInstance.GetComponentInChildren<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(StartDialogue);
        }
        Debug.Log("Button shown!");
    }

    private void HideButton()
    {
        // Pulizia listener non strettamente necessaria se distruggi, ma buona norma
        if (_currentInstance != null)
        {
            Button btn = _currentInstance.GetComponentInChildren<Button>();
            if (btn != null) btn.onClick.RemoveAllListeners();

            Destroy(_currentInstance);
            _currentInstance = null; // Fondamentale resettare a null
        }
    }

    public void StartDialogue()
    {
        if (dialogueAsset != null)
        {
            _inkManager.StartDialogue(dialogueAsset);
            // Opzionale: Nascondi il bottone quando inizia il dialogo
            HideButton();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanceToTrigger);
    }
}