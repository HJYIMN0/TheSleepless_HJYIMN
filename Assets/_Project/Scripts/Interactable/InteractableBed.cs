using UnityEngine;

public class InteractableBed : AbstractInteractable
{

    public override BoatLocations Location() => BoatLocations.Room;
    public override void Execute()
    {
        Debug.Log("BEd executed!");
    }
}
