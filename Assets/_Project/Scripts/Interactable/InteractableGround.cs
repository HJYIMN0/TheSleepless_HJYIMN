using UnityEngine;

public class InteractableGround : AbstractInteractable, IInteractable
{
    public override BoatLocations Location() => playerController != null ? playerController.PlayerBoatLocation : BoatLocations.Deck;

    public override void Execute(){}

    public override void Interact(PlayerController player, RaycastHit hit)
    {
        player.MoveToPoint(hit.point);
        StartCoroutine(WaitInteract(player));
    }
}