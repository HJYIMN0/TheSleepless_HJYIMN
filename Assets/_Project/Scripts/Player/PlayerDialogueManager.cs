using UnityEngine;

public class PlayerDialogueManager : MonoBehaviour
{
    private PlayerController _playerController;
    private InkManager _inkManager;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }

    private void Start()
    {
        _inkManager = InkManager.Instance;
        _playerController.enabled = true;
        this.enabled = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _inkManager?.ContinueDialogue();
        }
    }

    public void SwapPlayerState()
    {
        if (_playerController != null)
        {
            _playerController.enabled = !_playerController.enabled;
            this.enabled = !_playerController.enabled;
        }
    }

    private void OnEnable()
    {
        Debug.Log("playerDialogueManager is being enabled");
        if (_playerController != null && _playerController.enabled) 
        {
            Debug.Log("Disabling player controller");
            _playerController.enabled = false;
        }
    }

    private void OnDisable()
    {
        if (_playerController != null && !_playerController.enabled)
        {
            _playerController.enabled = true;
        }
    }
}
