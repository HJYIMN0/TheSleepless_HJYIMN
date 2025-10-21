using UnityEngine;

public class PlayerActionUi : MonoBehaviour
{
    private PlayerActionSystem _actions;
    private void Start()
    {
        _actions = PlayerActionSystem.Instance;
    }
    
    public void Work()
    {
        _actions.Work();
    }
}
