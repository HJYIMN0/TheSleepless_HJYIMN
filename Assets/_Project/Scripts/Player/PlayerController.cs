using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera _cam;
    [SerializeField] private CameraController _camController;

    private PlayerControlState _currentState = PlayerControlState.Idle;
    private NavMeshAgent _agent;
    public NavMeshAgent PlayerAgent => _agent;


    private float h;
    private float v;
    public PlayerControlState CurrentState => _currentState;
    private BoatLocations _playerBoatLocation;
    public BoatLocations PlayerBoatLocation => _playerBoatLocation;

    public void SetPlayerBoatLocation(BoatLocations newLocation)
    {
        if (newLocation != _playerBoatLocation)
        {
            _playerBoatLocation = newLocation;
            OnPlayerBoatLocationChange?.Invoke(newLocation);
        }
    }

    public Action<BoatLocations> OnPlayerBoatLocationChange;

    public void SetCam(Camera cam) => _cam = cam;
    private void Awake()
    {
        if (_cam == null)
        {
            Debug.Log("No camera assigned, using main camera.");
            _cam = Camera.main;
        }

        _agent = GetComponent<NavMeshAgent>();
        if (_agent == null) Debug.LogError("NavMeshAgent component is missing on PlayerController.");
    }

    private void Start()
    {
        _camController.onCameraChanged += SetCam;

        // Debug.Log($"PlayerController initialized. agent={_agent != null}, initialLocation={PlayerBoatLocation}");
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _currentState != PlayerControlState.Interacting)
        {
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit) &&
                hit.collider.TryGetComponent(out IInteractable clickable))
            {

                clickable.Interact(this, hit);
                Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 2f);
                Debug.Log($"collider hit = {hit.collider.gameObject.name}");
            }
        }

        if (_currentState == PlayerControlState.Walking && !_agent.pathPending)
        {
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
                {
                    _currentState = PlayerControlState.Idle;
                }
            }
        }
    }


    public void SetState(PlayerControlState newState)
    {
        if (_currentState != newState)
        {
            _currentState = newState;
        }
    }


    public void MoveToPoint(Vector3 point)
    {
        _agent.SetDestination(point);
        _currentState = PlayerControlState.Walking;
    }
}