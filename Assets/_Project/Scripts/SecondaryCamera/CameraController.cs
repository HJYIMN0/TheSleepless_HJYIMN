using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using DG.Tweening;
using UnityEngine.Rendering;
public class CameraController : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private RawImage cameraDisplay;

    [Header("Camera Settings")]
    [SerializeField] private Camera roomCam;
    [SerializeField] private Camera storehouseCam;


    [Header("Player Settings")]
    [SerializeField] private GameObject player;
    [SerializeField] private Transform roomTargetPos;
    [SerializeField] private Transform storehouseTargetPos;

    [Header("Animation Settings")]
    [SerializeField] private float scaleDuration = 0.3f;
    [SerializeField] private Ease scaleEase = Ease.OutBack;

    private Vector3 _playerOriginalPosition;
    private Quaternion _playerOriginalRotation;
    private NavMeshAgent _playerNavMeshAgent;
    private RenderTexture _renderTexture;

    private bool _isActive = false;
    private Camera _activeCam;

    private PlayerLocations _playerLocation = PlayerLocations.Deck;

    private void Awake()
    {
        // Ottieni il riferimento al NavMeshAgent del player
        if (player != null)
        {
            _playerNavMeshAgent = player.GetComponent<NavMeshAgent>();
        }
    }

    private void Start()
    {
        roomCam.gameObject.SetActive(false);
        storehouseCam.gameObject.SetActive(false);
        uiPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            WarpToRoom();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        { 
            WarpToStorehouse();
        }
    }

    public void WarpToDeck()
    {
        DisableCamera();
        WarpPlayer(_playerOriginalPosition);
        _playerLocation = PlayerLocations.Deck;
        _isActive = false;
    }

    public void WarpToRoom()
    {
        ToggleCamera(PlayerLocations.Room, roomTargetPos.transform.position, roomCam);
    }

    public void WarpToStorehouse()
    {

        ToggleCamera(PlayerLocations.Storehouse, storehouseTargetPos.transform.position, storehouseCam);
    }
    public void ToggleCamera(PlayerLocations targetLocation, Vector3 targetPosition, Camera targetCamera)
    {
        if (_isActive)
        {
            DisableCamera();
            Debug.Log(_playerOriginalPosition);
        }
        else 
        {
            EnableCamera(targetCamera, targetPosition, targetLocation);
        }
    }


    private void EnableCamera(Camera cam, Vector3 targetPos, PlayerLocations location)
    {
        _isActive = true;
        _activeCam = cam;
        // Salva la posizione originale del player
        WarpPlayer(targetPos);


        // Attiva il pannello UI
        if (uiPanel != null)
        {
            uiPanel.SetActive(true);

            // Animazione di scala da 0
            uiPanel.transform.localScale = Vector3.zero;
            uiPanel.transform.DOScale(Vector3.one, scaleDuration).SetEase(scaleEase);
        }

        // Attiva la seconda camera
        if (cam != null)
        {
            cam.gameObject.SetActive(true);
            // Crea o assegna render texture se necessario
            if (_renderTexture == null && cameraDisplay != null)
            {
                // Usa le dimensioni della RawImage
                RectTransform rt = cameraDisplay.GetComponent<RectTransform>();
                int width = Mathf.Max(256, (int)rt.rect.width);
                int height = Mathf.Max(256, (int)rt.rect.height);


                _renderTexture = new RenderTexture(width, height, 24);
            }

            cam.targetTexture = _renderTexture;
            cam.enabled = true;

            // Assegna la render texture alla RawImage
            if (cameraDisplay != null)
            {
                cameraDisplay.texture = _renderTexture;
                Debug.Log("Texture assegnata alla RawImage");
            }
        }
    }

    private void DisableCamera()
    {
        _isActive = false;
        // Riporta il player alla posizione originale
        WarpPlayer(_playerOriginalPosition);

        // Anima la scala a 0 e poi disattiva
        if (uiPanel != null)
        {
            uiPanel.transform.DOScale(Vector3.zero, scaleDuration)
                .SetEase(Ease.InBack)
                .OnComplete(() => uiPanel.SetActive(false));
        }

        // Disattiva la seconda camera
        if (_activeCam != null)
        {
            _activeCam.enabled = false;
            _activeCam.gameObject.SetActive(false);
        }
    }

    private void WarpPlayer(Vector3 playerFinalPos)
    {
        if (player != null)
        {
            if (_playerLocation == PlayerLocations.Deck)
            {
                _playerOriginalPosition = player.transform.position;
                _playerOriginalRotation = player.transform.rotation;
            }
            _playerNavMeshAgent.Warp(playerFinalPos);
        }
    }

    private void OnDestroy()
    {
        if (uiPanel != null)
        {
            uiPanel.transform.DOKill();
        }
    }

    public enum PlayerLocations 
    {
        Deck,
        Room,
        Storehouse
    }
}
