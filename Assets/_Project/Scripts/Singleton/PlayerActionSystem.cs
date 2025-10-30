using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerActionSystem : GenericSingleton<PlayerActionSystem>
{
    public override bool IsDestroyedOnLoad() => true;
    public override bool ShouldDetatchFromParent() => true;

    private GameManager _gm;
    [SerializeField] private PlayerController player;

    [Header("Distance settings")]
    [SerializeField] private float distance = 2f;

    [Header("Energy Settings")]
    [SerializeField] private int sleepEnergyRecover = 20;
    [SerializeField] private int timeToSleep = 8;
    [SerializeField] private Transform sleepLocation;

    [Header("Hunger Settings")]
    [SerializeField] private int energyToEat = 10;
    [SerializeField] private int hungerIncrease = 20;
    [SerializeField] private int timeToEat = 1;
    [SerializeField] private Transform foodLocation;

    [Header("Hygiene Settings")]
    [SerializeField] private int energyToWash = 30;
    [SerializeField] private int hygieneIncrease = 40;
    [SerializeField] private int timeToWash = 1;
    [SerializeField] private Transform washLocation;

    [Header("Direction Settings")]
    [SerializeField] private int energyToWork = 20;
    [SerializeField] private int directionIncrease = 15;
    [SerializeField] private int timeToWork = 2;
    [SerializeField] private Transform workLocation;

    [Header("Integrity Settings")]
    [SerializeField] private int energyToRepair = 25;
    [SerializeField] private int integrityIncrease = 30;
    [SerializeField] private int timeToRepair = 1;
    [SerializeField] private Transform repairLocation;



    [Header("Rooms Settings")]
    [SerializeField] private Transform RoomDoor;
    [SerializeField] private Transform StorehouseDoor;

    public NavMeshAgent PlayerNavMesh { get; private set; }
    public PlayerController PlayerController => player;
    
    public CameraController CameraController { get; private set; }
    private void Start()
    {
        _gm = GameManager.Instance;
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponentInParent<PlayerController>();
        }
        PlayerNavMesh = player.GetComponent<NavMeshAgent>();


        if (PlayerNavMesh == null)
        {
            Debug.LogError("Player GameObject not found!");
        }

        CameraController = Camera.main.GetComponent<CameraController>();
        if (CameraController == null)
        {
            CameraController = GameObject.FindAnyObjectByType<CameraController>();
            if (CameraController == null)
            {
                Debug.LogError("Missing CameraComponent Reference!");
            }
        }
    }

    public bool HasEnoughEnergy(int amountNeeded)
    {
        return _gm.Energy >= amountNeeded;
    }
    public bool CanSleep() => _gm.Paranoia < _gm.ParanoiaTrigger;
    public void Sleep()
    {
        //if (!CanSleep()) return;

        int sleepIncrese = _gm.Paranoia < _gm.ParanoiaTrigger ? _gm.MaxEnergy : sleepEnergyRecover;

        StartCoroutine(ExecuteActivity(sleepLocation,
                                       sleepIncrese,
                                       timeToSleep,
                                       () => _gm.IncreaseDay(1)));
    }

    public bool CanEat() => HasEnoughEnergy(energyToEat);
    public void Eat()
    {
        if (!CanEat()) return;
        if (PlayerNavMesh != null && foodLocation != null)
        {
            // avvia la coroutine che muove il player, decrementa energia e alla fine aumenta la fame
            StartCoroutine(ExecuteActivity(
                foodLocation,
                -energyToEat,
                timeToEat,
                () => _gm.IncreaseHunger(hungerIncrease)
            ));
        }
    }

    public bool CanWash() => HasEnoughEnergy(energyToWash);
    public void Wash()
    {
        if (!CanWash()) return;

        // usa la posizione corrente del player se non hai un Transform specifico
        StartCoroutine(ExecuteActivity(
            washLocation,
            -energyToWash,
            timeToWash,
            () => _gm.IncreaseHygiene(hygieneIncrease)
        ));
    }

    public bool CanWork() => HasEnoughEnergy(energyToWork);
    public void Work()
    {
        if (!CanWork()) return;

        StartCoroutine(ExecuteActivity(
            workLocation,
            -energyToWork,
            timeToWork,
            () => _gm.IncreaseDirection(directionIncrease)
        ));
    }

    public void ConsultMap()
    {
        if (!CanWork()) return;

        StartCoroutine(ExecuteActivity(
            workLocation,
            -energyToWork,
            timeToWork,
            () => _gm.IncreaseDirection(directionIncrease)
        ));
    }

    public bool CanRepair() => HasEnoughEnergy(energyToRepair);
    public void Repair()
    {
        if (!CanRepair()) return;

        StartCoroutine(ExecuteActivity(
            repairLocation,
            -energyToRepair,
            timeToRepair,
            () => _gm.IncreaseIntegrity(integrityIncrease)
        ));
    }

    public void GoToDeck() => CameraController.WarpToDeck();
    public void GoToRoom()
    {
        StartCoroutine(ExecuteActivity(
            RoomDoor,
            0,
            0,
            () => CameraController.WarpToRoom()));
    }

    public void GoToStorehouse()
    {
        StartCoroutine(ExecuteActivity(
        RoomDoor,
        0,
        0,
        () => CameraController.WarpToStorehouse()));

    }   


    // Coroutine generica: avvicina il player a activityPos (se fornito), applica energia e al termine invoca la callback
    private IEnumerator ExecuteActivity(Transform activityPos, int energyDelta, int timeElapsed, Action onComplete)
    { 
        //if we're far, we move to actity
        if (PlayerNavMesh != null)
        {
            while (Vector3.Distance(PlayerNavMesh.gameObject.transform.position, activityPos.transform.position) > distance)
            {
                if (player.CurrentState != PlayerControlState.Walking)
                {
                    // usa MoveToPoint del PlayerController per rispettare il flow originale
                    player.MoveToPoint(activityPos.transform.position);
                }
                yield return null;
            }
        }
        else
        {
            // se non abbiamo PlayerNavMesh o PlayerController, abortiamo con warning
            Debug.LogWarning("ExecuteActivity: Player o PlayerController mancante, esco.");
            yield break;
        }

        // Applichiamo subito la variazione di energia (negativa per consumo)
        _gm.IncreaseEnergy(energyDelta);
        _gm.IncreaseHour(timeElapsed);

        try
        {
            onComplete?.Invoke();
        }
        catch (Exception ex)
        {
            Debug.LogError($"ExecuteActivity: errore nella callback onComplete: {ex}");
        }
    }
}