using UnityEngine;

public class InteractableDoor : AbstractInteractable
{
    [SerializeField] private BoatLocations leadsTo = BoatLocations.Room;

    public override void Execute()
    {

        Debug.Log($"InteractableDoor.Execute on {name} ");
        Debug.Log($"leadsTo = {leadsTo} ");
        Debug.Log($"playerLocation = {playerController.PlayerBoatLocation}");

        if (playerActionSystem == null || playerActionSystem.CameraController == null)
        {
            Debug.LogError("Missing PlayerActionSystem.CameraController in InteractableDoor.Execute");
            return;
        }

        if (leadsTo == BoatLocations.Room)
        {
            playerActionSystem.CameraController.WarpToRoom();
        }
        else if (leadsTo == BoatLocations.Storehouse)
        {
            playerActionSystem.CameraController.WarpToStorehouse();
        }
        else if (leadsTo == BoatLocations.Deck)
        {
            playerActionSystem.CameraController.WarpToDeck();
        }
    }

    // Ora Location restituisce la destinazione di questa porta
    public override BoatLocations Location()
    {
        return leadsTo;
    }
}