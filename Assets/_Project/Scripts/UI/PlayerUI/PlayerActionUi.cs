using UnityEngine;
using UnityEngine.UI;


public class PlayerActionUi : MonoBehaviour
{
    [Header("Player attributes")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private CameraController cameraController;

    [Header("Buttons arrays")]
    [SerializeField] private Button[] deckButtons;
    [SerializeField] private Button[] roomButtons;
    [SerializeField] private Button[] storehouseButtons;

    private PlayerActionSystem _playerActionSystem;

   
    private void Start()
    {
        _playerActionSystem = PlayerActionSystem.Instance;

        if (playerController == null ) 
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponentInParent<PlayerController>();

        if (playerController != null)
            playerController.OnPlayerBoatLocationChange += SetButtons;



        if (cameraController == null || playerController == null)
        {
            Debug.LogWarning("Something ain't right");
            if (cameraController == null)
            {
                Debug.Log("Camera missing");
                cameraController = FindAnyObjectByType<CameraController>();
            }
            if (playerController == null)
            {
                Debug.Log("Player missing!");
            }
        }

        if (cameraController != null)
        {
            Debug.Log("Found!");
            cameraController.onBoatLocationsChanged += SetButtons;
        }
            

        SetButtons(BoatLocations.Deck);
    }

    public void Sleep() => _playerActionSystem?.Sleep();
    public void Work() => _playerActionSystem?.Work();
    public void Repair() => _playerActionSystem?.Repair();
    public void Wash() => _playerActionSystem?.Wash();
    public void Eat() => _playerActionSystem?.Eat();
    public void GoToDeck() => _playerActionSystem?.GoToDeck();
    public void GoToRoom() => _playerActionSystem?.GoToRoom();
    public void GoToStoreHouse() => _playerActionSystem?.GoToStorehouse();

    public void SetButtons(BoatLocations location)
    {

        foreach (Button b in roomButtons)       b.interactable = false;
        foreach(Button b in  deckButtons)       b.interactable = false;
        foreach (Button b in storehouseButtons) b.interactable = false;

        switch (location) 
        {
            case BoatLocations.Room:
                foreach (Button b in roomButtons) b.interactable = true;
                break;

            case BoatLocations.Storehouse:
                foreach (Button b in storehouseButtons) b.interactable = true;
                break;

            case BoatLocations.Deck:
                foreach (Button b in deckButtons) b.interactable = true; 
                break;
        }        
    }

    private void OnDestroy()
    {
        if (playerController != null)
        playerController.OnPlayerBoatLocationChange -= SetButtons;

        if (cameraController != null)
        cameraController.onBoatLocationsChanged -= SetButtons;
    }
}
