using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class PlayerActionUi : MonoBehaviour
{
    private Tween currentTween;
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
