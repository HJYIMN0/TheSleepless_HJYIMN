using System.Collections;
using UnityEngine;

public abstract class AbstractInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private float _interactionRadius = 1.5f;
    [SerializeField] private float _waitTimeout = 5f;

    public Transform Position => this.transform;
    protected PlayerController playerController;
    public abstract BoatLocations Location();
    public abstract void Execute();

    protected PlayerActionSystem playerActionSystem => PlayerActionSystem.Instance;

    public void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponentInParent<PlayerController>();
    }

    public virtual void Interact(PlayerController player, RaycastHit hit)
    {
        if (player.PlayerBoatLocation != Location())
        {
            StartCoroutine(TransportThenInteract(player, hit));
            return;
        }

        if (player.PlayerAgent != null && player.PlayerAgent.isOnNavMesh)
        {
            if (hit.collider != null)
                player.MoveToPoint(hit.point);
            else
                player.MoveToPoint(this.transform.position);
        }

        StartCoroutine(WaitInteract(player));
    }

    public virtual IEnumerator WaitInteract(PlayerController player)
    {
        float startTime = Time.time;
        BoatLocations initialLocation = player.PlayerBoatLocation;

        while (true)
        {
            if (player.PlayerBoatLocation != initialLocation)
            {
                yield break;
            }

            float distance = Vector3.Distance(player.transform.position, this.transform.position);
            if (distance <= _interactionRadius)
            {
                Execute();
                yield break;
            }

            if (Time.time - startTime > _waitTimeout)
            {
                Debug.LogWarning($"WaitInteract timeout on {name} initialLocation={initialLocation} currentLocation={player.PlayerBoatLocation} distance={Vector3.Distance(player.transform.position, this.transform.position)}");
                yield break;
            }

            //if (player.CurrentState != PlayerControlState.Walking && player.PlayerAgent != null && !player.PlayerAgent.hasPath)
            //{ 
            //    yield break;
            //}

            yield return null;
        }
    }

    private IEnumerator TransportThenInteract(PlayerController player, RaycastHit originalHit)
    {
        BoatLocations desired = Location(); // location dell'interactable finale
        AbstractInteractable closestTransport = SetClosestDoor(desired);
        Debug.Log($"TransportThenInteract: chosen transport = {closestTransport.name}");
        Debug.Log($" leadsTo = {closestTransport.Location()}");
        Debug.Log($" playerLocation = {player.PlayerBoatLocation}");
        Debug.Log($"desired = {desired}");
        if (closestTransport == null)
        {
            Debug.LogError("No door/trapdoor found to transport to " + desired);
            yield break;
        }

        player.MoveToPoint(closestTransport.Position.position);

        float startTime = Time.time;
        while (Vector3.Distance(player.transform.position, closestTransport.Position.position) > _interactionRadius)
        {
            if (Time.time - startTime > _waitTimeout)
            {
                Debug.LogWarning("TransportThenInteract: approach timeout");
                yield break;
            }
            yield return null;
        }

        closestTransport.Execute();

        float waitStart = Time.time;
        while (player.PlayerBoatLocation != desired)
        {
            if (Time.time - waitStart > _waitTimeout)
            {
                Debug.LogWarning("TransportThenInteract: waiting for location change timed out");
                break;
            }
            yield return null;
        }

        if (originalHit.collider != null)
        {
            player.MoveToPoint(originalHit.point);
        }
        else
        {
            player.MoveToPoint(this.transform.position);
        }

        yield return StartCoroutine(WaitInteract(player));
    }

    // Ora accetta la desired location e filtra i candidati in base a quella
    public AbstractInteractable SetClosestDoor(BoatLocations desired)
    {
        if (playerController == null) return null;

        float minDistance = Mathf.Infinity;
        AbstractInteractable best = null;

        switch (playerController.PlayerBoatLocation)
        {
            case BoatLocations.Room:
                {
                    InteractableDoor[] doors = GameObject.FindObjectsByType<InteractableDoor>(FindObjectsSortMode.None);
                    if (doors.Length == 0) return null;
                    foreach (InteractableDoor door in doors)
                    {
                        if (door.Location() != desired) continue;
                        float distance = Vector3.Distance(playerController.transform.position, door.transform.position);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            best = door;
                        }
                    }
                    return best;
                }

            case BoatLocations.Storehouse:
                {
                    InteractableGround[] trapDoors = GameObject.FindObjectsByType<InteractableGround>(FindObjectsSortMode.None);
                    if (trapDoors.Length == 0) return null;
                    foreach (InteractableGround trapDoor in trapDoors)
                    {
                        if (trapDoor.Location() != desired) continue;
                        float distance = Vector3.Distance(playerController.transform.position, trapDoor.transform.position);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            best = trapDoor;
                        }
                    }
                    return best;
                }

            case BoatLocations.Deck:
                {
                    if (desired == BoatLocations.Room)
                    {
                        InteractableDoor[] deckDoors = GameObject.FindObjectsByType<InteractableDoor>(FindObjectsSortMode.None);
                        if (deckDoors.Length == 0) return null;
                        foreach (InteractableDoor door in deckDoors)
                        {
                            if (door.Location() != desired) continue;
                            float distance = Vector3.Distance(playerController.transform.position, door.transform.position);
                            if (distance < minDistance)
                            {
                                minDistance = distance;
                                best = door;
                            }
                        }
                        return best;
                    }
                    else if (desired == BoatLocations.Storehouse)
                    {
                        InteractableGround[] _trapDoors = GameObject.FindObjectsByType<InteractableGround>(FindObjectsSortMode.None);
                        if (_trapDoors.Length == 0) return null;
                        foreach (InteractableGround trap in _trapDoors)
                        {
                            if (trap.Location() != desired) continue;
                            float distance = Vector3.Distance(playerController.transform.position, trap.transform.position);
                            if (distance < minDistance)
                            {
                                minDistance = distance;
                                best = trap;
                            }
                        }
                        return best;
                    }
                    break;
                }
        }

        return null;
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _interactionRadius);
    }
}